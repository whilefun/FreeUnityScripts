using UnityEngine;

public class Bobbler : Debuggable {

    private Vector3 startPos;
    private float bobbleFactor = 1.0f;

    protected override void Start ()
    {
        base.Start();

        startPos = transform.position;

    }

    protected override void Update ()
    {

        base.Update();

        transform.position = startPos + new Vector3(0.0f, Mathf.Sin(Time.time) * bobbleFactor, 0.0f);

    }

    protected override void updateDebugInfo()
    {

#if UNITY_EDITOR || DEVELOPMENT_BUILD

        MyDebugText = "Bobble: " + (startPos.y - transform.position.y);

#endif

    }

}
