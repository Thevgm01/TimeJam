using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    List<string> items;

    public Inventory()
    {
        items = new List<string>();
    }

    public void AddItem(string item)
    {
        items.Add(item);
    }

    public void RemoveItem(string item)
    {
        items.Remove(item);
    }

    public Inventory Clone()
    {
        Inventory newInventory = new Inventory();
        foreach (string item in items)
        {
            newInventory.AddItem(item);
        }
        return newInventory;
    }
}
