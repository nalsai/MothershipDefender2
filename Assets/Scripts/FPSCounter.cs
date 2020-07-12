using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
    [RequireComponent(typeof(Text))]
    public class FPSCounter : MonoBehaviour
    {
        private float m_FpsNextPeriod = 0;
        const float fpsMeasurePeriod = 0.2f;
        const string display = "{0} FPS";
        private Text m_Text;
        float deltaTime = 0.0f;
        int fps;
        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
            m_Text = GetComponent<Text>();
        }

        private void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }
        private void FixedUpdate()
        {
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                fps = (int)(1.0f / deltaTime);
                m_Text.text = string.Format(display, fps);
                m_FpsNextPeriod += fpsMeasurePeriod;
            }
        }
    }
}
