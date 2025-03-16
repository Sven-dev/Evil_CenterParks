using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestRoomManager : MonoBehaviour
{
    [SerializeField] private int DisturbanceLevel = 0;
    [Space]
    [SerializeField] private UnityVoidEvent OnIdle;
    [SerializeField] private UnityVoidEvent OnDisturbed;
    [SerializeField] private UnityVoidEvent OnDoorOpen;
    [SerializeField] private UnityVoidEvent OnDoorClose;
    [SerializeField] private UnityVoidEvent OnKill;

    [HideInInspector] public bool BeingDisturbed = false;
    private bool DisturbanceBuilding = false;
    public bool Killed = false;

    /// <summary>
    /// Gets called when the noise level of a room changes.
    /// </summary>
    public void CheckForDisturbance(float noiseLevel)
    {
        if (Killed)
        {
            return;
        }

        if (noiseLevel >= 4 && !DisturbanceBuilding)
        {
            DisturbanceBuilding = true;
            OnDisturbed?.Invoke();
            StartCoroutine(_disturbance());
        }
        else if (noiseLevel < 4)
        {
            DisturbanceBuilding = false;
            OnIdle?.Invoke();
        }
    }

    public void Kill()
    {
        Killed = true;
        OnKill?.Invoke();
        StopAllCoroutines();
    }

    private IEnumerator _disturbance()
    {
        while (DisturbanceBuilding)
        {
            yield return new WaitForSecondsRealtime(2f);
            DisturbanceLevel++;

            if (DisturbanceLevel >= 12)
            {
                //open door
                BeingDisturbed = true;
                OnDoorOpen?.Invoke();

                yield return new WaitForSecondsRealtime(10);

                DisturbanceLevel = 0;
                BeingDisturbed = false;
                OnDoorClose?.Invoke();
            }
        }
    }
}