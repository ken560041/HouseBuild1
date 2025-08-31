using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class FloorEdgeObjectTypeISO : ScriptableObject
{
    // Start is called before the first frame update


    public GameObject prefab;
    public GameObject visual;

    [Header("Đây là chiều ngang")]
    public int height;

    [Header("Chiều dọc")]
    public int width;

    
}
