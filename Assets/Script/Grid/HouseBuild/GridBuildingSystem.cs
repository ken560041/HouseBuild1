using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;
using static GridBuildingSystem;
using static FloorEdgePlacedObject;
using static FloorPlacedObject;
using static LooseObject;

public class GridBuildingSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static GridBuildingSystem Instance { get; private set; }

    [SerializeField] private GameObject optionMenu;

    //public string saveFile = HouseSaveManager.Instance.currentPath;

    public enum PlaceObjectType
    {
        GridObject,
        EdgeObject,
        LooseObject,
        DoorObject,
        WoodTriObject,
        RoofObject,
        WindowObject,
        WoodStructureObject,
    }

    public enum CompareEdge
    {
        Bigger,
        Smaller,
        Equal
    }

    PlaceObjectType placeObjectType;

    public PlaceObjectType deletePlaceObjectType=PlaceObjectType.GridObject;

    CompareEdge compareEdge;
    public LayerMask placedObjectEdgeColliderLayerMask;


    [SerializeField] Cinemachine.CinemachineFreeLook freeLookCamera;



    [SerializeField] private List<PlacedObjectISO> placedObjectISOList=null;

    private PlacedObjectISO placedObjectISO;

    [SerializeField] private List<FloorEdgeObjectTypeISO> floorEdgeObjectTypeISOList=null;

    private FloorEdgeObjectTypeISO floorEdgeObjectTypeISO;


    [SerializeField] private List<Transform> looseObjectTransformList=null;
    [SerializeField] private List<LooseObject> looseObjectList=null;

    private LooseObjectISO looseObjectISO;
    private float looseObjectEulerY;


    [SerializeField] private List<Window_DoorObjectTypeISO> window_DoorObjectTypeISOList = null;
    private Window_DoorObjectTypeISO window_doorObjectTypeISO;

    [SerializeField] private List<Basic_ObjectTypeISO> basic_ObjectTypeISOList=null;
    private Basic_ObjectTypeISO basic_ObjectTypeISO;

    [SerializeField] private List<RoofObjectTypeISO> roofObjectTypeISOList=null;
    private RoofObjectTypeISO roofObjectTypeISO;

    public Transform testTransform;

    private List<Grid<GridObject>> gridList;
    public Grid<GridObject> selectGrid;
    
    //Event
    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;
    public event EventHandler OnActiveGridLevelChanged;
    public event EventHandler OnSelectedDeleteChanged;
    

    private PlacedObjectISO.Dir dir=PlacedObjectISO.Dir.Down;


    public bool isDemolishActive=false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
        int gridWidth = 200; 
        int gridHeight = 200;
        float cellSize = 0.5f;
        gridList=new List<Grid<GridObject>>();
        int gridVerticalCount = 4;
        float gridVerticalSize = 3.0f;

        for(int i = 0; i < gridVerticalCount; i++)
        {
            Grid<GridObject> gridTemp = new Grid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, gridVerticalSize*i, 0), (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z));
            gridList.Add(gridTemp);
        }

        selectGrid = gridList[0];
        placedObjectISO = null;
        Cursor.visible = false;
        if (HouseSaveManager.Instance != null)
        {
            Load(HouseSaveManager.Instance.currentPath);
        }
    }



    public class GridObject
    {
        private Grid<GridObject> grid;
        private int x;
        private int z;
        private PlacedObject placedObject;
        private bool isPhysic;
        public GridObject(Grid<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
            placedObject = null;
            isPhysic = false;   
        }

        public void SetPlanedObject(PlacedObject _placedỌbject)
        {
            this.placedObject = _placedỌbject;
            TriggerGridObjectChanged();
        }

        public void SetPhysicGridObject(bool _isPhysic)
        {
            this.isPhysic=_isPhysic;
        }

        public bool isPhysicHave()
        {
            return isPhysic;
        }

        public PlacedObject GetPlacedObject()
        {
            return placedObject;
        }

        public void ClearPlanceObject()
        {
            placedObject = null;
            TriggerGridObjectChanged();
        }
        public bool CanBuild()
        {
            return placedObject == null;
        }
        public void TriggerGridObjectChanged()
        {
            grid.TriggerGridObjectChanged(x, z);
        }
        

    }

    private void Update()
    {


        HandleTypeSelect();

        //DeleteObjectPlacement();



        HandleDirRotation();
        HandleGridSelect();
        HandeleDeleteTypeSelect();

        HandleNormalObjectPlacement();
        HandlesEdgeObjectPlacement();

        HandleDoorPlacement();
        HandleLooseObjectPlacement();
        HandleWoodTriPlacement();
        HandleRoofPlacement();
        HandleWindowsPlacement();
        //HandleDemolish();

        HandleWoodStructureObjectPlacement();


        DeleteObjectSelectPlacement();
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Load();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            DeselectObjectType();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionMenu.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { placedObjectISO = placedObjectISOList[0]; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { placedObjectISO = placedObjectISOList[1]; }

        if (Input.GetKeyDown(KeyCode.Alpha0)) { RefreshSelectedObjectType(); }
        Cursor.visible = true;
    }


    private void HandleTypeSelect()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            isDemolishActive = !isDemolishActive;
            if (isDemolishActive)
            {
                placedObjectISO = null;
                window_doorObjectTypeISO = null;
                floorEdgeObjectTypeISO = null;
                basic_ObjectTypeISO = null;
                roofObjectTypeISO = null;
                //isDemolishActive = true;
                RefreshSelectedObjectType();
                RefreshDeleteObjectType();
                
            }
        }
    }

    private void HandleDemolish()
    {
        if(isDemolishActive && Input.GetMouseButtonDown(0) && UltisClass.IsPointerOverUI())
        {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();

            PlacedObject placedObject = selectGrid.GetGridObject(mousePosition).GetPlacedObject();
            if (placedObject != null)
            {
                placedObject.DestroyMySelf();

                List<Vector2Int> gridPositionList= placedObject.GetGridPositionList();
                foreach(Vector2Int position in gridPositionList)
                {
                    selectGrid.GetGridObject(position.x, position.y).ClearPlanceObject();
                }
            }

        }
    }


    private void HandleGridSelect()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            int nextSelectedGridIndex = (gridList.IndexOf(selectGrid) + 1) % gridList.Count;
            selectGrid = gridList[nextSelectedGridIndex];
            OnActiveGridLevelChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public static PlaceObjectType GetNextPlaceObjectType(PlaceObjectType placeObjectType)
    {
        switch (placeObjectType)
        {
            default:
            case PlaceObjectType.GridObject: { return PlaceObjectType.EdgeObject; }
            case PlaceObjectType.EdgeObject: { return PlaceObjectType.DoorObject; }
            case PlaceObjectType.DoorObject: { return PlaceObjectType.WoodTriObject; }

            case PlaceObjectType.WoodTriObject: { return PlaceObjectType.RoofObject; }
            case PlaceObjectType.RoofObject: { return PlaceObjectType.WindowObject; }
                

            case PlaceObjectType.WindowObject: { return PlaceObjectType.GridObject; }
        }
    }

    public void NextPlacedObjectType()
    {
        deletePlaceObjectType=GetNextPlaceObjectType(deletePlaceObjectType);
        OnSelectedDeleteChanged?.Invoke(this, EventArgs.Empty);
        //Debug.Log(deletePlaceObjectType.ToString());
    }


    private void HandeleDeleteTypeSelect()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            NextPlacedObjectType();
        }
    }


   /* private void DeleteObjectPlacement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GridObject gridObject = selectGrid.GetGridObject(Mouse3D.GetMouseWorldPosition());
            PlacedObject placedObject = gridObject.GetPlacedObject();
            if (placedObject != null)
            {
                placedObject.DestroyMySelf();
                List<Vector2Int> _gridPositionList = placedObject.GetGridPositionList();

                foreach (Vector2Int gridPosition in _gridPositionList)
                {
                    selectGrid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlanceObject();
                }


            }

        }

    }*/

    private void DeleteObjectSelectPlacement()
    {
        if (Input.GetMouseButtonDown(1) && isDemolishActive)
        {
            //HouseDeleteObjectGhost
            // Lấy dữ liệu từ housedeleteObjectGhost;
            if (HouseDeleteObjectGhost.Instance._current == null)
            {
                Debug.LogError("Object ko chi gi ca");
                return;
            }
            GameObject gameObject = HouseDeleteObjectGhost.Instance._current;

            switch (deletePlaceObjectType)
            {
                default: break;
                case PlaceObjectType.GridObject:
                    {
                        PlacedObject placedObject= gameObject.GetComponent<PlacedObject>();
                        placedObject.DestroyMySelf();

                        break;

                    }
                case PlaceObjectType.EdgeObject:
                    {
                        FloorEdgePlacedObject floorEdgePlacedObject= gameObject.GetComponent<FloorEdgePlacedObject>();
                        WallObject wallObject=gameObject.GetComponent<WallObject>();
                        if (floorEdgePlacedObject != null)
                        {
                            floorEdgePlacedObject.DestroyMySelf();
                        }
                        else if(wallObject != null) {

                            wallObject.DestroyMySelf();
                        }
                        break;
                    }
                case PlaceObjectType.WoodTriObject:
                    {
                        WoodTriObject woodTriObject = gameObject.GetComponent<WoodTriObject>();
                        woodTriObject.DestroyMySelf();
                        break;
                    }
                case PlaceObjectType.RoofObject:
                    {
                        RoofObject roofObject=gameObject.GetComponent<RoofObject>();
                        roofObject.DestroyMySelf();
                        break;
                    }
                case PlaceObjectType.DoorObject:
                    {

                        DoorObject doorObject= gameObject.GetComponent<DoorObject>();
                        Destroy(doorObject.gameObject); 
                        break;
                    }
                case PlaceObjectType.WindowObject:
                    {
                        WindowObject windowObject=gameObject.GetComponent<WindowObject>();
                        Destroy(windowObject.gameObject);
                        break;
                    }
                case PlaceObjectType.LooseObject:
                    {
                        LooseObject looseObject= gameObject.GetComponent<LooseObject>();
                        looseObject.DestroyMySelf();
                        break;
                    }
            }
            //Destroy(HouseDeleteObjectGhost.Instance._current);
        }
    }

    private void HandleNormalObjectPlacement()
    {
        

        if (placeObjectType==PlaceObjectType.GridObject)
        {
            if (!UltisClass.IsPointerOverUI())
            {
                if (Input.GetMouseButton(0) && placedObjectISO != null)
                {
                    Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
                    selectGrid.GetXZ(mousePosition, out int x, out int z);
                    Vector2Int placedObjectOrigin = new Vector2Int(x, z);

                    if (TryPlacedObject(placedObjectOrigin, placedObjectISO, dir, out PlacedObject placeObject))
                    {


                        PlacedObjectISO placedObjectISOtest = HouseBuildingSystemAssets.Instance.GetPlacedObjectISOFromName(placedObjectISO.name);

                        //Debug.Log(placedObjectISOtest.name);
                    }

                    else if (TryPlacedObjectLevel(placedObjectOrigin, placedObjectISO, dir, out PlacedObject placeObject2)) {
                        Debug.Log("1");
                    }

                    else
                    {
                        //Debug.Log("Lỗi");
                    }
                }
            }
        }
    }

    private void HandleWoodStructureObjectPlacement()
    {
        if (placeObjectType == PlaceObjectType.WoodStructureObject)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, placedObjectEdgeColliderLayerMask))
            {
                if (raycastHit.collider.TryGetComponent(out AnglePosition anglePosition)) { 
                
                    if(raycastHit.collider.transform.parent.TryGetComponent( out FloorEdgePlacedObject floorEdgePlacedObject))
                    {
                        if (Input.GetMouseButton(0) && basic_ObjectTypeISO != null &&GetPlaceObjectType() == GridBuildingSystem.PlaceObjectType.WoodStructureObject)
                        {

                            floorEdgePlacedObject.PlaceWoodStructure(anglePosition, basic_ObjectTypeISO, out WoodStructureObject woodStructureObject);

                            if (woodStructureObject != null)
                            {
                                woodStructureObject.gridLevel = floorEdgePlacedObject.gridLevel;
                                woodStructureObject.point = woodStructureObject.RotatePoints90CW(floorEdgePlacedObject.point, floorEdgePlacedObject.gameObject.transform.rotation.eulerAngles.y);
                            }
                           


                            //woodStructureObject.point= woodStructureObject.RotatePoints90CW(floorEdgePlacedObject.point);
                           
                            SetPhysicGrid(woodStructureObject.gridLevel, woodStructureObject.point);
                            Debug.Log(floorEdgePlacedObject.gameObject.transform.rotation.eulerAngles.y);
                        }
                    }


                    else if(raycastHit.collider.transform.parent.TryGetComponent(out WoodStructureObject woodStructureObject1))
                    {
                        if (Input.GetMouseButton(0) && basic_ObjectTypeISO != null && GetPlaceObjectType() == GridBuildingSystem.PlaceObjectType.WoodStructureObject)
                        {

                            woodStructureObject1.PlaceWoodStructureObject(anglePosition, basic_ObjectTypeISO, out WoodStructureObject woodStructureObject2);
                            if (woodStructureObject2 != null)
                            {
                                woodStructureObject2.gridLevel = woodStructureObject1.gridLevel;
                                woodStructureObject2.point = woodStructureObject2.NextPoint(woodStructureObject1.point, anglePosition.gameObject.transform.rotation.eulerAngles.y);
                            }
                            Debug.Log(anglePosition.gameObject.transform.rotation.eulerAngles.y);
                            SetPhysicGrid(woodStructureObject2.gridLevel, woodStructureObject2.point);
                        }
                    }

                }                        
            }


        

        }
    }



    private void DeselectObjectType()
    {
        placedObjectISO = null;
        floorEdgeObjectTypeISO = null;
        looseObjectISO = null;
        window_doorObjectTypeISO = null;
        basic_ObjectTypeISO = null;
        roofObjectTypeISO = null;
        RefreshSelectedObjectType();
    }

    public void SetDemolishActive()
    {
        placedObjectISO = null;
        //isDemolishActive = true;
        RefreshSelectedObjectType();

    }

    public void OnDeleteObjectActive()
    {
        //isDemolishActive = true;
        RefreshDeleteObjectType();
    }


    private void HandlesEdgeObjectPlacement()
    {
        if (placeObjectType == PlaceObjectType.EdgeObject)
        {
            //int wallLength = floorEdgeObjectTypeISO.width;
            if (!UltisClass.IsPointerOverUI())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 999f, Color.red);

                if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, placedObjectEdgeColliderLayerMask))
                {
                    if (raycastHit.collider.TryGetComponent(out FloorEdgePosition floorEdgePosition))
                    {
                        if (raycastHit.collider.transform.parent.TryGetComponent(out FloorPlacedObject floorPlacedObject))
                        {
                           // Debug.Log("Va cham" + floorEdgePosition.name);

                            if (Input.GetMouseButtonDown(0) && floorEdgeObjectTypeISO != null)
                            {

                                floorPlacedObject.PlaceEdge(floorEdgePosition.edge, floorEdgeObjectTypeISO, out FloorEdgePlacedObject floorEdgePlacedObject);
                                /*Debug.Log(((PlacedObject)floorPlacedObject).GetOrigin()+"    "+ ((PlacedObject)floorPlacedObject).GetGridLevel());
                                Debug.Log(floorEdgePosition.gameObject.transform.rotation.eulerAngles.y);*/

                                List<Vector2Int> temp= floorEdgePlacedObject.GetGridPositionPhysic(((PlacedObject)floorPlacedObject).GetOrigin(), floorEdgePosition.gameObject.transform.rotation.eulerAngles.y);
                                floorEdgePlacedObject.point = temp;
                                floorEdgePlacedObject.gridLevel = ((PlacedObject)floorPlacedObject).GetGridLevel();
                                foreach (Vector2Int tempInt in floorEdgePlacedObject.point)
                                {
                                   // Debug.Log(tempInt);
                                }
                                string typeName = floorEdgePlacedObject.GetFloorEdgeObjectTypeIsoName();
                                if (typeName != "WoodStairs2" && typeName != "WoodenFence_Large")
                                {
                                    SetPhysicGrid(floorEdgePlacedObject.gridLevel, floorEdgePlacedObject.point);
                                }

                            }
                        }

                    }


                    else if(raycastHit.collider.TryGetComponent(out WoodTrimPosition woodTrimPosition)){
                        if(raycastHit.collider.transform.parent.TryGetComponent(out FloorEdgePlacedObject floorEdgePlacedObject))
                        {
                            if (Input.GetMouseButtonDown(0) && floorEdgeObjectTypeISO != null)
                            {
                                floorEdgePlacedObject.PlacedWall(woodTrimPosition, floorEdgeObjectTypeISO,out FloorEdgePlacedObject floorEdgePlacedObject1);
                                floorEdgePlacedObject1.point=floorEdgePlacedObject.point;
                                floorEdgePlacedObject1.gridLevel = floorEdgePlacedObject.gridLevel + 1;
                                SetPhysicGrid(floorEdgePlacedObject1.gridLevel, floorEdgePlacedObject1.point);
                            }
                        }
                    }


                }

            }
        }
    }


    private void HandleDirRotation()
    {
        if (Input.GetKeyDown(KeyCode.R)&& placeObjectType==PlaceObjectType.GridObject)
        {
            dir = PlacedObjectISO.GetNextDir(dir);
            Debug.Log(dir);
        }
    }


    private void HandleDoorPlacement()
    {
        if (placeObjectType == PlaceObjectType.DoorObject)
        {
            if (!UltisClass.IsPointerOverUI())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, placedObjectEdgeColliderLayerMask))
                {
                    if(raycastHit.collider.TryGetComponent(out DoorPosition doorPosition))
                    {
                        if(raycastHit.collider.transform.parent.TryGetComponent(out WallObject wallObject))
                        {
                            if(Input.GetMouseButtonDown(0) && window_doorObjectTypeISO != null)
                            {
                                wallObject.PlaceDoor(doorPosition, window_doorObjectTypeISO);
                            }
                        }
                    }
                }
            }
        }
    }


    private void HandleWoodTriPlacement()
    {
        if (placeObjectType == PlaceObjectType.WoodTriObject)
        {
            if (!UltisClass.IsPointerOverUI())
            {
                Ray ray= Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray,out RaycastHit raycastHit, 999f, placedObjectEdgeColliderLayerMask))
                {
                    if(raycastHit.collider.TryGetComponent(out WoodTrimPosition woodTrimPosition))
                    {
                        if(raycastHit.collider.transform.parent.TryGetComponent(out FloorEdgePlacedObject floorEdgePlacedObject))
                        {
                            if(Input.GetMouseButtonDown(0)&& basic_ObjectTypeISO != null)
                            {
                                floorEdgePlacedObject.PlacedWoodTri(woodTrimPosition, basic_ObjectTypeISO);
                            }
                        }

                    }
                }
            }
        }
    }

    private void HandleRoofPlacement()
    {
        if(placeObjectType== PlaceObjectType.RoofObject)
        {
            if (!UltisClass.IsPointerOverUI())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, placedObjectEdgeColliderLayerMask))
                {
                    if(raycastHit.collider.TryGetComponent(out RoofTrussPosition roofTrussPosition))
                    {
                        if(raycastHit.collider.transform.parent.TryGetComponent(out WoodTriObject woodTriObject))
                        {

                            if (Input.GetMouseButtonDown(0) && roofObjectTypeISO != null) woodTriObject.PlaceRoofObject(roofTrussPosition, roofObjectTypeISO);
                        }
                        else if(raycastHit.collider.transform.parent.TryGetComponent(out RoofObject roofObject))
                        {
                            if (Input.GetMouseButtonDown(0) && roofObjectTypeISO != null) roofObject.PlaceRoofObject(roofTrussPosition, roofObjectTypeISO);
                        }
                    }
                }
            }
        }
    }

    private void HandleWindowsPlacement()
    {
        if (placeObjectType == PlaceObjectType.WindowObject)
        {
            if (!UltisClass.IsPointerOverUI())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, placedObjectEdgeColliderLayerMask))
                {
                    if (raycastHit.collider.TryGetComponent(out DefaultPosition windowPosition))
                    {
                        if (raycastHit.collider.transform.parent.TryGetComponent(out WallObject_Window wallObject_Window))
                        {

                            if (Input.GetMouseButtonDown(0) && window_doorObjectTypeISO != null) wallObject_Window.PlaceWindow(windowPosition,window_doorObjectTypeISO);
                        }
                        
                    }
                }
            }
        }
    }


    private void HandleLooseObjectPlacement()
    {
        if(placeObjectType == PlaceObjectType.LooseObject && looseObjectISO!=null)
        {
            if (!UltisClass.IsPointerOverUI())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if(Physics.Raycast(ray, out RaycastHit raycastHit)) {
                        // Raycast Hit something
                        if (Input.GetMouseButtonDown(0))
                        {
                            Transform looseObjectTransform = Instantiate(looseObjectISO.prefab.transform, raycastHit.point,  Quaternion.Euler(0, looseObjectEulerY, 0));

                            /*LooseObject looseObject=LooseObject.Create(looseObjectISO,raycastHit.point, new Vector3(0,looseObjectEulerY,0));
                            looseObjectList.Add(looseObject);*/
                            looseObjectTransformList.Add(looseObjectTransform);
                        }
                    }
            }
        }

        if (Input.GetKey(KeyCode.R)&& placeObjectType == PlaceObjectType.LooseObject)
        {
            looseObjectEulerY += Time.deltaTime * 90f;
        }

    }



    public bool TryPlacedObject(Grid<GridObject> grid, Vector2Int placedObjectOrigin, PlacedObjectISO placedObjectISO, PlacedObjectISO.Dir dir, out PlacedObject placedObject)
    {
        List<Vector2Int> _gridPositionList = placedObjectISO.GetGridPositionList(placedObjectOrigin, dir);
        bool _canBuild = true;
        foreach (Vector2Int _gridPosition in _gridPositionList)
        {
            bool isValidPosition=grid.IsValidGridPosition(_gridPosition);

            if (!isValidPosition)
            {
                _canBuild = false;
                break;
            }
            if (!grid.GetGridObject(_gridPosition.x, _gridPosition.y).CanBuild())
            {
                _canBuild = false;
                break;
            }
        }
        if (_canBuild)
        {

            //Debug.Log(gridList.IndexOf(grid)+1);
            Vector2Int rotationOffset = placedObjectISO.GetRotationOffset(dir);
            Vector3 placedObjectPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            placedObject = PlacedObject.Create(placedObjectPosition, placedObjectOrigin, dir, placedObjectISO, ((int)(gridList.IndexOf(grid) + 1)));
            foreach (Vector2Int gridPosition in _gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlanedObject(placedObject);
            }

            //OnObjectPlaced?.Invoke(placedObject, EventArgs.Empty);
            return true;
        }
        else
        {
            //Debug.Log("Ko dat dc o day");
            placedObject = null;
            return false;
        }


    }


    private void SetPhysicGrid(int gridLevel, List<Vector2Int> pointList)
    {
        if (gridLevel < 0 || gridLevel >= gridList.Count) return;

        Grid<GridObject> grid = gridList[gridLevel];

        foreach (var point in pointList)
        {
            GridObject gridObject = grid.GetGridObject(point.x, point.y);
            if (gridObject != null)
            {
                gridObject.SetPhysicGridObject(true);
                //Debug.Log("dat o" +point.x + point.y);
                // Nếu cần: gridObject.TriggerGridObjectChanged();
            }
        }
    }



    public bool TryPlacedObjectLevel(Vector2Int placedObjectOrigin, PlacedObjectISO placedObjectISO, PlacedObjectISO.Dir dir, out PlacedObject placedObject) {
        placedObject = null;
        if (gridList.IndexOf(selectGrid) != 0)
        {
           return TryPlacedObjectLevel(selectGrid, placedObjectOrigin, placedObjectISO, dir, out placedObject);
            
        }
        return false; 


    }



    private bool TryPlacedObjectLevel(Grid<GridObject> grid, Vector2Int placedObjectOrigin, PlacedObjectISO placedObjectISO, PlacedObjectISO.Dir dir, out PlacedObject placedObject) {
        placedObject=null;
        int count=0;
        List<Vector2Int> _gridPositionListBound = placedObjectISO.GetGridPositionFullList(placedObjectOrigin);
        foreach (Vector2Int _gridPosition in _gridPositionListBound)
        {
            if (grid.GetGridObject(_gridPosition.x, _gridPosition.y).isPhysicHave())
            {
                count++;
            }
         //  Debug.Log(_gridPosition.x +" "+ _gridPosition.y);
        }
        foreach (Vector2Int _gridPosition in _gridPositionListBound)
        {
           
           // Debug.Log(_gridPosition.x + " " + _gridPosition.y);
        }

        Debug.Log(count);

        if (count >=12) {

            Debug.Log("Có thể đặt tầng 2");
        }


        List<Vector2Int> _gridPositionList = placedObjectISO.GetGridPositionList(placedObjectOrigin, dir);
        bool _canBuild = true;
        foreach (Vector2Int _gridPosition in _gridPositionList)
        {
            bool isValidPosition = grid.IsValidGridPosition(_gridPosition);

            if (!isValidPosition)
            {
                _canBuild = false;
                break;
            }
            if (!grid.GetGridObject(_gridPosition.x, _gridPosition.y).CanBuild())
            {
                _canBuild = false;
                break;
            }
        }

        if (count <11)
        {
            _canBuild=false;    
        }

        if (_canBuild)
        {
            Vector2Int rotationOffset = placedObjectISO.GetRotationOffset(dir);
            Vector3 placedObjectPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            placedObject = PlacedObject.Create(placedObjectPosition, placedObjectOrigin, dir, placedObjectISO, ((int)(gridList.IndexOf(grid) + 1)));
            foreach (Vector2Int gridPosition in _gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlanedObject(placedObject);
            }

            //OnObjectPlaced?.Invoke(placedObject, EventArgs.Empty);
            return true;
        }
        else
        {
            //Debug.Log("Ko dat dc o day");
            placedObject = null;
            return false;
        }


    }


    public bool TryObjectEdge(FloorEdgePosition floorEdgePosition, FloorEdgeObjectTypeISO floorEdgeObjectTypeISO, out FloorEdgePlacedObject floorEdgePlacedObject)
    {

        floorEdgePlacedObject=FloorEdgePlacedObject.Create(floorEdgeObjectTypeISO, floorEdgePosition);
         
        return true;
    }



    public bool CanBuildFloor(Grid<GridObject>grid ,Vector2Int placedObjectOrigin, PlacedObjectISO placedObjectISO, PlacedObjectISO.Dir dir)
    {
        List<Vector2Int> _gridPositionList = placedObjectISO.GetGridPositionList(placedObjectOrigin, dir);
        bool canBuild = true;

        if (grid != gridList[0])
        {
            int count = 0;
            List<Vector2Int> _gridPositionListBound = placedObjectISO.GetGridPositionFullList(placedObjectOrigin);
            foreach (Vector2Int _gridPosition in _gridPositionListBound)
            {
                if (grid.GetGridObject(_gridPosition.x, _gridPosition.y).isPhysicHave())
                {
                    count++;
                }
                //  Debug.Log(_gridPosition.x +" "+ _gridPosition.y);
            }

            if (count < 11)
            {
                canBuild= false;
                
            }

        }

        foreach (Vector2Int _gridPosition in _gridPositionList)
        {
            bool isValidPosition = grid.IsValidGridPosition(_gridPosition);

            if (!isValidPosition)
            {
                canBuild = false;
                break;
                
            }
            if (!grid.GetGridObject(_gridPosition.x, _gridPosition.y).CanBuild())
            {
                canBuild = false;
                break;
            }
        }
        return canBuild;
    }

    public bool TryPlacedObject(Vector2Int placedObjectOrigin, PlacedObjectISO placedObjectISO, PlacedObjectISO.Dir dir, out PlacedObject placedObject)
    {
        placedObject = null;
        if (gridList.IndexOf(selectGrid) == 0)
        {
            return TryPlacedObject(selectGrid, placedObjectOrigin, placedObjectISO, dir, out placedObject);
        }
        return false;
    }
    private void RefreshSelectedObjectType()
    {
        

        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RefreshDeleteObjectType()
    {
        OnSelectedDeleteChanged?.Invoke(this, EventArgs.Empty);
    }



    public Vector3 GetMouseWorldPositionSnap()
    {
        Vector3 mousePosition=Mouse3D.GetMouseWorldPosition();
        selectGrid.GetXZ(mousePosition, out int x, out int z);
        if(placedObjectISO != null)
        {
            Vector2Int rotationOffset = placedObjectISO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = selectGrid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x,0,rotationOffset.y) * selectGrid.GetCellSize();
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
        
    }

    public Vector3 GetMGetMouseWorldPositionNoSnap()
    {
        Vector3 mousePosition=Mouse3D.GetMouseWorldPosition();
        selectGrid.GetXZ(mousePosition, out int x, out int z);

        Vector3 placedObjectWorldPosition = selectGrid.GetWorldPosition(x, z);
        
        
            return placedObjectWorldPosition;
        

    }


    public Vector3 GetMouseWorldPosition()
    {
        return  Mouse3D.GetMouseWorldPosition();
    }

    public FloorEdgePosition GetMouseFloorEdgePosition()
    {
        if (!UltisClass.IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, placedObjectEdgeColliderLayerMask))
            {
                if (hit.collider.TryGetComponent(out FloorEdgePosition floorEdgePosition))
                {
                    //Debug.Log(floorEdgePosition.name);
                    return floorEdgePosition;

                }
            }
        }
        return null;
    }

    public PlacedObject GetMousePlacedObject()
    {
        if (!UltisClass.IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit hit , 999f, placedObjectEdgeColliderLayerMask))
            {
                if(hit.collider.TryGetComponent(out PlacedObject placedObject))
                {
                    return placedObject;
                }
            }
        }

        return null;
    }

    public FloorPlacedObject GetFloorPlacedObject()
    {
        if (!UltisClass.IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, placedObjectEdgeColliderLayerMask))
            {
                if (hit.collider.TryGetComponent(out FloorPlacedObject floorPlacedObject))
                {
                    return floorPlacedObject;
                }
            }
        }

        return null;
    }

    public WoodTriObject GetWoodTriObject()
    {
        if (!UltisClass.IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, placedObjectEdgeColliderLayerMask))
            {
                if (hit.collider.TryGetComponent(out WoodTriObject woodTriObject))
                {
                    return woodTriObject;
                }
            }
        }

        return null;
    }

    public RoofObject GetRoofObject()
    {
        if (!UltisClass.IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, placedObjectEdgeColliderLayerMask))
            {
                if (hit.collider.TryGetComponent(out RoofObject roofObject))
                {
                    return roofObject;
                }
            }
        }

        return null;
    }

    public DoorObject GetDoorObject()
    {
        if (!UltisClass.IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, placedObjectEdgeColliderLayerMask))
            {
                if (hit.collider.TryGetComponent(out DoorObject doorObject))
                {
                    return doorObject;
                }
            }
        }

        return null;
    }

    public WindowObject GetWindowObject()
    {
        if (!UltisClass.IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, placedObjectEdgeColliderLayerMask))
            {
                if (hit.collider.TryGetComponent(out WindowObject windowObject))
                {
                    return windowObject;
                }
            }
        }

        return null;
    }


    public FloorEdgePlacedObject GetFloorEdgePlacedObject()
    {
        if (!UltisClass.IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, placedObjectEdgeColliderLayerMask))
            {
                if (hit.collider.TryGetComponent(out FloorEdgePlacedObject floorEdgePlacedObject))
                {
                    return floorEdgePlacedObject;
                }
            }
        }

        return null;
    }



    public DoorPosition GetMouseDoorPosition()
    {
        if (!UltisClass.IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out RaycastHit hit,999f, placedObjectEdgeColliderLayerMask))
            {
                if(hit.collider.TryGetComponent(out DoorPosition doorPosition))
                {
                    return doorPosition;
                }
            }
        }
        return null;
    }

    public DefaultPosition GetMouseWindowPosition()
    {
        if (!UltisClass.IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, placedObjectEdgeColliderLayerMask))
            {
                if (hit.collider.TryGetComponent(out DefaultPosition windowPosition))
                {
                    return windowPosition;
                }
            }
        }
        return null;
    }

    public WoodTrimPosition GetMouseWoodTriPosition()
    {
        if (!UltisClass.IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, 999f, placedObjectEdgeColliderLayerMask))
            {
                if(hit.collider.TryGetComponent(out WoodTrimPosition woodTrimPosition))
                {
                    return woodTrimPosition;
                }
            }
        }
        return null;
    }

    public RoofTrussPosition GetMouseRoofTrussPosition()
    {
        if (!UltisClass.IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, placedObjectEdgeColliderLayerMask))
            {
                if (hit.collider.TryGetComponent(out RoofTrussPosition roofTrussPosition))
                {
                    return roofTrussPosition;
                }
            }
        }
        return null;
    }

    public AnglePosition GetMouseAnglePosition()
    {
        if (!UltisClass.IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, placedObjectEdgeColliderLayerMask))
            {
                if (hit.collider.TryGetComponent(out AnglePosition anglePosition))
                {
                    return anglePosition;
                }
            }
        }
        return null;
    }



    public Quaternion GetPlacedObjectRotation()
    {
        if (placedObjectISO != null)
        {
            return Quaternion.Euler(0, placedObjectISO.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public PlaceObjectType GetPlaceObjectType()
    {
        return placeObjectType;
    }


    public PlacedObjectISO GetPlacedObjectTypeSO()
    {
        return placedObjectISO;
    }

    public FloorEdgeObjectTypeISO GetFloorEdgeObjectTypeSO()
    {
        return floorEdgeObjectTypeISO;
    }

    public Window_DoorObjectTypeISO GetWindow_DoorObjectTypeSO()
    {
        return window_doorObjectTypeISO;
    }

    public Basic_ObjectTypeISO GetBasic_ObjectTypeISO()
    {
        return basic_ObjectTypeISO;
    }


    public RoofObjectTypeISO GetRoofObjectTypeISO()
    {
        return roofObjectTypeISO;
    }
    

    public LooseObjectISO GetLooseObjectSO()
    {
        return looseObjectISO;
    }


    public PlacedObjectISO.Dir GetDir()
    {
        return dir;
    }

    public float GetLooseObjectEulerY()
    {
        return looseObjectEulerY;
    }


    // xử lý phần cạnh



    public bool CanBuildEdgeObject()
    {
        bool canBuild = false;
        if (placeObjectType == PlaceObjectType.EdgeObject)
        {
            if (!UltisClass.IsPointerOverUI())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, placedObjectEdgeColliderLayerMask))
                {
                    if (raycastHit.collider.TryGetComponent(out FloorEdgePosition floorEdgePosition))
                    {
                        canBuild =true;
                    }
                }

            }
        }
        return canBuild;
    }

    private CompareEdge LargerBackground()
    {
        if (placeObjectType == PlaceObjectType.EdgeObject)
        {
            int wallLength = floorEdgeObjectTypeISO.width;
            if (!UltisClass.IsPointerOverUI())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 999f, Color.red);

                if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, placedObjectEdgeColliderLayerMask))
                {
                    if (raycastHit.collider.TryGetComponent(out FloorEdgePosition floorEdgePosition))
                    {

                        int edgeCount = floorEdgePosition.floorChildEdgePositions.Count;
                        if (edgeCount < wallLength)
                        {

                            // cạnh bé hơn tường
                            return CompareEdge.Smaller;
                        }
                        else if (edgeCount > wallLength)
                        {
                            //cạnh lớn hơn tường 
                            return CompareEdge.Bigger;
                        }
                        else if (edgeCount == wallLength)
                        {

                            //cạnh bằng tường
                            return CompareEdge.Equal;
                        }

                    }
                }
            }
            
        }
        return default(CompareEdge);
    }

    public void SelectPlacedObjectTypeISO( PlacedObjectISO placedObjectISO)
    {
        placeObjectType = PlaceObjectType.GridObject;
        this.placedObjectISO = placedObjectISO;
        isDemolishActive=false;
        RefreshSelectedObjectType();
    }


    public void SelectFloorEdgeObjectTypeISO(FloorEdgeObjectTypeISO floorEdgeObjectTypeISO)
    {
        placeObjectType = PlaceObjectType.EdgeObject;
        this.floorEdgeObjectTypeISO = floorEdgeObjectTypeISO;
        isDemolishActive = false;
        RefreshSelectedObjectType();
    }


    public void SelectWindow_DoorObjectTypeISO(Window_DoorObjectTypeISO window_DoorObjectTypeISO)
    {
        placeObjectType = PlaceObjectType.DoorObject;
        this.window_doorObjectTypeISO = window_DoorObjectTypeISO;
        isDemolishActive = false;
        RefreshSelectedObjectType();
    }

    public void SelectWindowObjectTypeISO(Window_DoorObjectTypeISO window_DoorObjectTypeISO)
    {
        placeObjectType = PlaceObjectType.WindowObject;
        this.window_doorObjectTypeISO = window_DoorObjectTypeISO;
        isDemolishActive = false;
        RefreshSelectedObjectType();
    }

    public void SelectTri_WoodenObjectTypeISO(Basic_ObjectTypeISO basic_ObjectTypeISO)
    {
        placeObjectType = PlaceObjectType.WoodTriObject;
        this.basic_ObjectTypeISO = basic_ObjectTypeISO; isDemolishActive = false;
        RefreshSelectedObjectType();
    }


    public void SelectRoofObjectTypeISO(RoofObjectTypeISO roofObjectTypeISO)
    {
        placeObjectType=PlaceObjectType.RoofObject;
        this.roofObjectTypeISO=roofObjectTypeISO;
        isDemolishActive = false;
        RefreshSelectedObjectType();
    }

    public void SelectLooseObjectTypeISO(LooseObjectISO looseObjectISO)
    {
        placeObjectType= PlaceObjectType.LooseObject;
        this.looseObjectISO = looseObjectISO;
        RefreshSelectedObjectType();
    }


    public void SelectWoodStructureObjectTypeISO(Basic_ObjectTypeISO basic_ObjectTypeISO)
    {
        placeObjectType = PlaceObjectType.WoodStructureObject;
        this.basic_ObjectTypeISO = basic_ObjectTypeISO; isDemolishActive = false;
        RefreshSelectedObjectType();
    }


    public PlaceObjectType GetDeletePlaceObjectType()
    {
        return deletePlaceObjectType;
    }

    public int GetActiveGridLevel()
    {
        return gridList.IndexOf(selectGrid);
    }


    private void FreeGridObject()
    {
        foreach(Grid<GridObject> grid in gridList)
        {
            for(int x = 0; x < grid.GetHeight(); x++)
            {
                for(int y=0;y < grid.GetWidth(); y++)
                {
                    PlacedObject placedObject=grid.GetGridObject(x,y).GetPlacedObject();
                    if(placedObject != null)
                    {
                        placedObject.DestroyMySelf();
                        List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                        foreach (Vector2Int position in gridPositionList)
                        {
                            grid.GetGridObject(position.x, position.y).ClearPlanceObject();
                        }
                    }
                }
            }
        }
    }
   



    public void Save(string namefile)
    {
        List<PlacedObjectSaveObjectArray> placedObjectSaveObjectArrayList= new List<PlacedObjectSaveObjectArray>();
        foreach(Grid<GridObject> grid in gridList)
        {
            List<PlacedObject.SaveObject> saveObjectList = new List<PlacedObject.SaveObject>();
            List<PlacedObject>savePlacedObjectList=new List<PlacedObject>();

            for(int x=0;x<grid.GetHeight();x++)
            {
                for(int y = 0; y < grid.GetWidth(); y++)
                {
                    PlacedObject placedObject=grid.GetGridObject(x,y).GetPlacedObject();
                    if (placedObject != null && !savePlacedObjectList.Contains(placedObject))
                    {
                        // Save object
                        Debug.Log($"Saving object at ({x}, {y})");
                        savePlacedObjectList.Add(placedObject);
                        saveObjectList.Add(placedObject.GetSaveObject());


                        // placedObject.GetSaveObject() là dữ liệu của 1 cái nền 
                    }
                }
            }

           

            PlacedObjectSaveObjectArray placedObjectSaveObjectArray=new PlacedObjectSaveObjectArray { placedObjectSaveObjectArray=saveObjectList.ToArray()};



            // placedObjectSaveObjectArray lưu dữ liệu của  1 grid
            placedObjectSaveObjectArrayList.Add(placedObjectSaveObjectArray);
            // Có tổng 4 grid


        }


        List<LooseSaveObject> looseSaveObjectList=new List<LooseSaveObject>();
        foreach(Transform looseObjectTransform in looseObjectTransformList)
        {
            if (looseObjectTransform == null) { continue; }
            looseSaveObjectList.Add(new LooseSaveObject
            {
                looseObjectSOName = looseObjectTransform.GetComponent<LooseObject>().looseObjectISO.name,
                position = looseObjectTransform.transform.position,
                quaternion = looseObjectTransform.transform.rotation,
            }) ;
        }


        SaveObject saveObject=new SaveObject
        {
            placedObjectSaveObjectArrayArray= placedObjectSaveObjectArrayList.ToArray(),
            looseSaveObjectArray= looseSaveObjectList.ToArray(),
            //grid 1->4
        };
        string json=JsonUtility.ToJson(saveObject);
        SaveSystem.Save(namefile, json, true);
        Debug.Log("Save!!!!!"+ namefile);
    }

    public void Load(string namefile)
    {
        FreeGridObject();
        string json = SaveSystem.Load(namefile);
        Debug.Log(json);
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);

        if (saveObject == null)
        {
            Debug.LogError("Load failed: SaveObject is null.");
            return;
        }
        if (saveObject.placedObjectSaveObjectArrayArray == null || saveObject.placedObjectSaveObjectArrayArray.Length == 0)
        {
            Debug.LogError("Load failed: placedObjectSaveObjectArrayArray is null or empty.");
            return;
        }

        if (gridList == null || gridList.Count == 0)
        {
            Debug.LogError("Grid list is empty or null.");
            return;
        }



        for (int i = 0; i < gridList.Count; i++)
        {
            Grid<GridObject> grid = gridList[i];
            if (grid == null)
            {
                Debug.LogError($"Grid at index {i} is null.");
                continue;
            }

            foreach (PlacedObject.SaveObject placedObjectSaveObject in saveObject.placedObjectSaveObjectArrayArray[i].placedObjectSaveObjectArray)
            {
                PlacedObjectISO placedObjectISO=HouseBuildingSystemAssets.Instance.GetPlacedObjectISOFromName(placedObjectSaveObject.placedObjectISOName);

               // Debug.Log(placedObjectSaveObject.placedObjectISOName == "WoodenFloor1");

                if (placedObjectISO == null)
                {
                   // Debug.LogError($"Failed to find PlacedObjectISO with name: {placedObjectSaveObject.placedObjectISOName}");
                    continue;
                }

//Debug.Log($"Origin: {placedObjectSaveObject.origin}, ISO: {placedObjectISO.name}, Dir: {placedObjectSaveObject.dir}");

                

                TryPlacedObject(grid,placedObjectSaveObject.origin, placedObjectISO,placedObjectSaveObject.dir,out PlacedObject placedObject);
                if(placedObject is FloorPlacedObject)
                {
                    FloorPlacedObject floorPlacedObject= (FloorPlacedObject)placedObject;

                    Debug.Log(placedObjectSaveObject.floorPlacedObjectSave);
                    

                    floorPlacedObject.Load(placedObjectSaveObject.floorPlacedObjectSave);
                    //JsonUtility.FromJson<FloorEdgePlacedObjectSave>(placedObjectSaveObject.floorPlacedObjectSave);




                    var saveObjectTest = placedObjectSaveObject.floorPlacedObjectSave;

                    FloorSaveObject floorSaveObjectTemp = JsonUtility.FromJson<FloorSaveObject>(placedObjectSaveObject.floorPlacedObjectSave);


                  //  Debug.Log(floorSaveObjectTemp.ToString());

                   /* FloorEdgePlacedObject.FloorEdgePlacedObjectData data = JsonUtility.FromJson<FloorEdgePlacedObjectData>(
    floorSaveObjectTemp.upEdgeObjectName.data);
                    if(data != null)
                    {
                        Debug.Log("wallObjectName: " + data.wallObjectName);
                    }*/

                    //
                    
                }
            }
        }

        if (saveObject.looseSaveObjectArray != null)
        {
            foreach (LooseSaveObject looseSaveObject in saveObject.looseSaveObjectArray)
            {
                Transform looseObjectTransform = Instantiate(
                    HouseBuildingSystemAssets.Instance.GetLooseObjectISOFromName(looseSaveObject.looseObjectSOName).prefab.transform,
                    looseSaveObject.position,
                    looseSaveObject.quaternion
                );
                looseObjectTransformList.Add(looseObjectTransform);
            }
            Debug.Log("Load" + namefile);
        }
    }

    public void Save()
    {
        Save(HouseSaveManager.Instance.currentPath);
    }
    public void Load()
    {
        Load(HouseSaveManager.Instance.currentPath);
    }



    [System.Serializable]

    public class SaveObject
    {
        public PlacedObjectSaveObjectArray[] placedObjectSaveObjectArrayArray;

        public LooseSaveObject[] looseSaveObjectArray;

        //placedObjectSaveObjectArrayArray là 1 grid
    }

    [System.Serializable]
    public class PlacedObjectSaveObjectArray
    {
        public PlacedObject.SaveObject[] placedObjectSaveObjectArray;

        // Dữ liệu bên trong 1 grid
    }
    
}
