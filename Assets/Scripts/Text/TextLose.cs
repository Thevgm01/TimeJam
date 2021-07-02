using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLose : TextInventoryModifier
{
    public override string GetText()
    {
        return "You have lost " + item;
    }

    public override void ModifyInventory()
    {
        inventory.RemoveItem(item);
    }

    public TextLose(Inventory inventory, string item) : base(inventory, item) { }
}
