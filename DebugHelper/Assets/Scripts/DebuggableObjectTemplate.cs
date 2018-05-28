using UnityEngine;

public class DebuggableObjectTemplate : Debuggable {

    protected override void Start ()
    {
        base.Start();

        // Your Stuff Here

    }

    protected override void Update ()
    {

        base.Update();

        // Your Stuff Here

    }

    protected override void updateDebugInfo()
    {

#if UNITY_EDITOR || DEVELOPMENT_BUILD

        // Update MyDebugText and MyDebugIcon here

#endif

    }

}
