using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenTreeDialogue : DialogTrigger
{
    
    public override IEnumerator PlayDialog()
    {
        DisplayText(player, "Думаю, я могу пробраться под деревом на Shift", 3);
        yield return new WaitForSeconds(3);
    }
}