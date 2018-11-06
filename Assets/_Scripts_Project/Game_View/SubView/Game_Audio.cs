using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PSPUtil;
using PSPUtil.Control;
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


public class AudioResultBean
{

    public AudioClip AudioClip;
    public FileInfo File;

    public AudioResultBean(AudioClip audioClip, FileInfo file)
    {
        AudioClip = audioClip;
        File = file;
    }


}



public class Game_Audio : SubUI
{


    public void Show()
    {

    }



    #region 私有


    private EAudioType mCurrentIndex;
    // 模版
    private GameObject go_MoBan;
    private const string CREATE_FILE_NAME = "AudioFile";        // 模版产生的名



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




    IEnumerator EachSend(List<FileInfo> fileInfos)
    {
        foreach (FileInfo fileInfo in fileInfos)
        {
            MyEventCenter.SendEvent(E_GameEvent.DaoRu_Audio, mCurrentIndex, fileInfo, true);
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion




    protected override void OnStart(Transform root)
    {

        MyEventCenter.AddListener<EAudioType, FileInfo,bool>(E_GameEvent.DaoRu_Audio, E_DaoRu);


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
                        fileInfos.Add(fileInfo);
                    }
                    else
                    {
                        MyLog.Red("选择了其他的格式文件 —— " + fileInfo.Name);
                    }
                }
                Ctrl_Coroutine.Instance.StartCoroutine(EachSend(fileInfos));    //每个 FileInfo 分开来发送信息
            });
    }




    //———————————————————— 事件 ————————————————




    private void E_DaoRu(EAudioType type, FileInfo fileInfo, bool isSave)
    {

        // 保存一下信息
        if (isSave)
        {
//            bool isOk = Ctrl_TextureInfo.Instance.SaveAudio(type, fileInfo.FullName);
//            if (!isOk)
//            {
//                return;
//            }
        }

        // 1. 创建一个实例
        Transform t = InstantiateMoBan(go_MoBan, GetParentRT(type), CREATE_FILE_NAME);
        t.Find("TxName").GetComponent<Text>().text = fileInfo.Name;


    }





}
