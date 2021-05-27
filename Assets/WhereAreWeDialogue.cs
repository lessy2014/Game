using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhereAreWeDialogue : DialogTrigger
{
    public GameObject zoomObject;
    public override IEnumerator PlayDialog()
    {
        mage = Koldun.Instance.gameObject;
        player = Player.Instance.gameObject;
        archer = Archer.Instance.gameObject;
        yield return new WaitForSeconds(3f);
        DisplayText(player, "Вот, зараза", 3);
        DisplayText(archer, "Мага, где мы?", 3);
        yield return new WaitForSeconds(3f);
        DisplayText(mage, "Вообще-то мое имя...", 2);
        yield return new WaitForSeconds(2f);
        DisplayText(archer, "Мне все равно, вопрос бы...", 2);
        yield return new WaitForSeconds(2f);
        DisplayText(mage, "...Вейгар", 2);
        yield return new WaitForSeconds(2f);
        DisplayText(player, "Успокойтесь. Вейгар, где мы?", 2);
        yield return new WaitForSeconds(2f);
        DisplayText(mage, "Судя по знамёнам на замке, это Кислев", 3);
        yield return new WaitForSeconds(3f);
        DisplayText(archer, "Замка?", 2);
        DisplayText(player, "Охтыж...", 2);
        yield return new WaitForSeconds(2f);
        CameraController.Instance.objectInFocus = zoomObject;
        CameraController.Instance.isFocused = true;
        while(CameraController.Instance.camera.orthographicSize < 10)
            CameraController.Instance.camera.orthographicSize += Time.deltaTime;
        yield return new WaitForSeconds(5f);
        DisplayText(mage, "По-видимому, нам его не обойти...",3 );
        yield return new WaitForSeconds(3f);
        DisplayText(archer, "А зная нрав кислевитов, вести переговоры с ними не хочется", 4);
        yield return new WaitForSeconds(4f);
        DisplayText(player, "Это если кто-то жив, а говорят, Поражение началось как раз с севера", 4);
        yield return new WaitForSeconds(4f);

    }
}
