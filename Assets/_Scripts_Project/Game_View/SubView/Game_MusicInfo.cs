using System.IO;
using PSPUtil;
using PSPUtil.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class Game_MusicInfo : SubUI 
{
    protected override void OnStart(Transform root)
    {
        MyEventCenter.AddListener<Text,FileInfo,bool>(E_GameEvent.ShowMusicInfo, Show);
        MyEventCenter.AddListener(E_GameEvent.CloseMusicInfo, Close);
        mAudioSource = Get<AudioSource>("ShowMusic/AudioSource");

        // 音乐控制
        go_ShowMusic = GetGameObject("ShowMusic/MusicContrl");
        tx_InfoName = Get<Text>("ShowMusic/MusicContrl/Top/TxName");
        tx_Time = Get<Text>("ShowMusic/MusicContrl/Top/TxTime");
        go_Play = GetGameObject("ShowMusic/MusicContrl/Bottom/Play");
        go_Pause = GetGameObject("ShowMusic/MusicContrl/Bottom/Pause");
        AddButtOnClick("ShowMusic/MusicContrl/Bottom/Play", Btn_OnPlay);
        AddButtOnClick("ShowMusic/MusicContrl/Bottom/Pause", Btn_OnPause);
        AddButtOnClick("ShowMusic/MusicContrl/Bottom/Stop", Btn_OnStop);
        AddButtOnClick("ShowMusic/MusicContrl/BtnOpenFolder", Btn_OpenFolder);
        slider_Progress = Get<Slider>("ShowMusic/MusicContrl/Bottom/SliderProgress");
        SliderEvent sliderEvent = Get<SliderEvent>("ShowMusic/MusicContrl/Bottom/SliderProgress");
        sliderEvent.E_OnDrag += E_OnSliderDrag;
        sliderEvent.E_OnDragEnd += E_OnSliderDragEnd;
        d4_VolumeIcon = Get<DTToggle4_Fade>("ShowMusic/MusicContrl/Bottom/Volume/Icon");
        slider_Volume = Get<Slider>("ShowMusic/MusicContrl/Bottom/Volume/Slider");
        slider_Volume.value = mAudioSource.volume;
        AddSliderOnValueChanged(slider_Volume, Slider_OnVolumeChange);

        // 等待
        go_Wait = GetGameObject("Wait");
        tx_WaitName = Get<Text>("Wait/Middle/TxName");

        // 导入
        go_DaoRu = GetGameObject("ShowMusic/DaoRu");
        AddButtOnClick("ShowMusic/DaoRu/Contant/Btn1", () =>
        {
            ManyBtn_DaoRu(EAudioType.EasyMusic);
        });
        AddButtOnClick("ShowMusic/DaoRu/Contant/Btn2", () =>
        {
            ManyBtn_DaoRu(EAudioType.BGM);
        });
        AddButtOnClick("ShowMusic/DaoRu/Contant/Btn3", () =>
        {
            ManyBtn_DaoRu(EAudioType.Effect);
        });
        AddButtOnClick("ShowMusic/DaoRu/Contant/Btn4", () =>
        {
            ManyBtn_DaoRu(EAudioType.Click);
        });
        AddButtOnClick("ShowMusic/DaoRu/Contant/Btn5", () =>
        {
            ManyBtn_DaoRu(EAudioType.Perple);
        });
        AddButtOnClick("ShowMusic/BtnClose", Btn_OnClickClose);

    }


    public void OnUpdate()
    {
        if (mAudioSource.isPlaying)
        {
            if (!isOnSliderChange)
            {
                slider_Progress.value = mAudioSource.time;
            }
            tx_Time.text = mAudioSource.time.ToTiemStr() + " / " + mTotalTime;
        }
    }


    #region 私有

    private GameObject go_Wait,go_DaoRu, go_ShowMusic;
    private Text tx_WaitName,tx_InfoName,tx_Time;
    private GameObject go_Play,go_Pause;
    private AudioSource mAudioSource;
    private Slider slider_Progress,slider_Volume;
    private DTToggle4_Fade d4_VolumeIcon;

    private string mTotalTime;
    private bool isOnSliderChange = false;

    public override string GetUIPathForRoot()
    {
        return "MusicInfo";
    }



    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }



    #endregion


    private void Btn_OnClickClose()              // 点击关闭
    {
        Btn_OnStop();
        MyEventCenter.SendEvent(E_GameEvent.CloseMusicInfo);


    }


    private void Btn_OnPlay()                    // 点击播放
    {
        mAudioSource.Play();
        go_Play.SetActive(false);
        go_Pause.SetActive(true);
    }


    private void Btn_OnPause()                  // 点击暂停
    {
        mAudioSource.Pause();
        go_Play.SetActive(true);
        go_Pause.SetActive(false);
    }



    private void Btn_OnStop()                  // 点击停止 
    {
        mAudioSource.Stop();
        go_Play.SetActive(true);
        go_Pause.SetActive(false);
        tx_Time.text = "00:00" + " / " + mTotalTime;
        mAudioSource.time = 0;
        slider_Progress.value = 0;
    }



    private void Btn_OpenFolder()              // 打开文件夹
    {
        DirectoryInfo dir = mCurrentAudioResBean.YuanFileInfo.Directory;
        if (null!=dir)
        {
            Application.OpenURL(dir.FullName);
        }

    }



    private void E_OnSliderDrag()                           // 开始拖动 Slider 音乐条
    {
        isOnSliderChange = true;

    }

    private void E_OnSliderDragEnd()                        // 结束拖动 Slider 音乐条
    {
        if (isOnSliderChange)
        {
            isOnSliderChange = false;
            mAudioSource.time = slider_Progress.value;
        }

    }


    private void Slider_OnVolumeChange(float value)        // 拖动音量
    {
        mAudioSource.volume = value;
        if (value <= 0)
        {
            d4_VolumeIcon.Change2Four();
        }
        else if (value<0.3f)
        {
            d4_VolumeIcon.Change2Three();
        }else if (value <0.6f)
        {
            d4_VolumeIcon.Change2Two();
        }
        else
        {
            d4_VolumeIcon.Change2One();
        }
    }



    private void ManyBtn_DaoRu(EAudioType type)             // 点击导入
    {
        mCurrentAudioResBean.IsDaoRu = true;
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_Audio, type, mCurrentAudioResBean,true);
        if (null!= tx_Name)
        {
            tx_Name.color = Color.green;
            tx_Name = null;
        }

        Btn_OnClickClose();

    }

    //—————————————————— 事件 ——————————————————

    private AudioResBean mCurrentAudioResBean;
    private Text tx_Name;

    private void Show(Text txName ,FileInfo file,bool isNeedDaoRu)       // 显示音乐页事件
    {
        tx_Name = txName;
        mUIGameObject.SetActive(true);
        go_Wait.SetActive(true);
        go_ShowMusic.SetActive(false);
        go_DaoRu.SetActive(false);
        tx_WaitName.text = file.Name;
        Ctrl_LoadAudioClip.Instance.StartLoadAudioClip(file, (resBean) =>
        {
            mCurrentAudioResBean = resBean;
            go_Wait.SetActive(false);
            go_ShowMusic.SetActive(true);
            go_DaoRu.SetActive(isNeedDaoRu);
            tx_InfoName.text = file.Name;
            mTotalTime = resBean.Clip.length.ToTiemStr();
            mAudioSource.clip = resBean.Clip;
            slider_Progress.minValue = 0;
            slider_Progress.maxValue = resBean.Clip.length;

            Btn_OnPlay();
        });

    }


    private void Close()                                   // 关闭音乐页事件
    {
        mUIGameObject.SetActive(false);
        mAudioSource.clip = null;
        mAudioSource.Stop();
    }





}
