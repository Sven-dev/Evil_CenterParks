using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entities : MonoBehaviour
{
    public static Entities Instance;

    public Cork Cork;
    public Vial Vial;
    public Abhorwretch Abhorwretch;
    public Shade Shade;
    public Hallucination Hallucination;
    public Wanderer Wanderer;

    private void Awake()
    {
        Instance = this;
    }

    public void IncreaseBloodLust()
    {
        Cork.Bloodlust++;
        Vial.Bloodlust++;
        Abhorwretch.Bloodlust++;
        Shade.Bloodlust++;
        Hallucination.Bloodlust++;
        Wanderer.Bloodlust++;
    }
}