using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacialButton : MonoBehaviour
{
    public bool Interactable = true;
    [Space]
    public UnityVoidEvent OnClick;

    private void OnMouseDown()
    {
        if (Interactable)
        {
            OnClick?.Invoke();
        }
    }
}