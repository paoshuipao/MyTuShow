﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PSPUtil;
using PSPUtil.Control;
using UnityEngine;
using UnityEngine.UI;

public enum EJiHeType
{
    JiHe1,
    JiHe2,
    JiHe3,
    JiHe4,
    JiHe5,

}
public class Game_JiHeTu : SubUI
{

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



    #region 私有

    private EJiHeType mCurrentIndex = EJiHeType.JiHe1;
    private GameObject go_CurrentSelect; // 当前选择的对象
    private bool isSelect; // 是否之前点击了
    private FileInfo mCurrentSelectFile; // 当前点击的文件

    // 模版
    private GameObject go_MoBan;

    // 双击信息
    private GameObject go_Top, go_Bottom;

    // 上方
    private DTToggle5_Fade dt5_Contrl;
    private ScrollRect m_SrollView;


    // 底下
    private UGUI_ToggleGroup tg_BottomContrl;
    private Text tx_BottomName1, tx_BottomName2, tx_BottomName3, tx_BottomName4, tx_BottomName5;

    private const string ITEM_STR1 = "GeShiItem1";
    private const string ITEM_STR2 = "GeShiItem2";
    private const string ITEM_STR3 = "GeShiItem3";
    private const string ITEM_STR4 = "GeShiItem4";
    private const string ITEM_STR5 = "GeShiItem5";

    // 改变大小Slider
    private GameObject go_ChangeSize;
    private UGUI_Grid[] l_Grids;
    private Slider slider_ChangeSize;
    private Text tx_Size;
    private InputField input_Size;


    public override string GetUIPathForRoot()
    {
        return "Right/EachContant/JiHeTu";
    }



    public override void OnDisable()
    {
    }


    private IEnumerator CheckoubleClick() // 检测是否双击
    {
        isSelect = true;
        yield return new WaitForSeconds(Ctrl_UserInfo.DoubleClickTime);
        isSelect = false;
    }


    private RectTransform GetParent(EJiHeType type)
    {
        RectTransform rt = null; // 放在那里
        switch (type)
        {
            case EJiHeType.JiHe1:
                rt = dt5_Contrl.GO_One.transform as RectTransform;
                break;
            case EJiHeType.JiHe2:
                rt = dt5_Contrl.GO_Two.transform as RectTransform;
                break;
            case EJiHeType.JiHe3:
                rt = dt5_Contrl.GO_Three.transform as RectTransform;
                break;
            case EJiHeType.JiHe4:
                rt = dt5_Contrl.GO_Four.transform as RectTransform;
                break;
            case EJiHeType.JiHe5:
                rt = dt5_Contrl.GO_Five.transform as RectTransform;
                break;
            default:
                throw new Exception("还有其他？");
        }
        return rt;
    }



    private void InitMoBan(Transform t, ResultBean resultBean) // 初始化模版
    {
        t.Find("Tu").GetComponent<Image>().sprite = resultBean.SP;
        t.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (t.gameObject.Equals(go_CurrentSelect) && isSelect) // 双击
            {

                Btn_OnDoubleItemClick(resultBean);
            }
            else // 单击
            {
                go_CurrentSelect = t.gameObject;
                Ctrl_Coroutine.Instance.StartCoroutine(CheckoubleClick());
            }
        });
    }

    private void DeleteOneLine(EJiHeType type)
    {
        Ctrl_TextureInfo.Instance.DeleteJiHeOneLine(type);
        RectTransform rt = GetParent(type);
        for (int i = 0; i < rt.childCount; i++)
        {
            UnityEngine.Object.Destroy(rt.GetChild(i).gameObject);
        }
    }


    #endregion


    protected override void OnStart(Transform root)
    {
        MyEventCenter.AddListener<EJiHeType, List<FileInfo>>(E_GameEvent.DaoRu_JiHe_FromFile, E_OnDaoRu);
        MyEventCenter.AddListener<EJiHeType, List<ResultBean>>(E_GameEvent.DaoRu_JiHe_FromResult, E_ResultDaoRu);
        MyEventCenter.AddListener<EGameType>(E_GameEvent.ClickTrue, E_DelteTrue);
        MyEventCenter.AddListener<EGameType, ResultBean>(E_GameEvent.ShowSingleTuInfo, E_ShowNormalTuInfo);
        MyEventCenter.AddListener<EGameType>(E_GameEvent.CloseSingleTuInfo, E_CloseNormalTuInfo);
        MyEventCenter.AddListener<EGameType>(E_GameEvent.OnClickNoSaveThis, E_OnClickNoSaveThis);
        MyEventCenter.AddListener(E_GameEvent.DelteAll, E_DeleteAll);
        MyEventCenter.AddListener<bool>(E_GameEvent.ShowChangeSizeSlider, E_IsShowChangeSize);
        MyEventCenter.AddListener<EGameType, string>(E_GameEvent.SureGeiMing, E_OnSureGaiMing);


        // 模版
        go_MoBan = GetGameObject("Top/SrcollRect/MoBan");


        // 内容 
        dt5_Contrl = Get<DTToggle5_Fade>("Top/SrcollRect");
        m_SrollView = Get<ScrollRect>("Top/SrcollRect");


        // 底下
        tg_BottomContrl = Get<UGUI_ToggleGroup>("Bottom/Contant");
        tg_BottomContrl.OnChangeValue += E_OnBottomValueChange;
        tg_BottomContrl.OnDoubleClick += E_OnBottomDoubleClick;

        tx_BottomName1 = Get<Text>("Bottom/Contant/GeShiItem1/Text");
        tx_BottomName2 = Get<Text>("Bottom/Contant/GeShiItem2/Text");
        tx_BottomName3 = Get<Text>("Bottom/Contant/GeShiItem3/Text");
        tx_BottomName4 = Get<Text>("Bottom/Contant/GeShiItem4/Text");
        tx_BottomName5 = Get<Text>("Bottom/Contant/GeShiItem5/Text");



        // 双击显示信息
        go_Top = GetGameObject("Top");
        go_Bottom = GetGameObject("Bottom");

        // 右边
        AddButtOnClick("Top/Left/DaoRu", Btn_DaoRu);
        AddButtOnClick("Top/Left/DeleteAll", Btn_Delete);



        //改变 Grid 大小
        l_Grids = Gets<UGUI_Grid>("Top/SrcollRect");
        go_ChangeSize = GetGameObject("Top/Left/ChangeSize");
        slider_ChangeSize = Get<Slider>("Top/Left/ChangeSize/Slider");
        tx_Size = Get<Text>("Top/Left/ChangeSize/TxSize");
        AddSliderOnValueChanged(slider_ChangeSize, Slider_OnGridSizeChange);
        input_Size = Get<InputField>("Top/Left/ChangeSize/InputSize");
        AddInputOnEndEdit(input_Size, Input_SizeEdit);

    }


    public override void OnEnable()
    {
        // 是否显示 改变大小
        go_ChangeSize.SetActive(Ctrl_UserInfo.Instance.IsCanChangeSize);
        // 每个 Grid 的大小设置一下
        for (int i = 0; i < l_Grids.Length; i++)
        {
            l_Grids[i].CallSize = Ctrl_UserInfo.Instance.L_JiHeTuSize[i].CurrentSize;
        }
        // Slider 设置一下
        slider_ChangeSize.minValue = Ctrl_UserInfo.JiHeTuMinMax.x;
        slider_ChangeSize.maxValue = Ctrl_UserInfo.JiHeTuMinMax.y;
        slider_ChangeSize.value = Ctrl_UserInfo.Instance.L_JiHeTuSize[0].ChangeValue;
        tx_Size.text = Ctrl_UserInfo.Instance.L_JiHeTuSize[0].CurrentSize.x.ToString();


        // 底下的文字
        tx_BottomName1.text = Ctrl_UserInfo.Instance.BottomJiHeName[0];
        tx_BottomName2.text = Ctrl_UserInfo.Instance.BottomJiHeName[1];
        tx_BottomName3.text = Ctrl_UserInfo.Instance.BottomJiHeName[2];
        tx_BottomName4.text = Ctrl_UserInfo.Instance.BottomJiHeName[3];
        tx_BottomName5.text = Ctrl_UserInfo.Instance.BottomJiHeName[4];

    }



    //————————————————————————————————————

    private void E_OnBottomDoubleClick()                                     // 底下 双击 改名
    {
        MyEventCenter.SendEvent(E_GameEvent.ShowGeiMingUI, EGameType.JiHeTu, Ctrl_UserInfo.Instance.BottomJiHeName[(int)mCurrentIndex]);
    }



    private void E_OnBottomValueChange(string changeName) // 底下的切换
    {
        switch (changeName)
        {
            case ITEM_STR1:
                mCurrentIndex = EJiHeType.JiHe1;
                dt5_Contrl.Change2One();
                break;
            case ITEM_STR2:
                mCurrentIndex = EJiHeType.JiHe2;
                dt5_Contrl.Change2Two();
                break;
            case ITEM_STR3:
                mCurrentIndex = EJiHeType.JiHe3;
                dt5_Contrl.Change2Three();
                break;
            case ITEM_STR4:
                mCurrentIndex = EJiHeType.JiHe4;
                dt5_Contrl.Change2Four();
                break;
            case ITEM_STR5:
                mCurrentIndex = EJiHeType.JiHe5;
                dt5_Contrl.Change2Five();
                break;
        }
        slider_ChangeSize.value = Ctrl_UserInfo.Instance.L_JiHeTuSize[(int)mCurrentIndex].ChangeValue;
        tx_Size.text = Ctrl_UserInfo.Instance.L_JiHeTuSize[(int)mCurrentIndex].CurrentSize.x.ToString();
        m_SrollView.content = GetParent(mCurrentIndex);
    }


    private void Btn_DaoRu()             // 点击导入
    {
        MyOpenFileOrFolder.OpenFile(Ctrl_UserInfo.Instance.DaoRuFirstPath, "选择1个或多个 集合图片", EFileFilter.TuAndAll,
            (filePaths) =>
            {

                List<FileInfo> list = new List<FileInfo>(filePaths.Length);
                foreach (string filePath in filePaths)
                {
                    FileInfo file = new FileInfo(filePath);
                    if (MyFilterUtil.IsTu(file))
                    {
                        list.Add(file);
                    }
                }
                MyEventCenter.SendEvent(E_GameEvent.DaoRuTuFromFile,EGameType.JiHeTu, (ushort)mCurrentIndex, list, true);
            });
    }



    private void Btn_Delete()           // 删除
    {

        string tittle = "删除";
        switch (mCurrentIndex)
        {
            case EJiHeType.JiHe1:
                tittle += " 该集合1 所有的图片？";
                break;
            case EJiHeType.JiHe2:
                tittle += " 该集合2 所有的图片？";
                break;
            case EJiHeType.JiHe3:
                tittle += " 该集合3 所有的图片？";
                break;
            case EJiHeType.JiHe4:
                tittle += " 该集合4 所有的图片？";
                break;
            case EJiHeType.JiHe5:
                tittle += " 该集合5 所有的图片？";
                break;
        }
        MyEventCenter.SendEvent(E_GameEvent.ShowIsSure, EGameType.JiHeTu, tittle);

    }



    private void Slider_OnGridSizeChange(float value)          // 改变 Grid 大小
    {
        int gridIndex = (int)mCurrentIndex;
        int tmpValue = (int)value;
        Ctrl_UserInfo.Instance.L_JiHeTuSize[gridIndex].ChangeValue = tmpValue;
        Vector2 yuanSize = Ctrl_UserInfo.Instance.L_JiHeTuSize[gridIndex].YuanSize;
        Ctrl_UserInfo.Instance.L_JiHeTuSize[gridIndex].CurrentSize = new Vector2(yuanSize.x + tmpValue, yuanSize.y + tmpValue);
        l_Grids[gridIndex].CallSize = Ctrl_UserInfo.Instance.L_JiHeTuSize[gridIndex].CurrentSize;
        tx_Size.text = Ctrl_UserInfo.Instance.L_JiHeTuSize[gridIndex].CurrentSize.x.ToString();


    }

    private void Input_SizeEdit(string value)                      // Input 改大小
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }
        int intValue = Convert.ToInt32(value);
        Vector2 yuanSize = Ctrl_UserInfo.Instance.L_JiHeTuSize[(int)mCurrentIndex].YuanSize;   // 原大小

        intValue = intValue - (int)yuanSize.x;

        if (intValue < Ctrl_UserInfo.JiHeTuMinMax.x)
        {
            intValue = (int)Ctrl_UserInfo.JiHeTuMinMax.x;
        }
        if (intValue > Ctrl_UserInfo.JiHeTuMinMax.y)
        {
            intValue = (int)Ctrl_UserInfo.JiHeTuMinMax.y;
        }
        slider_ChangeSize.value = intValue;
        input_Size.text = "";

    }


    private void Btn_OnDoubleItemClick(ResultBean resultBean) // 双击显示信息
    {
        mCurrentSelectFile = resultBean.File;
        MyEventCenter.SendEvent(E_GameEvent.ShowSingleTuInfo, EGameType.JiHeTu, resultBean);
    }





    //—————————————————— 事件 ——————————————————


    private void E_OnDaoRu(EJiHeType type, List<FileInfo> infos) // 需要再次加载的 导入
    {
        Ctrl_Coroutine.Instance.StartCoroutine(OnDaoRu(type, infos));
    }

    IEnumerator OnDaoRu(EJiHeType type, List<FileInfo> infos)
    {
        foreach (FileInfo fileInfo in infos)
        {
            // 1. 创建一个实例
            Transform t = InstantiateMoBan(go_MoBan, GetParent(type));
            MyLoadTu.LoadSingleTu(fileInfo, (resBean) =>
            {
                InitMoBan(t, resBean);
            });
            yield return 0;
        }
    }



    private void E_ResultDaoRu(EJiHeType type, List<ResultBean> resultBeans) // 已有 ResultBean 直接的导入
    {
        Ctrl_Coroutine.Instance.StartCoroutine(ResultDaoRu(type, resultBeans));
    }


    IEnumerator ResultDaoRu(EJiHeType type, List<ResultBean> resultBeans)
    {
        foreach (ResultBean resultBean in resultBeans)
        {
            Transform t = InstantiateMoBan(go_MoBan, GetParent(type));
            InitMoBan(t, resultBean);
            yield return 0;
        }
    }




    //————————————————————————————————————

    private void E_DelteTrue(EGameType type)             // 真的删除一行
    {
        if (type == EGameType.JiHeTu)
        {
            DeleteOneLine(mCurrentIndex);
        }
    }


    private void E_ShowNormalTuInfo(EGameType type, ResultBean bean)        // 显示单图信息
    {
        if (type == EGameType.JiHeTu)
        {
            go_Top.SetActive(false);
            go_Bottom.SetActive(false);
        }
    }



    private void E_CloseNormalTuInfo(EGameType type)                        // 关闭意图信息
    {
        if (type == EGameType.JiHeTu)
        {
            go_Top.SetActive(true);
            go_Bottom.SetActive(true);
        }
    }



    private void E_OnClickNoSaveThis(EGameType type)                         // 点击了不保存这个
    {
        if (type == EGameType.JiHeTu)
        {
            Ctrl_TextureInfo.Instance.DeleteJiHeSave(mCurrentIndex, mCurrentSelectFile.FullName);
            UnityEngine.Object.Destroy(go_CurrentSelect);
            go_Top.SetActive(true);
            go_Bottom.SetActive(true);
        }
    }



    private void E_DeleteAll()                           // 删除所有
    {
        go_CurrentSelect = null;
        mCurrentSelectFile = null;
        foreach (EJiHeType type in Enum.GetValues(typeof(EJiHeType)))
        {
            DeleteOneLine(type);
        }

    }


    private void E_IsShowChangeSize(bool isOn)          // 是否显示改变大小的Slider
    {
        go_ChangeSize.SetActive(isOn);
    }



    private void E_OnSureGaiMing(EGameType type, string changeNamne)           // 确定改名
    {
        if (type == EGameType.JiHeTu)
        {
            switch (mCurrentIndex)
            {
                case EJiHeType.JiHe1:
                    tx_BottomName1.text = changeNamne;
                    break;
                case EJiHeType.JiHe2:
                    tx_BottomName2.text = changeNamne;
                    break;
                case EJiHeType.JiHe3:
                    tx_BottomName3.text = changeNamne;
                    break;
                case EJiHeType.JiHe4:
                    tx_BottomName4.text = changeNamne;
                    break;
                case EJiHeType.JiHe5:
                    tx_BottomName5.text = changeNamne;
                    break;
            }
            Ctrl_UserInfo.Instance.BottomJiHeName[(int)mCurrentIndex] = changeNamne;
        }
    }

}
