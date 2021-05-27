using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class OpenPortalJabi : DialogTrigger
{
    public Portal portal;
    // Start is called before the first frame update

    public override IEnumerator PlayDialog()
    {
        portal.Open();
        archer = Archer.Instance.gameObject;
        mage = Koldun.Instance.gameObject;
        player = Player.Instance.gameObject;
        DisplayText(player, "Вейгар, куда ты нас закинул?", 2);
        yield return new WaitForSeconds(2f);
        DisplayText(mage, "Мы в карак-ыбаж. \n Здесь 40 часов в сутки с неба падают форги", 2);
        yield return new WaitForSeconds(2f);
        DisplayText(player, "Форги", 2);
        DisplayText(archer, "40 часов?", 2);
        yield return new WaitForSeconds(2f);
        DisplayText(mage, "Мы на другой планете... \n А, неважно идём вперёд, пока я не восстановлю силы для телепортации", 4);
        

    }
}
