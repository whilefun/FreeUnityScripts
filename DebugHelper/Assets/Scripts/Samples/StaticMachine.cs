using UnityEngine;

public class StaticMachine : Debuggable {

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    [SerializeField]
    private Sprite offSprite = null;
    [SerializeField]
    private Sprite onSprite = null;
#endif

    private float changeInterval = 3.0f;
    private float myTimer = 3.0f;

    private enum eMachineState
    {
        OFF = 0,
        ON = 1
    }
    private eMachineState currentMachineState = eMachineState.OFF;

    protected override void Start ()
    {
        base.Start();

    }

    protected override void Update ()
    {

        base.Update();

        // A very silly "FSM" style behaviour strictly for illustrative purposes
        myTimer -= Time.deltaTime;
        if(myTimer <= 0.0f)
        {
            myTimer = changeInterval;
            if(currentMachineState == eMachineState.OFF)
            {
                currentMachineState = eMachineState.ON;
            }
            else
            {
                currentMachineState = eMachineState.OFF;
            }
        }

    }



    protected override void updateDebugInfo()
    {

#if UNITY_EDITOR || DEVELOPMENT_BUILD

        if (currentMachineState == eMachineState.OFF)
        {
            MyDebugIcon = offSprite;
            MyDebugText = "Machine: OFF";
        }
        else
        {
            MyDebugIcon = onSprite;
            MyDebugText = "Machine: ON";
        }

#endif

    }

}
