using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WoodStructureObject : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Basic_ObjectTypeISO basic_ObjectType;

     private PlacedObjectISO.Dir dir;
    WoodStructureObject next;

    public int gridLevel;
    public List<Vector2Int> point;

    public string GetWoodStructureBasicObjectTypeISOName()
    {
        return basic_ObjectType.name;
    }


    public static WoodStructureObject Create(AnglePosition woodStructurePosition, Basic_ObjectTypeISO basic_ObjectType) {

        if (basic_ObjectType == null)
        {
            Debug.LogError("basic_ObjectType is null!");
            return null;
        }
        if (basic_ObjectType.prefab == null)
        {
            Debug.LogError("basic_ObjectType.prefab is null!");
            return null;
        }
        if (woodStructurePosition == null)
        {
            Debug.LogError("floorEdgePosition is null!");
            return null;
        }

        if (woodStructurePosition.isHave == true) return null;
        Transform newTransform = Instantiate(basic_ObjectType.prefab.transform, woodStructurePosition.transform.position, woodStructurePosition.transform.rotation);
        // roofTrussPosition.isHave = true;
        woodStructurePosition.isHave = true;
        WoodStructureObject woodStructureObject = newTransform.GetComponent<WoodStructureObject>();
        return woodStructureObject;


    }


    public void PlaceWoodStructureObject(AnglePosition woodStructurePosition, Basic_ObjectTypeISO basic_ObjectType, out WoodStructureObject woodStructureObject) {

        
        woodStructureObject = WoodStructureObject.Create( woodStructurePosition, basic_ObjectType);


    }

    public void OnConnectedToServer(float eluess)
    {
        switch (eluess)
        {
            case 0: dir = PlacedObjectISO.Dir.Left; break;

            case 90: dir = PlacedObjectISO.Dir.Up; break;
            case 180: dir= PlacedObjectISO.Dir.Right;break;
            case 270: dir=PlacedObjectISO.Dir.Down; break;

        }


        // up la di len  down la xuong  right la sang phai left la sang trai
    }

    /*public List<Vector2Int> RotatePoints90CW(List<Vector2Int> points)
    {
        List<Vector2Int> rotatedPoints = new List<Vector2Int>();

        if (points == null || points.Count == 0) return rotatedPoints;

        // 1. Tìm điểm thấp nhất
        int minX = points.Min(p => p.x);
        int minY = points.Min(p => p.y);
        Vector2Int pivot = new Vector2Int(minX, minY);

        // 2. Xoay quanh pivot
        foreach (var point in points)
        {
            // Đưa về gốc
            int relativeX = point.x - pivot.x;
            int relativeY = point.y - pivot.y;

            // Xoay 90 độ theo chiều kim đồng hồ: (x, y) → (y, -x)
            int rotatedX = relativeY;
            int rotatedY = -relativeX;

            // Chuyển về lại tọa độ thật
            Vector2Int rotatedPoint = new Vector2Int(rotatedX + pivot.x, rotatedY + pivot.y);
            rotatedPoints.Add(rotatedPoint);
        }

        return rotatedPoints;
    }*/

    public List<Vector2Int> RotatePoints90CW(List<Vector2Int> points, float angleY)
    {
        List<Vector2Int> rotatedPoints = new List<Vector2Int>();

        if (points == null || points.Count == 0) return rotatedPoints;

        // Chuẩn hóa góc về 0 - 360
        angleY = (angleY + 360) % 360;

        Vector2Int pivot;

        // Chọn pivot theo từng góc
        if (Mathf.Abs(angleY - 0f) < 5f)
        {
            // Góc ~0 → X lớn nhất
            int maxX = points.Max(p => p.x);
            pivot = points.First(p => p.x == maxX);
        }
        else if (Mathf.Abs(angleY - 90f) < 5f)
        {
            // Góc ~90 → Y nhỏ nhất
            int minY = points.Min(p => p.y);
            pivot = points.First(p => p.y == minY);
        }
        else if (Mathf.Abs(angleY - 180f) < 5f)
        {
            // Góc ~180 → X nhỏ nhất
            int minX = points.Min(p => p.x);
            pivot = points.First(p => p.x == minX);
        }
        else if (Mathf.Abs(angleY - 270f) < 5f)
        {
            // Góc ~270 → Y lớn nhất
            int maxY = points.Max(p => p.y);
            pivot = points.First(p => p.y == maxY);
        }
        else
        {
            // Không trùng góc nào: mặc định lấy (0,0) hoặc điểm đầu tiên
            pivot = points[0];
        }

        // Xoay quanh pivot
        foreach (var point in points)
        {
            int relativeX = point.x - pivot.x;
            int relativeY = point.y - pivot.y;

            // Xoay 90 độ CW: (x, y) → (y, -x)
            int rotatedX = relativeY;
            int rotatedY = -relativeX;

            // Đưa về lại pivot
            Vector2Int rotatedPoint = new Vector2Int(rotatedX + pivot.x, rotatedY + pivot.y);
            rotatedPoints.Add(rotatedPoint);
        }

        return rotatedPoints;
    }

    public List<Vector2Int> NextPoint(List<Vector2Int> points, float angleY)
    {
        Vector2Int offset = Vector2Int.zero;

        if (Mathf.Approximately(angleY, 270))
        {
            offset = new Vector2Int(0, -6);
        }
        else if (Mathf.Approximately(angleY, 90))
        {
            offset = new Vector2Int(0, 6);
        }
        else if (Mathf.Approximately(angleY, 180))
        {
            offset = new Vector2Int(6, 0);
        }
        else if (Mathf.Approximately(angleY, 0))
        {
            offset = new Vector2Int(-6, 0);
        }

        List<Vector2Int> newPoints = new List<Vector2Int>();

        foreach (var point in points)
        {
            // Chỉ tạo giá trị mới, không ảnh hưởng đến list gốc
            newPoints.Add(point + offset);
        }

        return newPoints;
    }



}
