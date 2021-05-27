using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMonsterDialogue2 : DialogTrigger
{
    public Transform spawnPos;
    public TrashMonster trashMonster;
    public override IEnumerator PlayDialog()
    {
        mage = Koldun.Instance.gameObject;
        yield return new WaitForSeconds(0.1f);
        var trash = Instantiate(trashMonster, spawnPos.position, Quaternion.Euler(0, 180, 0));
        trash.hp = 50;
        trash.homePoint = spawnPos;
        DisplayText(mage, "А вот что! Они уже здесь! Сейчас я прицелюсь Мышью и справлюсь с этим своим заклинанием на F", 1);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
        Time.timeScale = 1;
        yield return new WaitForSeconds(1f);
        if (trash.hp <= 0)
            DisplayText(mage, "Точно в цель! А теперь бежим", 2);
        else
            DisplayText(mage, "Промах! Преломление! Марк, защити меня!", 2);
        yield return new WaitForSeconds(2f);
    }
}
