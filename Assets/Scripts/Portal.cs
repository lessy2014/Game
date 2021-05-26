using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Animator animator;
    public  readonly int IsClosed = Animator.StringToHash("isClosed");
    private static readonly int IsOpened = Animator.StringToHash("isOpened");
    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        animator.SetBool(IsClosed, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        animator.SetBool(IsClosed, false);
    }
}
