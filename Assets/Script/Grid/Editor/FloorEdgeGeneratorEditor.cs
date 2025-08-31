using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(FloorEdgePosition))]
public class FloorEdgeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FloorEdgePosition edgePosition = (FloorEdgePosition)target;

        if (GUILayout.Button("Generate Edge Points"))
        {
            GenerateEdgePoints(edgePosition);
        }
    }

    private void GenerateEdgePoints(FloorEdgePosition edgePosition)
    {
        // Xóa các điểm cạnh cũ
        for (int i = edgePosition.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(edgePosition.transform.GetChild(i).gameObject);
        }

        float gridSize = 0.5f;

        // Lấy kích thước nền từ collider
        Collider floorCollider = edgePosition.transform.root.GetComponentInChildren<Collider>();
        if (floorCollider == null) return;

        Vector3 size = floorCollider.bounds.size;
        int count = (edgePosition.edge == FloorPlacedObject.Edge.Up || edgePosition.edge == FloorPlacedObject.Edge.Down)
            ? Mathf.RoundToInt(size.x / gridSize)
            : Mathf.RoundToInt(size.z / gridSize);

        // Vị trí gốc của cạnh
        Vector3 basePos = edgePosition.transform.position;

        for (int i = 0; i < count; i++)
        {
            // Tạo đối tượng con cho điểm cạnh
            GameObject point = new GameObject("EdgePos_" + i);
            point.transform.SetParent(edgePosition.transform);
            point.transform.localPosition = GetOffset(edgePosition.edge, i, gridSize);

            // Thêm BoxCollider
            BoxCollider boxCollider = point.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(gridSize, 0.2f, gridSize);  // Kích thước collider
            boxCollider.isTrigger = true;                              // Đặt thành Trigger

            // Điều chỉnh center dựa trên hướng
            if (edgePosition.edge == FloorPlacedObject.Edge.Up || edgePosition.edge == FloorPlacedObject.Edge.Down)
            {
                boxCollider.center = new Vector3(gridSize / 2, 0.1f, 0);  // Cạnh ngang
            }
            else
            {
                boxCollider.center = new Vector3(0, 0.1f, gridSize / 2);  // Cạnh dọc
            }
        }
    }

    private Vector3 GetOffset(FloorPlacedObject.Edge edge, int i, float gridSize)
    {
        switch (edge)
        {
            case FloorPlacedObject.Edge.Down: return new Vector3(i * gridSize, 0, 0);     // +X
            case FloorPlacedObject.Edge.Left: return new Vector3(0, 0, -i * gridSize);    // -Z
            case FloorPlacedObject.Edge.Up: return new Vector3(-i * gridSize, 0, 0);    // -X
            case FloorPlacedObject.Edge.Right: return new Vector3(0, 0, i * gridSize);     // +Z
            default: return Vector3.zero;
        }
    }
}
