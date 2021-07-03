using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMajorChoice : TextMinorChoice
{
    Inventory inventory;

    public TextMajorChoice(Inventory inventory)
    {
        this.inventory = inventory;
    }
}
