using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhrase : DialogTrigger
{
    public override IEnumerator PlayDialog()
    {
        mage = Koldun.Instance.gameObject;
        player = Player.Instance.gameObject;
        archer = Archer.Instance.gameObject;
        DisplayText(player, "������ ���-�� ��������� � ���� ������...", 4);
        yield return new WaitForSeconds(4f);
        DisplayText(archer, "� ����� �� ������ ������� ���", 3);
    }
}
