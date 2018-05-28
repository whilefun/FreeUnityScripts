using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Debuggable {

    [Header("NPC Stuff")]
    [SerializeField]
    private Vector3 movementRange = Vector3.zero;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 targetPos;
    private float movementSpeed = 1.5f;
    private float movementSnap = 0.05f;

    protected override void Start ()
    {
        base.Start();

        startPos = transform.position;
        endPos = startPos + movementRange;
        targetPos = endPos;


    }

    protected override void Update ()
    {

        base.Update();

        transform.position = Vector3.Lerp(transform.position, targetPos, movementSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, targetPos) < movementSnap)
        {

            transform.position = targetPos;

            if (targetPos == endPos)
            {
                targetPos = startPos;
            }
            else
            {
                targetPos = endPos;
            }
        }

	}

    protected override void updateDebugInfo()
    {

#if UNITY_EDITOR || DEVELOPMENT_BUILD

        MyDebugText = "Pos:" + transform.position.ToString();

#endif

    }

}
