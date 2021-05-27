using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhrase : DialogTrigger
{
    public override IEnumerator PlayDialog()
    {
        mage = Koldun.Instance.gameObject;
        player = Player.Instance.gameObject;
        archer = Archer.Instance.gameObject;
        DisplayText(player, "Короля что-то связывает с этим местом...", 4);
        yield return new WaitForSeconds(4f);
        DisplayText(archer, "У трона мы сможем достать его", 3);
    }
}
