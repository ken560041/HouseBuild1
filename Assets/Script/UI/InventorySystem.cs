using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }
    public List<Item> items = new List<Item>();

    public GameObject InventoryItem;
    public Transform ItemConnect; //

    [SerializeField] ItemSlot[] itemSlots;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /*public void Add(Item item)
    {
        items.Add(item);
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }


    public void ListItem()
    {
        *//*foreach(Transform item in ItemConnect)
        {
            Destroy(item.gameObject);
        }

        foreach(var item in items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemConnect);
            var itemName = obj.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite=item.icon;
        }*//*
    }*/

    private void OnValidate()
    {
        if (ItemConnect != null)
        {
            itemSlots = ItemConnect.GetComponentsInChildren<ItemSlot>();


        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        int i = 0;
        for (; i < items.Count&& i<itemSlots.Length; i++)
        {
            itemSlots[i].Item = items[i];
        }
        for(;i<itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }
    }

    public bool AddItem(Item item)
    {
        if (isFull())
        {
            return false;

        }
        items.Add(item);
        RefreshUI();
        return true;
    }

    public bool RemoveItem(Item item) {
        if (items.Remove(item))
        {
            RefreshUI();
            return true;
        }
        return false;
     
    }
    public bool isFull()
    {
        return items.Count >= itemSlots.Length;
    }


}