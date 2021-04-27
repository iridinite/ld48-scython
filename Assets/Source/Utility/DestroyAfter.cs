using UnityEngine;

public class DestroyAfter : MonoBehaviour
{

    [Min(0f)] public float m_Delay = 1f;

    private void Start()
    {
        Destroy(gameObject, m_Delay);
    }

}
