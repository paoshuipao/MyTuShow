using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using PSPUtil;
using PSPUtil.Control;
using PSPUtil.StaticUtil;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public partial class Game_DaoRu : SubUI
{

    public void Show()
    {
        if (isFirstShow)    // 第一次显示
        {
            isFirstShow = false;

            // 收藏
            List<string> favPaths = Ctrl_UserInfo.Instance.L_FavoritesPath;
            foreach (string favPath in favPaths)
            {
                CreateFavoites(favPath);     // 获取之前已保存的，创建收藏按键出来
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt_ShuQianContant);


            // 过滤设置
            mFileBrowser.SetFilters(MyFilterUtil.ONLY_TU_AUDIO_FILTER);

            // 中间内容
            RefreshMiddleContent();          // 生成中间的内容
        }
    }



    #region 私有

    private bool isFirstShow = true;      // 是否第一次Show
    private FileBrowser mFileBrowser;     // 核心功能
    private RectTransform rt_Right;       // 总的右边
    private readonly Color LBColor = MyColor.GetColor(MyEnumColor.LightBlue);




    enum MiddleButtonType // 中间点击按钮的类型
    {
        File,        // 文件
        Folder,      // 文件夹
        Drive,       // 磁盘
        Computer,    // 我的电脑
        ZhuoMain,    // 桌面
        Music,       // 音乐文件
    }



    private GameObject go_CurrentSelect;    // 当前选择的按钮



    // 中间
    private bool isShift = false;                 // 是否按下 Shift
    private bool isNormalClick = true;            // 是否普通的单击
    private bool isSelect;                  // 是否之前点击了
    private readonly Dictionary<GameObject, GameObject> chooseGOK_BgV = new Dictionary<GameObject, GameObject>();    // 选中的对象作为Key，其背景作为 Value
    private readonly List<GameObject> l_MiddleItems = new List<GameObject>();   // 中间每一个 Item
    private Transform t_MiddleGrid;         // 中间 Grid  设置模版的父位置
    private UGUI_Grid grid_Contant;         // 中间核心 Grid

    //模版
    private GameObject moBan_File;          // 模版_文件
    private GameObject moBan_Folder;        // 模版_文件夹
    private GameObject moBan_YinPan;        // 模版_磁盘
    private GameObject moBan_Computer;      // 模版_我的电脑
    private GameObject moBan_ZhuoMain;      // 模版_桌面
    private GameObject moBan_Music;         // 模版_音乐文件
    

    private const string FILE_NAME = "File";           // 根据模版产生的对象的名字
    private const string FLODER_NAME = "Folder";
    private const string YINPAN_NAME = "YinPan";
    private const string COMPUTER_NAME = "Computer";
    private const string ZHUOMAIN_NAME = "ZhuoMain";
    private const string MUSIC_NAME = "Music";

    private IEnumerator CheckoubleClick()           // 检测是否双击
    {
        isSelect = true;
        yield return new WaitForSeconds(Ctrl_UserInfo.DoubleClickTime);
        isSelect = false;

    }




    #region 头部栏
    private string[] l_AddressPaths = new string[5];

    // 上
    private GameObject go_ItemPath2, go_ItemPath3, go_ItemPath4, go_ItemPath5, go_Add;
    private RectTransform rt_Top;
    private UGUI_ToggleGroup tg_ItemPath;
    private Text tx_TopPath1, tx_TopPath2, tx_TopPath3, tx_TopPath4, tx_TopPath5;
    private ushort mCurrentTopIndex = 0;                  // 当前上头的索引   1 就是第一页

    // 下
    public Text tx_Path;          // 搜索字段文本区域
    private Toggle toggle_Star;   // 星星（收藏）
    private Button btn_HistoryPre,btn_HistoryNext;

    #endregion

    #region 大小
    private static readonly Vector2 SIZE_BIG = new Vector2(200, 240);    //最大尺寸
    private static readonly Vector2 SIZE_MIDDLE = new Vector2(136, 169);  //中等尺寸
    private static readonly Vector2 SIZE_SMALL = new Vector2(88, 110);    //最小尺寸
    private Text tx_SizeBig, tx_SizeMiddle, tx_SizeSmall; // 字体没选中的用灰色
    private Text tx_Size1;
    private UGUI_ToggleGroup tg_ModeSize;
    private GameObject go_ModeSize;
    private DOTweenAnimation anim_SizeIcon;

    #endregion

    #region 文件过滤
    private bool isOnlyShowFolder;                 // true：只显示文件夹
    private Text tx_FilterAll, tx_FilterTexture, tx_FilterFolder; // 字体没选中的用灰色
    private Text tx_Filter1;
    private UGUI_ToggleGroup tg_FilterMode;
    private GameObject go_ModeFilter;
    private DOTweenAnimation anim_FilterIcon;
    private const string SHOW_NAME1 = "显示全部";
    private const string SHOW_NAME2 = "图片音频";
    private const string SHOW_NAME3 = "仅文件夹";





    #endregion

    #region 排序

    private Text tx_SortName, tx_SortType, tx_SortDate; // 字体没选中的用灰色
    private Text tx_Sort1;
    private UGUI_ToggleGroup tg_SortMode;
    private GameObject go_ModeSorting;
    private DOTweenAnimation anim_SortIcon;

    #endregion

    #region 书签

    private GameObject go_ShuQian;
    private GameObject moBan_Favorites; // 模板_单个书签按钮
    private RectTransform rt_ShuQianContant;
    private readonly List<GameObject> mb_Favorites = new List<GameObject>();

    #endregion

    #region 导入

    private Text tx_TipZhang;
    private Button btnDaoRu;

    #endregion

    #region 框选

    private RectTransform rt_Kuang;
    private KuangXuan mKuangXuan;

    #endregion

    #region 单张详细信息

    private GameObject go_ShowTuInfo;
    private FileInfo mCurrentFile;
    private float yuanLaiWidth, yuanLaiHidth;

    // 左边
    private Image Sp_Tu;
    private RectTransform rt_TuSize;
    private Slider slider_Width,slider_Height;
    private Vector2 TuSize = new Vector2(512, 512);
    private Text tx_WidthSize, tx_HeightSize;

    //右边
    private Text tx_FileName, tx_HuoZhui,tx_TuSize;


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

    #region 多张导入

    private GameObject mCuurentChooseBg;
    private GameObject go_ShowDuoTu;
    private GameObject moBan_DuoTuItem;
    private RectTransform rt_DuoTuContant;

    private void ChangeDicIndex(GameObject go, int add)               // 改 Item 上下
    {
        List<GameObject> goList = new List<GameObject>(itemSelectK_ResutltV.Keys);
        int index = goList.IndexOf(go);    // 索引是 0 开始   GetSiblingIndex 是从 1 开始
        int want2Index = index + add;      // 要到的 索引
        if (want2Index < 0 || want2Index >= goList.Count)
        {
            return;
        }

        GameObject changeGO = goList[want2Index];
        // List 交换
        goList[index] = changeGO;
        goList[want2Index] = go;

        go.transform.SetSiblingIndex(want2Index + 1);    // 因为从 1 开始
        Dictionary<GameObject, ResultBean> tmp = new Dictionary<GameObject, ResultBean>();

        foreach (GameObject tmpGO in goList)
        {
            tmp.Add(tmpGO, itemSelectK_ResutltV[tmpGO]);
        }
        itemSelectK_ResutltV = tmp;
    }

    #endregion

    #region 导入结果

    private GameObject go_Result,go_TittleOK,go_TittleError,go_ErrorInfo;
    private Text tx_GoTo;
    private RectTransform rt_ErrorContant;
    private GameObject go_ErrorMoBan;



    #endregion





    #region 私有


    public override void OnEnable()
    {
        MyEventCenter.AddListener<EGameType, bool,List<FileInfo>>(E_GameEvent.DaoRuResult, ShowResult);

    }

    public override void OnDisable()
    {
        MyEventCenter.RemoveListener<EGameType, bool, List<FileInfo>>(E_GameEvent.DaoRuResult, ShowResult);

    }


    public override string GetUIPathForRoot()
    {
        return "Right/EachContant/DaoRu";
    }


    #endregion

    #endregion



    protected override void OnStart(Transform root)
    {
        // 总
        rt_Right = Get<RectTransform>("Right/Contant");

        l_AddressPaths[0] = Ctrl_UserInfo.Instance.ShowFirstPath;
        l_AddressPaths[1] = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        l_AddressPaths[2] = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        l_AddressPaths[3] = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        l_AddressPaths[4] = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        mFileBrowser = string.IsNullOrEmpty(l_AddressPaths[0]) ? new FileBrowser() : new FileBrowser(l_AddressPaths[0]);

        #region 书签

        go_ShuQian = GetGameObject("Right/Contant/GO_ShuQian");
        moBan_Favorites = GetGameObject("Right/Contant/GO_ShuQian/ScrollView/Contant/MoBan_ItemFav");
        AddToggleOnValueChanged("Right/Contant/Toggle_ShuQian", OnToggle_ShuQian);
        rt_ShuQianContant = Get<RectTransform>("Right/Contant/GO_ShuQian/ScrollView/Contant");

        #endregion

        #region 大小

        go_ModeSize = GetGameObject("Right/Contant/Size2_Mode");
        tx_Size1 = Get<Text>("Right/Contant/Size1_Btn/Text");
        tx_SizeBig = Get<Text>("Right/Contant/Size2_Mode/Contant/ItemBig/Text");
        tx_SizeMiddle = Get<Text>("Right/Contant/Size2_Mode/Contant/ItemMiddle/Text");
        tx_SizeSmall = Get<Text>("Right/Contant/Size2_Mode/Contant/ItemSmall/Text");
        tg_ModeSize = Get<UGUI_ToggleGroup>("Right/Contant/Size2_Mode/Contant");
        anim_SizeIcon = Get<DOTweenAnimation>("Right/Contant/Size1_Btn/Left");
        tg_ModeSize.OnChangeValue += OnToggle_ChangeSizeMode;
        tg_ModeSize.OnEachClick += Btn_CloseSize;
        AddButtOnClick("Right/Contant/Size1_Btn", Btn_OpenSize);

        #endregion

        #region 过滤

        go_ModeFilter = GetGameObject("Right/Contant/Filter2_Mode");
        tx_Filter1 = Get<Text>("Right/Contant/Filter1_Btn/Text");
        tx_FilterAll = Get<Text>("Right/Contant/Filter2_Mode/Contant/ItemAll/Text");
        tx_FilterTexture = Get<Text>("Right/Contant/Filter2_Mode/Contant/ItemTexture/Text");
        tx_FilterFolder = Get<Text>("Right/Contant/Filter2_Mode/Contant/ItemFolder/Text");
        tg_FilterMode = Get<UGUI_ToggleGroup>("Right/Contant/Filter2_Mode/Contant");
        anim_FilterIcon = Get<DOTweenAnimation>("Right/Contant/Filter1_Btn/Left");
        tg_FilterMode.OnChangeValue += OnToggle_ChangeFilterMode;
        tg_FilterMode.OnEachClick += Btn_CloseFilter;
        AddButtOnClick("Right/Contant/Filter1_Btn", Btn_OpenFilter);


        #endregion

        #region 排序

        go_ModeSorting = GetGameObject("Right/Contant/Sorting2_Mode");
        tx_Sort1 = Get<Text>("Right/Contant/Sorting1_Btn/Text");
        tx_SortName = Get<Text>("Right/Contant/Sorting2_Mode/Contant/ItemName/Text");
        tx_SortType = Get<Text>("Right/Contant/Sorting2_Mode/Contant/ItemType/Text");
        tx_SortDate = Get<Text>("Right/Contant/Sorting2_Mode/Contant/ItemDate/Text");
        tg_SortMode = Get<UGUI_ToggleGroup>("Right/Contant/Sorting2_Mode/Contant");
        anim_SortIcon = Get<DOTweenAnimation>("Right/Contant/Sorting1_Btn/Left");
        tg_SortMode.OnChangeValue += OnToggle_ChangeSortMode;
        tg_SortMode.OnEachClick += Btn_CloseSorting;
        AddButtOnClick("Right/Contant/Sorting1_Btn", Btn_OpenSorting);

        #endregion

        #region 头部栏

        // 上方的头部菜单
        go_ItemPath2 = GetGameObject("Top/Top/ItemPath2");
        go_ItemPath3 = GetGameObject("Top/Top/ItemPath3");
        go_ItemPath4 = GetGameObject("Top/Top/ItemPath4");
        go_ItemPath5 = GetGameObject("Top/Top/ItemPath5");
        go_Add = GetGameObject("Top/Top/Add");
        rt_Top = Get<RectTransform>("Top/Top");
        tg_ItemPath = Get<UGUI_ToggleGroup>("Top/Top");
        tg_ItemPath.OnChangeValue += E_OnTopPathChange;
        AddButtOnClick("Top/Top/Add/Btn", Btn_AddItem);
        AddButtOnClick("Top/Top/ItemPath2/Close", () => { CloseItemPath(go_ItemPath2); });
        AddButtOnClick("Top/Top/ItemPath3/Close", () => { CloseItemPath(go_ItemPath3); });
        AddButtOnClick("Top/Top/ItemPath4/Close", () => { CloseItemPath(go_ItemPath4); });
        AddButtOnClick("Top/Top/ItemPath5/Close", () => { CloseItemPath(go_ItemPath5); });

        tx_TopPath1 = Get<Text>("Top/Top/ItemPath1/Text");
        tx_TopPath2 = Get<Text>("Top/Top/ItemPath2/Text");
        tx_TopPath3 = Get<Text>("Top/Top/ItemPath3/Text");
        tx_TopPath4 = Get<Text>("Top/Top/ItemPath4/Text");
        tx_TopPath5 = Get<Text>("Top/Top/ItemPath5/Text");



        // 下方的 历史、地址栏
        tx_Path = Get<Text>("Top/Bottom/Middle/AddressPath/Text");

        // 收藏的星星
        toggle_Star = Get<Toggle>("Top/Bottom/Middle/ToggleStar");
        AddToggleOnValueChanged(toggle_Star, Toggle_ChangeIsStar);

        // 历史的左右按钮
        btn_HistoryPre = Get<Button>("Top/Bottom/Left/BtnLeft");
        btn_HistoryNext = Get<Button>("Top/Bottom/Left/BtnRight");
        AddButtOnClick(btn_HistoryPre, Btn_OnHistoryPre);
        AddButtOnClick(btn_HistoryNext, Btn_OnHistoryNext);
        // 中间的地址栏
        AddButtOnClick("Top/Bottom/Middle/AddressPath/Btn", Btn_OnClickAddressPath);
        // 右边上层
        AddButtOnClick("Top/Bottom/Right/BtnUp", Btn_OnGoToParent);
        AddButtOnClick("Top/Bottom/Right/BtnOpenFolder", Btn_OpenFolder);



        #endregion

        #region 导入


        tx_TipZhang = Get<Text>("Right/BtnDaoRu/Tip/Num");
        btnDaoRu = Get<Button>("Right/BtnDaoRu");
        AddButtOnClick(btnDaoRu, Btn_OnDaoRuClick);

        #endregion

        #region 中间

        t_MiddleGrid = GetTransform("Bottom/ScrollView/Contant");
        grid_Contant = t_MiddleGrid.GetComponent<UGUI_Grid>();
        moBan_File = GetGameObject("Bottom/ScrollView/MoBan_File");
        moBan_Folder = GetGameObject("Bottom/ScrollView/MoBan_Folder");
        moBan_YinPan = GetGameObject("Bottom/ScrollView/MoBan_YinPan");
        moBan_Computer = GetGameObject("Bottom/ScrollView/MoBan_Computer");
        moBan_ZhuoMain = GetGameObject("Bottom/ScrollView/MoBan_ZhuoMain");
        moBan_Music = GetGameObject("Bottom/ScrollView/MoBan_Music");


        MyEventCenter.AddListener(E_GameEvent.OnClickDown_Shift, E_OnShiftClick);    
        MyEventCenter.AddListener(E_GameEvent.OnClickUp_Shift, E_OnShiftUp);
        MyEventCenter.AddListener(E_GameEvent.OnClickDown_Ctrl, E_OnCtrlClick);
        MyEventCenter.AddListener(E_GameEvent.OnClickUp_Ctrl, E_OnCtrlUp);



        #endregion

        #region 框选
        rt_Kuang = Get<RectTransform>("Bottom/ScrollView/KuangXuan");
        mKuangXuan = rt_Kuang.GetComponent<KuangXuan>();
        mKuangXuan.Init(chooseGOK_BgV);
        UGUI_KuangXuan kuangXuan = Get<UGUI_KuangXuan>("Bottom/ScrollView");

        kuangXuan.E_OnClickDown += E_OnClickKuangDown;
        kuangXuan.E_OnDarg += E_OnKuangDarg;
        kuangXuan.E_OnClickUp += E_OnClickKuangUp;
        #endregion

        #region 单张信息

        go_ShowTuInfo = GetGameObject("ShowTuInfo");
        AddButtOnClick("ShowTuInfo/BtnClose", Btn_CloseTuInfo);

        // 左边
        Sp_Tu = Get<Image>("ShowTuInfo/Left/Contant/Tu/TuSize/Image");
        rt_TuSize = Get<RectTransform>("ShowTuInfo/Left/Contant/Tu/TuSize");
        slider_Width = Get<Slider>("ShowTuInfo/Left/Contant/SliderWidth/Slider");
        tx_WidthSize = Get<Text>("ShowTuInfo/Left/Contant/SliderWidth/TxValue");
        slider_Height = Get<Slider>("ShowTuInfo/Left/Contant/SliderHeight/Slider");
        tx_HeightSize = Get<Text>("ShowTuInfo/Left/Contant/SliderHeight/TxValue");
        AddSliderOnValueChanged(slider_Width, (value) =>
        {
            SetTuSize(value);
        });
        AddSliderOnValueChanged(slider_Height, (value) =>
        {
            SetTuSize(0, value);
        });
        AddButtOnClick("ShowTuInfo/Left/Contant/BtnSize/BtnPlusHalf", () =>
        {

            SetTuSize(yuanLaiWidth * 0.5f, yuanLaiHidth * 0.5f);
        });
        AddButtOnClick("ShowTuInfo/Left/Contant/BtnSize/BtnFirst",() =>
        {
            SetTuSize(yuanLaiWidth, yuanLaiHidth);
        });
        AddButtOnClick("ShowTuInfo/Left/Contant/BtnSize/BtnAddHalf", () =>
        {
            SetTuSize(yuanLaiWidth * 1.5f, yuanLaiHidth * 1.5f);

        });
        AddButtOnClick("ShowTuInfo/Left/Contant/BtnSize/BtnAddTwo", () =>
        {
            SetTuSize(yuanLaiWidth * 2f, yuanLaiHidth * 2f);
        });


        // 右边
        tx_FileName = Get<Text>("ShowTuInfo/Right/InfoName/Name");
        tx_HuoZhui = Get<Text>("ShowTuInfo/Right/InfoHuoZhui/TxNum");
        tx_TuSize = Get<Text>("ShowTuInfo/Right/InfoSize/TxNum");
        AddButtOnClick("ShowTuInfo/Right/BtnOpenFolder/BtnOpenFolder", Btn_OnClickOpenFolder);
        AddButtOnClick("ShowTuInfo/Right/BtnOpenFolder/BtnOpenFile", Btn_OnClickOpenFile);
        // 透明图
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR1", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.XiTong);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR2", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.WenZi);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR3", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.WuQi);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR4", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.DaoJu);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_Png/Contant/BtnDR5", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.ChengJi);
        });

        // 集合序列图
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR1", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT1);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR2", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT2);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR3", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT3);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR4", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT4);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_JiHeXuLie/Contant/BtnDR5", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT5);
        });



        // Jpg
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR1", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg1);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR2", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg2);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR3", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg3);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR4", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg4);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_Jpg/Contant/BtnDR5", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg5);
        });


        // 集合图
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR1", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe1);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR2", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe2);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR3", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe3);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR4", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe4);
        });
        AddButtOnClick("ShowTuInfo/Right/Item/ScrollRect/Contant/Item_JiHe/Contant/BtnDR5", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe5);
        });

        #endregion

        #region 多张导入

        rt_DuoTuContant = Get<RectTransform>("ShowDuoTu/Top/ScrollView/Contant");
        go_ShowDuoTu = GetGameObject("ShowDuoTu");
        moBan_DuoTuItem = GetGameObject("ShowDuoTu/Top/ScrollView/Contant/MoBan");


        AddButtOnClick("ShowDuoTu/BtnClose",Btn_OnCloseDuoTu);
        // 序列图
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_XunLieTu/Contant/Item1/Btn", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G1Zheng,0);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_XunLieTu/Contant/Item2/Btn", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G2Zheng_XiTong,1);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_XunLieTu/Contant/Item3/Btn", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G3Zheng_Big,2);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_XunLieTu/Contant/Item4/BtnHeng", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G4Two_Heng,3);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_XunLieTu/Contant/Item4/BtnShu", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G4Two_Shu,3);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_XunLieTu/Contant/Item5/BtnHeng", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G5Three_Heng,4);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_XunLieTu/Contant/Item5/BtnShu", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G5Three_Shu,4);
        });
       
        // 集合序列图
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_JiHeXuLie/Contant/Btn1/BtnMDR", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT1);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_JiHeXuLie/Contant/Btn2/BtnMDR", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT2);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_JiHeXuLie/Contant/Btn3/BtnMDR", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT3);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_JiHeXuLie/Contant/Btn4/BtnMDR", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT4);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_JiHeXuLie/Contant/Btn5/BtnMDR", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT4);
        });

        // 透明图
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_Png/Contant/Btn1/BtnMDR", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.XiTong);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_Png/Contant/Btn2/BtnMDR", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.WenZi);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_Png/Contant/Btn3/BtnMDR", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.WuQi);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_Png/Contant/Btn4/BtnMDR", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.DaoJu);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_Png/Contant/Btn5/BtnMDR", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.ChengJi);
        });

        // Jpg
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_Jpg/Contant/Btn1/BtnMDR", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg1);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_Jpg/Contant/Btn2/BtnMDR", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg2);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_Jpg/Contant/Btn3/BtnMDR", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg3);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_Jpg/Contant/Btn4/BtnMDR", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg4);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_Jpg/Contant/Btn5/BtnMDR", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg5);
        });




        // 集合图
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_JiHe/Contant/Btn1/BtnMDR", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe1);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_JiHe/Contant/Btn2/BtnMDR", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe2);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_JiHe/Contant/Btn3/BtnMDR", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe3);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_JiHe/Contant/Btn4/BtnMDR", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe4);
        });
        AddButtOnClick("ShowDuoTu/BottomContrl/Contant/Item_JiHe/Contant/Btn5/BtnMDR", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe5);
        });

        #endregion

        #region 导入结果


        go_Result = GetGameObject("Result");
        tx_GoTo = Get<Text>("Result/Contant/BtnGoTo/Text");
        go_TittleOK = GetGameObject("Result/Contant/OK");
        go_TittleError = GetGameObject("Result/Contant/Error");
        rt_ErrorContant = Get<RectTransform>("Result/Contant/Error/Contant");
        go_ErrorMoBan = GetGameObject("Result/Contant/Error/Contant/MoBan");
        go_ErrorInfo = GetGameObject("Result/Contant/ErrorInfo");


        AddButtOnClick("Result/Contant/BtnGoTo", GoToDaoRuWhere);
        AddButtOnClick("Result/Contant/BtnFanHui",JiXuDaoRu);
        #endregion




    }


    //—————————————————— 中间——————————————————

    private readonly Dictionary<GameObject, ResultBean> allGoK_ResultBeanV = new Dictionary<GameObject, ResultBean>();


    private void RefreshMiddleContent()                                                        // 刷新中间的内容
    {
        //——————————————————  1. 先清除原来的   ——————————————————
        btnDaoRu.interactable = false;                  // 不能导入
        ClearAllChooseZhong();                          // 清除所有选中的
        Ctrl_Coroutine.Instance.StopAllCoroutines();    // 关闭所有协程
        for (int i = 0; i < l_MiddleItems.Count; i++)   // 删除原来生成的
        {
            Object.Destroy(l_MiddleItems[i].gameObject);
        }
        l_MiddleItems.Clear();
        allGoK_ResultBeanV.Clear();

        //—————————————————— 2. 判断是否桌面，如果是，再添加我的电脑——————————————————

        DirectoryInfo dir = mFileBrowser.GetCurrentDirectory();      // 当前文件夹路径
        if (null != dir && dir.FullName == System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop))
        {
            AddMiddleButton(dir, MiddleButtonType.Computer);       // 我的电脑
        }

        //—————————————————— 3. 添加文件夹和文件——————————————————
        DirectoryInfo[] folderList = mFileBrowser.GetChildDirectories();        // 获得所有文件夹
        Ctrl_Coroutine.Instance.StartCoroutine(LoadFileAndFolder(folderList, dir));



        //—————————————————— 4. 设置头部栏 历史、地址、书签——————————————————



        btn_HistoryPre.interactable = mFileBrowser.GetIsHasHistoryPre;          // 有没有上下历史
        btn_HistoryNext.interactable = mFileBrowser.GetIsHasHistoryNext;


        // 设置头部栏的地址
        string topPath;
        if (dir != null) 
        {
            tx_Path.text = dir.FullName;
            topPath = dir.Name;
            l_AddressPaths[mCurrentTopIndex] = dir.FullName;
            Ctrl_UserInfo.Instance.ShowFirstPath = dir.FullName;
        }
        else
        {
            tx_Path.text = "计算机";
            topPath = "计算机";
        }

        switch (mCurrentTopIndex)
        {
            case 0:
                tx_TopPath1.text = topPath;
                break;
            case 1:
                tx_TopPath2.text = topPath;
                break;
            case 2:
                tx_TopPath3.text = topPath;
                break;
            case 3:
                tx_TopPath4.text = topPath;
                break;
            case 4:
                tx_TopPath5.text = topPath;
                break;
        }



        // 这个路径是否书签
        bool isFavorite = false; 
        foreach (string path in Ctrl_UserInfo.Instance.L_FavoritesPath)
        {
            if (tx_Path.text.Equals(path))
            {
                isFavorite = true;
                break;
            }
        }
        toggle_Star.isOn = isFavorite;



    }


    IEnumerator LoadFileAndFolder(DirectoryInfo[] foloderList, DirectoryInfo currentDic)       // 加载文件和文件夹
    {
        for (int i = 0; i < foloderList.Length; i++)
        {
            if (foloderList[i].Parent != null)
            {
                AddMiddleButton(foloderList[i], MiddleButtonType.Folder);       // 有父文件夹的是文件夹
            }
            else
            {
                AddMiddleButton(foloderList[i], MiddleButtonType.Drive);         // 没有的就是磁盘啊
            }
            yield return 0;
        }

        if (!isOnlyShowFolder)
        {

            FileInfo[] files = mFileBrowser.GetFiles();   // 获得所有文件
            foreach (FileInfo fileInfo in files)
            {
                if (MyFilterUtil.IsAudio(fileInfo))
                {
                    AddMiddleButton(fileInfo, MiddleButtonType.Music);
                }
                else
                {
                    Transform t = AddMiddleButton(fileInfo, MiddleButtonType.File);
                    if (MyFilterUtil.IsTu(fileInfo))          // 是图片那就加载图片
                    {
                        InitMoBan_Tu(t, fileInfo);
                    }
                }
                yield return new WaitForEndOfFrame();

            }
        }


        if (null == currentDic)
        {
            AddMiddleButton(null, MiddleButtonType.ZhuoMain);       // 桌面
        }

    }


    private void Btn_OnClickOpenFile()                // 点击打开当前路径的文件
    {
        Application.OpenURL(mCurrentFile.FullName);

    }

    private void Btn_OnClickOpenFolder()              // 点击打开文件夹(适用于文件打开信息的点击)
    {
        if (mCurrentFile.Directory != null)
        {
            Application.OpenURL(mCurrentFile.Directory.FullName);
        }
    }



    #region 中间


    private void E_OnShiftClick()                // 按下了 Shift
    {
        isShift = true;
    }


    private void E_OnShiftUp()                   // 松开了 Shift
    {
        isShift = false;

    }


    private void E_OnCtrlClick()                 // 按下 Ctrl
    {
        isNormalClick = false;

    }

    private void E_OnCtrlUp()                    // 松开 Shift
    {
        isNormalClick = true;

    }


    private void Btn_OnDaoRuClick()                                                 // 点击导入
    {
        if (chooseGOK_BgV.Count > 1)               // 选择了 多张
        {
            ShowDuoTu();
        }
        else if (chooseGOK_BgV.Count == 1)       // 选择了 1 张
        {
            foreach (GameObject go in chooseGOK_BgV.Keys)
            {
                ShowTuInfo(allGoK_ResultBeanV[go]);
                break;
            }
        }
        else
        {
            MyLog.Red("不可能吧");
        }

    }


    private Transform AddMiddleButton(FileSystemInfo fileInfo, MiddleButtonType type)       // 按分类添加中间按钮
    {
        Transform t;
        switch (type)
        {
            case MiddleButtonType.File:       // 文件
                t = InstantiateMoBan(moBan_File, t_MiddleGrid, FILE_NAME,true);
                t.Find("Text").GetComponent<Text>().text = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                t.Find("GeiShi/Text").GetComponent<Text>().text = fileInfo.Extension.Substring(1);

                break;
            case MiddleButtonType.Folder:    // 文件夹
                t = InstantiateMoBan(moBan_Folder, t_MiddleGrid, FLODER_NAME, true);
                t.Find("Text").GetComponent<Text>().text = fileInfo.Name;
                break;
            case MiddleButtonType.Drive:     // 磁盘
                t = InstantiateMoBan(moBan_YinPan, t_MiddleGrid, YINPAN_NAME, true);
                string reStr = fileInfo.Name;
                reStr= reStr.Replace(":\\", " 盘");
                t.Find("Text").GetComponent<Text>().text = reStr;
                break;
            case MiddleButtonType.Computer:  // 我的电脑
                t = InstantiateMoBan(moBan_Computer, t_MiddleGrid, COMPUTER_NAME, true);
                t.Find("Text").GetComponent<Text>().text ="我的电脑";
                break;
            case MiddleButtonType.ZhuoMain:  // 桌面
                t = InstantiateMoBan(moBan_ZhuoMain, t_MiddleGrid, ZHUOMAIN_NAME, true);
                t.Find("Text").GetComponent<Text>().text = "桌面";
                break;
            case MiddleButtonType.Music:    // 音频文件
                t = InstantiateMoBan(moBan_Music, t_MiddleGrid, MUSIC_NAME, true);
                t.Find("Text").GetComponent<Text>().text = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                t.Find("GeiShi/Text").GetComponent<Text>().text = fileInfo.Extension.Substring(1);
                break;
            default:
                throw new Exception("未定义");
        }

        GameObject go = t.gameObject;
        if (type != MiddleButtonType.File)                             // 文件加载完图才添加按钮事件
        {
            t.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (go.Equals(go_CurrentSelect) && isSelect)            // 双击
                {
                    isSelect = false;
                    switch (go_CurrentSelect.name)
                    {
                        case FLODER_NAME:    //双击文件夹
                        case YINPAN_NAME:    //双击硬盘
                            mFileBrowser.GoInSubDirectory(fileInfo.Name);
                            RefreshMiddleContent();      // 刷新
                            break;
                        case COMPUTER_NAME: // 双击我的电脑
                            mFileBrowser.GoToRoot(true);
                            RefreshMiddleContent();      // 刷新
                            break;
                        case ZHUOMAIN_NAME: // 双击桌面
                            string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            mFileBrowser.RetrieveFiles(new DirectoryInfo(Desktop), true);
                            RefreshMiddleContent();      // 刷新
                            break;
                        case MUSIC_NAME:    //音乐文件
                            MyEventCenter.SendEvent(E_GameEvent.ShowMusicInfo, t.Find("Text").GetComponent<Text>(),(FileInfo)fileInfo,true);
                            break;
                        default:
                            throw new Exception("没有定义 —— "+ go_CurrentSelect.name);
                    }
     
                }
                else
                {
                    go_CurrentSelect = go;
                    Ctrl_Coroutine.Instance.StartCoroutine(CheckoubleClick());
                }
            });
        }
        l_MiddleItems.Add(go);
        return t;
    }




    private void InitMoBan_Tu(Transform t, FileInfo fileInfo)   // 初始化图片文件
    {
        MyLoadTu.LoadSingleTu(fileInfo, (bean) =>
        {
            allGoK_ResultBeanV.Add(t.gameObject, bean);
            t.Find("Icon").GetComponent<Image>().sprite = bean.SP;
            t.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (t.gameObject.Equals(go_CurrentSelect) && isSelect)            // 双击
                {
                    isSelect = false;
                    ShowTuInfo(bean);
                }
                else                                                              // 单击
                {
                    go_CurrentSelect = t.gameObject;

                    if (isNormalClick && !isShift)
                    {
                        if (chooseGOK_BgV.Count > 0)
                        {
                            foreach (GameObject bgGo in chooseGOK_BgV.Values)
                            {
                                bgGo.SetActive(false);
                            }
                            chooseGOK_BgV.Clear();
                        }
                    }
                    if (isShift && chooseGOK_BgV.Count > 0)         // 按下 Shift
                    {

                        List<GameObject> tmpList = new List<GameObject>(chooseGOK_BgV.Keys);
                        GameObject lastGo = tmpList[tmpList.Count - 1];
                        int index1 = l_MiddleItems.IndexOf(go_CurrentSelect);
                        int index2 = l_MiddleItems.IndexOf(lastGo);
                        int minIndex = Mathf.Min(index1, index2);
                        int maxIndex = Mathf.Max(index1, index2);
                        for (int i = minIndex + 1; i < maxIndex; i++)
                        {
                            GameObject tmpGo = l_MiddleItems[i];
                            GameObject tmpGoBG = tmpGo.transform.Find("Bg").gameObject;

                            if (!chooseGOK_BgV.ContainsKey(tmpGo) && !tmpGoBG.activeSelf)
                            {
                                tmpGoBG.SetActive(true);
                                chooseGOK_BgV.Add(tmpGo, tmpGoBG);
                            }
                        }

                    }

                    GameObject goBg = t.Find("Bg").gameObject;
                    if (!chooseGOK_BgV.ContainsKey(go_CurrentSelect) && !goBg.activeSelf)
                    {
                        goBg.SetActive(true);
                        chooseGOK_BgV.Add(go_CurrentSelect, goBg);
                    }

                    tx_TipZhang.text = chooseGOK_BgV.Count.ToString();
                    btnDaoRu.interactable = true;
                    Ctrl_Coroutine.Instance.StartCoroutine(CheckoubleClick());
                }
            });

        });
    }


    #endregion


    #region 书签

    private void OnToggle_ShuQian(bool ison) // 切换 书签
    {
        go_ShuQian.SetActive(ison);
        if (ison)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt_Right);
        }
    }



    private void CreateFavoites(string favPath)     // 创建收藏的按钮出来
    {
        DirectoryInfo dir = new DirectoryInfo(favPath);
        Transform t = InstantiateMoBan(moBan_Favorites, rt_ShuQianContant);
        GameObject go = t.gameObject;
        t.Find("Text").GetComponent<Text>().text = dir.Name; // 设置文字
        t.GetComponent<Button>().onClick.AddListener(() =>   // 设置点击回调
        {
            mFileBrowser.Relocate(dir.FullName);
            RefreshMiddleContent();  // 刷新中间
        });
        go.SetActive(true);
        mb_Favorites.Add(go);

    }


    #endregion


    #region 大小

    private void Btn_OpenSize()  // 打开大小设置
    {
        if (!go_ModeSize.activeSelf)
        {
            anim_SizeIcon.DOPlayForward();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt_Right);
            go_ModeSize.SetActive(true);
            if (go_ModeFilter.activeSelf&& go_ModeSorting.activeSelf)
            {
                Btn_CloseFilter();
                Btn_CloseSorting();

            }
        }
        else
        {
            Btn_CloseSize();
        }
    }

    private void Btn_CloseSize() // 关闭大小设置
    {
        anim_SizeIcon.DOPlayBackwards();
        go_ModeSize.SetActive(false);
    }


    private void OnToggle_ChangeSizeMode(string changeName) // 切换大小模式
    {
        tx_SizeBig.color = Color.gray;
        tx_SizeMiddle.color = Color.gray;
        tx_SizeSmall.color = Color.gray;

        switch (changeName)
        {
            case "ItemBig":
                tx_SizeBig.color = LBColor;
                tx_Size1.text = "大图标";
                grid_Contant.CallSize = SIZE_BIG;
                break;
            case "ItemMiddle":
                tx_SizeMiddle.color = LBColor;
                tx_Size1.text = "中等图标";
                grid_Contant.CallSize = SIZE_MIDDLE;
                break;
            case "ItemSmall":
                tx_SizeSmall.color = LBColor;
                tx_Size1.text = "小图标";
                grid_Contant.CallSize = SIZE_SMALL;
                break;
            default:
                throw new Exception("未定义 —— "+changeName);
        }



    }

    #endregion


    #region 过滤

    private void Btn_OpenFilter()                  // 打开过滤设置
    {
        if (!go_ModeFilter.activeSelf)
        {
            anim_FilterIcon.DOPlayForward();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt_Right);
            go_ModeFilter.SetActive(true);

            if (go_ModeSize.activeSelf&& go_ModeSorting.activeSelf)
            {
                Btn_CloseSize();
                Btn_CloseSorting();

            }
        }
        else
        {
            Btn_CloseFilter(); // 如果开着就关闭啊
        }
    }

    private void Btn_CloseFilter()                  // 关闭过滤设置
    {
        anim_FilterIcon.DOPlayBackwards();
        go_ModeFilter.SetActive(false);
    }

    private void OnToggle_ChangeFilterMode(string changeName) // 切换过滤模式
    {
        tx_FilterAll.color = Color.gray;
        tx_FilterTexture.color = Color.gray;
        tx_FilterFolder.color = Color.gray;

        switch (changeName)
        {
            case "ItemAll":
                tx_FilterAll.color = LBColor;
                tx_Filter1.text = SHOW_NAME1;
                isOnlyShowFolder = false;
                mFileBrowser.SetFilters(null);
                break;
            case "ItemTexture":
                tx_FilterTexture.color = LBColor;
                tx_Filter1.text = SHOW_NAME2;
                isOnlyShowFolder = false;
                mFileBrowser.SetFilters(MyFilterUtil.ONLY_TU_AUDIO_FILTER);
                break;
            case "ItemFolder":
                tx_FilterFolder.color = LBColor;
                tx_Filter1.text = SHOW_NAME3;
                isOnlyShowFolder = true;
                break;
            default:
                throw new Exception("未定义 —— " + changeName);
        }

        RefreshMiddleContent();         // 中间的内容刷新
    }

    #endregion


    #region 排序


    private void Btn_OpenSorting()  // 打开排序设置
    {
        if (!go_ModeSorting.activeSelf)
        {
            anim_SortIcon.DOPlayForward();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt_Right);
            go_ModeSorting.SetActive(true);

            if (go_ModeSize.activeSelf&& go_ModeFilter.activeSelf)
            {
                Btn_CloseSize();
                Btn_CloseFilter();

            }
        }
        else
        {
            Btn_CloseSorting();
        }
    }

    private void Btn_CloseSorting() // 关闭排序设置
    {
        anim_SortIcon.DOPlayBackwards();
        go_ModeSorting.SetActive(false);
    }


    private void OnToggle_ChangeSortMode(string changeName) // 切换排序模式
    {
        tx_SortName.color = Color.gray;
        tx_SortType.color = Color.gray;
        tx_SortDate.color = Color.gray;

        switch (changeName)
        {
            case "ItemName":
                tx_SortName.color = LBColor;
                tx_Sort1.text = "名称排序";
                mFileBrowser.SetSortMode(FileBrowser.SortingMode.Name);
                break;
            case "ItemType":
                tx_SortType.color = LBColor;
                tx_Sort1.text = "类型排序";
                mFileBrowser.SetSortMode(FileBrowser.SortingMode.Type);
                break;
            case "ItemDate":
                tx_SortDate.color = LBColor;
                tx_Sort1.text = "日期排序";
                mFileBrowser.SetSortMode(FileBrowser.SortingMode.Date);
                break;
            default:
                throw new Exception("未定义 —— " + changeName);
        }


        RefreshMiddleContent();

    }

    #endregion



    #region  头部栏

    // 上
    private void CloseItemPath(GameObject go)               // 点击了文件路径的 x，关闭自己 
    {
        go.SetActive(false);
        if (!go_Add.activeSelf)
        {
            go_Add.SetActive(true);
        }
    }


    private void Btn_AddItem()                             // 点击再添加头文件夹 菜单
    {
        if (!go_ItemPath2.activeSelf)
        {
            go_ItemPath2.SetActive(true);
            tg_ItemPath.ChangeToggleOn(go_ItemPath2.name);
            mCurrentTopIndex = 1;
        }
        else if (!go_ItemPath3.activeSelf)
        {
            go_ItemPath3.SetActive(true);
            tg_ItemPath.ChangeToggleOn(go_ItemPath3.name);
            mCurrentTopIndex = 2;
        }
        else if (!go_ItemPath4.activeSelf)
        {
            go_ItemPath4.SetActive(true);
            tg_ItemPath.ChangeToggleOn(go_ItemPath4.name);
            mCurrentTopIndex = 3;
        }
        else if (!go_ItemPath5.activeSelf)
        {
            go_ItemPath5.SetActive(true);
            tg_ItemPath.ChangeToggleOn(go_ItemPath5.name);
            go_Add.SetActive(false);
            mCurrentTopIndex = 4;
        }


        LayoutRebuilder.ForceRebuildLayoutImmediate(rt_Top);
    }


    private void E_OnTopPathChange(string changeName)      // 切换头部路径
    {
        switch (changeName)
        {
            case "ItemPath1":
                mCurrentTopIndex = 0;
                break;
            case "ItemPath2":
                mCurrentTopIndex = 1;
                break;
            case "ItemPath3":
                mCurrentTopIndex = 2;
                break;
            case "ItemPath4":
                mCurrentTopIndex = 3;
                break;
            case "ItemPath5":
                mCurrentTopIndex = 4;
                break;
            default:
                MyLog.Red("为什么还有其他？ —— "+ changeName);
                break;
        }
        mFileBrowser.Relocate(l_AddressPaths[mCurrentTopIndex]);
        RefreshMiddleContent();

    }





    // 下

    private void Btn_OnHistoryPre()                   // 点击 <-  转到上一个打开的文件夹
    {
        mFileBrowser.GoToPrevious();
        RefreshMiddleContent();
    }

    private void Btn_OnHistoryNext()                  // 点击 ->  转到下一个打开的文件夹
    {
        mFileBrowser.GotToNext();
        RefreshMiddleContent();
    }
      

    private void Btn_OnClickAddressPath()             // 点击更改路径
    {

        MyOpenFileOrFolder.OpenFolder(tx_Path.text, "选择文件夹", (path) =>
        {
            tx_Path.text = path;
            mFileBrowser.Relocate(path);
            RefreshMiddleContent();       // 刷新下中间
        });
    }


    private void Toggle_ChangeIsStar(bool isOn)       // 切换是否收藏
    {
        string favPath = tx_Path.text;
        if (isOn) 
        {
            if (!Ctrl_UserInfo.Instance.L_FavoritesPath.Contains(favPath))
            {
                CreateFavoites(favPath);
                Ctrl_UserInfo.Instance.L_FavoritesPath.Add(favPath);
                LayoutRebuilder.ForceRebuildLayoutImmediate(rt_ShuQianContant);
            }
        }
        else
        {
            if (Ctrl_UserInfo.Instance.L_FavoritesPath.Contains(favPath))
            {
                int index = Ctrl_UserInfo.Instance.L_FavoritesPath.IndexOf(favPath);
                GameObject go = mb_Favorites[index];
                Ctrl_UserInfo.Instance.L_FavoritesPath.RemoveAt(index);
                mb_Favorites.RemoveAt(index);
                Object.DestroyImmediate(go);
            }
        }
    }



    private void Btn_OpenFolder()                     // 打开文件夹
    {
        string path = tx_Path.text;
        Application.OpenURL(path);

    }

    private void Btn_OnGoToParent()                   // 去父文件夹中
    {
        mFileBrowser.GoInParent();
        RefreshMiddleContent();

    }

    #endregion


    #region  框选  选中


    private void ClearAllChooseZhong()              // 清除所有选中的
    {
        if (chooseGOK_BgV.Count > 0)
        {
            foreach (GameObject bgGo in chooseGOK_BgV.Values)
            {
                bgGo.SetActive(false);
            }
            chooseGOK_BgV.Clear();
            tx_TipZhang.text = "0";
            btnDaoRu.interactable = false;
        }
    }

    private void E_OnClickKuangDown(Vector2 startPosition)                 // 开始框选 或者 单单是点击一下
    {
        rt_Kuang.gameObject.SetActive(true);
        rt_Kuang.anchoredPosition = startPosition;
        ClearAllChooseZhong();
        mKuangXuan.OnClickDown();
    }


    private void E_OnKuangDarg(Vector2 widthHeigh)                        // 在拖动 调整框的大小
    {
        rt_Kuang.sizeDelta = widthHeigh;
    }


    private void E_OnClickKuangUp()                                       // 结束框选
    {
        rt_Kuang.sizeDelta = Vector2.zero;
        mKuangXuan.OnClickUp();
        tx_TipZhang.text = chooseGOK_BgV.Count.ToString();
        if (chooseGOK_BgV.Count > 0)
        {
            btnDaoRu.interactable = true;
        }

    }


    #endregion


    #region 单张详细信息(单张导入)


    private void ShowTuInfo(ResultBean bean)                         // 打开图的详细信息
    {
        mCurrentFile = bean.File;
        tx_FileName.text = Path.GetFileNameWithoutExtension(mCurrentFile.FullName);
        tx_HuoZhui.text = mCurrentFile.Extension.Substring(1);
        tx_TuSize.text = "";
        go_ShowTuInfo.SetActive(true);

        Sp_Tu.sprite = bean.SP;
        yuanLaiWidth = bean.Width;
        yuanLaiHidth = bean.Height;
        tx_TuSize.text = yuanLaiWidth + " x " + yuanLaiHidth;
        SetTuSize(yuanLaiWidth, yuanLaiHidth);

    }


    private void Btn_CloseTuInfo()                    // 关闭详细信息页
    {
        go_ShowTuInfo.SetActive(false);
        Sp_Tu.sprite = null;
    }




    #endregion


    #region 多张导入

    private Dictionary<GameObject,ResultBean> itemSelectK_ResutltV = new Dictionary<GameObject, ResultBean>();     // item每行的作为 Key 结果为Value

    private void ShowDuoTu()                                      // 显示 多张图导入
    {
        go_ShowDuoTu.SetActive(true);
        Ctrl_Coroutine.Instance.StartCoroutine(StartLoadDuoTu());

    }



    IEnumerator StartLoadDuoTu()                                // 用线程 多张图的每个 Item
    {
        foreach (GameObject go in chooseGOK_BgV.Keys)
        {
            ResultBean bean = allGoK_ResultBeanV[go];
            Transform t = InstantiateMoBan(moBan_DuoTuItem, rt_DuoTuContant);
            itemSelectK_ResutltV.Add(t.gameObject, bean);

            // 图标
            Transform btnIcon = t.Find("BtnIcon");
            btnIcon.GetComponent<Image>().sprite = bean.SP;
            btnIcon.GetComponent<Button>().onClick.AddListener(() =>
            {
                Application.OpenURL(bean.File.FullName);
            });


            // 文件名
            t.Find("FileName").GetComponent<Text>().text = bean.File.Name;
            // 大小
            t.Find("Size").GetComponent<Text>().text = bean.Width+ " x " + bean.Height;

            // 单击这一项
            GameObject chooseGOBg = t.Find("Choose/Bg").gameObject;
            t.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (null != mCuurentChooseBg)
                {
                    mCuurentChooseBg.SetActive(false);
                }

                mCuurentChooseBg = chooseGOBg;
                mCuurentChooseBg.SetActive(true);
            });
            // 删除按钮
            t.Find("Choose/BtnContrl/BtnDelete").GetComponent<Button>().onClick.AddListener(() =>
            {
                EachBtn_Delete(t.gameObject);
            });
            // Up 按钮
            t.Find("Choose/BtnContrl/BtnUp").GetComponent<Button>().onClick.AddListener(() =>
            {
                EeachBtn_Up(t.gameObject);
            });
            // Down 按钮
            t.Find("Choose/BtnContrl/BtnDown").GetComponent<Button>().onClick.AddListener(() =>
            {
                EachBtn_Down(t.gameObject);
            });
            yield return 0;

        }

    }



    // UI事件
    private void Btn_OnCloseDuoTu()                                   // 点击关闭
    {
        go_ShowDuoTu.SetActive(false);
        if (itemSelectK_ResutltV.Count>0)
        {
            List<GameObject> list = new List<GameObject>(itemSelectK_ResutltV.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                Object.Destroy(list[i]);
            }
            itemSelectK_ResutltV.Clear();
        }
    }


    private void EachBtn_Delete(GameObject go)                        // 点击了 Item 的 Delete
    {
        itemSelectK_ResutltV.Remove(go);
        Object.Destroy(go);
    }


    private void EeachBtn_Up(GameObject go)                           // 点击了 Item 的 Up
    {

        ChangeDicIndex(go,-1);
    }


    private void EachBtn_Down(GameObject go)                         // 点击了 Item 的 Down
    {
        ChangeDicIndex(go, 1);
    }






    private void ManyBtn_XunLieTuDaoRu(EXunLieTu tuType, int index)           // 点击了序列图的导入
    {
        List<ResultBean> resultBeans = new List<ResultBean>(itemSelectK_ResutltV.Values);

        // 1. 发送导入的事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_XunLieTu, tuType, resultBeans);

        // 2. 把底下的文字改成 绿色
        foreach (GameObject go in chooseGOK_BgV.Keys)
        {
            go.transform.Find("Text").GetComponent<Text>().color = Color.green;
        }
        // 选择第几个
        mSelectIndex = index;
        // 关闭多选信息
        Btn_OnCloseDuoTu();
        // 清空选择
        ClearAllChooseZhong();    

    }




    #endregion




    // 导入按钮
    private void ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType type)            // 点击导入 集合序列图
    {

        List<ResultBean> list = new List<ResultBean>(chooseGOK_BgV.Count);
        // 1. 把底下的文字改成 绿色
        foreach (GameObject go in chooseGOK_BgV.Keys)
        {
            go.transform.Find("Text").GetComponent<Text>().color = Color.green;
            list.Add(allGoK_ResultBeanV[go]);
        }
        // 发送事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_JiHeXuLieTu, type, list);
        // 选择第几个
        mSelectIndex = (int)type;
        // 关闭这个信息页
        Btn_CloseTuInfo();
        Btn_OnCloseDuoTu();
        // 清空选择
        ClearAllChooseZhong();
    }

    private void ManyBtn_DaoRuTaoMingTu(ETaoMingType type)               // 点击导入 透明图
    {

        List<ResultBean> list = new List<ResultBean>(chooseGOK_BgV.Count);
        // 1. 把底下的文字改成 绿色
        foreach (GameObject go in chooseGOK_BgV.Keys)
        {
            go.transform.Find("Text").GetComponent<Text>().color = Color.green;
            list.Add(allGoK_ResultBeanV[go]);
        }
        // 发送事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_TaoMingTu, type, list);
        // 选择第几个
        mSelectIndex = (int)type;
        // 关闭这个信息页
        Btn_CloseTuInfo();
        Btn_OnCloseDuoTu();
        // 清空选择
        ClearAllChooseZhong();
    }

    private void ManyBtn_DaoRuJpgTu(ENormalTuType type)                 // 点击导入 Jpg
    {
        List<ResultBean> list = new List<ResultBean>(chooseGOK_BgV.Count);
        // 1. 把底下的文字改成 绿色
        foreach (GameObject go in chooseGOK_BgV.Keys)
        {
            go.transform.Find("Text").GetComponent<Text>().color = Color.green;
            list.Add(allGoK_ResultBeanV[go]);
        }
        // 发送事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_NormalTu, type, list);
        // 选择第几个
        mSelectIndex = (int)type;
        // 关闭这个信息页
        Btn_CloseTuInfo();
        Btn_OnCloseDuoTu();
        // 清空选择
        ClearAllChooseZhong();
    }


    private void ManyBtn_DaoRuJiHeTu(EJiHeType type)                 // 点击导入 集合
    {
        List<ResultBean> list = new List<ResultBean>(chooseGOK_BgV.Count);
        // 1. 把底下的文字改成 绿色
        foreach (GameObject go in chooseGOK_BgV.Keys)
        {
            go.transform.Find("Text").GetComponent<Text>().color = Color.green;
            list.Add(allGoK_ResultBeanV[go]);
        }
        // 发送事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_JiHeTu, type, list);
        // 选择第几个
        mSelectIndex = (int)type;
        // 关闭这个信息页
        Btn_CloseTuInfo();
        Btn_OnCloseDuoTu();
        // 清空选择
        ClearAllChooseZhong();
    }


    #region 导入结果

    private EGameType mSelectType;
    private int mSelectIndex = 0;
    private readonly List<GameObject> errorList = new List<GameObject>();
    private void ShowResult(EGameType gameType, bool isOk,List<FileInfo> errorInfos)            // 显示导入结果
    {
        if (errorList.Count>0)
        {
            for (int i = 0; i < errorList.Count; i++)
            {
                Object.Destroy(errorList[i]);
            }
            errorList.Clear();
        }
        mSelectType = gameType;
        go_Result.SetActive(true);
        go_TittleOK.SetActive(isOk);
        go_TittleError.SetActive(!isOk);
        go_ErrorInfo.SetActive(false);
        string str = "";
        switch (gameType)
        {
            case EGameType.JiHeXuLieTu:
                str = "去集合序列图页";
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



    private void GoToDaoRuWhere()                                   // 去到刚刚导入的地方
    {
        go_Result.SetActive(false);
        MyEventCenter.SendEvent<EGameType,int>(E_GameEvent.ChangGameToggleType, mSelectType, mSelectIndex);
    }



    private void JiXuDaoRu()                                       // 继承导入
    {
        go_Result.SetActive(false);

    }


    #endregion







}