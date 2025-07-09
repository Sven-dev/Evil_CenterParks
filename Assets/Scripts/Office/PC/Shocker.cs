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
