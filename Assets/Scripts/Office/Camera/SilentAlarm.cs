using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilentAlarm : MonoBehaviour
{
    [SerializeField] private Perspective Perspective;
    [SerializeField] private float CooldownTime = 1f;
    [Space]
    [SerializeField] private CameraSwitcher CameraTracker;

    private bool OnCooldown = false;

    public void OnMouseDown()
    {
        if (Perspective.Active && !OnCooldown)
        {
            if (RoomController.Instance.GetRoom(CameraTracker.ActiveCamera) == RoomController.Instance.FindEntity(EntityType.Vial))
            {
                Entities.Instance.Vial.KickOutOfOffice();
            }

            OnCooldown = true;
            Invoke("CooldownDone", CooldownTime);
        }
    }

    private void CooldownDone()
    {
        OnCooldown = false;
    }
}