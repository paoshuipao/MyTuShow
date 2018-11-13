﻿using System.Collections.Generic;
using System.IO;
using PSPUtil;
using UnityEngine;
using UnityEngine.UI;

public class Game_SingleTuDaoRu : SubUI
{

    protected override void OnStart(Transform root)
    {

        MyEventCenter.AddListener<ResultBean>(E_GameEvent.ShowSingleTuDaoRu, E_Show);


        AddButtOnClick("BtnClose", Btn_OnCloseThis);

        // 左边
        Sp_Tu = Get<Image>("Left/Contant/Tu/TuSize/Image");
        rt_TuSize = Get<RectTransform>("Left/Contant/Tu/TuSize");
        slider_Width = Get<Slider>("Left/Contant/SliderWidth/Slider");
        tx_WidthSize = Get<Text>("Left/Contant/SliderWidth/TxValue");
        slider_Height = Get<Slider>("Left/Contant/SliderHeight/Slider");
        tx_HeightSize = Get<Text>("Left/Contant/SliderHeight/TxValue");

        tx_DRJHXuLie1 = Get<Text>("Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR1/TxSingleDR");
        tx_DRJHXuLie2 = Get<Text>("Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR2/TxSingleDR");
        tx_DRJHXuLie3 = Get<Text>("Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR3/TxSingleDR");
        tx_DRJHXuLie4 = Get<Text>("Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR4/TxSingleDR");
        tx_DRJHXuLie5 = Get<Text>("Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR5/TxSingleDR");
        tx_DRTaoMing1 = Get<Text>("Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR1/TxSingleDR");
        tx_DRTaoMing2 = Get<Text>("Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR2/TxSingleDR");
        tx_DRTaoMing3 = Get<Text>("Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR3/TxSingleDR");
        tx_DRTaoMing4 = Get<Text>("Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR4/TxSingleDR");
        tx_DRTaoMing5 = Get<Text>("Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR5/TxSingleDR");
        tx_DRJpg1 = Get<Text>("Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR1/TxSingleDR");
        tx_DRJpg2 = Get<Text>("Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR2/TxSingleDR");
        tx_DRJpg3 = Get<Text>("Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR3/TxSingleDR");
        tx_DRJpg4 = Get<Text>("Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR4/TxSingleDR");
        tx_DRJpg5 = Get<Text>("Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR5/TxSingleDR");
        tx_DRJiHe1 = Get<Text>("Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR1/TxSingleDR");
        tx_DRJiHe2 = Get<Text>("Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR2/TxSingleDR");
        tx_DRJiHe3 = Get<Text>("Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR3/TxSingleDR");
        tx_DRJiHe4 = Get<Text>("Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR4/TxSingleDR");
        tx_DRJiHe5 = Get<Text>("Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR5/TxSingleDR");



        AddSliderOnValueChanged(slider_Width, (value) =>
        {
            SetTuSize(value);
        });
        AddSliderOnValueChanged(slider_Height, (value) =>
        {
            SetTuSize(0, value);
        });
        AddButtOnClick("Left/Contant/BtnSize/BtnPlusHalf", () =>
        {

            SetTuSize(yuanLaiWidth * 0.5f, yuanLaiHidth * 0.5f);
        });
        AddButtOnClick("Left/Contant/BtnSize/BtnFirst", () =>
        {
            SetTuSize(yuanLaiWidth, yuanLaiHidth);
        });
        AddButtOnClick("Left/Contant/BtnSize/BtnAddHalf", () =>
        {
            SetTuSize(yuanLaiWidth * 1.5f, yuanLaiHidth * 1.5f);

        });
        AddButtOnClick("Left/Contant/BtnSize/BtnAddTwo", () =>
        {
            SetTuSize(yuanLaiWidth * 2f, yuanLaiHidth * 2f);
        });


        // 右边
        tx_FileName = Get<Text>("Right/InfoName/Name");
        tx_HuoZhui = Get<Text>("Right/InfoHuoZhui/TxNum");
        tx_TuSize = Get<Text>("Right/InfoSize/TxNum");
        AddButtOnClick("Right/BtnOpenFolder/BtnOpenFolder", Btn_OnClickOpenFolder);
        AddButtOnClick("Right/BtnOpenFolder/BtnOpenFile", Btn_OnClickOpenFile);
        // 透明图
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR1", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.XiTong);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR2", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.WenZi);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR3", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.WuQi);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR4", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.DaoJu);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR5", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.ChengJi);
        });

        // 集合序列图
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR1", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT1);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR2", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT2);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR3", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT3);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR4", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT4);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR5", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT5);
        });



        // Jpg
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR1", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg1);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR2", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg2);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR3", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg3);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR4", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg4);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR5", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg5);
        });


        // 集合图
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR1", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe1);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR2", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe2);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR3", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe3);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR4", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe4);
        });
        AddButtOnClick("Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR5", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe5);
        });

    }



    #region 私有

    private ResultBean mCurrentBean;
    private float yuanLaiWidth, yuanLaiHidth;

    // 左边
    private Image Sp_Tu;
    private RectTransform rt_TuSize;
    private Slider slider_Width, slider_Height;
    private Vector2 TuSize = new Vector2(512, 512);
    private Text tx_WidthSize, tx_HeightSize;

    //右边
    private Text tx_FileName, tx_HuoZhui, tx_TuSize;

    // 导入 Text
    private Text tx_DRJHXuLie1, tx_DRJHXuLie2, tx_DRJHXuLie3, tx_DRJHXuLie4, tx_DRJHXuLie5;
    private Text tx_DRTaoMing1, tx_DRTaoMing2, tx_DRTaoMing3, tx_DRTaoMing4, tx_DRTaoMing5;
    private Text tx_DRJpg1, tx_DRJpg2, tx_DRJpg3, tx_DRJpg4, tx_DRJpg5;
    private Text tx_DRJiHe1, tx_DRJiHe2, tx_DRJiHe3, tx_DRJiHe4, tx_DRJiHe5;


    public override string GetUIPathForRoot()
    {
        return "Right/SingleTuDaoRu";
    }



    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }




    private void SetTuSize(float width = 0, float height = 0)          // 设置图大小
    {
        if (width > 0)
        {
            if (width < 8)
            {
                width = 8;
            }
            if (width > 512)
            {
                width = 512;
            }
            TuSize.x = width;
            slider_Width.value = width;
            tx_WidthSize.text = width.ToString();

        }
        if (height > 0)
        {
            if (height < 8)
            {
                height = 8;
            }
            if (height > 512)
            {
                height = 512;
            }
            TuSize.y = height;
            slider_Height.value = height;
            tx_HeightSize.text = height.ToString();
        }
        rt_TuSize.sizeDelta = TuSize;
    }


    #endregion




    private void Btn_OnClickOpenFile()                // 点击打开当前路径的文件
    {
        Application.OpenURL(mCurrentBean.File.FullName);

    }

    private void Btn_OnClickOpenFolder()              // 点击打开文件夹(适用于文件打开信息的点击)
    {
        if (mCurrentBean.File.Directory != null)
        {
            Application.OpenURL(mCurrentBean.File.Directory.FullName);
        }
    }


    private void Btn_OnCloseThis()                    // 关闭详细信息页
    {
        mUIGameObject.SetActive(false);
        Sp_Tu.sprite = null;
    }


    //—————————————————— 导入按钮 ——————————————————

    private void ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType type)           // 点击导入 集合序列图
    {
        List<ResultBean> list = new List<ResultBean> {mCurrentBean};
        // 1. 发送导入的事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_JiHeXuLieTu, type, list);
        MyEventCenter.SendEvent(E_GameEvent.OnClickDaoRu, list, (int)type);
        // 2.关闭多选信息
        Btn_OnCloseThis();

    }

    private void ManyBtn_DaoRuTaoMingTu(ETaoMingType type)               // 点击导入 透明图
    {
        List<ResultBean> list = new List<ResultBean> { mCurrentBean };
        // 1. 发送导入的事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_TaoMingTu, type, list);
        MyEventCenter.SendEvent(E_GameEvent.OnClickDaoRu, list, (int)type);
        // 2.关闭多选信息
        Btn_OnCloseThis();
    }

    private void ManyBtn_DaoRuJpgTu(ENormalTuType type)                  // 点击导入 Jpg
    {
        List<ResultBean> list = new List<ResultBean> { mCurrentBean };
        // 1. 发送导入的事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_NormalTu, type, list);
        MyEventCenter.SendEvent(E_GameEvent.OnClickDaoRu, list, (int)type);
        // 2.关闭多选信息
        Btn_OnCloseThis();

    }


    private void ManyBtn_DaoRuJiHeTu(EJiHeType type)                     // 点击导入 集合
    {

        List<ResultBean> list = new List<ResultBean> { mCurrentBean };
        // 1. 发送导入的事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_JiHeTu, type, list);
        MyEventCenter.SendEvent(E_GameEvent.OnClickDaoRu, list, (int)type);
        // 2.关闭多选信息
        Btn_OnCloseThis();

    }



    //—————————————————— 事件 ——————————————————


    private void E_Show(ResultBean bean) 
    {
        mCurrentBean = bean;
        tx_FileName.text = Path.GetFileNameWithoutExtension(bean.File.FullName);
        tx_HuoZhui.text = bean.File.Extension.Substring(1);
        tx_TuSize.text = "";

        mUIGameObject.SetActive(true);

        Sp_Tu.sprite = bean.SP;
        yuanLaiWidth = bean.Width;
        yuanLaiHidth = bean.Height;
        tx_TuSize.text = yuanLaiWidth + " x " + yuanLaiHidth;
        SetTuSize(yuanLaiWidth, yuanLaiHidth);

        tx_DRJHXuLie1.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[0];
        tx_DRJHXuLie2.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[1];
        tx_DRJHXuLie3.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[2];
        tx_DRJHXuLie4.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[3];
        tx_DRJHXuLie5.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[4];

        tx_DRTaoMing1.text = Ctrl_UserInfo.Instance.BottomTaoMingName[0];
        tx_DRTaoMing2.text = Ctrl_UserInfo.Instance.BottomTaoMingName[1];
        tx_DRTaoMing3.text = Ctrl_UserInfo.Instance.BottomTaoMingName[2];
        tx_DRTaoMing4.text = Ctrl_UserInfo.Instance.BottomTaoMingName[3];
        tx_DRTaoMing5.text = Ctrl_UserInfo.Instance.BottomTaoMingName[4];

        tx_DRJpg1.text = Ctrl_UserInfo.Instance.BottomJpgName[0];
        tx_DRJpg2.text = Ctrl_UserInfo.Instance.BottomJpgName[1];
        tx_DRJpg3.text = Ctrl_UserInfo.Instance.BottomJpgName[2];
        tx_DRJpg4.text = Ctrl_UserInfo.Instance.BottomJpgName[3];
        tx_DRJpg5.text = Ctrl_UserInfo.Instance.BottomJpgName[4];

        tx_DRJiHe1.text = Ctrl_UserInfo.Instance.BottomJiHeName[0];
        tx_DRJiHe2.text = Ctrl_UserInfo.Instance.BottomJiHeName[1];
        tx_DRJiHe3.text = Ctrl_UserInfo.Instance.BottomJiHeName[2];
        tx_DRJiHe4.text = Ctrl_UserInfo.Instance.BottomJiHeName[3];
        tx_DRJiHe5.text = Ctrl_UserInfo.Instance.BottomJiHeName[4];
    }



}
