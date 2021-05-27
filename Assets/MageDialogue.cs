using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageDialogue : DialogTrigger
{
    public Koldun theMage;
    public Transform spawnPos;
    public Transform friendlyCharacters;
    public override IEnumerator PlayDialog()
    {
        var koldun = Instantiate(theMage, spawnPos.position, Quaternion.Euler(0, 180, 0), friendlyCharacters);
        koldun.SetToPlayerPosition();
        koldun.playerPosition = Player.Instance.supportPosition;
        koldun.player = Player.Instance.gameObject.transform;
        koldun.groundCheck = Player.Instance.groundCheck;
        koldun.isFollowPlayer = true;
        mage = Koldun.Instance.gameObject;
        DisplayText(player, "Вейгар? Ты должен был оповестить об угрозе Альтдорф, почему ты здесь?",5);
        yield return new WaitForSeconds(5f);
        DisplayText(mage.gameObject, "Альтдорфа больше нет, и этого поселения скоро тоже не будет. Я помогу тебе, Марк, но мы должны спешить", 5);
        yield return new WaitForSeconds(5f);
        DisplayText(player, "Конечно, но расскажи, что случилось", 3);
        yield return new WaitForSeconds(3f);
    }
}
