using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TextInventoryModifier : ITextInventory, ITextDisplayable
{
    protected string item;
    protected Inventory inventory;

    public abstract string GetText();
    public abstract void ModifyInventory();

    public TextInventoryModifier(Inventory inventory, string item)
    {
        this.inventory = inventory;
        this.item = item;
    }
}
