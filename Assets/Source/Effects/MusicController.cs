using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    public AudioClip[] m_PoolEarly;
    public AudioClip[] m_PoolLate;
    [Min(0.1f)] public float m_TransitionTime = 1f;

    private const float k_GlobalMusicMult = 0.3f;

    private AudioSource m_Source;

    private void Start()
    {
        m_Source = GetComponent<AudioSource>();
        m_Source.clip = m_PoolEarly[Random.Range(0, m_PoolEarly.Length)];
        m_Source.volume = k_GlobalMusicMult;
        m_Source.Play();
    }

    public void RequestFadeOut()
    {
        StartCoroutine(TransitionOut());
    }

    public void RequestLateGame()
    {
        var newClip = m_PoolLate[Random.Range(0, m_PoolLate.Length)];
        StartCoroutine(TransitionTo(newClip));
    }

    private IEnumerator TransitionTo(AudioClip newClip)
    {
        // Fade out
        yield return TransitionOut();

        // Swap in the new song
        m_Source.clip = newClip;
        m_Source.volume = 0f;
        m_Source.Play();

        // Fade back in
        float volume = 0f;
        while (volume < 1f)
        {
            volume += Time.fixedDeltaTime / m_TransitionTime;
            m_Source.volume = Mathf.Clamp01(volume) * k_GlobalMusicMult;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator TransitionOut()
    {
        float volume = 1f;
        while (volume > 0f)
        {
            volume -= Time.fixedDeltaTime / m_TransitionTime;
            m_Source.volume = Mathf.Clamp01(volume) * k_GlobalMusicMult;
            yield return new WaitForFixedUpdate();
        }

        m_Source.Stop();
    }

}
