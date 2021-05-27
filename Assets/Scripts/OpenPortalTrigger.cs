using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPortalTrigger : DialogTrigger
{
    public Portal portal;
    // Start is called before the first frame update

    public override IEnumerator PlayDialog()
    {
        portal.Open();
        archer = Archer.Instance.gameObject;
        mage = Koldun.Instance.gameObject;
        player = Player.Instance.gameObject;
        DisplayText(player, "Вейгар, портал!",2);
        DisplayText(mage, "Знаю я! Знаю! Сейчас!", 2);
        yield return new WaitForSeconds(2f);
    }
}
