using UnityEngine;

//
// ObjectThrower
// A class that calculates arcs based on a raycast, and throws objects along an arc to hit the landing spot.
// Based in part on the Kinematic Equation tutorials by Sebastian Lague (see https://www.youtube.com/watch?v=IvT8hjy6q4o)
//
// To use, place on an object in your scene that also contains some geometry (a plane), and a Main Camera. This object
// will attach itself to the main camera and throw an object when left mouse button is pressed.
//
public class ObjectThrower : MonoBehaviour {

    [SerializeField, Tooltip("An prefab to represent your target. A flat-ish object facing Z+ is recommended")]
    private GameObject targetPrefab;
    [SerializeField, Tooltip("An prefab you want to throw. Must have a RigidBody.")]
    private GameObject throwablePrefab;
    [SerializeField, Tooltip("The layers that are considered 'world surfaces'")]
    private LayerMask landingSurfaceMask;

    [Header("Debug")]
    public bool drawDebugGizmos = true;

    private ThrowArcNode[] nodeSpheres = null;
    private Vector3 landingPosition = Vector3.zero;
    private float minThrowingRange = 0.5f;
    private float maxThrowingRange = 20.0f;
    private bool landingAvailable = false;
    private Vector3[] arcNodes;
    private float arcLength = 0.0f;
    
    private GameObject myTarget = null;
    private GameObject nodePointsContainer = null;

    // Object Throwing Bits //
    private Transform launchTransform = null;
    private float arcDistanceDamping = 5.0f;
    private float gravity = Physics.gravity.y;
    
    private int arcResolution = 16;
    private Vector3[] arcPoints = null;


    private struct ThrowData
    {

        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public ThrowData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }

    void Awake()
    {

        arcPoints = new Vector3[arcResolution];

        for (int i = 0; i < arcResolution; i++)
        {
            arcPoints[i] = Vector3.zero;
        }

        launchTransform = transform.Find("LaunchTransform");
        nodePointsContainer = transform.Find("NodePointsContainer").gameObject;

        if (!launchTransform || !nodePointsContainer)
        {
            Debug.LogError("ObjectThrower:: Cannot find Launch Transform and/or Node Points Container child objects!");
        }

    }

    void Start () {

        transform.parent = Camera.main.transform;
        nodeSpheres = gameObject.GetComponentsInChildren<ThrowArcNode>();

        if(nodeSpheres.Length != arcResolution)
        {
            Debug.LogWarning("Number of objects in NodePointsContainer does not match Arc Resolution. Arc node visualization will look incorrect.");
        }

        myTarget = GameObject.Instantiate(targetPrefab);
        hideTarget();

    }

    void Update()
    {

        launchTransform.position = Camera.main.transform.position - (Camera.main.transform.up * 1.0f) + (Camera.main.transform.forward * 1.0f);

        // TODO: Once integrated as an active tool (say, in an First Person game), delegate input to be central input "use" command or whatever, and let it call DoThrow function.
        if (Input.GetMouseButtonDown(0))
        {
            if (landingAvailable)
            {
                DoThrow();
            }
        }

        CalculateThrowArc();

        // "Billboard" self so the throw arc is always "straight ahead" regardless of aim pitch
        Vector3 myRotationAngles = transform.rotation.eulerAngles;
        myRotationAngles.x = 0.0f;
        myRotationAngles.z = 0.0f;
        transform.rotation = Quaternion.Euler(myRotationAngles);

        // See if we hit anything we can throw things at
        Ray interactionRay = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit landingHit;
        Physics.Raycast(interactionRay, out landingHit, maxThrowingRange, landingSurfaceMask);

        if (landingHit.transform != null)
        {

            landingPosition = landingHit.point;

            Vector3 landingPositionXZ = landingPosition;
            landingPositionXZ.y = 0.0f;

            Vector3 throwOriginXZ = transform.position;
            throwOriginXZ.y = 0.0f;

            arcLength = Vector3.Distance(throwOriginXZ, landingPositionXZ);

            // We only accept throw ranges that are inside min and max throw range
            if (arcLength > minThrowingRange)
            {

                landingAvailable = true;
                nodePointsContainer.SetActive(true);
                myTarget.gameObject.SetActive(true);
                myTarget.transform.position = landingPosition;
                myTarget.transform.forward = -landingHit.normal;

                for (int i = 0; i < arcPoints.Length; i++)
                {
                    nodeSpheres[i].transform.position = arcPoints[i];
                }

            }
            else
            {
                hideTarget();
            }

        }
        else
        {
            hideTarget();
        }

    }

    private void hideTarget()
    {

        nodePointsContainer.SetActive(false);
        landingPosition = Vector3.zero;
        landingAvailable = false;
        myTarget.gameObject.SetActive(false);

    }

    private void DoThrow()
    {

        GameObject tempThrowable = GameObject.Instantiate(throwablePrefab);
        tempThrowable.transform.position = launchTransform.position;
        // Did you remember to make the throwable prefab have a rigidbody?
        tempThrowable.GetComponent<Rigidbody>().velocity = CalculateLaunchData().initialVelocity;

    }

    // Calculation based on the "SUVAT" Kinematic Equations (https://en.wikipedia.org/wiki/Equations_of_motion)
    private ThrowData CalculateLaunchData()
    {

        float displacementY = myTarget.transform.position.y - launchTransform.position.y;
        Vector3 displacementXZ = new Vector3(myTarget.transform.position.x - launchTransform.position.x, 0, myTarget.transform.position.z - launchTransform.position.z);
        
        // Make the arc height be displacement plus some factor based on throwing distance. If you make arc height less than total vertical displacement, you'll be taking
        // the square root of zero and you're gonna have a bad time. A very flat and fast throw straight ahead can be achieved by making arc height equal y displacement.
        float arcHeight = Mathf.Abs(displacementY) + (displacementXZ.magnitude / arcDistanceDamping);

        float time = Mathf.Sqrt(-2 * arcHeight / gravity) + Mathf.Sqrt(2 * (displacementY - arcHeight) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * arcHeight);
        Vector3 velocityXZ = displacementXZ / time;

        return new ThrowData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);

    }

    private void CalculateThrowArc()
    {

        ThrowData throwData = CalculateLaunchData();
        Vector3 previousDrawPoint = launchTransform.position;

        for (int i = 0; i < arcResolution; i++)
        {

            float simulationTime = i / (float)arcResolution * throwData.timeToTarget;
            Vector3 displacement = throwData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2.0f;
            Vector3 drawPoint = launchTransform.position + displacement;
            arcPoints[i] = launchTransform.position + displacement;
            previousDrawPoint = drawPoint;

        }

    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {

        if (drawDebugGizmos)
        {

            Color c = Color.red;

            if (landingAvailable)
            {

                c = Color.red;
                c.a = 0.5f;
                Gizmos.color = c;

                Gizmos.DrawWireSphere(landingPosition, 0.5f);

                if (arcPoints != null)
                {

                    c = Color.green;
                    Gizmos.color = c;

                    Vector3 previousDrawPoint = arcPoints[0];

                    for (int i = 0; i < arcResolution; i++)
                    {

                        Gizmos.DrawLine(previousDrawPoint, arcPoints[i]);
                        previousDrawPoint = arcPoints[i];

                    }

                }

            }

        }

    }

#endif

}
