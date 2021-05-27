using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownDialogue : DialogTrigger
{
    public override IEnumerator PlayDialog()
    {
        mage = Koldun.Instance.gameObject;
        player = Player.Instance.gameObject;
        DisplayText(player, "Пожар! Нульн горит!",2);
        DisplayText(mage.gameObject, "Опоздали... Поможем защитникам", 2);
        yield return new WaitForSeconds(2f);
    }
}
