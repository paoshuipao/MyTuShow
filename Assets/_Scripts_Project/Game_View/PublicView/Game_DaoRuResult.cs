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
       
        MyEventCenter.AddListener<EGameType,ushort,List<FileInfo>,bool>(E_GameEvent.DaoRuTuFromFile, E_DaoRuTuFromFile);
        MyEventCenter.AddListener<EGameType, ushort, List<ResultBean>, bool>(E_GameEvent.DaoRuTuFromResult, E_DaoRuFromTuResult);

        MyEventCenter.AddListener<EAudioType, AudioResBean>(E_GameEvent.ResultDaoRu_Audio, E_DaoRuAudio);  // 导入音频了


        go_Ok = GetGameObject("Contant/Ok");
        go_Error = GetGameObject("Contant/Error");

        // 模版_错误用
        go_ErrorMoBan = GetGameObject("Contant/Error/ErrorInfo/MoBan");
        rt_ErrorContant = Get<RectTransform>("Contant/Error/ErrorInfo/Contant");


        // 按钮
        tx_GoTo = Get<Text>("Contant/BottomBtn/BtnGoTo/Text");
        AddButtOnClick("Contant/BottomBtn/BtnGoTo", Btn_GoToDaoRuWhere);
        AddButtOnClick("Contant/BottomBtn/BtnFanHui", Btn_JiXuDaoRu);
        AddButtOnClick("Contant/BottomBtn/BtnNext", Btn_OnNextFolder);

    }



    #region 私有



    private EGameType mSelectType;
    private int mSelectIndex = 0;
    private Text tx_GoTo; // 去那按钮的文字

    private GameObject go_Ok,go_Error;
    // 模版_错误用
    private GameObject go_ErrorMoBan;
    private RectTransform rt_ErrorContant;




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
        for (int i = 0; i < rt_ErrorContant.childCount; i++)
        {
            Object.Destroy(rt_ErrorContant.GetChild(i).gameObject);
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



    private readonly List<string> l_ErrorList_Name = new List<string>();


    private void E_DaoRuTuFromFile(EGameType type, ushort index, List<FileInfo> fileInfos, bool isSave)
    {
        mSelectIndex = index;
        bool isNoError = true;
        if (isSave)   // 要保存的才显示
        {
            string[] paths = new string[fileInfos.Count];
            for (int i = 0; i < fileInfos.Count; i++)
            {
                paths[i] = fileInfos[i].FullName;
            }
            isNoError = IsSaveOk(type, index, paths);   
            Show(type, isNoError);
        }
        if (isNoError)       // 没有错那就真正导入
        {
            switch (type)
            {
                case EGameType.XuLieTu:
                    MyEventCenter.SendEvent(E_GameEvent.DaoRu_XLT_FromFile,(EXuLieTu)index, fileInfos);
                    if (index == 3 || index ==4)
                    {
                        mSelectIndex = 3;
                    }else if (index >=5)
                    {
                        mSelectIndex = 4;
                    }
                    break;
                case EGameType.XuLieTu222:
                    MyEventCenter.SendEvent(E_GameEvent.DaoRu_XLT222_FromFile,(EXuLieTu222)index, fileInfos);
                    break;
                case EGameType.JiHeXuLieTu:
                    MyEventCenter.SendEvent(E_GameEvent.DaoRu_JiHeXLT_FromFile,(EJiHeXuLieTuType)index, fileInfos);
                    break;
                case EGameType.TaoMingTu:
                    MyEventCenter.SendEvent(E_GameEvent.DaoRu_TaoMing_FromFile, (ETaoMingType)index, fileInfos);
                    break;
                case EGameType.NormalTu:
                    MyEventCenter.SendEvent(E_GameEvent.DaoRu_Jpg_FromFile, (ENormalTuType)index, fileInfos);
                    break;
                case EGameType.JiHeTu:
                    MyEventCenter.SendEvent(E_GameEvent.DaoRu_JiHe_FromFile, (EJiHeType)index, fileInfos);
                    break;
                default:
                    throw new Exception("未定义");
            }
        }

    }


    private void E_DaoRuFromTuResult(EGameType type, ushort index, List<ResultBean> resultBeans, bool isFromDaoRu)
    {
        mSelectIndex = index;
        string[] paths = new string[resultBeans.Count];
        for (int i = 0; i < resultBeans.Count; i++)
        {
            paths[i] = resultBeans[i].File.FullName;
        }
        bool isNoError = IsSaveOk(type, index, paths);
        Show(type,isNoError);
        if (isNoError)       // 没有错那就真正导入
        {
            switch (type)
            {
                case EGameType.XuLieTu:
                    MyEventCenter.SendEvent(E_GameEvent.DaoRu_XLT_FromResult, (EXuLieTu)index, resultBeans);
                    if (index == 3 || index == 4)
                    {
                        mSelectIndex = 3;
                    }
                    else if (index >= 5)
                    {
                        mSelectIndex = 4;
                    }
                    break;
                case EGameType.XuLieTu222:
                    MyEventCenter.SendEvent(E_GameEvent.DaoRu_XLT222_FromResult, (EXuLieTu222)index, resultBeans);
                    break;
                case EGameType.JiHeXuLieTu:
                    MyEventCenter.SendEvent(E_GameEvent.DaoRu_JiHeXLT_FromResult, (EJiHeXuLieTuType)index, resultBeans);
                    break;
                case EGameType.TaoMingTu:
                    MyEventCenter.SendEvent(E_GameEvent.DaoRu_TaoMing_FromResult, (ETaoMingType)index, resultBeans);
                    break;
                case EGameType.NormalTu:
                    MyEventCenter.SendEvent(E_GameEvent.DaoRu_Jpg_FromResult, (ENormalTuType)index, resultBeans);
                    break;
                case EGameType.JiHeTu:
                    MyEventCenter.SendEvent(E_GameEvent.DaoRu_JiHe_FromResult, (EJiHeType)index, resultBeans);
                    break;
            }
        }


    }



    private bool IsSaveOk(EGameType type, ushort index,string[] paths)          // 判断是否保存成功
    {
        l_ErrorList_Name.Clear();
        switch (type)
        {
            case EGameType.XuLieTu:
                if (!Ctrl_TextureInfo.Instance.SaveXunLieTu(index, paths))
                {
                    l_ErrorList_Name.Add(Path.GetFileNameWithoutExtension(paths[0]));
                }
                break;
            case EGameType.XuLieTu222:
                if (!Ctrl_TextureInfo.Instance.SaveXunLieTu222(index, paths))
                {
                    l_ErrorList_Name.Add(Path.GetFileNameWithoutExtension(paths[0]));
                }
                break;
            case EGameType.JiHeXuLieTu:
                foreach (string path in paths)
                {
                    if (!Ctrl_TextureInfo.Instance.SaveJiHeXuLieTu(index, path))
                    {
                        l_ErrorList_Name.Add(Path.GetFileNameWithoutExtension(path));
                    }
                }
                break;
            case EGameType.TaoMingTu:
                foreach (string path in paths)
                {
                    if (!Ctrl_TextureInfo.Instance.SaveTaoMingTu(index, path))
                    {
                        l_ErrorList_Name.Add(Path.GetFileNameWithoutExtension(path));
                    }
                }
                break;
            case EGameType.NormalTu:
                foreach (string path in paths)
                {
                    if (!Ctrl_TextureInfo.Instance.SaveJpgTu(index, path))
                    {
                        l_ErrorList_Name.Add(Path.GetFileNameWithoutExtension(path));
                    }
                }
                break;
            case EGameType.JiHeTu:
                foreach (string path in paths)
                {
                    if (!Ctrl_TextureInfo.Instance.SaveJiHeTu(index, path))
                    {
                        l_ErrorList_Name.Add(Path.GetFileNameWithoutExtension(path));
                    }
                }
                break;
            default:
                throw new Exception("未定义");
        }

        return l_ErrorList_Name.Count <= 0;  // 少于 0个 ，表示没错咯
    }






    private void Show(EGameType gameType, bool isOk)      // 显示导入结果
    {
        mSelectType = gameType;
        mUIGameObject.SetActive(true);
        // 是否成功
        go_Ok.SetActive(isOk);       
        go_Error.SetActive(!isOk);  
        string str = "去";
        switch (gameType)
        {
            case EGameType.XuLieTu:
                str += Ctrl_UserInfo.XuLieTu_LeftStr;
                break;
            case EGameType.XuLieTu222:
                str += Ctrl_UserInfo.XuLieTu222_LeftStr;
                break;
            case EGameType.JiHeXuLieTu:
                str += Ctrl_UserInfo.JiHeXuLieTu_LeftStr;
                break;
            case EGameType.TaoMingTu:
                str += Ctrl_UserInfo.TaoMingTu_LeftStr;
                break;
            case EGameType.NormalTu:
                str += Ctrl_UserInfo.JpgTu_LeftStr;
                break;
            case EGameType.JiHeTu:
                str += Ctrl_UserInfo.JiHeTu_LeftStr;
                break;
            case EGameType.Audio:
                str += Ctrl_UserInfo.Aduio_LeftStr;
                break;
            default:
                throw new Exception("未定义");
        }
        tx_GoTo.text = str+"处";

        if (!isOk)         // 不成功产生错误信息
        {
            foreach (string name in l_ErrorList_Name)
            {
                Transform t = InstantiateMoBan(go_ErrorMoBan, rt_ErrorContant);
                t.Find("TxName").GetComponent<Text>().text = name;
            }

        }

    }



}
