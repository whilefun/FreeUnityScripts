using UnityEngine;
using UnityEngine.UI;

public abstract class Debuggable : MonoBehaviour {

#if UNITY_EDITOR || DEVELOPMENT_BUILD

    [Header("Debug Rig Setup")]
    [SerializeField]
    private Sprite myIconSprite = null;
    public Sprite MyDebugIcon {
        set {
            myIconSprite = value;
            debugIcon.overrideSprite = myIconSprite;
        }
    }

    private Image debugIcon = null;
    private Text debugText = null;

    private string myDebugText = "";
    public string MyDebugText {

        set{
            myDebugText = value;
            debugText.text = myDebugText;
        }

    }

    [SerializeField]
    private bool billboardAtCamera = true;
    private Transform lookTarget = null;

    [SerializeField]
    private Vector3 debugOffset = new Vector3(0.0f,0.0f,0.0f);

    GameObject myDebugRig = null;

#endif

    protected virtual void Start () {

#if UNITY_EDITOR || DEVELOPMENT_BUILD

        myDebugRig = GameObject.Instantiate(Resources.Load("DebugHelper/DebugRig"), gameObject.transform) as GameObject;
        myDebugRig.transform.name = "MyDebugHelper";
        myDebugRig.transform.position = gameObject.transform.position + debugOffset;

        debugIcon = gameObject.transform.Find("MyDebugHelper/DebugCanvas/DebugIcon").GetComponent<Image>();
        debugText = gameObject.transform.Find("MyDebugHelper/DebugCanvas/DebugText").GetComponent<Text>();

        if (!debugIcon || !debugText)
        {
            Debug.LogError("DebugHelper:: Cannot find one of my UI components. Debug visuals won't work as expected.");
        }

        if(myIconSprite != null)
        {
            debugIcon.overrideSprite = myIconSprite;
        }
        else
        {
            debugIcon.enabled = false;
        }

        debugText.text = "My Debug Info...";

        if (billboardAtCamera)
        {
            lookTarget = Camera.main.gameObject.transform;
        }

#endif

    }
	
	protected virtual void Update ()
    {

#if UNITY_EDITOR || DEVELOPMENT_BUILD

        updateDebugInfo();

        if(myIconSprite != null)
        {
            debugIcon.enabled = true;
            debugIcon.overrideSprite = myIconSprite;
        }
        else
        {
            debugIcon.enabled = false;
        }

        if (billboardAtCamera)
        {
            myDebugRig.transform.LookAt(lookTarget);
        }

#endif

    }

    protected abstract void updateDebugInfo();

}
