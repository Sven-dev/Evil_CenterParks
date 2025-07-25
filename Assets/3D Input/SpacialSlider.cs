using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacialSlider : MonoBehaviour
{
    public bool Interactable = true;
    [Range(0f, 1f)]
    [SerializeField] private float Value = 0;
    [Space]
    [SerializeField] private Transform Background;
    [SerializeField] private Transform Handle;
    [SerializeField] private Transform EmptyPivot;
    [SerializeField] private Transform FullPivot;
    [Space]
    public UnityFloatEvent OnValueChanged;
    public UnityVoidEvent OnGrabbed;
    public UnityVoidEvent OnLetGo;

    public float value
    {
        get { return Value; }
        set
        {
            Value = (float)System.Math.Round(value, 2);
            SetHandlePosition();
        }
    }

    private bool grabbed = false;


    private void OnMouseDown()
    {
        if (Interactable)
        {
            grabbed = true;
            UpdateHandlePosition();
            OnGrabbed?.Invoke();
        }
    }

    private void OnMouseDrag()
    {
        if (Interactable)
        {
            UpdateHandlePosition();
        }
        else if (grabbed)
        {          
            OnMouseUp();
            grabbed = false;
        }
    }

    private void OnMouseUp()
    {
        UpdateHandlePosition();
        OnLetGo?.Invoke();       
    }

    /// <summary>
    /// Update the position of the handle based on the mouse position.
    /// </summary>
    private void UpdateHandlePosition()
    {
        //Determine position of slider based on cursor position.
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float depth = 0;
        if (Physics.Raycast(ray, out hit))
        {
            depth = hit.distance;
        }

        //Clamp the handle between the minimum and maximum values (localspace to make it independent of orientation)
        Vector3 cursorPos = ray.GetPoint(depth);
        cursorPos = Background.InverseTransformPoint(cursorPos);
        Handle.localPosition = new Vector3(
            Handle.localPosition.x,
            Handle.localPosition.y,
            Mathf.Clamp(cursorPos.z, FullPivot.localPosition.z, EmptyPivot.localPosition.z));

        //Calculate new slider value
        float distanceBetweenStartingPoints = Vector3.Distance(FullPivot.position, EmptyPivot.position);
        float distanceBetweenObjects = Vector3.Distance(FullPivot.position, Handle.position);
        float percentage = (float)System.Math.Round(1 - distanceBetweenObjects / distanceBetweenStartingPoints, 2);
        if (percentage != Value)
        {
            Value = percentage;
            OnValueChanged?.Invoke(Value);
        }      
    }

    /// <summary>
    /// Set the position of the handle based on the value of the slider.
    /// </summary>
    private void SetHandlePosition()
    {
        Handle.localPosition = new Vector3(
            Handle.localPosition.x,
            Handle.localPosition.y,
            Mathf.Lerp(EmptyPivot.localPosition.z, FullPivot.localPosition.z, Value));
            
        OnValueChanged?.Invoke(Value);
    }

    private void OnValidate()
    {
        SetHandlePosition();
    }
}