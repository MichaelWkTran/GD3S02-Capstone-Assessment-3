using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioClip[] audioClips;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlaySound(int _num, float _vol, GameObject _srcObject, bool _isLoop, bool _is3D)
    {
        if (!_srcObject.GetComponent<AudioSource>())
        {
            _srcObject.AddComponent<AudioSource>();
        }
        if (audioClips.Length >= _num)
        {
            AudioSource audioSource = _srcObject.GetComponent<AudioSource>();
            audioSource.clip = audioClips[_num];
            audioSource.loop = _isLoop;
            
            if (!_is3D)
                audioSource.volume = _vol;
            else
            {
                audioSource.volume = 0.4f;
                audioSource.spatialBlend = 1;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.maxDistance = 2;
            }

            audioSource.Play();
        }
    }
}
