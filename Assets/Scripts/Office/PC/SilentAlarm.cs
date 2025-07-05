using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilentAlarm : MonoBehaviour
{
    [SerializeField] private Perspective Perspective;
    [SerializeField] private float CooldownTime = 1f;
    [Space]
    [SerializeField] private CameraSwitcher CameraTracker;
    [SerializeField] private UnityVoidEvent OnBuzzerSound;

    private bool OnCooldown = false;

    public void Update()
    {
        if (Perspective.Active && !OnCooldown)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                BuzzVial();
            }
        }
    }

    public void BuzzVial()
    {
        if (RoomController.Instance.GetRoom(CameraTracker.ActiveCamera) == RoomController.Instance.FindEntity(EntityType.Vial))
        {
            Entities.Instance.Vial.KickOutOfOffice();
        }
        OnBuzzerSound?.Invoke();
        OnCooldown = true;
        Invoke("CooldownDone", CooldownTime);
    }

    private void CooldownDone()
    {
        OnCooldown = false;
    }
}