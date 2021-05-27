using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class FollowTrigger : DialogTrigger
{

    public Support follower;
    public bool isTriggered;
    public Transform spawnPos;
    public TrashMonster trash;

    public override IEnumerator PlayDialog()
    {
        follower.isFollowPlayer = true;
        follower.SetToPlayerPosition();
        archer = Archer.Instance.gameObject;
        mage = Koldun.Instance.gameObject;
        player = Player.Instance.gameObject;
        DisplayText(archer, "Маркус? Не ожидала тебя тут увидеть после того случая", 2);
        yield return new WaitForSeconds(2f);
        DisplayText(player, "Не сейчас, Зираэль. Мы с миссией, какого состояние гарнизона?", 2);
        DisplayText(mage, "Хохо", 1);
        yield return new WaitForSeconds(3f);
        DisplayText(archer, "Нет его. Ничего больше нет. Надо оставлять город", 2);
        yield return new WaitForSeconds(2f);
        DisplayText(mage, "Быстро же мы сдались...", 1);
        yield return new WaitForSeconds(1f);
        for (var i = 0; i < 80; i++)
        {
            var singleTrash = Instantiate(trash, spawnPos.position, Quaternion.identity);
            singleTrash.homePoint = player.transform;
            singleTrash.hp = 1000;
        }
        yield return new WaitForSeconds(2f);
        DisplayText(mage, "Орда! Нам с ней не справиться!", 2);
        DisplayText(player, "О, нет-НеТ-НЕТ-НЕТ, бежим!", 2);
        // DisplayText(mage, "Орда! Нам с ней не справиться!", 2);
        DisplayText(archer, "Да на них стрел не хватит!", 2);
        yield return new WaitForSeconds(2f);
    }
}
