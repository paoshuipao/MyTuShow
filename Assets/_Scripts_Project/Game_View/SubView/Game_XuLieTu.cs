using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PSPUtil;
using PSPUtil.Control;
using PSPUtil.StaticUtil;
using UnityEngine;
using UnityEngine.UI;

public enum EXunLieTu
{
    G1Zheng,
    G2Zheng_XiTong,
    G3Zheng_Big,
    G4Two_Heng,
    G4Two_Shu,
    G5Three_Heng,
    G5Three_Shu,
}


public class Game_XuLieTu : SubUI
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

    private bool isSelect; // 是否之前点击了
    private EXunLieTu mCurrentIndex;
    private GameObject go_CurrentSelect; // 当前选择的对象
    private ResultBean[] l_CurrentResultBeans; // 当前选择的文件集合
    private const string ITEM_STR1 = "GeShiItem1";
    private const string ITEM_STR2 = "GeShiItem2";
    private const string ITEM_STR3 = "GeShiItem3";
    private const string ITEM_STR4 = "GeShiItem4";
    private const string ITEM_STR5 = "GeShiItem5";


    // 模版
    private GameObject go_MoBan;
    private const string CREATE_FILE_NAME = "XuLieTu"; // 模版产生的名

    // 上方
    private GameObject go_Top;
    private DTToggle5_Fade toggle5_Contant;
    private RectTransform rt_Grid1, rt_Grid2, rt_Grid3, rt_Grid4_Shu, rt_Grid4_Heng, rt_Grid5_Shu, rt_Grid5_Heng;
    private Button btn_DaoRu;


    // 底下
    private GameObject go_Bottom;
    private UGUI_ToggleGroup tg_BottomContrl;
    private ScrollRect mScrollRect;


    // 双击显示信息
    private readonly List<GameObject> l_InfoItems = new List<GameObject>(); // 双击 右边的 Item
    private float yuanLaiWidth, yuanLaiHidth;
    private GameObject go_ShowTuInfo;

    // 双击显示信息左边
    private RectTransform rtAnimTu;

    private UGUI_SpriteAnim anim_Tu;
    private Slider slider_Width, slider_Height;
    private Text tx_WidthSize, tx_HeightSize;


    // 双击显示信息右边
    private Text tx_InfoName, tx_InfoNum;
    private GameObject go_ItemMoBan;
    private RectTransform rt_ItemContant;
    private Vector2 TuSize = new Vector2(512, 512);


    // 改变大小Slider
    private bool isShowSize;
    private GameObject go_ChangeSize;
    private UGUI_Grid[] l_Grids;
    private Slider slider_ChangeSize;
    private Text tx_GridSize;


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


    public override string GetUIPathForRoot()
    {
        return "Right/EachContant/XuLieTu";
    }


    public override void OnEnable()
    {
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


    private Sprite[] GetSpriteList(ResultBean[] beans)
    {
        Sprite[] sps = new Sprite[beans.Length];
        for (int i = 0; i < beans.Length; i++)
        {
            sps[i] = beans[i].SP;
        }
        return sps;
    }


    IEnumerator LoadInfoItem(ResultBean[] resultBeans)
    {
        foreach (ResultBean bean in resultBeans)
        {
            Transform t = InstantiateMoBan(go_ItemMoBan, rt_ItemContant);
            t.Find("Icon").GetComponent<Image>().sprite = bean.SP;
            t.Find("TextName").GetComponent<Text>().text = bean.SP.name;
            t.Find("TextSize").GetComponent<Text>().text = bean.Width + " x " + bean.Height;
            l_InfoItems.Add(t.gameObject);
            yield return 0;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(rt_ItemContant);
    }


    private RectTransform GetParentRT(EXunLieTu tuType)
    {
        RectTransform rt = null; // 放在那里
        switch (tuType)
        {
            case EXunLieTu.G1Zheng:
                rt = rt_Grid1;
                break;
            case EXunLieTu.G2Zheng_XiTong:
                rt = rt_Grid2;
                break;
            case EXunLieTu.G3Zheng_Big:
                rt = rt_Grid3;
                break;
            case EXunLieTu.G4Two_Heng:
                rt = rt_Grid4_Heng;
                break;
            case EXunLieTu.G4Two_Shu:
                rt = rt_Grid4_Shu;
                break;
            case EXunLieTu.G5Three_Heng:
                rt = rt_Grid5_Heng;
                break;
            case EXunLieTu.G5Three_Shu:
                rt = rt_Grid5_Shu;
                break;
            default:
                throw new Exception("还有其他？");
        }
        return rt;
    }


    private void InitMoBan(Transform t, ResultBean[] resultBeans, EXunLieTu tuType) // 初始化模版
    {
        GameObject go = t.gameObject;
        t.Find("Tu").GetComponent<UGUI_SpriteAnim>().ChangeAnim(GetSpriteList(resultBeans));
        t.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (go.Equals(go_CurrentSelect) && isSelect) // 双击
            {
                mCurrentIndex = tuType;
                l_CurrentResultBeans = resultBeans;
                Btn_OnDoubleItemClick(resultBeans);
            }
            else // 单击
            {
                go_CurrentSelect = go;
                Ctrl_Coroutine.Instance.StartCoroutine(CheckoubleClick());
            }
        });
    }


    private void DeleteOneLine(EXunLieTu type)           // 删除整行
    {
        Ctrl_TextureInfo.Instance.DeleteXuLieTuOneLine(type);
        RectTransform rt = GetParentRT(type);
        for (int i = 0; i < rt.childCount; i++)
        {
            UnityEngine.Object.Destroy(rt.GetChild(i).gameObject);
        }
    }


    #endregion


    protected override void OnStart(Transform root)
    {
        MyEventCenter.AddListener<EXunLieTu, List<FileInfo>, bool>(E_GameEvent.DaoRu_XunLieTu, E_OnDaoRu);
        MyEventCenter.AddListener<EXunLieTu, List<ResultBean>>(E_GameEvent.ResultDaoRu_XunLieTu, E_ResultDaoRu);
        MyEventCenter.AddListener<EGameType>(E_GameEvent.ClickTrue, E_DelteTrue);
        MyEventCenter.AddListener(E_GameEvent.DelteAll, E_DeleteAll);
        MyEventCenter.AddListener<bool>(E_GameEvent.ShowChangeSizeSlider, E_IsShowChangeSize);
        // 模版
        go_MoBan = GetGameObject("Top/Contant/MoBan");


        // 上方
        go_Top = GetGameObject("Top");
        toggle5_Contant = Get<DTToggle5_Fade>("Top/Contant/ScrollView");
        mScrollRect = Get<ScrollRect>("Top/Contant/ScrollView");
        rt_Grid1 = Get<RectTransform>("Top/Contant/ScrollView/Item1");
        rt_Grid2 = Get<RectTransform>("Top/Contant/ScrollView/Item2");
        rt_Grid3 = Get<RectTransform>("Top/Contant/ScrollView/Item3");
        rt_Grid4_Shu = Get<RectTransform>("Top/Contant/ScrollView/Item4/Shu");
        rt_Grid4_Heng = Get<RectTransform>("Top/Contant/ScrollView/Item4/Heng");
        rt_Grid5_Shu = Get<RectTransform>("Top/Contant/ScrollView/Item5/Shu");
        rt_Grid5_Heng = Get<RectTransform>("Top/Contant/ScrollView/Item5/Heng");



        // 底下
        go_Bottom = GetGameObject("Bottom");
        tg_BottomContrl = Get<UGUI_ToggleGroup>("Bottom/Contant");
        tg_BottomContrl.OnChangeValue += E_OnBottomContrlChange;

        #region 双击显示信息

        go_ShowTuInfo = GetGameObject("ShowTuInfo");
        AddButtOnClick("ShowTuInfo/Contant/BtnClose", Btn_OnCloseShowInfo);

        //  左边
        rtAnimTu = Get<RectTransform>("ShowTuInfo/Contant/Left/Contant/Tu/AnimTu");
        anim_Tu = Get<UGUI_SpriteAnim>("ShowTuInfo/Contant/Left/Contant/Tu/AnimTu/Anim");
        tx_WidthSize = Get<Text>("ShowTuInfo/Contant/Left/Contant/SliderWidth/TxValue");
        tx_HeightSize = Get<Text>("ShowTuInfo/Contant/Left/Contant/SliderHeight/TxValue");
        slider_Width = Get<Slider>("ShowTuInfo/Contant/Left/Contant/SliderWidth/Slider");
        slider_Height = Get<Slider>("ShowTuInfo/Contant/Left/Contant/SliderHeight/Slider");
        AddButtOnClick("ShowTuInfo/Contant/Left/Contant/Tu/AnimTu", Btn_OnAnimTuClick);
        AddSliderOnValueChanged(slider_Width, (value) =>
        {
            SetTuSize(value);
        });
        AddSliderOnValueChanged(slider_Height, (value) =>
        {
            SetTuSize(0, value);
        });
        AddButtOnClick("ShowTuInfo/Contant/Left/Contant/BtnSize/BtnFirst", () =>
        {
            SetTuSize(yuanLaiWidth, yuanLaiHidth);
        });
        AddButtOnClick("ShowTuInfo/Contant/Left/Contant/BtnSize/BtnPlusHalf", () =>
        {
            SetTuSize(yuanLaiWidth * 0.5f, yuanLaiHidth * 0.5f);
        });
        AddButtOnClick("ShowTuInfo/Contant/Left/Contant/BtnSize/BtnAddHalf", () =>
        {
            SetTuSize(yuanLaiWidth * 1.5f, yuanLaiHidth * 1.5f);
        });
        AddButtOnClick("ShowTuInfo/Contant/Left/Contant/BtnSize/BtnAddTwo", () =>
        {
            SetTuSize(yuanLaiWidth * 2f, yuanLaiHidth * 2f);
        });

        // 右边
        tx_InfoName = Get<Text>("ShowTuInfo/Contant/Right/InfoName/Name");
        tx_InfoNum = Get<Text>("ShowTuInfo/Contant/Right/InfoNum/TxNum");
        go_ItemMoBan = GetGameObject("ShowTuInfo/Contant/Right/Item/ScrollRect/Contant/MoBan");
        rt_ItemContant = Get<RectTransform>("ShowTuInfo/Contant/Right/Item/ScrollRect/Contant");
        AddButtOnClick("ShowTuInfo/Contant/Right/BtnOpenFolder/Btn", Btn_OnOpenFolder);
        AddButtOnClick("ShowTuInfo/Contant/Right/BtnDelete/Btn", Btn_OnNoSaveThis);

        #endregion



        // 右边
        btn_DaoRu = Get<Button>("Top/Left/DaoRu");
        AddButtOnClick(btn_DaoRu, Btn_OnDaoRu);
        AddButtOnClick("Top/Left/DeleteAll", Btn_DeleteOneLine);


        //改变 Grid 大小
        l_Grids = Gets<UGUI_Grid>("Top/Contant/ScrollView");
        for (int i = 0; i < l_Grids.Length; i++)
        {
            l_Grids[i].CallSize = Ctrl_UserInfo.Instance.L_XuLieTuSize[i].CurrentSize;
        }
        tx_GridSize = Get<Text>("Top/Left/ChangeSize/TxValue");

        go_ChangeSize = GetGameObject("Top/Left/ChangeSize");
        isShowSize = Ctrl_UserInfo.Instance.IsCanChangeSize;
        go_ChangeSize.SetActive(isShowSize);

        slider_ChangeSize = Get<Slider>("Top/Left/ChangeSize/Slider");
        AddSliderOnValueChanged(slider_ChangeSize, Slider_OnGridSizeChange);
        slider_ChangeSize.value = Ctrl_UserInfo.Instance.L_XuLieTuSize[0].ChangeValue;
    }


    //——————————————————— UI —————————————————


    private void Btn_OnDaoRu()                 // 点击导入
    {
        MyOpenFileOrFolder.OpenFile(Ctrl_UserInfo.Instance.DaoRuFirstPath, "选择多个文件（序列图）", EFileFilter.TuAndAll,
            (filePaths) =>
            {
                List<FileInfo> fileInfos = new List<FileInfo>(filePaths.Length);
                foreach (string filePath in filePaths)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (MyFilterUtil.IsTu(fileInfo))
                    {
                        fileInfos.Add(fileInfo);
                    }
                    else
                    {
                        MyLog.Red("选择了其他的格式文件 —— " + fileInfo.Name);
                    }
                }
                MyEventCenter.SendEvent(E_GameEvent.DaoRu_XunLieTu, mCurrentIndex, fileInfos, true);
            });
    }



    private void Btn_DeleteOneLine()           // 点击删除
    {
        string tittle = "删除";
        switch (mCurrentIndex)
        {
            case EXunLieTu.G1Zheng:
                tittle += " 小等边 的所有序列图片？";
                break;
            case EXunLieTu.G2Zheng_XiTong:
                tittle += " 系统等边 的所有序列图片？";
                break;
            case EXunLieTu.G3Zheng_Big:
                tittle += " 大等边 的所有序列图片？";
                break;
            case EXunLieTu.G4Two_Heng:
                tittle += " 2倍 的所有序列图片？";
                break;
            case EXunLieTu.G5Three_Heng:
                tittle += " 3倍 的所有序列图片？";
                break;
        }
        MyEventCenter.SendEvent(E_GameEvent.ShowIsSure, EGameType.XunLieTu, tittle);
    }



    private void E_OnBottomContrlChange(string changeName) // 总控制，底下的切换
    {
        switch (changeName)
        {
            case ITEM_STR1:
                toggle5_Contant.Change2One();
                mCurrentIndex = EXunLieTu.G1Zheng;
                btn_DaoRu.interactable = true;
                mScrollRect.content = rt_Grid1;
                break;
            case ITEM_STR2:
                toggle5_Contant.Change2Two();
                mCurrentIndex = EXunLieTu.G2Zheng_XiTong;
                btn_DaoRu.interactable = true;
                mScrollRect.content = rt_Grid2;
                break;
            case ITEM_STR3:
                toggle5_Contant.Change2Three();
                mCurrentIndex = EXunLieTu.G3Zheng_Big;
                btn_DaoRu.interactable = true;
                mScrollRect.content = rt_Grid3;
                break;
            case ITEM_STR4:
                toggle5_Contant.Change2Four();
                mCurrentIndex = EXunLieTu.G4Two_Heng;
                btn_DaoRu.interactable = false;
                mScrollRect.content = rt_Grid4_Shu;
                break;
            case ITEM_STR5:
                toggle5_Contant.Change2Five();
                mCurrentIndex = EXunLieTu.G5Three_Heng;
                btn_DaoRu.interactable = false;
                mScrollRect.content = rt_Grid5_Shu;
                break;
        }
        if (isShowSize)
        {
            if (mCurrentIndex == EXunLieTu.G1Zheng || mCurrentIndex == EXunLieTu.G2Zheng_XiTong || mCurrentIndex == EXunLieTu.G3Zheng_Big)
            {
                go_ChangeSize.SetActive(true);
                slider_ChangeSize.value = Ctrl_UserInfo.Instance.L_XuLieTuSize[(int)mCurrentIndex].ChangeValue;
                tx_GridSize.text = l_Grids[(int)mCurrentIndex].CallSize.x.ToString();
            }
            else
            {
                go_ChangeSize.SetActive(false);

            }
        }


    }

    private void Slider_OnGridSizeChange(float value)          // 改变 Grid 大小
    {
        int gridIndex = (int)mCurrentIndex;
        int tmpValue = (int)value;
        Ctrl_UserInfo.Instance.L_XuLieTuSize[gridIndex].ChangeValue = tmpValue;
        Vector2 yuanSize = Ctrl_UserInfo.Instance.L_XuLieTuSize[gridIndex].YuanSize;
        Ctrl_UserInfo.Instance.L_XuLieTuSize[gridIndex].CurrentSize = new Vector2(yuanSize.x + tmpValue, yuanSize.y + tmpValue);
        l_Grids[gridIndex].CallSize = Ctrl_UserInfo.Instance.L_XuLieTuSize[gridIndex].CurrentSize;
        tx_GridSize.text = l_Grids[gridIndex].CallSize.x.ToString();


    }





    //—————————————————— 双击显示信息 ——————————————————

    private void Btn_OnDoubleItemClick(ResultBean[] resultBeans) // 双击其一一个 Item 了
    {
        go_ShowTuInfo.SetActive(true);
        go_Top.SetActive(false);
        go_Bottom.SetActive(false);
        tx_InfoName.text = resultBeans[0].SP.name;
        tx_InfoNum.text = resultBeans.Length.ToString();
        anim_Tu.ChangeAnim(GetSpriteList(resultBeans));
        yuanLaiWidth = resultBeans[0].Width;
        yuanLaiHidth = resultBeans[0].Height;
        SetTuSize(yuanLaiWidth, yuanLaiHidth);
        Ctrl_Coroutine.Instance.StartCoroutine(LoadInfoItem(resultBeans));
    }


    private void Btn_OnCloseShowInfo() // 关闭打开的信息
    {
        go_ShowTuInfo.SetActive(false);
        go_Top.SetActive(true);
        go_Bottom.SetActive(true);
        Ctrl_Coroutine.Instance.StopAllCoroutines();
        for (int i = 0; i < l_InfoItems.Count; i++)
        {
            UnityEngine.Object.Destroy(l_InfoItems[i]);
        }
        l_InfoItems.Clear();
    }


    private void Btn_OnAnimTuClick() // 点击头像
    {
        string path = l_CurrentResultBeans[0].File.FullName;
        Application.OpenURL(path);
    }


    private void Btn_OnOpenFolder() // 点击打开文件夹
    {
        DirectoryInfo directoryInfo = l_CurrentResultBeans[0].File.Directory;
        if (directoryInfo != null)
        {
            string path = directoryInfo.FullName;
            Application.OpenURL(path);
        }
    }


    private void Btn_OnNoSaveThis() // 点击不保存这个
    {
        string[] paths = new string[l_CurrentResultBeans.Length];
        for (int i = 0; i < l_CurrentResultBeans.Length; i++)
        {
            paths[i] = l_CurrentResultBeans[i].File.FullName;
        }
        Ctrl_TextureInfo.Instance.DeleteXuLieTuSave(mCurrentIndex, paths);
        UnityEngine.Object.Destroy(go_CurrentSelect);
        Btn_OnCloseShowInfo();
    }


    //—————————————————— 事件 ——————————————————


    private void E_OnDaoRu(EXunLieTu tuType, List<FileInfo> fileInfos, bool isSave) // 接收导入事件 ，创建一个序列图
    {
        string[] paths = new string[fileInfos.Count];
        ;
        for (int i = 0; i < fileInfos.Count; i++)
        {
            paths[i] = fileInfos[i].FullName;
        }

        // 保存一下信息
        if (isSave)
        {
            bool isOk = Ctrl_TextureInfo.Instance.SaveXunLieTu(tuType, paths);
            MyEventCenter.SendEvent<EGameType, bool, List<FileInfo>>(E_GameEvent.DaoRuResult, EGameType.XunLieTu, isOk,null);
            if (!isOk)
            {
                return;
            }
        }

        // 1. 创建一个实例
        Transform t = InstantiateMoBan(go_MoBan, GetParentRT(tuType), CREATE_FILE_NAME);

        // 2. 加载图片
        MyLoadTu.LoadMultipleTu(paths, (resBean) =>
        {
            // 3. 完成后把图集加上去
            InitMoBan(t, resBean, tuType);
        });
    }


    private void E_ResultDaoRu(EXunLieTu tuType, List<ResultBean> resultBeans)
    {
        string[] paths = new string[resultBeans.Count];
        ;
        for (int i = 0; i < resultBeans.Count; i++)
        {
            paths[i] = resultBeans[i].File.FullName;
        }
        // 保存一下信息
        bool isOk = Ctrl_TextureInfo.Instance.SaveXunLieTu(tuType, paths);
        MyEventCenter.SendEvent<EGameType, bool, List<FileInfo>>(E_GameEvent.DaoRuResult, EGameType.XunLieTu, isOk,null);
        if (!isOk)
        {
            return;
        }
        Transform t = InstantiateMoBan(go_MoBan, GetParentRT(tuType), CREATE_FILE_NAME);
        InitMoBan(t, resultBeans.ToArray(), tuType);
    }





    //————————————————————————————————————

    private void E_DelteTrue(EGameType type)             // 真的删除
    {
        if (type == EGameType.XunLieTu)
        {
            switch (mCurrentIndex)
            {
                case EXunLieTu.G4Two_Heng:
                case EXunLieTu.G4Two_Shu:
                    DeleteOneLine(EXunLieTu.G4Two_Heng);
                    DeleteOneLine(EXunLieTu.G4Two_Shu);
                    break;
                case EXunLieTu.G5Three_Heng:
                case EXunLieTu.G5Three_Shu:
                    DeleteOneLine(EXunLieTu.G5Three_Heng);
                    DeleteOneLine(EXunLieTu.G5Three_Shu);
                    break;
                default:
                    DeleteOneLine(mCurrentIndex);
                    break;
            }
        }
    }



    private void E_DeleteAll()                           // 删除所有
    {
        go_CurrentSelect = null;
        l_CurrentResultBeans = null;
        foreach (EXunLieTu type in Enum.GetValues(typeof(EXunLieTu)))
        {
            DeleteOneLine(type);
        }

    }



    private void E_IsShowChangeSize(bool isOn)          // 是否显示改变大小的Slider
    {
        isShowSize = isOn;
        go_ChangeSize.SetActive(isOn);

    }



}