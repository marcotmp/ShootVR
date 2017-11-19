using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    // need one audio source for the flying loop
    public AudioSource flyAudioSource;
    public AudioSource shootSource;
    public AudioSource audioSource;
    public AudioClip quackSound;
    public AudioClip flySound;
    public AudioClip shootSound;
    public AudioClip scoreSound;
    public AudioClip loseSound;
    public AudioClip winSound;
    public AudioClip startRoundSound;

    public void PlayFly()
    {
        flyAudioSource.clip = flySound;
        flyAudioSource.loop = true;
        flyAudioSource.Play();
    }

    public void StopFly()
    {
        flyAudioSource.Stop();
    }

    public void PlayShoot()
    {
        shootSource.clip = shootSound;
        shootSource.Play();
    }

    public void PlayScoring()
    {
        audioSource.clip = scoreSound;
        audioSource.Play();
    }

    public void PlayLose()
    {
        audioSource.clip = loseSound;
        audioSource.Play();
    }

    public void PlayWin()
    {
        audioSource.clip = winSound;
        audioSource.Play();
    }

    public void PlayStartRound()
    {
        audioSource.clip = startRoundSound;
        audioSource.Play();
    }
}
