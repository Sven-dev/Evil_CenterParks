using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Determines what objects are interactable based on what the player is looking at.
/// </summary>
public class Perspective : MonoBehaviour
{
    public bool Active { get; private set; } = false;
    [SerializeField] private int ID;
    [SerializeField] private UnityVoidEvent OnPerspectiveActivate;
    [SerializeField] private UnityVoidEvent OnPerspectiveDeactivate;

    public virtual void Toggle(int cameraPerspective)
    {
        if (cameraPerspective == ID)
        {
            Active = true;
            OnPerspectiveActivate?.Invoke();
        }
        else
        {
            Active = false;
            OnPerspectiveDeactivate?.Invoke();
        }
    }
}