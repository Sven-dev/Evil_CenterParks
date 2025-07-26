using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shocker : Powerable
{
    [SerializeField] private Animator Animator;
    [SerializeField] private AudioSource ShockAudio;
    [SerializeField] private NoiseMaker NoiseMaker;

    public void OnMouseDown()
    {
        Animator.SetBool("HoldUp", true);
        if (HasPower)
        {
            ShockAudio.PlayDelayed(0.216f);
            UsingPower = true;
            NoiseMaker.MakingNoise = true;
            StartCoroutine("_ShockLoop");          
        }
    }
    public void OnMouseUp()
    {
        Animator.SetBool("HoldUp", false);

        ShockAudio.Stop();
        UsingPower = false;
        NoiseMaker.MakingNoise = false;
        StopCoroutine("_ShockLoop");
    }

    private IEnumerator _ShockLoop()
    {       
        while (true)
        {
            //Loop to kick cork out of room 12 while the player holds down the shocker
            if (RoomController.Instance.GetRoom(12) == RoomController.Instance.FindEntity(EntityType.Cork))
            {
                Entities.Instance.Cork.KickCorkToRoom(7);
            }

            LampManager.Instance.Flicker();
            yield return new WaitForSecondsRealtime(1);
        }
    }

    protected override void LosePower()
    {
        base.LosePower();
        StopCoroutine("_ShockLoop");
    }
}