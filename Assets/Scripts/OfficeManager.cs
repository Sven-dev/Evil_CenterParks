using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManager : MonoBehaviour
{
    [SerializeField] private Room Room;

    [SerializeField] private UnityVoidEvent OnDeath;
    [SerializeField] private UnityVoidEvent OnVialEnter;
    [SerializeField] private UnityVoidEvent OnVialLeave;

    public bool ShutterOpen = true;
    public bool ModemWorking = true;
     public bool RadioWorking = false;
    public bool PcRunning = false;
    public bool FanRunning = false;

    public void Kill(EntityType killer)
    {
        print("Whoops! You were killed by " + killer);
        OnDeath?.Invoke();
    }

    public void CorkEnter()
    {
        CorkDeathTimer = _CorkDeathTimer();
        StartCoroutine(CorkDeathTimer);
    }

    private IEnumerator CorkDeathTimer;
    private IEnumerator _CorkDeathTimer()
    {
        float timeTillDeath = Random.Range(4f, 6f);
        while (timeTillDeath > 0)
        {
            if (!ShutterOpen)
            {
                float holdDownTime = Random.Range(4f, 6f);
                while (holdDownTime > 0)
                {
                    if (ShutterOpen)
                    {
                        OnDeath?.Invoke();
                        StopCoroutine(CorkDeathTimer);
                    }

                    holdDownTime -= Time.deltaTime;
                    yield return null;
                }

                //To do: Force cork to leave the room
                StopCoroutine(CorkDeathTimer);
            }

            timeTillDeath -= Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        if (ShutterOpen)
        {
            OnDeath?.Invoke();
            StopCoroutine(CorkDeathTimer);
        }

        //To do: Force cork to leave the room
    }

    public void VialEnter()
    {
        OnVialEnter?.Invoke();
    }

    public void VialLeave()
    {
        if (Room.Entities.Contains(EntityType.Vial))
        {
            OnVialLeave?.Invoke();
        }
    }

    public void ModemFixed()
    {
        ModemWorking = true;
    }

    public void ModemBroken()
    {
        ModemWorking = false;
    }

        public void RadioOn()
    {
        RadioWorking = true;
    }

    public void RadioOff()
    {
        RadioWorking = false;
    }
   
    public void PCON()
    {
        PcRunning = true;
    }

    public void PCOFF()
    {
        PcRunning = false;
    }

    public void PCFANON()
    {
        FanRunning = true;
    }

    public void PCFANOFF()
    {
        FanRunning = false;
    }
}
