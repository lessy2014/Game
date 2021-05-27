using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public GameObject archer;
    public GameObject player;
    public GameObject mage;
    public List<GameObject> speakers;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        StartCoroutine(PlayDialog());
    }

    public virtual IEnumerator PlayDialog()
    {
        DisplayText(speakers[0], "Дальше я не поеду", 3);
        yield return new WaitForSeconds(3);
        DisplayText(player, "Ладно, хоть и пешком, передвигаясь на A и D, но я должен доставить послание", 3);
        yield return new WaitForSeconds(3);
    }

    public void DisplayText(GameObject target, string text, int duration)
    {
        var textComponent = target.GetComponent<TextDisplay>();
        if (textComponent == null)
        {
            target.AddComponent<TextDisplay>();
            textComponent = target.gameObject.GetComponent<TextDisplay>();
        }
        // textComponent = target.gameObject.GetComponent<TextDisplay>();
        textComponent.text = text;
        textComponent.duration = duration;
        textComponent.fontSize = 20;
    }
}
