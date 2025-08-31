using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemContainer : MonoBehaviour, IItemContainer
{
    // Start is called before the first frame update

    public List<ItemSlot> ItemSlots;

    public virtual bool CanAddItem(Item item, int amount = 1)
    {

        foreach(ItemSlot slot in ItemSlots)
        {
            if(slot.Item == null)
            {
                return default;
            }
        }
        return false;
    }


    public virtual bool AddItem(Item item)
    {
        return default;
    }
    public virtual Item RemoveItem(string itemID)
    {return null;

    }
    public virtual bool RemoveItem(Item item)
    {
        return default(Item);
    }

    public virtual void Clear()
    {
        
    }
    public virtual int ItemCount(string itemID)
    {
        return 10;    
    }

}
