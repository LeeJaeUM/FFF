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
        DoorOpening,             // 1. 문이 열리는 소리
        DoorClosing,             // 2. 문이 닫히는 소리
        KeyUsage,                // 3. 열쇠 사용하는 소리
        MetalDoorOpening,        // 4. 철제 문이 열리는 소리
        PouringOil,              // 5. 기름을 붓는 소리
        PouringOilContainer,     // 6. 기름통 윗면에 기름을 따르는 소리
        AxeHittingOilCan,        // 7. 기름통 윗면에 도끼로 따는 소리
        KeypadInput,             // 8. 키패드 입력 소리
        PasswordSuccess,         // 9. 비밀번호 입력 성공 소리
        PasswordFailure,         // 10. 비밀번호 입력 실패 소리
        WoodChoppingWithAxe,     // 11. 나무를 도끼로 패는 소리
        FallingTree,             // 12. 나무가 쓰러지는 소리
        PickaxeMiningIronOre,    // 13. 곡괭이로 철광석을 캐는 소리
        HammerHittingMetalDoor,  // 14. 망치로 철제 문을 두드리는 소리
        DynamiteIgnition,        // 15. 다이너마이트에 불을 붙이는 소리
        DynamiteExplosion,       // 16. 다이너마이트 폭발음
        BreakingWall,            // 17. 벽이 부서지는 소리
        AxeHittingBox,           // 18. 도끼로 상자를 부수는 소리
        OpeningCannedFood,       // 19. 통조림을 열고 있는 소리
        EatingCannedFood,        // 20. 통조림을 먹는 소리
        TurningCorpse,           // 21. 시체를 뒤척이는 소리
        FlippingPages,           // 22. 책을 넘기는 소리
        BuildingWall,            // 23. 벽을 만드는 소리
        UnableToBuildWall,       // 24. 벽 생성 불가 소리
        PressingBuildButton      // 25. 빌드 버튼을 누르는 소리
    }


    private void Awake()
    {
        instance = this;
        Init();
    }
    private void Start()
    {
        //PlayBgm(true);

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