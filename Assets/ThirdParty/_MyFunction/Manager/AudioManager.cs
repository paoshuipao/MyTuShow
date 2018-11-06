/*
using System;
using System.Collections.Generic;
using PSPUtil.StaticUtil;
using UnityEngine;

public class AudioManager : Manager
{

    public float BgVolume                                        //背景音乐大小
    {
        get
        {
            return PlayerPrefs.GetFloat(BG_VOLUMES);
        }
        set
        {
            float volume = Mathf.Clamp01(value);
            if (null!= as_BgLoop)
            {
                as_BgLoop.volume = volume;
            }
            PlayerPrefs.SetFloat(BG_VOLUMES, volume);
        }
    }

    public float EffectVolume                                    //Effect音效大小
    {
        get
        {
            return PlayerPrefs.GetFloat(EFFECT_VOLUMES);
        }
        set
        {
            float volume = Mathf.Clamp01(value);
            if (null!= as_GameKill && queue_Effect.Count>0)
            {
                foreach (AudioSource audioSource in queue_Effect)
                {
                    audioSource.volume = volume;
                }
                as_GameKill.volume = volume;
            }

            PlayerPrefs.SetFloat(EFFECT_VOLUMES, volume);
        }
    }


    public void PlayBackground(string path)                      // 播放背景音乐
    {
        AudioClip clip = mLoadManager.LoadRes<AudioClip>(path); // 有缓存的
        if (null == as_BgLoop ||  null == clip || ( null != as_BgLoop.clip  && as_BgLoop.clip.name == clip.name))
        {
            return;
        }
        as_BgLoop.clip = clip;
        as_BgLoop.Play();
    }




    public void PlayEffectAudio(string path)                     // 播放特效音效(一有就马上播放)
    {
        AudioClip clip = mLoadManager.LoadRes<AudioClip>(path);  // 有缓存的
        if (clip == null)
        {
            return;
        }
        AudioSource adio = queue_Effect.Dequeue();   // 用顶部那个
        adio.clip = clip;
        adio.volume = 1.0f;
        adio.Play();
        queue_Effect.Enqueue(adio);                  // 把它放到最后
    }



    //—————————————————— 击杀音效 ——————————————————


    public int GetKillAuioWaitCount                             // 获得 下面的击杀音效 等待 数量
    {
        get
        {
            int tmp = l_GameKillClip.Count-1;
            if (tmp<=0)
            {
                tmp = 0;
            }
            return tmp;
        }
    }



    public float GetKillJinDu                                    // 获得 当前击杀的进度
    {
        get
        {
            if (null== as_GameKill || null == as_GameKill.clip)
            {
                return 0;
            }
            float length = as_GameKill.clip.length;
            float current = as_GameKill.time;
            return current / length;
        }
    }



    public void GoHeadKillAudio()
    {
        as_GameKill.Play();
        isPause = true;
    }

    public void PauseKillAudio()
    {
        as_GameKill.Pause();
        isPause = true;

    }


    public void StopKillAudio()                                  // 停止 下面的击杀音效
    {
        as_GameKill.Stop();
        l_GameKillClip.Clear();
        isPause = true;

    }



    public void PlayGameKillAudio(string path)                   // 播放 类似击杀音效（一个个叠加上去的那种就调用这个方法）
    {
        AudioClip clip = mLoadManager.LoadRes<AudioClip>(path); // 有缓存的
        if (clip == null)
        {
            return;
        }
        isPause = false;
        l_GameKillClip.Add(clip);
    }





    #region 私有


    private const int MaxEffectAudioSource = 6;     // 弄6个特效播放器

    // 10 个 特效播放器
    private readonly Queue<AudioSource> queue_Effect = new Queue<AudioSource>();
    private readonly List<AudioClip> l_GameKillClip = new List<AudioClip>();
    private AudioSource as_BgLoop;             // 循环背景韵播放器
    private AudioSource as_GameKill;           // 击杀播放器
    //存储在PlayerPrefs的背景音乐
    private const string BG_VOLUMES = "AudioBackgroundVolumns";
    private const string EFFECT_VOLUMES = "AudioEffectVolumns";
    private const string IS_SETTED_VOLUMES = "IsSettedVolumes";
    private LoadManager mLoadManager;
    private bool isPause;

    protected override void OnInitAsync()
    {
        base.OnInitAsync();
        mLoadManager = Get<LoadManager>(EF_Manager.Load);

        //没有设置过音量大小
        if (PlayerPrefs.GetInt(IS_SETTED_VOLUMES) == 0)
        {
            BgVolume = 0.5f;
            EffectVolume = 0.5f;
            PlayerPrefs.SetInt(IS_SETTED_VOLUMES, 1);
        }


        as_BgLoop = gameObject.AddComponent<AudioSource>();
        as_BgLoop.loop = true;
        as_BgLoop.playOnAwake = false;
        as_BgLoop.volume = BgVolume;


        for (int im = 0; im < MaxEffectAudioSource; im++)
        {
            AudioSource ad = gameObject.AddComponent<AudioSource>();
            ad.volume = EffectVolume;
            ad.playOnAwake = false;
            queue_Effect.Enqueue(ad);
        }



        as_GameKill = gameObject.AddComponent<AudioSource>();
        as_GameKill.volume = EffectVolume;
        as_GameKill.playOnAwake = false;


    }




    void Update()
    {
        if (null != as_GameKill &&!as_GameKill.isPlaying && l_GameKillClip.Count > 0&& !isPause)
        {
            AudioClip adClip = l_GameKillClip[0];
            as_GameKill.clip = adClip;
            as_GameKill.Play();
            l_GameKillClip.Remove(adClip);
        }
    }

    #endregion

}
*/
