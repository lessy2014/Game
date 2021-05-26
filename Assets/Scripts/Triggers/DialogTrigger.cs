using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public GameObject archer;
    public GameObject player;

    private void Start()
    {
        archer = GameObject.Find("Archer");
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        StartCoroutine(PlayDialog());
    }

    private IEnumerator PlayDialog()
    {
        DisplayText(archer, "hello", 3);
        yield return new WaitForSeconds(3);
        DisplayText(player, "hello", 3);
        yield return new WaitForSeconds(3);
        DisplayText(archer, "world", 3);
        yield return new WaitForSeconds(3);
        DisplayText(player, "world", 3);
    }

    private void DisplayText(GameObject target, string text, int duration)
    {
        target.AddComponent(typeof(TextDisplay));
        var component = target.gameObject.GetComponent<TextDisplay>();
        component.text = text;
        component.duration = duration;
    }
}
