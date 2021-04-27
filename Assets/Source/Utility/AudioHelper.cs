using UnityEngine;

public static class AudioHelper
{

    private static AudioSource m_2D;

    private static AudioSource Get2D()
    {
        if (m_2D == null)
        {
            var gobj = new GameObject("Audio Helper (2D)");
            m_2D = gobj.AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(gobj);
        }

        return m_2D;
    }

    public static void PlayOneshot2D(AudioClip clip, float volume = 1f)
    {
        Get2D().PlayOneShot(clip, volume);
    }

    public static void PlayOneshot3D(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f,
        float blend = 0.7f, float minDist = 1f, float maxDist = 10f)
    {
        var gobj = new GameObject("Audio Helper (3D)");
        gobj.transform.position = position;

        var src = gobj.AddComponent<AudioSource>();
        src.clip = clip;
        src.spatialBlend = blend;
        src.minDistance = minDist;
        src.maxDistance = maxDist;
        src.volume = volume * 1f;
        src.pitch = pitch;
        src.Play();

        Object.Destroy(gobj, clip.length * (Time.timeScale >= 0.01f ? Time.timeScale : 0.01f));
    }

}