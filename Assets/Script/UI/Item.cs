using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="NewFile",menuName ="Item/Create new item")]
public class Item : ScriptableObject
{
    // Start is called before the first frame update
    public int id;
    public string itemName;
    public int values;
    public Sprite icon;

    

}
