using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private Animator Lamp;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(12.5f, 42.5f));
            Lamp.SetTrigger("Flicker");
        }
    }
}