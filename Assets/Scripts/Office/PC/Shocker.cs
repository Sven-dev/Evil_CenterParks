using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shocker : MonoBehaviour
{
    [SerializeField] private Perspective Perspective;
    [Space]
    [SerializeField] private UnityVoidEvent OnFenceSound;
    [SerializeField] private UnityVoidEvent OnShockSound;

    [SerializeField] private Entity Cork;
    public bool CorkOnFence = false;

    private void Update()
    {
        if (!CorkOnFence)
        {
            if (RoomController.Instance.GetRoom(12) == RoomController.Instance.FindEntity(EntityType.Cork))
            {
                CorkOnFence = true;
                OnFenceSound?.Invoke(); 
            }
        }            
    }

    public void StartShock()
    {
        if (Perspective.Active)
        {
            if (CorkOnFence == true)
            {
                Cork.KickCorkToRoom(7);
                CorkOnFence = false;
            }

            LampManager.Instance.Flicker();
            OnShockSound?.Invoke();
        }
    }
}
