using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    private AudioSource bgmSource;

    private AudioClip clip;

    private void Awake()
    {
        Instance = this;
    }

    // Initialize
    public void Init()
    {
        bgmSource = gameObject.GetComponent<AudioSource>();
    }

    // Play BGM
    public void PlayBGM(string bgmName, bool isLoop)
    {
        // Load BGM clip
        clip = Resources.Load<AudioClip>("Sound/" + bgmName);

        bgmSource.clip = clip;

        bgmSource.loop = isLoop;

        bgmSource.Play();
    }

    // Play Effect
    public void PlayEffect(string effectName)
    {
        // Load effect clip
        clip = Resources.Load<AudioClip>("Sound/" + effectName);

        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}
