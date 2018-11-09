using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using PSPUtil;
using PSPUtil.Control;
using PSPUtil.StaticUtil;
using UnityEngine;
using UnityEngine.UI;


public enum EGameType
{
    XunLieTu,
    JiHeXuLieTu,
    TaoMingTu,
    NormalTu,
    JiHeTu,
    Audio,
    DaoRu,
}



public class UIStart_Game : BaseUI
{
    protected override void OnStart(Transform root)
    {

        #region 左边

        anim_LeftContrl = Get<DTExpansion_Contrl>("Left");
        AddButtOnClick("Left/BtnBig", Btn_OnBig);

        go_XuLieChoose1 = GetGameObject("Left/Contant/Group/XuLieTu/Choose1");
        go_XuLieChoose2 = GetGameObject("Left/Contant/Group/XuLieTu/Kuang/Choose2");
        go_TaoMingChoose1 = GetGameObject("Left/Contant/Group/TaoMingTu/Choose1");
        go_TaoMingChoose2 = GetGameObject("Left/Contant/Group/TaoMingTu/Kuang/Choose2");
        go_NormalChoose1 = GetGameObject("Left/Contant/Group/NormalTu/Choose1");
        go_NormalChoose2 = GetGameObject("Left/Contant/Group/NormalTu/Kuang/Choose2");
        go_JiHeChoose1 = GetGameObject("Left/Contant/Group/JiHeTu/Choose1");
        go_JiHeChoose2 = GetGameObject("Left/Contant/Group/JiHeTu/Kuang/Choose2");
        go_AudioChoose1 = GetGameObject("Left/Contant/Group/Audio/Choose1");
        go_AudioChoose2 = GetGameObject("Left/Contant/Group/Audio/Kuang/Choose2");
        go_DaoRuChoose1 = GetGameObject("Left/Contant/Group/DaoRu/Choose1");
        go_DaoRuChoose2 = GetGameObject("Left/Contant/Group/DaoRu/Kuang/Choose2");
        go_JiHeXuLieTuChoose1 = GetGameObject("Left/Contant/Group/JiHeXuLieTu/Choose1");
        go_JiHeXuLieTuChoose2 = GetGameObject("Left/Contant/Group/JiHeXuLieTu/Kuang/Choose2");

        AddButtOnClick("Left/Contant/Group/XuLieTu", () =>
        {
            Btn_OnLeftClick(EGameType.XunLieTu);
        });
        AddButtOnClick("Left/Contant/Group/JiHeXuLieTu", () =>
        {
            Btn_OnLeftClick(EGameType.JiHeXuLieTu);
        });
        AddButtOnClick("Left/Contant/Group/TaoMingTu", () =>
        {
            Btn_OnLeftClick(EGameType.TaoMingTu);
        });
        AddButtOnClick("Left/Contant/Group/NormalTu", () =>
        {
            Btn_OnLeftClick(EGameType.NormalTu);
        });
        AddButtOnClick("Left/Contant/Group/JiHeTu", () =>
        {
            Btn_OnLeftClick(EGameType.JiHeTu);
        });
        AddButtOnClick("Left/Contant/Group/Audio", () =>
        {
            Btn_OnLeftClick(EGameType.Audio);
        });
        AddButtOnClick("Left/Contant/Group/DaoRu", () =>
        {
            Btn_OnLeftClick(EGameType.DaoRu);
        });




        #endregion


        // 左下设置
        dt2_Setting = Get<DTToggle2_Fade>("Left/Bottom");
        AddButtOnClick("Left/Bottom/BtnIcon", Btn_OnSetting);
        AddButtOnClick("Left/Bottom/Setting/Contant/Btn/BtnDelete", Btn_OnClickZhongZhi);
        Get<UGUI_PointEnterAndExit>("Left/Bottom/Setting").E_OnMouseExit += E_OnMouseExitSetting;

        Toggle toggle1 = Get<Toggle>("Left/Bottom/Setting/Contant/ToggleTip");
        toggle1.isOn = Ctrl_UserInfo.Instance.IsXuLieTuShowTip;
        AddToggleOnValueChanged(toggle1, Toggle_IsShowTip);

        Toggle toggle2 = Get<Toggle>("Left/Bottom/Setting/Contant/ToggleIsChangeSize");
        toggle2.isOn = Ctrl_UserInfo.Instance.IsCanChangeSize;
        AddToggleOnValueChanged(toggle2, Toggle_IsCanChangeSize);



        // 右边
        rt_Right = Get<RectTransform>("Right");
        d7_RightContant = Get<DTToggle7_Fade>("Right/EachContant");


        // 底下的等待UI
        go_WaitBrowser = GetGameObject("OpenBrowser");
        tx_Wait = Get<Text>("OpenBrowser/Text");
        go_WaitProgress = GetGameObject("WithProgress");


        // 确定是否界面
        go_IsSure = GetGameObject("Right/IsSure");
        tx_IsSureTittle = Get<Text>("Right/IsSure/Contant/Tittle");
        go_IsSureTip = GetGameObject("Right/IsSure/Contant/Tip");
        AddButtOnClick("Right/IsSure/Contant/BtnSure", Btn_OnSureClick);
        AddButtOnClick("Right/IsSure/Contant/BtnFalse", Btn_OnFalseClick);



        #region 图片信息


        go_TuInfo = GetGameObject("Right/ShowTuInfo");
        tx_InfoName = Get<Text>("Right/ShowTuInfo/Right/InfoName/Name");
        tx_HuoZhui = Get<Text>("Right/ShowTuInfo/Right/InfoHuoZhui/TxNum");
        tx_Size = Get<Text>("Right/ShowTuInfo/Right/InfoSize/TxNum");
        sp_Image = Get<Image>("Right/ShowTuInfo/Left/Contant/Tu/TuSize/Image");
        slider_Width = Get<Slider>("Right/ShowTuInfo/Left/Contant/SliderWidth/Slider");
        slider_Height = Get<Slider>("Right/ShowTuInfo/Left/Contant/SliderHeight/Slider");
        tx_WidthSize = Get<Text>("Right/ShowTuInfo/Left/Contant/SliderWidth/TxValue");
        tx_HeightSize = Get<Text>("Right/ShowTuInfo/Left/Contant/SliderHeight/TxValue");
        rtAnimTu = Get<RectTransform>("Right/ShowTuInfo/Left/Contant/Tu/TuSize");

        AddSliderOnValueChanged(slider_Width, (value) =>
        {
            SetTuSize(value);
        });
        AddSliderOnValueChanged(slider_Height, (value) =>
        {
            SetTuSize(0, value);
        });

        AddButtOnClick("Right/ShowTuInfo/Left/Contant/BtnSize/BtnPlusHalf", () =>
        {
            SetTuSize(yuanLaiWidth * 0.5f, yuanLaiHidth * 0.5f);
        });
        AddButtOnClick("Right/ShowTuInfo/Left/Contant/BtnSize/BtnFirst", () =>
        {
            SetTuSize(yuanLaiWidth, yuanLaiHidth);
        });
        AddButtOnClick("Right/ShowTuInfo/Left/Contant/BtnSize/BtnAddHalf", () =>
        {
            SetTuSize(yuanLaiWidth * 1.5f, yuanLaiHidth * 1.5f);
        });
        AddButtOnClick("Right/ShowTuInfo/Left/Contant/BtnSize/BtnAddTwo", () =>
        {
            SetTuSize(yuanLaiWidth * 2f, yuanLaiHidth * 2f);
        });

        AddButtOnClick("Right/ShowTuInfo/Right/BtnOpenFolder/BtnOpenFolder", Btn_OpenFolder);
        AddButtOnClick("Right/ShowTuInfo/Right/BtnOpenFolder/BtnOpenFile", Btn_OpenFile);
        AddButtOnClick("Right/ShowTuInfo/Right/BtnDlelte/Btn", Btn_OnDelete);
        AddButtOnClick("Right/ShowTuInfo/BtnClose", Btn_OnCloseInfo);
        #endregion


        // 一开始把之前所有的都加载进来
        Ctrl_Coroutine.Instance.StartCoroutine(StartFirst());
    }

    protected override void OnEnable()
    {
        E_OnToggleChange(EGameType.XunLieTu,0);

    }

    protected override void OnAddListener()
    {
        MyEventCenter.AddListener(E_GameEvent.OpenFileContrl, OnShowGameWaitUI_File);
        MyEventCenter.AddListener(E_GameEvent.OpenFolderContrl, OnShowGameWaitUI_Folder);
        MyEventCenter.AddListener(E_GameEvent.CloseFileOrFolderContrl, OnHideGameWaitUI_Browser);
        MyEventCenter.AddListener(E_GameEvent.OpenProgressWait,OpenProgressWaitUI);
        MyEventCenter.AddListener(E_GameEvent.CloseProgressWait, CloseProgressWaitUI);
        MyEventCenter.AddListener<EGameType, int>(E_GameEvent.ChangGameToggleType, E_OnToggleChange);

        MyEventCenter.AddListener<EGameType,string>(E_GameEvent.ShowIsSure, E_ShowIsSure);
        MyEventCenter.AddListener<EGameType, ResultBean>(E_GameEvent.ShowNormalTuInfo, E_ShowNormalTuInfo);
        MyEventCenter.AddListener<EGameType>(E_GameEvent.CloseNormalTuInfo, E_CloseNormalTuInfo);
    }

    protected override void OnRemoveListener()
    {
        MyEventCenter.RemoveListener(E_GameEvent.OpenFileContrl, OnShowGameWaitUI_File);
        MyEventCenter.RemoveListener(E_GameEvent.OpenFolderContrl, OnShowGameWaitUI_Folder);
        MyEventCenter.RemoveListener(E_GameEvent.CloseFileOrFolderContrl, OnHideGameWaitUI_Browser);
        MyEventCenter.RemoveListener(E_GameEvent.OpenProgressWait, OpenProgressWaitUI);
        MyEventCenter.RemoveListener(E_GameEvent.CloseProgressWait, CloseProgressWaitUI);
        MyEventCenter.RemoveListener<EGameType, int>(E_GameEvent.ChangGameToggleType, E_OnToggleChange);
        MyEventCenter.RemoveListener<EGameType,string>(E_GameEvent.ShowIsSure, E_ShowIsSure);
        MyEventCenter.RemoveListener<EGameType, ResultBean>(E_GameEvent.ShowNormalTuInfo, E_ShowNormalTuInfo);
        MyEventCenter.RemoveListener<EGameType>(E_GameEvent.CloseNormalTuInfo, E_CloseNormalTuInfo);


    }

    protected override void OnUpdate()
    {
        sub_MusicInfo.OnUpdate();
        sub_Audio.OnUpdate();
        
    }



    #region 私有
    private bool isBig =false;  // 是否最大化
    private EGameType mCurrentGameType;
    private FileInfo mCurrentFile;



    // 底下的等待UI
    private GameObject go_WaitBrowser,go_WaitProgress;
    private Text tx_Wait;
    private const string WAIT_FILE = "等待,选择文件中...";
    private const string WAIT_FOLDER = "等待,选择文件夹中...";


    // 左边
    private DTExpansion_Contrl anim_LeftContrl;
    private static readonly Vector2 FirstOffsetMin = new Vector2(272, 0);
    private static readonly Vector2 ToOffsetMin = new Vector2(84, 0);
    private GameObject go_XuLieChoose1, go_XuLieChoose2;
    private GameObject go_JiHeXuLieTuChoose1, go_JiHeXuLieTuChoose2;
    private GameObject go_TaoMingChoose1, go_TaoMingChoose2;
    private GameObject go_NormalChoose1, go_NormalChoose2;
    private GameObject go_JiHeChoose1, go_JiHeChoose2;
    private GameObject go_AudioChoose1, go_AudioChoose2;
    private GameObject go_DaoRuChoose1,go_DaoRuChoose2;
    private EGameType mCurrentType;

    // 左下设置 
    private DTToggle2_Fade dt2_Setting;



    // 右边
    private DTToggle7_Fade d7_RightContant;
    private RectTransform rt_Right;
    private GameObject go_IsSure;
    private Text tx_IsSureTittle;
    private GameObject go_IsSureTip;



    // 单图信息
    private GameObject go_TuInfo; // 双击弹出信息
    private Text tx_InfoName, tx_HuoZhui, tx_Size;
    private float yuanLaiWidth, yuanLaiHidth;
    private Slider slider_Width, slider_Height;
    private Text tx_WidthSize, tx_HeightSize;
    private RectTransform rtAnimTu;
    private Image sp_Image;
    private Vector2 TuSize = new Vector2(512, 512);



    // 右边的子UI
    private readonly Game_XuLieTu sub_XuLieTu1 = new Game_XuLieTu();     // 序列图
    private readonly Game_JiHeXuLieTu sub_JiHeXuLieTu = new Game_JiHeXuLieTu(); // 集合序列图
    private readonly Game_TaoMingTu sub_TaoMing = new Game_TaoMingTu();   // 透明图
    private readonly Game_NormalTu sub_Jpg = new Game_NormalTu();         // Jpg
    private readonly Game_JiHeTu sub_JiHeTu = new Game_JiHeTu();          // 集合图
    private readonly Game_Audio sub_Audio = new Game_Audio();             // 音频
    private readonly Game_DaoRu sub_DaoRu1 = new Game_DaoRu();            // 导入
    private readonly Game_GaiMing sub_GaiMing = new Game_GaiMing();       // 改名

    // 其他子UI
    private readonly Game_MusicInfo sub_MusicInfo = new Game_MusicInfo();    // 音乐信息



    protected override SubUI[] GetSubUI()
    {
        return new SubUI[] { sub_XuLieTu1, sub_DaoRu1, sub_TaoMing ,sub_Audio, sub_MusicInfo, sub_JiHeXuLieTu, sub_Jpg, sub_JiHeTu , sub_GaiMing };
    }


    #region 私有


    protected override E_GameEvent GetShowEvent()
    {
        return E_GameEvent.ShowStartGameUI;
    }

    protected override E_GameEvent GetHideEvent()
    {
        return E_GameEvent.HideStartGameUI;
    }

    protected override string GetResName()
    {
        return "UI/UIStart_Game";
    }

    public override EF_Scenes GetSceneType()
    {
        return EF_Scenes._0_Main;
    }


    protected override void OnDisable()
    {
    }



    private void SetTuSize(float width = 0, float height = 0) // 设置图大小
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
        rtAnimTu.sizeDelta = TuSize;
    }


    #endregion


    #endregion



    IEnumerator StartFirst()                                      // 一开始显示调用这里加载原来存储的
    {

        while (!Ctrl_TextureInfo.Instance.IsInitFinish)
        {
            yield return 0;
        }

        #region 序列图

        foreach (EXunLieTu type in Enum.GetValues(typeof(EXunLieTu)))
        {
            List<string[]> list = Ctrl_TextureInfo.Instance.GetXunLieTuPaths(type); // 获得这一页的所有数据
            for (int i = 0; i < list.Count; i++) // 加载每一个
            {
                string[] tmpLists = list[i];
                List<FileInfo> fileInfos = new List<FileInfo>(tmpLists.Length);
                bool isChuZai = true; // 这些路径是否存在
                for (int j = 0; j < tmpLists.Length; j++)
                {
                    FileInfo fileInfo = new FileInfo(tmpLists[j]);
                    if (!fileInfo.Exists)
                    {
                        isChuZai = false;
                        break;
                    }
                    fileInfos.Add(fileInfo);
                }
                if (isChuZai) // 存在就导入进来
                {
                    MyEventCenter.SendEvent(E_GameEvent.DaoRu_XunLieTu, type, fileInfos, false);
                    yield return new WaitForSeconds(0.1f);
                }
                else // 不存在就删除存储的
                {
                    Ctrl_TextureInfo.Instance.DeleteXuLieTuSave(type, tmpLists);
                }
            }
            yield return 0;
        }
        #endregion

        yield return 0;


        #region 集合序列图

        foreach (EJiHeXuLieTuType type in Enum.GetValues(typeof(EJiHeXuLieTuType)))
        {
            List<string> list = Ctrl_TextureInfo.Instance.GetJiHeXuLieTuPaths(type);
            List<FileInfo> tmpFileInfos = new List<FileInfo>();
            foreach (string path in list)
            {
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)       // 存在就加载
                {
                    tmpFileInfos.Add(fileInfo);

                }
                else                       // 不存在删除
                {
                    Ctrl_TextureInfo.Instance.DeleteJiHeXuLieSave(type, path);
                }
            }
            MyEventCenter.SendEvent(E_GameEvent.DaoRu_JiHeXuLieTu, type, tmpFileInfos, false);
            yield return new WaitForSeconds(0.1f);

        }
        #endregion

        yield return 0;


        #region 透明图

        foreach (ETaoMingType type in Enum.GetValues(typeof(ETaoMingType)))
        {

            List<string> list = Ctrl_TextureInfo.Instance.GetTaoMingTuPaths(type);
            List<FileInfo> tmpFileInfos = new List<FileInfo>();
            foreach (string path in list)
            {
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)       // 存在就加载
                {
                    tmpFileInfos.Add(fileInfo);
                }
                else                       // 不存在删除
                {
                    Ctrl_TextureInfo.Instance.DeleteTaoMingSave(type, path);
                }
            }
            MyEventCenter.SendEvent(E_GameEvent.DaoRu_TaoMingTu, type, tmpFileInfos, false);
            yield return new WaitForSeconds(0.1f);
        }
        #endregion


        yield return 0;

        #region Jpg

        foreach (ENormalTuType type in Enum.GetValues(typeof(ENormalTuType)))
        {

            List<string> list = Ctrl_TextureInfo.Instance.GetJpgTuPaths(type);
            List<FileInfo> tmpFileInfos = new List<FileInfo>();
            foreach (string path in list)
            {
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)       // 存在就加载
                {
                    tmpFileInfos.Add(fileInfo);
                }
                else                       // 不存在删除
                {
                    Ctrl_TextureInfo.Instance.DeleteJpgSave(type, path);
                }
            }
            MyEventCenter.SendEvent(E_GameEvent.DaoRu_NormalTu, type, tmpFileInfos, false);
            yield return new WaitForSeconds(0.1f);
        }
        #endregion


        yield return 0;

        #region 集合

        foreach (EJiHeType type in Enum.GetValues(typeof(EJiHeType)))
        {
            List<string> list = Ctrl_TextureInfo.Instance.GetJiHeTuPaths(type);
            List<FileInfo> tmpFileInfos = new List<FileInfo>();
            foreach (string path in list)
            {
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)       // 存在就加载
                {
                    tmpFileInfos.Add(fileInfo);
                }
                else                       // 不存在删除
                {
                    Ctrl_TextureInfo.Instance.DeleteJiHeSave(type, path);
                }
            }
            MyEventCenter.SendEvent(E_GameEvent.DaoRu_JiHeTu, type, tmpFileInfos, false);
            yield return new WaitForSeconds(0.1f);
        }
        #endregion

        foreach (EAudioType type in Enum.GetValues(typeof(EAudioType)))
        {
            List<string> paths = Ctrl_TextureInfo.Instance.GetAudioPaths(type);
            List<FileInfo> tmpFileInfos = new List<FileInfo>();
            foreach (string path in paths)
            {
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)       // 存在就加载
                {
                    tmpFileInfos.Add(fileInfo);
                }
                else                       // 不存在删除
                {
                    Ctrl_TextureInfo.Instance.DeleteAudioSave(type, path);
                }
            }

            yield return sub_Audio.DaoRuFromFile(type, tmpFileInfos,false);

        }

        MyLog.Green("加载完成");

    }



    //————————————————————————————————————



    private void Btn_OnLeftClick(EGameType type)              // 点击左边的按钮
    {
        if (type == mCurrentType)
        {
            return;
        }
        MyEventCenter.SendEvent(E_GameEvent.ChangGameToggleType, type, 0);

    }


    private void Btn_OnBig()                                  // 左边 点击最大化
    {

        if (!isBig)
        {
            DOTween.To(() => rt_Right.offsetMin,x=> rt_Right.offsetMin =x, ToOffsetMin,0.2f);
            anim_LeftContrl.DOPlayForward();
        }
        else
        {
            DOTween.To(() => rt_Right.offsetMin, x => rt_Right.offsetMin = x, FirstOffsetMin, 0.2f);
            anim_LeftContrl.DOPlayBackwards();
        }
        isBig = !isBig;

    }



    //——————————————————底下设置——————————————————

    private void Btn_OnSetting()                           // 点击了设置
    {
        dt2_Setting.Change2Two();
    }


    private bool isClickZhongZhi;

    private void Btn_OnClickZhongZhi()                       // 点击了重置
    {
        isClickZhongZhi = true;
        go_IsSureTip.SetActive(true);
        tx_IsSureTittle.text = "<color=red>      是否删除全部？</color>";
        go_IsSure.SetActive(true);
    }
    private void E_OnMouseExitSetting()                     // 鼠标离开设置选项
    {
        dt2_Setting.Change2One();

    }

    private void Toggle_IsShowTip(bool isOn)               // 是否显示提示
    {
        Ctrl_UserInfo.Instance.IsXuLieTuShowTip = isOn;
    }

    private void Toggle_IsCanChangeSize(bool isOn)               // 是否显示改变大小
    {
        MyEventCenter.SendEvent(E_GameEvent.ShowChangeSizeSlider,isOn);
        Ctrl_UserInfo.Instance.IsCanChangeSize = isOn;
    }




    //—————————————————— 确定是否 ——————————————————

    private void Btn_OnSureClick()        // 点击确定
    {
        if (isClickZhongZhi)
        {
            MyEventCenter.SendEvent(E_GameEvent.DelteAll);
            Ctrl_TextureInfo.Instance.DeleteAlll();
            go_IsSureTip.SetActive(false);
            isClickZhongZhi = false;
        }
        else
        {
            MyEventCenter.SendEvent(E_GameEvent.ClickTrue, mCurrentGameType);
        }
        go_IsSure.SetActive(false);


    }

    private void Btn_OnFalseClick()        // 点击取消
    {
        if (isClickZhongZhi)
        {
            go_IsSureTip.SetActive(false);
            isClickZhongZhi = false;
        }
        else
        {
            MyEventCenter.SendEvent(E_GameEvent.ClickFalse, mCurrentGameType);
        }
        go_IsSure.SetActive(false);

    }


    //——————————————————单图信息——————————————————

    private void Btn_OnDelete() // 不保存这个
    {
        MyEventCenter.SendEvent(E_GameEvent.OnClickNoSaveThis,mCurrentGameType);
        go_TuInfo.SetActive(false);


    }



    private void Btn_OnCloseInfo() // 关闭信息
    {
        MyEventCenter.SendEvent(E_GameEvent.CloseNormalTuInfo,mCurrentGameType);
    }


    private void Btn_OpenFile() // 打开文件
    {
        if (null == mCurrentFile)
        {
            MyLog.Red("为空？");
            return;
        }
        Application.OpenURL(mCurrentFile.FullName);

    }


    private void Btn_OpenFolder() // 打开文件夹
    {
        if (null == mCurrentFile)
        {
            MyLog.Red("为空？");
            return;
        }
        DirectoryInfo dir = mCurrentFile.Directory;
        if (null != dir)
        {
            Application.OpenURL(dir.FullName);
        }
    }




    //—————————————————— 事件 ——————————————————

    private void E_OnToggleChange(EGameType type,int choose)                  // 切换左边选项
    {
        switch (mCurrentType)
        {
            case EGameType.XunLieTu:
                go_XuLieChoose1.SetActive(false);
                go_XuLieChoose2.SetActive(false);
                break;
            case EGameType.JiHeXuLieTu:
                go_JiHeXuLieTuChoose1.SetActive(false);
                go_JiHeXuLieTuChoose2.SetActive(false);
                break;
            case EGameType.TaoMingTu:
                go_TaoMingChoose1.SetActive(false);
                go_TaoMingChoose2.SetActive(false);
                break;
            case EGameType.NormalTu:
                go_NormalChoose1.SetActive(false);
                go_NormalChoose2.SetActive(false);
                break;
            case EGameType.JiHeTu:
                go_JiHeChoose1.SetActive(false);
                go_JiHeChoose2.SetActive(false);
                break;
            case EGameType.Audio:
                go_AudioChoose1.SetActive(false);
                go_AudioChoose2.SetActive(false);
                break;
            case EGameType.DaoRu:
                go_DaoRuChoose1.SetActive(false);
                go_DaoRuChoose2.SetActive(false);
                break;

        }
        sub_Audio.ChangeOtherPage();
        switch (type)
        {
            case EGameType.XunLieTu:
                go_XuLieChoose1.SetActive(true);
                go_XuLieChoose2.SetActive(true);
                sub_XuLieTu1.Show(choose);
                d7_RightContant.Change2One();
                break;
            case EGameType.JiHeXuLieTu:
                go_JiHeXuLieTuChoose1.SetActive(true);
                go_JiHeXuLieTuChoose2.SetActive(true);
                sub_JiHeXuLieTu.Show(choose);
                d7_RightContant.Change2Two();
                break;
            case EGameType.TaoMingTu:
                go_TaoMingChoose1.SetActive(true);
                go_TaoMingChoose2.SetActive(true);
                sub_TaoMing.Show(choose);
                d7_RightContant.Change2Three();
                break;
            case EGameType.NormalTu:
                go_NormalChoose1.SetActive(true);
                go_NormalChoose2.SetActive(true);
                sub_Jpg.Show(choose);
                d7_RightContant.Change2Four();
                break;
            case EGameType.JiHeTu:
                go_JiHeChoose1.SetActive(true);
                go_JiHeChoose2.SetActive(true);
                sub_JiHeTu.Show(choose);
                d7_RightContant.Change2Five();
                break;
            case EGameType.Audio:
                go_AudioChoose1.SetActive(true);
                go_AudioChoose2.SetActive(true);
                d7_RightContant.Change2Six();
                sub_Audio.Show(choose);
                break;
            case EGameType.DaoRu:
                go_DaoRuChoose1.SetActive(true);
                go_DaoRuChoose2.SetActive(true);
                sub_DaoRu1.Show();
                d7_RightContant.Change2Seven();
                break;

        }

        mCurrentType = type;

    }


    //—————————————————— 文件、文件夹事件 ——————————————————


    private void OnShowGameWaitUI_File()            // 接收 显示等待的界面 文件 事件
    {
        go_WaitBrowser.SetActive(true);
        tx_Wait.text = WAIT_FILE;

    }

    private void OnShowGameWaitUI_Folder()          // 接收 显示等待的界面 文件夹 事件
    {
        go_WaitBrowser.SetActive(true);
        tx_Wait.text = WAIT_FOLDER;

    }



    private void OnHideGameWaitUI_Browser()         // 接收 取消等待浏览器的界面 事件
    {
        go_WaitBrowser.SetActive(false);
    }



    private void OpenProgressWaitUI()              // 打开带进度条的等待UI
    {
        go_WaitProgress.SetActive(true);
    }


    private void CloseProgressWaitUI()             // 关闭带进度条的等待UI
    {
        go_WaitProgress.SetActive(false);
    }





    private void E_ShowIsSure(EGameType type,string tittle)     // 显示 确定是否界面
    {
        mCurrentGameType = type;
        tx_IsSureTittle.text = tittle;
        go_IsSure.SetActive(true);
    }



    private void E_ShowNormalTuInfo(EGameType type, ResultBean resultBean)  // 显示单图信息
    {
        mCurrentGameType = type;
        mCurrentFile = resultBean.File;
        go_TuInfo.SetActive(true);
        tx_InfoName.text = resultBean.SP.name;
        sp_Image.sprite = resultBean.SP;
        tx_HuoZhui.text = resultBean.File.Extension;
        tx_Size.text = resultBean.Width + " x " + resultBean.Height;
        yuanLaiWidth = resultBean.Width;
        yuanLaiHidth = resultBean.Height;
        SetTuSize(yuanLaiWidth, yuanLaiHidth);


    }


    private void E_CloseNormalTuInfo(EGameType type)                  // 关闭单图信息
    {
        go_TuInfo.SetActive(false);
        mCurrentFile = null;
    }

}
