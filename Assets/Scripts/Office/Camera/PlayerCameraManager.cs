using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OfficePerspective
{
    RadioShock = 0,
    Pc = 1,
    Shutter = 2,
    Modem = 3
}

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] private OfficePerspective CurrentPerspective;
    [SerializeField] private Camera Camera;
    [SerializeField] private float Duration;
    [SerializeField] private AnimationCurve MovementCurve;
    [SerializeField] private List<Transform> Perspectives;
    [Space]
    [SerializeField] private GameObject LeftButton;
    [SerializeField] private GameObject RightButton;
    [Space]
    [SerializeField] private UnityIntEvent OnCameraPerpectiveChange;

    private bool Moving = false;
    private bool RadioAccessible = true;

    //Debug
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveToPerspective(OfficePerspective.Shutter);
        }*/
    }

    public void MovePerspective(int direction)
    {
        if (!Moving)
        {
            MoveToPerspective(CurrentPerspective + direction);
        }  
    }

    public void MoveToPerspective(OfficePerspective newPerspective)
    {
        StartCoroutine(_Move(CurrentPerspective, newPerspective));

        CurrentPerspective = newPerspective;
        OnCameraPerpectiveChange?.Invoke((int)CurrentPerspective);
        UpdateControls();
    }

    public void RemoveRadioAccess()
    {
        RadioAccessible = false;
        if (CurrentPerspective == OfficePerspective.Shutter)
        {
            RightButton.SetActive(false);
        }
        else if (CurrentPerspective == OfficePerspective.Modem)
        {
            MoveToPerspective(OfficePerspective.Shutter);
        }
    }

    private IEnumerator _Move(OfficePerspective from, OfficePerspective to)
    {
        Moving = true;

        Vector3 startPosition = Perspectives[(int)from].position;
        Vector3 endPosition = Perspectives[(int)to].position;

        Quaternion startRotation = Perspectives[(int)from].rotation;
        Quaternion endRotation = Perspectives[(int)to].rotation;

        float progress = 0;
        while (progress < 1)
        {
            progress = Mathf.Clamp01(progress += Time.deltaTime / Duration);
            Camera.transform.position = Vector3.Lerp(startPosition, endPosition, MovementCurve.Evaluate(progress));
            Camera.transform.rotation = Quaternion.Lerp(startRotation, endRotation, MovementCurve.Evaluate(progress));
            yield return null;
        }

        Moving = false;
    }

    /// <summary>
    /// Enables or disables certain buttons based on where the player is looking.
    /// </summary>
    private void UpdateControls()
    {
        LeftButton.SetActive(true);
        RightButton.SetActive(true);
       
        if ((int)CurrentPerspective == 0)
        {
            //If perspective is the leftmost one, disable the left button
            LeftButton.SetActive(false);
        }      
        else if ((int)CurrentPerspective == Perspectives.Count -1)
        {
            //If perspective is the rightmost one, disable the right button
            RightButton.SetActive(false);
        }
        else if (CurrentPerspective == OfficePerspective.Shutter && !RadioAccessible)
        {
            RightButton.SetActive(false);
        }
    }
}