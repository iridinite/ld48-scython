using System.Collections;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{

    [Header("my hands are typing words")]
    public Camera m_Camera;
    public Light m_SunLight;

    [Header("aasasdfdfjk")]
    public Gradient m_CameraGradient;
    public Gradient m_SunGradient;
    [Min(0.1f)] public float m_TransitionTime = 1f;

    [Header("Haaaaaaands")]
    public GameObject m_VFXplaceholder;

    private int m_LastKnownLayer;

    private void Awake()
    {
        ApplyFactor(0f);
    }

    private void FixedUpdate()
    {
        int currentLayer = GameSession.Instance.PortalStage;
        if (currentLayer != m_LastKnownLayer)
        {
            StopAllCoroutines();
            StartCoroutine(TransitionCoroutine(m_LastKnownLayer, currentLayer));
            m_LastKnownLayer = currentLayer;
        }
    }

    private IEnumerator TransitionCoroutine(int from, int to)
    {
        float a = from / (float)GameSession.Instance.PortalStageCount;
        float b = to / (float)GameSession.Instance.PortalStageCount;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.fixedDeltaTime / m_TransitionTime;

            float factor = Mathf.Lerp(a, b, t);
            ApplyFactor(factor);

            yield return new WaitForFixedUpdate();
        }

        // Safeguard
        ApplyFactor(b);
    }

    private void ApplyFactor(float factor)
    {
        m_Camera.backgroundColor = m_CameraGradient.Evaluate(factor);
        m_SunLight.color = m_SunGradient.Evaluate(factor);
    }

}
