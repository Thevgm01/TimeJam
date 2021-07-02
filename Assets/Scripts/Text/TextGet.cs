using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextGet : TextInventoryModifier
{
    public override string GetText()
    {
        return "You have gained " + item;
    }

    public override void ModifyInventory()
    {
        inventory.AddItem(item);
    }

    public TextGet(Inventory inventory, string item) : base(inventory, item) { }
}
