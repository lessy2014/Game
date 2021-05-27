using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterKillDialogue : DialogTrigger
     {
         public TrashMonster trashMonster;
         public override IEnumerator PlayDialog()
         {
             yield return new WaitUntil(() => trashMonster.hp <= 0);
             DisplayText(player, "Как с манекеном! Но неужели Конец времён и в правду близится? Надо поторопиться - с ордой мне не справится", 5);
             yield return new WaitForSeconds(5f);
         }
}
