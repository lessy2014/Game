using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetArcherDialogue : DialogTrigger
{
    public override IEnumerator PlayDialog()
    {
        archer = Archer.Instance.gameObject;
        mage = Koldun.Instance.gameObject;
        player = Player.Instance.gameObject;
        DisplayText(player, "Она выглядит знакомо...",2);
        DisplayText(mage, "Впереди лучница! Поддержим ее", 2);
        yield return new WaitForSeconds(2f);
    }
}
