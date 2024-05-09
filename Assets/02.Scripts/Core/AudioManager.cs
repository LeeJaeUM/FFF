using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    /// <summary>
    /// 필요한 소리
    /// 문여는 소리, 문 안열리는 소리 , 철제문 열리는 소리
    /// startDoor 열리는 소리, 나무, 곡괭이질, deadlyBody는 고어 소리에서
    /// 종이 넘기는 소리, 키패드 입력소리, 열릴떄와 틀릴떄 효과음, 
    /// 열쇠 따는소리, 음식 먹는 소리, Box류 부시는 소리, 터지는 소리
    /// 기름통 뚜껑따는 소리
    /// 
    /// </summary>

    public enum Sfx
    {
        Knock,
        Knock_Power,
        Choke,
        SellPhone,
        Bell,
    }

    private void Awake()
    {
        instance = this;
        Init();
    }
    private void Start()
    {
        //PlayBgm(true);

        PlaySfx(Sfx.Knock);
    }

    private void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;
            //플레이 중이면 반복 넘김
            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

}