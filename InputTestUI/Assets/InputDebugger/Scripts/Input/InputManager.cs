using UnityEngine;
using UnityEngine.InputSystem;

namespace Whilefun.Input
{

    //
    // InputManager
    // This is mostly a throwaway demo class as a stand-in for something like Rewired
    //
    public class InputManager : MonoBehaviour
    {

        private static InputManager _instance = null;
        public static InputManager Instance { get { return _instance; } }

        [Header("Mouse Cursor Settings")]
        [SerializeField] private bool m_CursorLocked = true;

        private Keyboard m_CurrentKeyboard = null;
        private Mouse m_CurrentMouse = null;


        public enum eInputs
        {

            LOOK_HORIZONTAL = 0,
            LOOK_VERTICAL = 1,
            JUMP = 2,
            CROUCH = 3,

        }

        // Note: If you're using something like Rewired, you don't need to do this
        private bool[] m_ButtonStates_Held = null;
        private bool[] m_ButtonStates_PressedThisFrame = null;
        private bool[] m_ButtonStates_ReleasedThisFrame = null;
        private float[] m_AxisStates = null;

        private bool m_Initialized = false;

        private void Awake()
        {

            // If instance already exists, it means there is another one of this class somewhere in the scene. Destroy this one so we will still only have a single instance.
            if (_instance != null)
            {

                Debug.LogWarning("InputManager:: Duplicate InputManager '" + this.gameObject.name + "', deleting duplicate instance.");
                Destroy(this.gameObject);

            }
            else
            {

                // If this was the first instance of the class, save the reference to the instance variable, and optionally move to DontDestroyOnLoad
                _instance = this;
                DontDestroyOnLoad(this.gameObject);

                // And if there's lots of stuff to initialize, optionally do that in another function
                if (!m_Initialized)
                {

                    m_Initialized = true;
                    initialize();

                }

            }

        }

        private void initialize()
        {

            m_CurrentKeyboard = Keyboard.current;
            m_CurrentMouse = Mouse.current;

            // Note: Slightly wasteful to just store extra floats for buttons, and bools for axes. See header comment.
            m_ButtonStates_Held = new bool[System.Enum.GetValues(typeof(eInputs)).Length];
            m_ButtonStates_PressedThisFrame = new bool[System.Enum.GetValues(typeof(eInputs)).Length];
            m_ButtonStates_ReleasedThisFrame = new bool[System.Enum.GetValues(typeof(eInputs)).Length];
            m_AxisStates = new float[System.Enum.GetValues(typeof(eInputs)).Length];

        }

        private void Update()
        {

            // Note: If you're using something like Rewired, you don't need to do any of this

            m_ButtonStates_Held[(int)(eInputs.JUMP)] = m_CurrentKeyboard.spaceKey.isPressed;
            m_ButtonStates_PressedThisFrame[(int)(eInputs.JUMP)] = m_CurrentKeyboard.spaceKey.wasPressedThisFrame;
            m_ButtonStates_ReleasedThisFrame[(int)(eInputs.JUMP)] = m_CurrentKeyboard.spaceKey.wasReleasedThisFrame;

            m_ButtonStates_Held[(int)(eInputs.CROUCH)] = m_CurrentKeyboard.cKey.isPressed;
            m_ButtonStates_PressedThisFrame[(int)(eInputs.CROUCH)] = m_CurrentKeyboard.cKey.wasPressedThisFrame;
            m_ButtonStates_ReleasedThisFrame[(int)(eInputs.CROUCH)] = m_CurrentKeyboard.cKey.wasReleasedThisFrame;

            m_AxisStates[(int)(eInputs.LOOK_HORIZONTAL)] = m_CurrentMouse.delta.x.value;
            m_AxisStates[(int)(eInputs.LOOK_VERTICAL)] = m_CurrentMouse.delta.y.value;

        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(m_CursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }


        public bool GetButton(eInputs input)
        {
            return m_ButtonStates_Held[(int)input];
        }

        public float GetAxis(eInputs input)
        {
            return m_AxisStates[(int)input];
        }

        // TODO: Other functions for getting raw axis values, get button down, get button up, etc., etc.

    }

}