using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Range(1, 20)]
    [SerializeField] protected int AILevel = 10;
    [Range(1, 10)]
    [SerializeField] protected float MovementOpportunityCooldown = 4.9f;
    [SerializeField] public bool NoiseMaker = false;
    [SerializeField] protected Room CurrentRoom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}