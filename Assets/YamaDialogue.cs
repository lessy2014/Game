using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YamaDialogue : DialogTrigger
{
    public override IEnumerator PlayDialog()
    {
        DisplayText(player, "Вот зараза", 1);
        yield return new WaitForSeconds(1.1f);
        DisplayText(player, "С тех камней я мог бы запрыгнуть наверх с помощью W или Space", 3);
        yield return new WaitForSeconds(3f);
    }
}
