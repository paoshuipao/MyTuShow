using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PSPUtil;
using PSPUtil.Control;
using PSPUtil.Extensions;
using PSPUtil.StaticUtil;
using UnityEngine;
using UnityEngine.UI;


public enum EAudioType
{
    EasyMusic,
    BGM,
    Effect,
    Click,
    Perple
}



public class Game_Audio : SubUI
{

    public IEnumerator DaoRuFromFile(EAudioType type,List<FileInfo> files,bool isSave)
    {
        foreach (FileInfo file in files)
        {
            Ctrl_LoadAudioClip.Instance.StartLoadAudioClip(file, (resBean) =>
            {
                E_DaoRu(type, resBean, isSave);
            });
            yield return new WaitForEndOfFrame();
        }
    }

    public void Show(int index)
    {
        switch (index)
        {
            case 0:
                tg_BottomContrl.ChangeToggleOn(ITEM_STR1);
                break;
            case 1:
                tg_BottomContrl.ChangeToggleOn(ITEM_STR2);
                break;
            case 2:
                tg_BottomContrl.ChangeToggleOn(ITEM_STR3);
                break;
            case 3:
                tg_BottomContrl.ChangeToggleOn(ITEM_STR4);
                break;
            case 4:
                tg_BottomContrl.ChangeToggleOn(ITEM_STR5);
                break;
        }
    }



    public void OnUpdate()
    {
        if (null != mCurrentPlayBean)
        {
            mCurrentPlayBean.Update(mAudioSource, () =>
            {
                int index = l_AudioItemBeans.IndexOf(mCurrentPlayBean);
                index++;
                if (index >= l_AudioItemBeans.Count)
                {
                    index = 0;
                }
                mCurrentPlayBean = l_AudioItemBeans[index];
                mCurrentPlayBean.Play(mAudioSource);

            });
        }
    }



    #region 私有

    private EAudioType mCurrentIndex;



    // 模版
    private GameObject go_MoBan;
    private const string CREATE_FILE_NAME = "AudioFile";        // 模版产生的名
    private AudioSource mAudioSource;

    // 上方
    private ScrollRect m_SrollView;
    private DTToggle5_Fade dt5_Contrl;

    // 底下
    private UGUI_ToggleGroup tg_BottomContrl;
    private const string ITEM_STR1 = "GeShiItem1";
    private const string ITEM_STR2 = "GeShiItem2";
    private const string ITEM_STR3 = "GeShiItem3";
    private const string ITEM_STR4 = "GeShiItem4";
    private const string ITEM_STR5 = "GeShiItem5";



    public override string GetUIPathForRoot()
    {
        return "Right/EachContant/Audio";
    }


    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }



    private RectTransform GetParentRT(EAudioType type)
    {
        RectTransform rt = null;     // 放在那里
        switch (type)
        {
            case EAudioType.EasyMusic:
                rt = dt5_Contrl.GO_One.transform as RectTransform;
                break;
            case EAudioType.BGM:
                rt = dt5_Contrl.GO_Two.transform as RectTransform;
                break;
            case EAudioType.Effect:
                rt = dt5_Contrl.GO_Three.transform as RectTransform;
                break;
            case EAudioType.Click:
                rt = dt5_Contrl.GO_Four.transform as RectTransform;
                break;
            case EAudioType.Perple:
                rt = dt5_Contrl.GO_Five.transform as RectTransform;
                break;
            default:
                throw new Exception("还有其他？");
        }
        return rt;
    }




    #endregion




    protected override void OnStart(Transform root)
    {

        MyEventCenter.AddListener<EAudioType, AudioResBean,bool>(E_GameEvent.ResultDaoRu_Audio, E_DaoRu);
        mAudioSource = Get<AudioSource>();


        // 内容 
        go_MoBan = GetGameObject("Top/Contant/ScrollView/Item1/MoBan");
        m_SrollView = Get<ScrollRect>("Top/Contant/ScrollView");
        dt5_Contrl = Get<DTToggle5_Fade>("Top/Contant/ScrollView");
        AddButtOnClick("Top/Left/DaoRu", Btn_OnDaoRu);


        // 底下
        tg_BottomContrl = Get<UGUI_ToggleGroup>("Bottom/Contant");
        tg_BottomContrl.OnChangeValue += E_OnBottomValueChange;
    }


    //————————————————————————————————————


    private void E_OnBottomValueChange(string changeName)                     // 底下的切换
    {

        switch (changeName)
        {
            case ITEM_STR1:
                mCurrentIndex = EAudioType.EasyMusic;
                dt5_Contrl.Change2One();
                break;
            case ITEM_STR2:
                mCurrentIndex = EAudioType.BGM;
                dt5_Contrl.Change2Two();
                break;
            case ITEM_STR3:
                mCurrentIndex = EAudioType.Effect;
                dt5_Contrl.Change2Three();
                break;
            case ITEM_STR4:
                mCurrentIndex = EAudioType.Click;
                dt5_Contrl.Change2Four();
                break;
            case ITEM_STR5:
                mCurrentIndex = EAudioType.Perple;
                dt5_Contrl.Change2Five();
                break;
        }

        m_SrollView.content = GetParentRT(mCurrentIndex);


    }



    private void Btn_OnDaoRu()                                                // 点击导入
    {
        MyOpenFileOrFolder.OpenFile(Ctrl_UserInfo.Instance.DaoRuFirstPath, "选择一个或多个音频文件", EFileFilter.AudioAndAll,
            (filePaths) =>
            {

                List<FileInfo> fileInfos = new List<FileInfo>(filePaths.Length);
                foreach (string filePath in filePaths)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (MyFilterUtil.IsAudio(fileInfo))
                    {
                        if (fileInfo.Extension ==".mp3")
                        {
                            MyLog.Red("该导入暂不支持导入 Mp3，去快速导入处导入 Mp3");
                        }
                        else
                        {
                            fileInfos.Add(fileInfo);
                        }
                    }
                    else
                    {
                        MyLog.Red("选择了其他的格式文件 —— " + fileInfo.Name);
                    }
                }
                Ctrl_Coroutine.Instance.StartCoroutine(DaoRuFromFile(mCurrentIndex,fileInfos,true));    //每个 FileInfo 分开来发送信息
            });
    }



    //———————————————————— 事件 ————————————————



    private EachItemBean mCurrentPlayBean;        // 当前播放
    private readonly List<EachItemBean> l_AudioItemBeans = new List<EachItemBean>();

    private void E_DaoRu(EAudioType type, AudioResBean resBean,bool isSave)             // 导入事件
    {

        // 1.保存一下信息
        if (isSave)
        {
            bool isSaveOk = Ctrl_TextureInfo.Instance.SaveAudio(type, resBean.SavePath);
            MyEventCenter.SendEvent<EGameType,bool, List<FileInfo>>(E_GameEvent.DaoRuResult, EGameType.Audio, isSaveOk, null);
            if (!isSaveOk)
            {
                return;
            }
        }
        // 2.创建一个实例
        Transform t = InstantiateMoBan(go_MoBan, GetParentRT(type), CREATE_FILE_NAME);
        t.Find("Top/TxName").GetComponent<Text>().text = Path.GetFileNameWithoutExtension(resBean.YuanPath);
        Text tx_ZhongTime = t.Find("Top/TxZhongTime").GetComponent<Text>();
        tx_ZhongTime.text = resBean.Clip.length.ToTiemStr();
        Text tx_CurrentTime = t.Find("Bottom/TxCurrentTime").GetComponent<Text>();
        Button btn_Play = t.Find("Bottom/BtnPlay").GetComponent<Button>();
        Button btn_Pause = t.Find("Bottom/BtnPause").GetComponent<Button>();
        Slider slider_Progress = t.Find("Bottom/Slider_Progress").GetComponent<Slider>();

        EachItemBean itemBean = new EachItemBean(resBean.Clip,btn_Play.gameObject,btn_Pause.gameObject, slider_Progress, tx_CurrentTime);
        l_AudioItemBeans.Add(itemBean);

        btn_Play.onClick.AddListener(() =>
        {
            if (null != mCurrentPlayBean)
            {
                mCurrentPlayBean.Stop(mAudioSource);
            }
            mCurrentPlayBean = itemBean;
            mCurrentPlayBean.Play(mAudioSource);

        });

        btn_Pause.onClick.AddListener(() =>
        {
            mCurrentPlayBean.Pause(mAudioSource);
        });

    }






    //———————————————— EachItemBean ————————————————————


    public class EachItemBean
    {
        private readonly GameObject go_Play;
        private readonly GameObject go_Pause;
        private readonly Slider slider_Progress;
        private readonly Text tx_CurrentTime;
        private readonly AudioClip mAudioClip;

        private bool isPlaying;

        public EachItemBean(AudioClip clip, GameObject goPlay, GameObject goPause, Slider sliderProgress, Text txCurrentTime)
        {
            mAudioClip = clip;
            go_Play = goPlay;
            go_Pause = goPause;
            slider_Progress = sliderProgress;
            tx_CurrentTime = txCurrentTime;

            slider_Progress.minValue = 0;
            slider_Progress.maxValue = clip.length;
            slider_Progress.value = 0;
        }


        public void Play(AudioSource audioSource)
        {
            isPlaying = true;
            go_Play.SetActive(false);
            go_Pause.SetActive(true);
            tx_CurrentTime.gameObject.SetActive(true);
            if (audioSource.clip != mAudioClip)
            {
                audioSource.clip = mAudioClip;
            }
            audioSource.Play();

        }

        public void Pause(AudioSource audioSource)
        {
            go_Play.SetActive(true);
            go_Pause.SetActive(false);
            isPlaying = false;
            audioSource.Pause();
        }


        public void Stop(AudioSource audioSource)
        {
            slider_Progress.value = 0;
            go_Play.SetActive(true);
            go_Pause.SetActive(false);
            tx_CurrentTime.gameObject.SetActive(false);

            isPlaying = false;
            audioSource.Stop();
        }

        public void Update(AudioSource audioSource,Action onFinsh)
        {
            if (isPlaying)
            {
                if (audioSource.isPlaying)
                {
                    tx_CurrentTime.text = audioSource.time.ToTiemStr();
                    slider_Progress.value = audioSource.time;
                }
                else
                {
                    Stop(audioSource);
                    onFinsh();
                }
            }
  


        }


    }
}




