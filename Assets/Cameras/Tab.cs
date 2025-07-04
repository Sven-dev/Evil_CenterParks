using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab : MonoBehaviour
{
    [SerializeField] private bool Maximized = false;
    [SerializeField] private Animator Animator;

    private void Start()
    {
        Maximize();
    }

    public void Maximize()
    {
        if (!Maximized)
        {
            Maximized = true;
            Animator.Play("Maximize");
        }
    }

    public void Minimize()
    {
        if (Maximized)
        {
            Maximized = false;
            Animator.Play("Minimize");
        }
    }
}
