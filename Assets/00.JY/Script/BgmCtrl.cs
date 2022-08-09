using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmCtrl : MonoBehaviour
{
    public static AudioSource levelaudio;
    public static AudioClip Level1bgm;
    public static AudioClip Level2bgm;
    public static AudioClip Level3bgm;
    public static AudioClip BossRoombgm;

    private PlayerInteraction thePlayer;
    
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerInteraction>();

        levelaudio = GetComponent<AudioSource>();
        Level1bgm = Resources.Load<AudioClip>("LevelBgm/Level1");
        Level2bgm = Resources.Load<AudioClip>("LevelBgm/Level2");
        Level3bgm = Resources.Load<AudioClip>("LevelBgm/Level3");
        BossRoombgm = Resources.Load<AudioClip>("LevelBgm/BossRoom");

    }
    
    void Update()
    {
        switch(thePlayer.currentMapName)
        {
            case "Level1":
                levelaudio.clip = Level1bgm;
                if (!levelaudio.isPlaying)
                    levelaudio.Play();
                break;
            case "Level2":
                levelaudio.clip = Level2bgm;
                if (!levelaudio.isPlaying)
                    levelaudio.Play();
                break;
            case "Level3":
                levelaudio.clip = Level3bgm;
                if (!levelaudio.isPlaying)
                    levelaudio.Play();
                break;
            case "BossRoom":
                levelaudio.clip = BossRoombgm;
                if (!levelaudio.isPlaying)
                    levelaudio.Play();
                break;
        }
    }
}
