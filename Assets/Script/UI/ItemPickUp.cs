using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour,IInteractable
{
    // Start is called before the first frame update
    public Item item;

    void PickUp()
    {
        Destroy(gameObject);
        InventorySystem.Instance.AddItem(item);
        
    }

    public void Interact()
    {
        PickUp();
    }

    public string GetInteractText()
    {
        return  item.itemName;
    }

    /*private void (Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            PickUp();
        }
    }*/

}
