using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shock : MonoBehaviour
{
    [SerializeField] private int ActivePerspective;
    [Space]
    [SerializeField] private Button ShockButton;
    [Space]
    [SerializeField] private Button SirenButton;
    [Space]
    [SerializeField] public Room CurrentRoom;
    [Space]
    [Space]
    [SerializeField] private UnityVoidEvent OnFenceSound;
    [SerializeField] private UnityVoidEvent OnShockSound;
    public Entity Entities;

    public bool CorkFence = false;

    public void ToggleActive(int cameraPerspective)
    {
        if (cameraPerspective == ActivePerspective)
        {
            ShockButton.interactable = true;
            SirenButton.interactable = true;
        }
        else
        {
            ShockButton.interactable = false;
            SirenButton.interactable = false;
        }
    }
    void Update()
    {
        if (CorkFence == false)
        {
            if (RoomController.Instance.GetRoom(12) == RoomController.Instance.FindEntity(EntityType.Cork))
            {
                CorkFence = true;
                OnFenceSound?.Invoke(); 
            }
        }            
    }
    public void StartShock()
    {
        if (CorkFence == true)
        {    
            Entities.KickCorkToRoom(7); 
        }
        OnShockSound?.Invoke();
        CorkFence = false;
    }
}
