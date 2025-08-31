using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public struct ItemAmount
{
    public Item item;
    [Range(1,999)]
    public int amount;
}


[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    public List<ItemAmount> materials;
    public List<ItemAmount> resualts;

   public bool CanCraft(IItemContainer itemContainer)
    {
        return HasMaterials(itemContainer);

    }

    private bool HasMaterials(IItemContainer itemContainer)
    {
        foreach(ItemAmount itemAmount in materials)
        {
/*            if (itemContainer.ItemCount(itemAmount.item.id) < itemAmount.amount)
            {

            }*/
        }
        return true;
    }


}
