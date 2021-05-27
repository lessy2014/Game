using System.Collections;
using System.Collections.Generic;
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
        DisplayText(mage, "Мы в карак-ыбаж. \n Здесь 40 часов в сутки с неба падают форги", 4);
        yield return new WaitForSeconds(4f);
        DisplayText(player, "Форги?", 3);
        DisplayText(archer, "40 часов?", 3);
        yield return new WaitForSeconds(3f);
        DisplayText(mage, "Мы на другой планете... \n А, неважно идём вперёд, пока я не восстановлю силы для телепортации", 5);
        

    }
}
