using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Whilefun.Input
{

    public class InputDebugIndicator : MonoBehaviour
    {

        [SerializeField] private string m_InputLabelName = null;
        [SerializeField] private TextMeshProUGUI m_InputLabelNameText = null;
        [SerializeField] private TextMeshProUGUI m_InputValueText = null;
        [SerializeField] private Image m_InputActiveIndicatorImage = null;

        private void OnValidate()
        {

            if (m_InputLabelNameText)
            {
                m_InputLabelNameText.text = m_InputLabelName;
            }

        }

        public void SetInputButtonActiveState(bool active)
        {

            m_InputActiveIndicatorImage.enabled = active;
            m_InputValueText.text = "";

        }

        public void SetInputAxisActiveState(bool active, float axisValue)
        {

            m_InputActiveIndicatorImage.enabled = active;
            m_InputValueText.text = string.Format("{0:00.00}", axisValue);

        }

        public void SetInputValue(bool active)
        {
            m_InputActiveIndicatorImage.enabled = active;
        }

    }

}