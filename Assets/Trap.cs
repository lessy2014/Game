using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : DialogTrigger
{

    public Transform spawnPoint;
    public Transform spawnPoint2;
    public Random_warrior warrior;

    public override IEnumerator PlayDialog()
    {
        var warrior1 = Instantiate(warrior, spawnPoint.position, Quaternion.identity);
        warrior1.homePoint = spawnPoint;
        warrior1.randomModifier = 1;
        DisplayText(warrior1.gameObject, "Бросай меч!", 1);
        yield return new WaitForSeconds(1.2f);
        DisplayText(player, "Блок! R! Точно!", 1);
        Time.timeScale = 0f;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.8f);
        DisplayText(player, "Было близко... А теперь ответный удар!", 1);
        Time.timeScale = 0f;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        Time.timeScale = 1f;
        var warrior2 = Instantiate(warrior, spawnPoint2.position, Quaternion.identity);
        warrior2.homePoint = spawnPoint2;
        DisplayText(warrior2.gameObject, "Мыкола!", 1);
        DisplayText(player, "Еще один сзади?! Никак вы не научитесь", 1);
        yield return new WaitUntil(() => warrior2.hp <= 0);
        var warrior3 = Instantiate(warrior, spawnPoint.position, Quaternion.identity);
        var warrior4 = Instantiate(warrior, spawnPoint2.position, Quaternion.identity);
        warrior3.homePoint = spawnPoint;
        warrior4.homePoint = spawnPoint2;
        warrior3.hp = 250;
        warrior4.hp = 250;
        DisplayText(warrior3.gameObject, "С нами тебе точно не справится!", 1);
        DisplayText(warrior4.gameObject, "Чур, сапоги мои!", 1);
        DisplayText(player, "А я как раз разогрелся, узрите гнев Империи!", 3);
        yield return new WaitUntil(() => warrior3.hp <= 0 && warrior4.hp <= 0);
        DisplayText(player, "Ох...Интересно, были ли они Поражёнными? А, может, это обычные разбойники? Уже неважно", 5);
        Player.Instance.SetRageModifierAfterScene();
        yield return new WaitForSeconds(5f);
    }
}
