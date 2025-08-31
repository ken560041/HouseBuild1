using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseObject : MonoBehaviour
{
    // Start is called before the first frame update
    public LooseObjectISO looseObjectISO;



    public static LooseObject Create(LooseObjectISO looseObjectISO, Vector3 position, Vector3 rotation)
    {
        Transform looseObjectTransform = Instantiate(looseObjectISO.prefab.transform, position, Quaternion.Euler(rotation));
        LooseObject looseObject= looseObjectTransform.GetComponent<LooseObject>();
        return looseObject;
    }



    public void DestroyMySelf()
    {
        Destroy(this.gameObject);
    }


    

    [System.Serializable]
    public class LooseSaveObject
    {

        public string looseObjectSOName;
        public Vector3 position;
        public Quaternion quaternion;

    }
}
