using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMonsterDialogue : DialogTrigger
{
    public override IEnumerator PlayDialog()
    {
        DisplayText(player, "Уже здесь? Хм, он всего один и ведет себя странно. Думаю, я легко справлюсь с ним, многократно нажимая на E", 3);
        yield return new WaitForSeconds(3f);
    }
}
