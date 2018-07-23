using UnityEngine;

public class BoltConnector : MonoBehaviour {

    [SerializeField]
    private Transform startTransform;
    [SerializeField]
    private Transform endTransform;

    private bool boltConnected = false;

	void Start () {

        Debug.Log("Press <space> to connect and disconnect the bolt.");

        if(!startTransform || !endTransform)
        {
            Debug.LogError("Looks like you forgot to assign the start and end transforms to BoltConnector object '"+gameObject.name+"'");
        }

	}
	
	void Update () {


        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (boltConnected)
            {
                boltConnected = false;
                gameObject.GetComponent<ElectricalBolt>().DisconnectBolt();
            }
            else
            {
                boltConnected = true;
                gameObject.GetComponent<ElectricalBolt>().ConnectBolt(startTransform, endTransform);
            }

        }

	}

}
