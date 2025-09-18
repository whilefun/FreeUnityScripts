using UnityEngine;

namespace Whilefun.Input
{

    public class InputDebugger : MonoBehaviour
    {

        [SerializeField] private bool m_StartActive = true;
        [SerializeField] private GameObject m_childCanvas = null;

        // A couple of test axes
        [SerializeField] private InputDebugIndicator m_LookHorizontal = null;
        [SerializeField] private InputDebugIndicator m_LookVertical = null;

        // And a couple of test buttons
        [SerializeField] private InputDebugIndicator m_Jump = null;
        [SerializeField] private InputDebugIndicator m_Crouch = null;

        private InputManager m_InputManager = null;
        private bool m_Active = false;

        private void Start()
        {

            m_InputManager = InputManager.Instance;

            if (m_StartActive)
            {
                TurnOnDebuggerUI();
            }
            else
            {
                TurnOffDebuggerUI();
            }

        }

        private void Update()
        {

            if (!m_InputManager)
            {
                Debug.LogWarning("InputDebugger:: InputManager not found!");
                return;
            }

            // Debug: Press 'T' to toggle
            if (UnityEngine.InputSystem.Keyboard.current.tKey.wasPressedThisFrame)
            {
                ToggleDebuggerUI();
            }

            if (m_Active)
            {

                // Note: Replace the fetched values below with calls to your own InputManager, Rewired, etc.
                Vector2 lookVector = Vector2.zero;
                lookVector.y = m_InputManager.GetAxis(InputManager.eInputs.LOOK_VERTICAL);
                lookVector.x = m_InputManager.GetAxis(InputManager.eInputs.LOOK_HORIZONTAL);

                m_LookHorizontal.SetInputAxisActiveState((lookVector.x != 0.0f), lookVector.x);
                m_LookVertical.SetInputAxisActiveState((lookVector.y != 0.0f), lookVector.y);

                m_Jump.SetInputButtonActiveState(m_InputManager.GetButton(InputManager.eInputs.JUMP));
                m_Crouch.SetInputButtonActiveState(m_InputManager.GetButton(InputManager.eInputs.CROUCH));

            }

        }

        public void ToggleDebuggerUI()
        {

            if (m_Active)
            {
                TurnOffDebuggerUI();
            }
            else
            {
                TurnOnDebuggerUI();
            }

        }

        private void TurnOnDebuggerUI()
        {
            m_Active = true;
            m_childCanvas.SetActive(m_Active);
        }

        private void TurnOffDebuggerUI()
        {
            m_Active = false;
            m_childCanvas.SetActive(m_Active);
        }

    }

}