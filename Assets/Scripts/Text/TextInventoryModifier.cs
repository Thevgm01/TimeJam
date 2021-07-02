using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInventoryModifier : ITextInventory, ITextDisplayable
{
    public enum Operation
    {
        Add,
        Remove
    }

    string item;
    Inventory inventory;
    Operation operation;

    public string GetText()
    {
        switch (operation)
        {
            case Operation.Add: return "You have gained " + item;
            case Operation.Remove: return "You have lost " + item;
        }
        return "";
    }

    public void ModifyInventory()
    {
        switch (operation)
        {
            case Operation.Add: inventory.AddItem(item); return;
            case Operation.Remove: inventory.RemoveItem(item); return;
        }
    }

    public TextInventoryModifier(Inventory inventory, string item, Operation operation)
    {
        this.inventory = inventory;
        this.item = item;
        this.operation = operation;
    }
}
