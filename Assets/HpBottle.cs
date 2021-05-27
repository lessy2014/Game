using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBottle : MonoBehaviour
{
    public bool isFull;
    public Sprite fullBottle;
    public Sprite emptyBottle;
    Image bottleImage;
    // Start is called before the first frame update
    void Start()
    {
        bottleImage = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.Instance.isHpBottleFull)
            bottleImage.sprite = fullBottle;
        else
            bottleImage.sprite = emptyBottle;
            
    }
}
