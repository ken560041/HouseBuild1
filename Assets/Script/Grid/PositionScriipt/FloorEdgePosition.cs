using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorEdgePosition : MonoBehaviour
{
    public FloorPlacedObject.Edge edge;
    public bool isHave;

    private BoxCollider boxCollider;
    public List<FloorChildEdgePosition> floorChildEdgePositions;
    public GameObject floorChildEdgePrefab;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        floorChildEdgePositions = new List<FloorChildEdgePosition>();
        CreateFloorChildEdges();
    }

    public Vector3 GetBoxColliderSize()
    {
        if (boxCollider != null)
        {
            return boxCollider.size;
        }
        return Vector3.zero;
    }

    public void PlaceEdgeChildSize()
    {

    }


    private void CreateFloorChildEdges()
    {
        if (floorChildEdgePrefab == null) return;

        int count = Mathf.FloorToInt(boxCollider.size.x / 0.5f);

        for (int i = 0; i < count; i++)
        {
            GameObject child = Instantiate(floorChildEdgePrefab, transform);
            child.name = $"FloorChildEdge_{i}";

            Vector3 position = Vector3.zero;
            switch (edge)
            {
                case FloorPlacedObject.Edge.Left:
                    position = new Vector3(i * 0.5f, 0, 0);
                    break;
                case FloorPlacedObject.Edge.Right:
                    position = new Vector3(i * 0.5f, 0, 0);
                    break;
                case FloorPlacedObject.Edge.Up:
                    position = new Vector3(i * 0.5f, 0, 0);
                    break;
                case FloorPlacedObject.Edge.Down:
                    position = new Vector3(i * 0.5f, 0, 0);
                    break;
            }
            child.transform.localPosition = position;

            BoxCollider childCollider = child.GetComponent<BoxCollider>();
            if (childCollider != null)
            {
                childCollider.size = new Vector3(0.5f, boxCollider.size.y, boxCollider.size.z);
            }

            FloorChildEdgePosition childEdgePosition = child.GetComponent<FloorChildEdgePosition>();
            if (childEdgePosition != null)
            {
                floorChildEdgePositions.Add(childEdgePosition);
            }
        }
    }
}
