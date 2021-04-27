using UnityEngine;

public class AudioSpawner : MonoBehaviour
{

    public AudioClip[] m_Candidates;
    [Range(0f, 2f)] public float m_VolumeMin = 1f;
    [Range(0f, 2f)] public float m_VolumeMax = 1f;
    [Range(0f, 4f)] public float m_PitchMin = 1f;
    [Range(0f, 4f)] public float m_PitchMax = 1f;
    [Range(0f, 1f)] public float m_SpatialBlend = 0.7f;
    [Range(0f, 100f)] public float m_DistMin = 1f;
    [Range(0f, 100f)] public float m_DistMax = 10f;

    private void Start()
    {
        AudioHelper.PlayOneshot3D(m_Candidates[Random.Range(0, m_Candidates.Length)], transform.position, Random.Range(m_VolumeMin, m_VolumeMax), Random.Range(m_PitchMin, m_PitchMax), m_SpatialBlend, m_DistMin, m_DistMax);
        Destroy(gameObject, 0.01f);
    }

}
