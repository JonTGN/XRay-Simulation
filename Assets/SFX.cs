using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{

    public SoundManager soundManager;

    private int volume => soundManager.GetVolume();

    private bool isAudioPlaying = false;

    public AudioSource audioSource;

    public AudioClip XrayScanningClip;
    public AudioClip XrayDonClip;

    public void TakeXray()
    {
        Debug.Log("Attempting to take xray");
        if (isAudioPlaying) { Debug.Log("xray failed | already taking one");  return; }

        StartCoroutine(Scan());
    }

    IEnumerator Scan()
    {

        isAudioPlaying = true;

        audioSource.PlayOneShot(XrayScanningClip, volume);

        yield return new WaitForSeconds(8.8f);

        audioSource.PlayOneShot(XrayDonClip, volume);

        isAudioPlaying = false;

        Debug.Log("xray completed!");
    }
}
