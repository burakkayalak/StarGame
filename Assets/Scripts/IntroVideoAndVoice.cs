using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroVideoAndVoice : MonoBehaviour
{
    public float globalDelay = 0.5f;
    public float delayBetweenVoiceAndVideo = 0.5f;
    public float waitBeforeNextScene = 1f;

    public Color startColor;

    private VideoPlayer _videoPlayer;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;

    private float _timerGlobalDelay = 0f;
    private float _timerVideoDelay = 0f;

    private bool _videoStarted;
    private bool _audioStarted;
    
    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _audioSource = GetComponent<AudioSource>();
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = startColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer.loopPointReached += VideoEnded;
    }

    private void VideoEnded(VideoPlayer source)
    {
        StartCoroutine(WaitAndLoadScene(waitBeforeNextScene));
    }

    IEnumerator WaitAndLoadScene(float waitNextScene)
    {
        yield return new WaitForSeconds(waitNextScene);
        
        SceneManager.LoadScene("Game");
    }

    // Update is called once per frame
    void Update()
    {
        PlayVideoAndSound();
    }

    private void PlayVideoAndSound()
    {
        _timerGlobalDelay += Time.deltaTime;

        if (_timerGlobalDelay > globalDelay)
        {
            if (!_videoStarted)
            {
                PlaySound();
                PlayVideo();
                PauseVideo();
                _videoStarted = true;
            }
            else
            {
                _timerVideoDelay += Time.deltaTime;

                if (_timerVideoDelay > delayBetweenVoiceAndVideo && !_audioStarted)
                {
                    PlayVideo();
                    _audioStarted = true;
                }
            }
        }
    }

    private void PlayVideo()
    {
        _videoPlayer.Play();
        _spriteRenderer.color = Color.white;
    }

    private void PauseVideo()
    {
        _videoPlayer.Pause();
    }

    private void PlaySound()
    {
        _audioSource.Play();   
    }

    private void PauseSound()
    {
        _audioSource.Pause();
    }
}
