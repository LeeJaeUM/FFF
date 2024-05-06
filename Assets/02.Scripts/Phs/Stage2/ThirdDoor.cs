using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdDoor : MonoBehaviour
{
    Animator animator;

    readonly int Open_Hash = Animator.StringToHash("open");

    private bool isOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnOpenClose()
    {
        animator.SetBool(Open_Hash, isOpen);
    }
}
