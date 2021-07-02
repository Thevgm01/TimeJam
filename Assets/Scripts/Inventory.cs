using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    List<string> items;

    public void AddItem(string item)
    {
        items.Add(item);
    }

    public void RemoveItem(string item)
    {
        items.Remove(item);
    }
}
