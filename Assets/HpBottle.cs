using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBottle : MonoBehaviour
{
    public bool isFull;
    public Sprite fullBottle;
    public Sprite emptyBottle;
    public Image bottleImage;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        bottleImage = this.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        isFull = player.isHpBottleFull;
        if (player.isHpBottleFull)
        {
            print('f');
            bottleImage.sprite = fullBottle;
        }
            
        else
        {
            print('e');
            bottleImage.sprite = emptyBottle;
        }            
    }
}
