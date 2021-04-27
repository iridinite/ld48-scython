using UnityEngine;

public class Hover : MonoBehaviour
{

    public AnimationCurve m_CurveX;
    public AnimationCurve m_CurveY;
    public AnimationCurve m_CurveZ;
    [Min(0.01f)] public float m_Time;
    [Min(0f)] public float m_Delay;

    private Transform m_Transform;
    private Vector3 m_BasePosition;

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_BasePosition = m_Transform.localPosition;
    }

    private void Update()
    {
        float factor = Mathf.Repeat((Time.time + m_Delay) / m_Time, 1f);
        Vector3 localPos = new Vector3(
            m_CurveX.Evaluate(factor),
            m_CurveY.Evaluate(factor),
            m_CurveZ.Evaluate(factor)
        );

        m_Transform.localPosition = m_BasePosition + localPos;
    }

}
