using UnityEngine;

//
// ElectricalBolt
// A simple interface to make a LineRenderer vibrate and wiggle when connected 
// to two transforms like a bolt of electricity.
//
// Made with Unity 2018.1.0f2. Probably works in older versions, but was not tested with them.
//
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class ElectricalBolt : MonoBehaviour
{

    // Must always be greater than 2!
    [SerializeField, Range(3,99), Tooltip("Number of nodes in the bolt. Default is 8.")]
    private int boltResolution = 8;
    [SerializeField, Tooltip("The magnitude of random noise along the bolt at each node. Default is 0.15.")]
    private float noiseMagnitude = 0.15f;
    [SerializeField, Tooltip("The number of seconds between each bolt refresh. Default is 0.05.")]
    private float boltRefreshInterval = 0.05f;

    private Transform start;
    private Transform end;
    private Vector3[] baseSetOfPoints;
    private Vector3[] boltPoints;
    private float boltVibrationCounter = 0.0f;
    private LineRenderer myLineRenderer = null;
    private AudioSource mySpeaker;
    private bool boltConnected = false;

    void Start()
    {

        baseSetOfPoints = new Vector3[boltResolution];
        boltPoints = new Vector3[boltResolution];
        myLineRenderer = gameObject.GetComponent<LineRenderer>();
        boltVibrationCounter = boltRefreshInterval;
        mySpeaker = gameObject.GetComponent<AudioSource>();

        DisconnectBolt();

    }

    void Update()
    {

        if (boltConnected)
        {

            boltVibrationCounter -= Time.deltaTime;

            if (boltVibrationCounter <= 0.0f)
            {

                boltVibrationCounter = boltRefreshInterval;
                refreshBolt();

            }

        }

    }

    public void ConnectBolt(Transform boltStart, Transform boltEnd)
    {

        if (boltConnected == false)
        {

            start = boltStart;
            end = boltEnd;

            calculateBoltPoints();

            for (int i = 0; i < boltPoints.Length; i++)
            {

                boltPoints[i].x = baseSetOfPoints[i].x;
                boltPoints[i].y = baseSetOfPoints[i].y;
                boltPoints[i].z = baseSetOfPoints[i].z;

            }

            myLineRenderer.positionCount = boltPoints.Length;
            myLineRenderer.SetPositions(boltPoints);
            myLineRenderer.enabled = true;
            mySpeaker.Play();

            boltConnected = true;

        }

    }

    public void DisconnectBolt()
    {

        boltConnected = false;
        mySpeaker.Stop();
        myLineRenderer.enabled = false;

    }

    private void calculateBoltPoints()
    {

        for (int i = 0; i < baseSetOfPoints.Length; i++)
        {

            if (i == 0)
            {
                baseSetOfPoints[i] = start.position;
            }
            else if (i == boltPoints.Length - 1)
            {
                baseSetOfPoints[i] = end.position;
            }
            else
            {

                float p = (float)((float)i / ((float)baseSetOfPoints.Length - 1));
                baseSetOfPoints[i] = start.position + (end.position - start.position) * p;

            }

        }

    }

    public void MuteBolt()
    {
        mySpeaker.volume = 0.0f;
    }

    public void UnMuteBolt()
    {
        mySpeaker.volume = 1.0f;
    }

    private void refreshBolt()
    {

        if (boltConnected)
        {

            calculateBoltPoints();

            boltPoints[0] = start.position;
            boltPoints[boltPoints.Length - 1] = end.position;

            // Note: We skip the first and last points because they are fixed to Start and End
            for (int i = 1; i < (boltPoints.Length - 1); i++)
            {

                boltPoints[i].x = baseSetOfPoints[i].x + Random.Range(-noiseMagnitude, noiseMagnitude);
                boltPoints[i].y = baseSetOfPoints[i].y + Random.Range(-noiseMagnitude, noiseMagnitude); ;
                boltPoints[i].z = baseSetOfPoints[i].z + Random.Range(-noiseMagnitude, noiseMagnitude); ;

            }

            myLineRenderer.SetPositions(boltPoints);

        }
        else
        {

            if (start != null && end != null)
            {
                ConnectBolt(start, end);
            }
            else
            {
                Debug.LogError("RefreshBolt called but bolt is not connected and start/end are null!");
            }

        }

    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {

        Color c = Color.green;

        if (start != null && end != null)
        {

            Gizmos.color = c;
            Gizmos.DrawWireSphere(start.position, 0.2f);
            c = Color.red;
            Gizmos.color = c;
            Gizmos.DrawWireSphere(end.position, 0.2f);

        }

        if (baseSetOfPoints != null)
        {

            c = Color.yellow;
            Gizmos.color = c;

            for (int i = 0; i < baseSetOfPoints.Length; i++)
            {

                Gizmos.DrawWireSphere(baseSetOfPoints[i], 0.1f);

            }

        }

        if (boltPoints != null && boltConnected == true)
        {

            c = Color.cyan;
            Gizmos.color = c;

            for (int i = 0; i < boltPoints.Length; i++)
            {
                Gizmos.DrawWireSphere(boltPoints[i], 0.1f);
            }

        }

    }

#endif

}
