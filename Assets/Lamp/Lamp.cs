using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] private Animator Animator;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(12.5f, 42.5f));
            Animator.SetTrigger("Flicker");
        }
    }

    public void Flicker()
    {
        Animator.SetTrigger("Flicker");
    }
}