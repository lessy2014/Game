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
        DisplayText(player, "������, ���� �� ��� �������?", 2);
        yield return new WaitForSeconds(2f);
        DisplayText(mage, "�� � �����-����. \n ����� 40 ����� � ����� � ���� ������ �����", 2);
        yield return new WaitForSeconds(2f);
        DisplayText(player, "�����", 2);
        DisplayText(archer, "40 �����?", 2);
        yield return new WaitForSeconds(2f);
        DisplayText(mage, "�� �� ������ �������... \n �, ������� ��� �����, ���� � �� ����������� ���� ��� ������������", 4);
        

    }
}
