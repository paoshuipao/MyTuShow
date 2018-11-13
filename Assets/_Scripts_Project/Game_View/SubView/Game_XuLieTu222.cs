using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PSPUtil;
using PSPUtil.Control;
using PSPUtil.StaticUtil;
using UnityEngine;
using UnityEngine.UI;

public enum EXuLieTu222
{
    XLT222_1,
    XLT222_2,
    XLT222_3,
    XLT222_4,
    XLT222_5,
}



public class Game_XuLieTu222 : SubUI
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

    private EXuLieTu222 mCurrentIndex = EXuLieTu222.XLT222_1;
    private bool isSelect; // 是否之前点击了
    private GameObject go_CurrentSelect; // 当前选择的对象


    // 模版
    private GameObject go_MoBan;
    private const string CREATE_FILE_NAME = "XuLieTu222"; // 模版产生的名


    // 上方
    private GameObject go_Top;
    private DTToggle5_Fade dt5_Contrl;
    private ScrollRect m_SrollView;


    // 底下
    private GameObject go_Bottom;
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
    private Text tx_GridSize;



    public override string GetUIPathForRoot()
    {
        return "Right/EachContant/XuLieTu222";

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


    private RectTransform GetParent(EXuLieTu222 type)
    {
        RectTransform rt = null; // 放在那里
        switch (type)
        {
            case EXuLieTu222.XLT222_1:
                rt = dt5_Contrl.GO_One.transform as RectTransform;
                break;
            case EXuLieTu222.XLT222_2:
                rt = dt5_Contrl.GO_Two.transform as RectTransform;
                break;
            case EXuLieTu222.XLT222_3:
                rt = dt5_Contrl.GO_Three.transform as RectTransform;
                break;
            case EXuLieTu222.XLT222_4:
                rt = dt5_Contrl.GO_Four.transform as RectTransform;
                break;
            case EXuLieTu222.XLT222_5:
                rt = dt5_Contrl.GO_Five.transform as RectTransform;
                break;
            default:
                throw new Exception("还有其他？");
        }
        return rt;
    }

    private void InitMoBan(Transform t, ResultBean[] resultBeans) // 初始化模版
    {
        GameObject go = t.gameObject;
        t.Find("Tu").GetComponent<UGUI_SpriteAnim>().ChangeAnim(GetSpriteList(resultBeans));
        t.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (go.Equals(go_CurrentSelect) && isSelect) // 双击
            {
                go_Top.SetActive(false);
                go_Bottom.SetActive(false);
                MyEventCenter.SendEvent(E_GameEvent.ShowDuoTuInfo, EGameType.XunLieTu222, resultBeans);

            }
            else // 单击
            {
                go_CurrentSelect = go;
                Ctrl_Coroutine.Instance.StartCoroutine(CheckoubleClick());
            }
        });
    }

    private void DeleteOneLine(EXuLieTu222 type)           // 删除整行
    {
        Ctrl_TextureInfo.Instance.DeleteXuLieTu222OneLine(type);
        RectTransform rt = GetParent(type);
        for (int i = 0; i < rt.childCount; i++)
        {
            UnityEngine.Object.Destroy(rt.GetChild(i).gameObject);
        }
    }

    #endregion

    protected override void OnStart(Transform root)
    {
        MyEventCenter.AddListener<EXuLieTu222, List<FileInfo>, bool>(E_GameEvent.DaoRu_XunLieTu222, E_OnDaoRu);
        MyEventCenter.AddListener<EXuLieTu222, List<ResultBean>>(E_GameEvent.ResultDaoRu_XunLieTu222, E_ResultDaoRu);
        MyEventCenter.AddListener<bool>(E_GameEvent.ShowChangeSizeSlider, E_IsShowChangeSize);
        MyEventCenter.AddListener<EGameType>(E_GameEvent.ClickTrue, E_DelteTrue);                                 // 确定删除
        MyEventCenter.AddListener(E_GameEvent.DelteAll, E_DeleteAll);                                             // 删除全部
        MyEventCenter.AddListener<EGameType>(E_GameEvent.CloseDuoTuInfo, E_CloseDuoTuInfo);                       // 关闭多图信息
        MyEventCenter.AddListener<EGameType, string[]>(E_GameEvent.OnClickNoSaveThisDuoTu, E_DeleteOne);          // 多图信息中删除一个

        // 模版
        go_MoBan = GetGameObject("Top/SrcollRect/MoBan");


        // 上方
        go_Top = GetGameObject("Top");
        dt5_Contrl = Get<DTToggle5_Fade>("Top/SrcollRect");
        m_SrollView = Get<ScrollRect>("Top/SrcollRect");


        // 底下
        go_Bottom = GetGameObject("Bottom");
        tg_BottomContrl = Get<UGUI_ToggleGroup>("Bottom/Contant");
        tg_BottomContrl.OnChangeValue += E_OnBottomValueChange;
        tg_BottomContrl.OnDoubleClick += E_OnBottomDoubleClick;


        tx_BottomName1 = Get<Text>("Bottom/Contant/GeShiItem1/Text");
        tx_BottomName2 = Get<Text>("Bottom/Contant/GeShiItem2/Text");
        tx_BottomName3 = Get<Text>("Bottom/Contant/GeShiItem3/Text");
        tx_BottomName4 = Get<Text>("Bottom/Contant/GeShiItem4/Text");
        tx_BottomName5 = Get<Text>("Bottom/Contant/GeShiItem5/Text");


        // 导入 清空
        AddButtOnClick("Top/Left/DaoRu", Btn_OnDaoRu);
        AddButtOnClick("Top/Left/DeleteAll", Btn_DeleteOneLine);


        //改变 Grid 大小
        l_Grids = Gets<UGUI_Grid>("Top/SrcollRect");
        tx_GridSize = Get<Text>("Top/Left/ChangeSize/TxValue");
        go_ChangeSize = GetGameObject("Top/Left/ChangeSize");
        slider_ChangeSize = Get<Slider>("Top/Left/ChangeSize/Slider");
        AddSliderOnValueChanged(slider_ChangeSize, Slider_OnGridSizeChange);


    }


    public override void OnEnable()
    {
        go_ChangeSize.SetActive(Ctrl_UserInfo.Instance.IsCanChangeSize);
        for (int i = 0; i < l_Grids.Length; i++)
        {
            l_Grids[i].CallSize = Ctrl_UserInfo.Instance.L_XuLieTu222Size[i].CurrentSize;
        }
        slider_ChangeSize.value = Ctrl_UserInfo.Instance.L_XuLieTu222Size[0].ChangeValue;

        tx_BottomName1.text = Ctrl_UserInfo.Instance.BottomXuLeTu222Name[0];
        tx_BottomName2.text = Ctrl_UserInfo.Instance.BottomXuLeTu222Name[1];
        tx_BottomName3.text = Ctrl_UserInfo.Instance.BottomXuLeTu222Name[2];
        tx_BottomName4.text = Ctrl_UserInfo.Instance.BottomXuLeTu222Name[3];
        tx_BottomName5.text = Ctrl_UserInfo.Instance.BottomXuLeTu222Name[4];
    }



    //————————————————————————————————————

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
                MyEventCenter.SendEvent(E_GameEvent.DaoRu_XunLieTu222, mCurrentIndex, fileInfos, true);
            });
    }



    private void Btn_DeleteOneLine()           // 点击删除
    {
        string tittle = "删除";
        switch (mCurrentIndex)
        {
            case EXuLieTu222.XLT222_1:
                tittle += " 序图1 的所有序列图片？";
                break;
            case EXuLieTu222.XLT222_2:
                tittle += " 序图2 的所有序列图片？";
                break;
            case EXuLieTu222.XLT222_3:
                tittle += " 序图3 的所有序列图片？";
                break;
            case EXuLieTu222.XLT222_4:
                tittle += " 序图4 的所有序列图片？";
                break;
            case EXuLieTu222.XLT222_5:
                tittle += " 序图5 的所有序列图片？";
                break;
        }
        MyEventCenter.SendEvent(E_GameEvent.ShowIsSure, EGameType.XunLieTu222, tittle);
    }



    private void E_OnBottomDoubleClick()                            // 底下 双击 改名
    {
        MyEventCenter.SendEvent(E_GameEvent.ShowGeiMingUI, EGameType.XunLieTu222, Ctrl_UserInfo.Instance.BottomXuLeTu222Name[(int)mCurrentIndex]);
    }



    private void E_OnBottomValueChange(string changeName)          // 底下的切换
    {
        switch (changeName)
        {
            case ITEM_STR1:
                mCurrentIndex = EXuLieTu222.XLT222_1;
                dt5_Contrl.Change2One();
                break;
            case ITEM_STR2:
                mCurrentIndex = EXuLieTu222.XLT222_2;
                dt5_Contrl.Change2Two();
                break;
            case ITEM_STR3:
                mCurrentIndex = EXuLieTu222.XLT222_3;
                dt5_Contrl.Change2Three();
                break;
            case ITEM_STR4:
                mCurrentIndex = EXuLieTu222.XLT222_4;
                dt5_Contrl.Change2Four();
                break;
            case ITEM_STR5:
                mCurrentIndex = EXuLieTu222.XLT222_5;
                dt5_Contrl.Change2Five();
                break;
        }
        slider_ChangeSize.value = Ctrl_UserInfo.Instance.L_XuLieTu222Size[(int)mCurrentIndex].ChangeValue;
        tx_GridSize.text = l_Grids[(int)mCurrentIndex].CallSize.x.ToString();
        m_SrollView.content = GetParent(mCurrentIndex);
    }


    private void Slider_OnGridSizeChange(float value)              // 改变 Grid 大小
    {
        int gridIndex = (int)mCurrentIndex;
        int tmpValue = (int)value;
        Ctrl_UserInfo.Instance.L_XuLieTu222Size[gridIndex].ChangeValue = tmpValue;
        Vector2 yuanSize = Ctrl_UserInfo.Instance.L_XuLieTu222Size[gridIndex].YuanSize;
        Ctrl_UserInfo.Instance.L_XuLieTu222Size[gridIndex].CurrentSize = new Vector2(yuanSize.x + tmpValue, yuanSize.y + tmpValue);
        l_Grids[gridIndex].CallSize = Ctrl_UserInfo.Instance.L_XuLieTu222Size[gridIndex].CurrentSize;
        tx_GridSize.text = l_Grids[gridIndex].CallSize.x.ToString();


    }





    //—————————————————— 事件 ——————————————————


    private void E_OnDaoRu(EXuLieTu222 tuType, List<FileInfo> fileInfos, bool isSave) // 接收导入事件 ，创建一个序列图
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
            bool isOk = Ctrl_TextureInfo.Instance.SaveXunLieTu222(tuType, paths);
            MyEventCenter.SendEvent<EGameType, bool, List<FileInfo>>(E_GameEvent.DaoRuResult, EGameType.XunLieTu222, isOk, null);
            if (!isOk)
            {
                return;
            }
        }

        // 1. 创建一个实例
        Transform t = InstantiateMoBan(go_MoBan, GetParent(tuType), CREATE_FILE_NAME);

        // 2. 加载图片
        MyLoadTu.LoadMultipleTu(paths, (resBean) =>
        {
            // 3. 完成后把图集加上去
            InitMoBan(t, resBean);
        });
    }



    private void E_ResultDaoRu(EXuLieTu222 tuType, List<ResultBean> resultBeans)
    {
        string[] paths = new string[resultBeans.Count];
        ;
        for (int i = 0; i < resultBeans.Count; i++)
        {
            paths[i] = resultBeans[i].File.FullName;
        }
        // 保存一下信息
        bool isOk = Ctrl_TextureInfo.Instance.SaveXunLieTu222(tuType, paths);
        MyEventCenter.SendEvent<EGameType, bool, List<FileInfo>>(E_GameEvent.DaoRuResult, EGameType.XunLieTu222, isOk, null);
        if (!isOk)
        {
            return;
        }
        Transform t = InstantiateMoBan(go_MoBan, GetParent(tuType), CREATE_FILE_NAME);
        InitMoBan(t, resultBeans.ToArray());
    }


    //————————————————————————————————————

    private void E_IsShowChangeSize(bool isOn)          // 是否显示改变大小的Slider
    {
        go_ChangeSize.SetActive(isOn);

    }

    private void E_CloseDuoTuInfo(EGameType type)        // 关闭显示多图信息
    {
        if (type == EGameType.XunLieTu222)
        {
            go_Top.SetActive(true);
            go_Bottom.SetActive(true);
        }
    }


    private void E_DeleteOne(EGameType type, string[] paths)               // 多图信息中删除一个
    {
        if (type == EGameType.XunLieTu222)
        {
            Ctrl_TextureInfo.Instance.DeleteXuLieTu222Save(mCurrentIndex, paths);
            UnityEngine.Object.Destroy(go_CurrentSelect);
        }
    }



    private void E_DelteTrue(EGameType type)               // 真的删除
    {
        if (type == EGameType.XunLieTu222)
        {
            DeleteOneLine(mCurrentIndex);
        }
    }


    private void E_DeleteAll()                             // 删除所有
    {
        go_CurrentSelect = null;
        foreach (EXuLieTu222 type in Enum.GetValues(typeof(EXuLieTu222)))
        {
            DeleteOneLine(type);
        }

    }



}
