using UnityEngine;

public class ScreenShaker : MonoBehaviour
{

    private struct Shake
    {
        public float intensity;
        public float time;
        public float timeMax;
    }

    private const int k_MaxShakes = 8;

    // Times per second that the camera changes direction while shaking.
    private const float k_ScreenShake_BaseSpeed = 64.0f;

    private static ScreenShaker m_Inst;
    private Shake[] m_Shakes = new Shake[k_MaxShakes];
    private int m_NextIndex = 0;

    private Transform m_Transform;
    private Vector3 m_LerpCurrent;
    private Vector3 m_LerpNext;
    private float m_LerpTime;

    private void Awake()
    {
        m_Inst = this;
        m_Transform = GetComponent<Transform>();
        m_LerpTime = 1f;
    }

    private void OnDestroy()
    {
        m_Inst = null;
    }

    private void Update()
    {
        if (m_LerpTime >= 1f)
        {
            // find the strongest currently active screen shake
            float highestIntensity = 0f;
            for (int i = 0; i < k_MaxShakes; i++)
            {
                m_Shakes[i].time -= Time.deltaTime;

                float intensity = m_Shakes[i].intensity * Mathf.Clamp01(m_Shakes[i].time / m_Shakes[i].timeMax);
                if (intensity > highestIntensity)
                    highestIntensity = intensity;
            }

            var points = Random.insideUnitCircle;

            m_LerpCurrent = m_LerpNext;
            m_LerpNext = new Vector3(points.x, points.y, 0f) * highestIntensity;
            m_LerpTime = 0f;
        }

        var offset = Vector3.Lerp(m_LerpCurrent, m_LerpNext, m_LerpTime);
        m_LerpTime += Time.deltaTime * k_ScreenShake_BaseSpeed;
        m_Transform.localPosition = offset;
    }

    public static void RequestShake(float intensity, float time)
    {
        if (intensity <= 0f || time <= 0f)
            return;

        int index = m_Inst.m_NextIndex++;
        if (m_Inst.m_NextIndex >= k_MaxShakes)
            m_Inst.m_NextIndex = 0;

        m_Inst.m_Shakes[index].intensity = intensity;
        m_Inst.m_Shakes[index].time = time;
        m_Inst.m_Shakes[index].timeMax = time;
    }

}
