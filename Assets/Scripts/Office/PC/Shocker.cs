using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shocker : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private Perspective Perspective;
    [Space]
    [SerializeField] private UnityVoidEvent OnFenceSound;
    [SerializeField] private UnityVoidEvent OnShockSound;
    [SerializeField] private UnityVoidEvent OnShockRelease;

    public bool CorkOnFence = false;

    private void Update()
    {
        //Checks whether Cork has entered room 12, if he has it plays a noise for the player
        if (!CorkOnFence)
        {
            if (RoomController.Instance.GetRoom(12) == RoomController.Instance.FindEntity(EntityType.Cork))
            {
                CorkOnFence = true;
                OnFenceSound?.Invoke();
            }
        }
    }

    private IEnumerator _ShockLoop()
    {
        //Loop to kick cork out of room 12 while the player holds down the shocker
        while (true)
        {
            if (CorkOnFence == true)
            {
                Entities.Instance.Cork.KickCorkToRoom(7);
                CorkOnFence = false;
            }

            LampManager.Instance.Flicker();
            yield return new WaitForSecondsRealtime(1);
        }
    }

    public void OnMouseDown()
    {
        if (Perspective.Active)
        {
            StartCoroutine("_ShockLoop");
            OnShockSound?.Invoke();
            Animator.SetBool("HoldUp", true);
        }
    }
    public void OnMouseUp()
    {
        OnShockRelease?.Invoke();
        Animator.SetBool("HoldUp", false);
        StopCoroutine("_ShockLoop");
    }
}
