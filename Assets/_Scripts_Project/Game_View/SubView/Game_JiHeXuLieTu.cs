using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PSPUtil;
using PSPUtil.Control;
using UnityEngine;
using UnityEngine.UI;

public enum EJiHeXuLieTuType
{
    JiHeXLT1,
    JiHeXLT2,
    JiHeXLT3,
    JiHeXLT4,
    JiHeXLT5
}

public class Game_JiHeXuLieTu : SubUI
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


    private EJiHeXuLieTuType mCurrentIndex = EJiHeXuLieTuType.JiHeXLT1;
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
    private InputField input_GridSize;




    private IEnumerator CheckoubleClick() // 检测是否双击
    {
        isSelect = true;
        yield return new WaitForSeconds(Ctrl_UserInfo.DoubleClickTime);
        isSelect = false;
    }


    private RectTransform GetParent(EJiHeXuLieTuType type)
    {
        RectTransform rt = null; // 放在那里
        switch (type)
        {
            case EJiHeXuLieTuType.JiHeXLT1:
                rt = dt5_Contrl.GO_One.transform as RectTransform;
                break;
            case EJiHeXuLieTuType.JiHeXLT2:
                rt = dt5_Contrl.GO_Two.transform as RectTransform;
                break;
            case EJiHeXuLieTuType.JiHeXLT3:
                rt = dt5_Contrl.GO_Three.transform as RectTransform;
                break;
            case EJiHeXuLieTuType.JiHeXLT4:
                rt = dt5_Contrl.GO_Four.transform as RectTransform;
                break;
            case EJiHeXuLieTuType.JiHeXLT5:
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



    public override string GetUIPathForRoot()
    {
        return "Right/EachContant/JiHeXuLieTu";
    }



    public override void OnDisable()
    {
    }


    private void DeleteOneLine(EJiHeXuLieTuType type)
    {
        RectTransform rt = GetParent(type);
        for (int i = 0; i < rt.childCount; i++)
        {
            UnityEngine.Object.Destroy(rt.GetChild(i).gameObject);
        }
    }

    #endregion

    protected override void OnStart(Transform root)
    {

        MyEventCenter.AddListener<EJiHeXuLieTuType, List<FileInfo>, bool>(E_GameEvent.DaoRu_JiHeXuLieTu, E_OnDaoRu);
        MyEventCenter.AddListener<EJiHeXuLieTuType, List<ResultBean>>(E_GameEvent.ResultDaoRu_JiHeXuLieTu, E_ResultDaoRu);
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
        input_GridSize = Get<InputField>("Top/Left/ChangeSize/InputField");
        go_ChangeSize = GetGameObject("Top/Left/ChangeSize");
        slider_ChangeSize = Get<Slider>("Top/Left/ChangeSize/Slider");
        AddSliderOnValueChanged(slider_ChangeSize,Slider_OnGridSizeChange);

    }

    public override void OnEnable()
    {
        go_ChangeSize.SetActive(Ctrl_UserInfo.Instance.IsCanChangeSize);
        for (int i = 0; i < l_Grids.Length; i++)
        {
            l_Grids[i].CallSize = Ctrl_UserInfo.Instance.L_JiHeXuLieTuSize[i].CurrentSize;
        }
        slider_ChangeSize.value = Ctrl_UserInfo.Instance.L_JiHeXuLieTuSize[0].ChangeValue;


        tx_BottomName1.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[0];
        tx_BottomName2.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[1];
        tx_BottomName3.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[2];
        tx_BottomName4.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[3];
        tx_BottomName5.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[4];
    }


    //————————————————————————————————————

    private void E_OnBottomDoubleClick()                                     // 底下 双击 改名
    {
        MyEventCenter.SendEvent(E_GameEvent.ShowGeiMingUI, EGameType.JiHeXuLieTu, Ctrl_UserInfo.Instance.BottomJiHeXLTName[(int)mCurrentIndex]);


    }


    private void E_OnBottomValueChange(string changeName) // 底下的切换
    {
        switch (changeName)
        {
            case ITEM_STR1:
                mCurrentIndex = EJiHeXuLieTuType.JiHeXLT1;
                dt5_Contrl.Change2One();
                break;
            case ITEM_STR2:
                mCurrentIndex = EJiHeXuLieTuType.JiHeXLT2;
                dt5_Contrl.Change2Two();
                break;
            case ITEM_STR3:
                mCurrentIndex = EJiHeXuLieTuType.JiHeXLT3;
                dt5_Contrl.Change2Three();
                break;
            case ITEM_STR4:
                mCurrentIndex = EJiHeXuLieTuType.JiHeXLT4;
                dt5_Contrl.Change2Four();
                break;
            case ITEM_STR5:
                mCurrentIndex = EJiHeXuLieTuType.JiHeXLT5;
                dt5_Contrl.Change2Five();
                break;
        }
        slider_ChangeSize.value = Ctrl_UserInfo.Instance.L_JiHeXuLieTuSize[(int) mCurrentIndex].ChangeValue;
        input_GridSize.text = l_Grids[(int)mCurrentIndex].CallSize.x.ToString();
        m_SrollView.content = GetParent(mCurrentIndex);
    }



    private void Btn_DaoRu()             // 点击导入
    {
        MyOpenFileOrFolder.OpenFile(Ctrl_UserInfo.Instance.DaoRuFirstPath, "选择1个或多个 集合序列图", EFileFilter.PngAndTuAndAll,
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
                MyEventCenter.SendEvent(E_GameEvent.DaoRu_JiHeXuLieTu, mCurrentIndex, list, false);
            });
    }




    private void Btn_Delete()           // 删除
    {

        string tittle = "删除";
        switch (mCurrentIndex)
        {
            case EJiHeXuLieTuType.JiHeXLT1:
                tittle += "集合1中的所有图片？";
                break;
            case EJiHeXuLieTuType.JiHeXLT2:
                tittle += "集合2中的所有图片";
                break;
            case EJiHeXuLieTuType.JiHeXLT3:
                tittle += "集合3中的所有图片";
                break;
            case EJiHeXuLieTuType.JiHeXLT4:
                tittle += "集合4中的所有图片";
                break;
            case EJiHeXuLieTuType.JiHeXLT5:
                tittle += "集合5中的所有图片";
                break;
        }
        MyEventCenter.SendEvent(E_GameEvent.ShowIsSure, EGameType.JiHeXuLieTu, tittle);



    }




    private void Slider_OnGridSizeChange(float value)          // 改变 Grid 大小
    {
        int gridIndex = (int) mCurrentIndex;
        int tmpValue = (int)value;
        Ctrl_UserInfo.Instance.L_JiHeXuLieTuSize[gridIndex].ChangeValue = tmpValue;
        Vector2 yuanSize = Ctrl_UserInfo.Instance.L_JiHeXuLieTuSize[gridIndex].YuanSize;
        Ctrl_UserInfo.Instance.L_JiHeXuLieTuSize[gridIndex].CurrentSize = new Vector2(yuanSize.x + tmpValue, yuanSize.y + tmpValue);
        l_Grids[gridIndex].CallSize = Ctrl_UserInfo.Instance.L_JiHeXuLieTuSize[gridIndex].CurrentSize;
        input_GridSize.text = l_Grids[gridIndex].CallSize.x.ToString();


    }



    private void Btn_OnDoubleItemClick(ResultBean resultBean) // 双击显示信息
    {
        mCurrentSelectFile = resultBean.File;
        MyEventCenter.SendEvent(E_GameEvent.ShowSingleTuInfo, EGameType.JiHeXuLieTu, resultBean);
    }


    //—————————————————— 事件 ——————————————————


    private void E_OnDaoRu(EJiHeXuLieTuType type, List<FileInfo> infos, bool isSave) // 需要再次加载的 导入
    {
        Ctrl_Coroutine.Instance.StartCoroutine(OnDaoRu(type, infos, isSave));
    }


    IEnumerator OnDaoRu(EJiHeXuLieTuType type, List<FileInfo> infos, bool isSave)
    {
        List<FileInfo> errorList = new List<FileInfo>();
        foreach (FileInfo fileInfo in infos)
        {
            // 保存一下信息
            if (isSave)
            {
                bool isOk = Ctrl_TextureInfo.Instance.SaveJiHeXuLieTu(type, fileInfo.FullName);
                if (!isOk)
                {
                    errorList.Add(fileInfo);
                    continue;
                }
            }
            // 1. 创建一个实例
            Transform t = InstantiateMoBan(go_MoBan, GetParent(type));
            MyLoadTu.LoadSingleTu(fileInfo, (resBean) =>
            {
                InitMoBan(t, resBean);
            });
            yield return 0;
        }
        if (isSave)
        {
            MyEventCenter.SendEvent(E_GameEvent.DaoRuResult, EGameType.JiHeXuLieTu, errorList.Count == 0, errorList);
        }
    }



    private void E_ResultDaoRu(EJiHeXuLieTuType type, List<ResultBean> resultBeans) // 已有 ResultBean 直接的导入
    {
        Ctrl_Coroutine.Instance.StartCoroutine(ResultDaoRu(type, resultBeans));
    }


    IEnumerator ResultDaoRu(EJiHeXuLieTuType type, List<ResultBean> resultBeans)
    {
        List<FileInfo> errorList = new List<FileInfo>();

        foreach (ResultBean resultBean in resultBeans)
        {
            bool isOk = Ctrl_TextureInfo.Instance.SaveJiHeXuLieTu(type, resultBean.File.FullName);
            if (!isOk)
            {
                errorList.Add(resultBean.File);
                continue;
            }
            Transform t = InstantiateMoBan(go_MoBan, GetParent(type));
            InitMoBan(t, resultBean);
            yield return 0;
        }

        MyEventCenter.SendEvent(E_GameEvent.DaoRuResult, EGameType.JiHeXuLieTu, errorList.Count == 0, errorList);

    }

    //————————————————————————————————————

    private void E_DelteTrue(EGameType type)             // 真的删除
    {
        if (type == EGameType.JiHeXuLieTu)
        {
            Ctrl_TextureInfo.Instance.DeleteJiHeXuLieOneLine(mCurrentIndex);
            DeleteOneLine(mCurrentIndex);
        }
    }



    private void E_ShowNormalTuInfo(EGameType type, ResultBean bean)        // 显示单图信息
    {
        if (type == EGameType.JiHeXuLieTu)
        {
            go_Top.SetActive(false);
            go_Bottom.SetActive(false);
        }
    }



    private void E_CloseNormalTuInfo(EGameType type)                        // 关闭意图信息
    {
        if (type == EGameType.JiHeXuLieTu)
        {
            go_Top.SetActive(true);
            go_Bottom.SetActive(true);
        }
    }



    private void E_OnClickNoSaveThis(EGameType type)                         // 点击了不保存这个
    {
        if (type == EGameType.JiHeXuLieTu)
        {
            Ctrl_TextureInfo.Instance.DeleteJiHeXuLieSave(mCurrentIndex, mCurrentSelectFile.FullName);
            UnityEngine.Object.Destroy(go_CurrentSelect);
            go_Top.SetActive(true);
            go_Bottom.SetActive(true);
        }
    }



    private void E_DeleteAll()                           // 删除所有
    {
        go_CurrentSelect = null;
        mCurrentSelectFile = null;
        foreach (EJiHeXuLieTuType type in Enum.GetValues(typeof(EJiHeXuLieTuType)))
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
        if (type == EGameType.JiHeXuLieTu)
        {
            switch (mCurrentIndex)
            {
                case EJiHeXuLieTuType.JiHeXLT1:
                    tx_BottomName1.text = changeNamne;
                    break;
                case EJiHeXuLieTuType.JiHeXLT2:
                    tx_BottomName2.text = changeNamne;
                    break;
                case EJiHeXuLieTuType.JiHeXLT3:
                    tx_BottomName3.text = changeNamne;
                    break;
                case EJiHeXuLieTuType.JiHeXLT4:
                    tx_BottomName4.text = changeNamne;
                    break;
                case EJiHeXuLieTuType.JiHeXLT5:
                    tx_BottomName5.text = changeNamne;
                    break;
            }
            Ctrl_UserInfo.Instance.BottomJiHeXLTName[(int)mCurrentIndex] = changeNamne;
        }
    }


}
