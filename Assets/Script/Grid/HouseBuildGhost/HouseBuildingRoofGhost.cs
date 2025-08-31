    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class HouseBuildingRoofGhost : MonoBehaviour
    {
        // Start is called before the first frame update
        private Transform visual;

        public bool canBuild;

        private bool _canBuild
        {
            get => canBuild;
            set
            {
                canBuild = value;
                if (visual.gameObject != null)
                {
                    if (canBuild)
                    {
                    visual.gameObject.layer = LayerMask.NameToLayer("Ghost");
                    }
                    else
                    {
                    visual.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
        }


        private RoofObject.Edge? previousEdge = null;
        private void Start()
        {
            GridBuildingSystem.Instance.OnSelectedChanged += Instante_OnSelectedChanged;
        
        }

        private void OnEnable()
        {
 
        }
        private void LateUpdate()
        {
            //WoodTrimPosition woodTrimPosition = GridBuildingSystem.Instance.GetMouseWoodTriPosition();
            RoofTrussPosition roofTrussPosition = GridBuildingSystem.Instance.GetMouseRoofTrussPosition();

            /*if (woodTrimPosition != null)
            {
                transform.position = Vector3.Lerp(transform.position, woodTrimPosition.transform.position, Time.deltaTime * 15f);
                transform.rotation = Quaternion.Lerp(transform.rotation, woodTrimPosition.transform.rotation, Time.deltaTime * 25f);
            }*/
            if (roofTrussPosition != null && roofTrussPosition.isHave == false)
            {
                transform.position = Vector3.Lerp(transform.position, roofTrussPosition.transform.position, Time.deltaTime * 15f);
                transform.rotation = Quaternion.Lerp(transform.rotation, roofTrussPosition.transform.rotation, Time.deltaTime * 25f);
                canBuild = true;
            if (roofTrussPosition.edge != previousEdge)
            {
                previousEdge = roofTrussPosition.edge;
                RefreshVisual(roofTrussPosition);
            }
        }

            else
            {
                Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldPositionSnap();
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 25f);

            if (previousEdge != null)
            {
                previousEdge = null;
                RefreshVisual(null); // không có vị trí hợp lệ
            }

        }
        }

        private void Instante_OnSelectedChanged(object sander, System.EventArgs eventArgs)
        {
            RoofTrussPosition currentRoofTrussPosition = GridBuildingSystem.Instance.GetMouseRoofTrussPosition();
            RefreshVisual(currentRoofTrussPosition);
        }

        private void RefreshVisual(RoofTrussPosition roofTrussPosition)
        {
            if(visual != null)
            {
                Destroy(visual.gameObject);
                visual = null;
            }
            if (GridBuildingSystem.Instance.GetPlaceObjectType() == GridBuildingSystem.PlaceObjectType.RoofObject)
            {
                RoofObjectTypeISO roofObjectTypeISO = GridBuildingSystem.Instance.GetRoofObjectTypeISO();
                if(roofObjectTypeISO != null)
                {

                //if (roofTrussPosition.edge == RoofObject.Edge.Right)
                //Debug.Log(roofTrussPosition.edge.ToString());

                if (roofTrussPosition != null && roofTrussPosition.isHave == false && roofTrussPosition.edge == RoofObject.Edge.Right)
                {
                    
                    Debug.Log(roofTrussPosition.edge.ToString());
                    if (roofTrussPosition.transform.parent.GetComponent<WoodTriObject>())
                    {
                        visual = Instantiate(roofObjectTypeISO.right.visual.transform, Vector3.zero, Quaternion.identity);
                        _canBuild = true;
                    }
                    else
                    {

                        visual = Instantiate(roofObjectTypeISO.left.visual.transform, Vector3.zero, Quaternion.identity);
                        _canBuild = true;

                    }
                }
                    else if(roofTrussPosition != null && roofTrussPosition.isHave==false && roofTrussPosition.edge == RoofObject.Edge.Left)
                    {
                    

                    if (roofTrussPosition.transform.parent.GetComponent<WoodTriObject>())
                    {
                        visual = Instantiate(roofObjectTypeISO.left.visual.transform, Vector3.zero, Quaternion.identity);
                        _canBuild = true;
                    }

                    

                    else{ visual = Instantiate(roofObjectTypeISO.right.visual.transform, Vector3.zero, Quaternion.identity);
                        _canBuild = true;
                    }
                    
                    }
                    else
                    {
                        visual = Instantiate(roofObjectTypeISO.left.visual.transform, Vector3.zero, Quaternion.identity);
                    _canBuild = false;
                    }
                
                    visual.parent = transform;
                    visual.localEulerAngles = Vector3.zero;
                    visual.localPosition = Vector3.zero;
                }
            }


        }

    }
