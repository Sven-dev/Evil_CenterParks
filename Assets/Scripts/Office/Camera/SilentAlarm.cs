using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilentAlarm : MonoBehaviour
{
    [SerializeField] private CameraSwitcher CameraTracker;
    public Entity Entities;
    private bool OnCooldown = false;
    [SerializeField] private float CooldownTime = 1f;


    public void Scare()
    {
        if (OnCooldown == false)
        {
            OnCooldown = true;
            Invoke("CooldownDone", CooldownTime);
            if (RoomController.Instance.GetRoom(CameraTracker.CameraIndex+1) == RoomController.Instance.FindEntity(EntityType.Vial))
            {
                   Entities.KickOutOfOffice();
            }
        print ("AAAAAAAA");
        }
    }

    private void CooldownDone()
    {
        OnCooldown = false;
    }
}
