using System.Collections;
using UnityEngine;

public class PCBooter : Powerable
{
    [SerializeField] private Perspective Perspective;
    [SerializeField] private PcFan Fan;
    [SerializeField] private GameObject Screen;
    [SerializeField] private NoiseMaker NoiseMaker;

    [Header("Screens")]
    [SerializeField] private GameObject BootScreen;
    [SerializeField] private GameObject DesktopScreen;
    [SerializeField] private GameObject ShutDownScreen;

    private bool Booting = false;
    private bool Running = false;

    public void Start()
    {
        base.Start();
        Boot();
    }

    public void Update()
    {
        if (Perspective.Active && HasPower && Input.GetKeyDown(KeyCode.LeftControl))
        {
            TogglePc();          
        }
    }

    public void TogglePc()
    {
        //If the pc is on, turn the pc & fan off
        if (Running)
        {
            Shutdown();
        }
        //If the pc is off and not booting, start the boot sequence
        else if (!Booting)
        {
            Boot();          
        }
    }

    private void Boot()
    {
        UsingPower = true;
        
        BootScreen.SetActive(true);
        DesktopScreen.SetActive(false);
        ShutDownScreen.SetActive(false);

        NoiseMaker.MakingNoise = true;
        StartCoroutine("_StartBootDelay");
    }

    private void AbortBoot()
    {
        UsingPower = false;           
        BootScreen.SetActive(false);
        NoiseMaker.MakingNoise = false;
    }

    private void LoadDesktop()
    {
        Running = true;

        BootScreen.SetActive(false);
        DesktopScreen.SetActive(true);
        ShutDownScreen.SetActive(false);

        Fan.Power(true);
    }

    public void Shutdown()
    {
        UsingPower = false;
        Running = false;
        
        BootScreen.SetActive(false);
        DesktopScreen.SetActive(false);
        ShutDownScreen.SetActive(true);

        NoiseMaker.MakingNoise = false;
        Fan.Power(false);
    }

    private IEnumerator _StartBootDelay()
    {
        //Starts boot, waiting a set period before turning the pc & fan back on
        Booting = true;

        float timer = 3;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;

            if (Perspective.Active && HasPower && Input.GetKeyDown(KeyCode.LeftControl))
            {
                Booting = false;
                AbortBoot();

                StopCoroutine("_StartBootDelay");
            }
        }

        LoadDesktop();     
        Booting = false;
    }

    protected override void LosePower()
    {
        base.LosePower();
        Screen.SetActive(false);
        Shutdown();
        ShutDownScreen.SetActive(false);
    }

    protected override void RegainPower()
    {
        base.RegainPower();
        Screen.SetActive(true);

    }
}