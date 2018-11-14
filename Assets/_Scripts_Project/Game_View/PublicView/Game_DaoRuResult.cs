using System;
using System.Collections.Generic;
using System.IO;
using PSPUtil;
using PSPUtil.StaticUtil;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Game_DaoRuResult : SubUI 
{

    protected override void OnStart(Transform root)
    {
        MyEventCenter.AddListener<EGameType, bool, List<FileInfo>>(E_GameEvent.DaoRuResult, E_Show);       // 显示
        MyEventCenter.AddListener<EAudioType, AudioResBean>(E_GameEvent.ResultDaoRu_Audio, E_DaoRuAudio);  // 导入音频了
        MyEventCenter.AddListener<List<ResultBean>, int>(E_GameEvent.OnClickDaoRu, E_OnDuoTuDaoRu);        // 导入图片了


        tx_GoTo = Get<Text>("Contant/BottomBtn/BtnGoTo/Text");
        go_TittleOK = GetGameObject("Contant/OK");
        go_TittleError = GetGameObject("Contant/Error");
        rt_ErrorContant = Get<RectTransform>("Contant/Error/Contant");
        go_ErrorMoBan = GetGameObject("Contant/Error/Contant/MoBan");
        go_ErrorInfo = GetGameObject("Contant/ErrorInfo");


        AddButtOnClick("Contant/BottomBtn/BtnGoTo", Btn_GoToDaoRuWhere);
        AddButtOnClick("Contant/BottomBtn/BtnFanHui", Btn_JiXuDaoRu);
        AddButtOnClick("Contant/BottomBtn/BtnNext", Btn_OnNextFolder);



    }




    #region 私有
    private EGameType mSelectType;
    private int mSelectIndex = 0;
    private readonly List<GameObject> errorList = new List<GameObject>();



    private GameObject  go_TittleOK, go_TittleError, go_ErrorInfo;
    private Text tx_GoTo;
    private RectTransform rt_ErrorContant;
    private GameObject go_ErrorMoBan;




    public override string GetUIPathForRoot()
    {
        return "Right/DaoRuResult";
    }


    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }

    private void CloseThis()                 // 关闭
    {
        mUIGameObject.SetActive(false);
        if (errorList.Count > 0)
        {
            for (int i = 0; i < errorList.Count; i++)
            {
                Object.Destroy(errorList[i]);
            }
            errorList.Clear();
        }
    }



    #endregion




    private void Btn_GoToDaoRuWhere()                                  // 点击 去到刚刚导入的地方
    {
        MyEventCenter.SendEvent<EGameType, int>(E_GameEvent.ChangGameToggleType, mSelectType, mSelectIndex);
        CloseThis();
    }


    private void Btn_JiXuDaoRu()                                       // 点击 继续导入
    {
        CloseThis();
    }


    private void Btn_OnNextFolder()                                    // 点击 到下个文件夹
    {
        MyEventCenter.SendEvent(E_GameEvent.GoToNextFolderDaoRu);
        CloseThis();
    }



    //—————————————————— 事件 ——————————————————



    private void E_DaoRuAudio(EAudioType type, AudioResBean bean)                      // 导入音频的事件
    {
        mSelectIndex = (int)type;
    }

    private void E_OnDuoTuDaoRu(List<ResultBean> resultBeans, int index)               // 点击了信息页的导入
    {
        mSelectIndex = index;
    }


    private void E_Show(EGameType gameType, bool isOk, List<FileInfo> errorInfos)      // 显示导入结果
    {
        mSelectType = gameType;
        mUIGameObject.SetActive(true);
        go_TittleOK.SetActive(isOk);
        go_TittleError.SetActive(!isOk);
        go_ErrorInfo.SetActive(false);
        string str = "";
        switch (gameType)
        {
            case EGameType.JiHeXuLieTu:
                str = "去集合序列图页";
                break;
            case EGameType.XunLieTu222:
                str = "去序列图(自定)页";
                break;
            case EGameType.XunLieTu:
                str = "去序列图页";
                break;
            case EGameType.TaoMingTu:
                str = "去透明图页";
                break;
            case EGameType.NormalTu:
                str = "去普通图页";
                break;
            case EGameType.JiHeTu:
                str = "去集合图页";
                break;
            case EGameType.Audio:
                str = "去音频页";
                break;
            default:
                throw new Exception("未定义");
        }
        tx_GoTo.text = str;

        if (!isOk)
        {
            if (null != errorInfos)
            {
                foreach (FileInfo errorInfo in errorInfos)
                {
                    Transform t = InstantiateMoBan(go_ErrorMoBan, rt_ErrorContant);
                    t.Find("TxName").GetComponent<Text>().text = errorInfo.Name;
                }
            }
            else
            {
                go_ErrorInfo.SetActive(true);
            }

        }

    }



}
