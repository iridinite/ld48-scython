using UnityEngine;

public class Billboard : MonoBehaviour
{

    private Transform m_Camera;
    private Transform m_Transform;

    private void Awake()
    {
        m_Camera = Camera.main.GetComponent<Transform>();
        m_Transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        m_Transform.localRotation = m_Camera.localRotation;
    }

}
