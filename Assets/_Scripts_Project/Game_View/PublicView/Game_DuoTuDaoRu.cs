using System.Collections;
using System.Collections.Generic;
using System.IO;
using PSPUtil;
using PSPUtil.Control;
using UnityEngine;
using UnityEngine.UI;

public class Game_DuoTuDaoRu : SubUI 
{
    protected override void OnStart(Transform root)
    {

        MyEventCenter.AddListener<ResultBean[],string>(E_GameEvent.ShowDuoTuDaoRu, E_Show);


        rt_Contant = Get<RectTransform>("Top/ScrollView/Contant");
        moBan_Item = GetGameObject("Top/ScrollView/MoBan");

        Get<Text>("BottomContrl/ScrollView/Contant/Item_XunLieTu/Contant/Item1/Btn/ItemDaoRu").text = Ctrl_UserInfo.BottomXuLieTuName[0];
        Get<Text>("BottomContrl/ScrollView/Contant/Item_XunLieTu/Contant/Item2/Btn/ItemDaoRu").text = Ctrl_UserInfo.BottomXuLieTuName[1];
        Get<Text>("BottomContrl/ScrollView/Contant/Item_XunLieTu/Contant/Item3/Btn/ItemDaoRu").text = Ctrl_UserInfo.BottomXuLieTuName[2];
        tx_DuoXLT222_1 = Get<Text>("BottomContrl/ScrollView/Contant/Item_XunLieTu222/Contant/Btn1/BtnMDR/ItemDaoRu");
        tx_DuoXLT222_2 = Get<Text>("BottomContrl/ScrollView/Contant/Item_XunLieTu222/Contant/Btn2/BtnMDR/ItemDaoRu");
        tx_DuoXLT222_3 = Get<Text>("BottomContrl/ScrollView/Contant/Item_XunLieTu222/Contant/Btn3/BtnMDR/ItemDaoRu");
        tx_DuoXLT222_4 = Get<Text>("BottomContrl/ScrollView/Contant/Item_XunLieTu222/Contant/Btn4/BtnMDR/ItemDaoRu");
        tx_DuoXLT222_5 = Get<Text>("BottomContrl/ScrollView/Contant/Item_XunLieTu222/Contant/Btn5/BtnMDR/ItemDaoRu");
        tx_DuoJHXuLie1 = Get<Text>("BottomContrl/ScrollView/Contant/Item_JiHeXuLie/Contant/Btn1/BtnMDR/ItemDaoRu");
        tx_DuoJHXuLie2 = Get<Text>("BottomContrl/ScrollView/Contant/Item_JiHeXuLie/Contant/Btn2/BtnMDR/ItemDaoRu");
        tx_DuoJHXuLie3 = Get<Text>("BottomContrl/ScrollView/Contant/Item_JiHeXuLie/Contant/Btn3/BtnMDR/ItemDaoRu");
        tx_DuoJHXuLie4 = Get<Text>("BottomContrl/ScrollView/Contant/Item_JiHeXuLie/Contant/Btn4/BtnMDR/ItemDaoRu");
        tx_DuoJHXuLie5 = Get<Text>("BottomContrl/ScrollView/Contant/Item_JiHeXuLie/Contant/Btn5/BtnMDR/ItemDaoRu");
        tx_DuoTaoMing1 = Get<Text>("BottomContrl/ScrollView/Contant/Item_Png/Contant/Btn1/BtnMDR/ItemDaoRu");
        tx_DuoTaoMing2 = Get<Text>("BottomContrl/ScrollView/Contant/Item_Png/Contant/Btn2/BtnMDR/ItemDaoRu");
        tx_DuoTaoMing3 = Get<Text>("BottomContrl/ScrollView/Contant/Item_Png/Contant/Btn3/BtnMDR/ItemDaoRu");
        tx_DuoTaoMing4 = Get<Text>("BottomContrl/ScrollView/Contant/Item_Png/Contant/Btn4/BtnMDR/ItemDaoRu");
        tx_DuoTaoMing5 = Get<Text>("BottomContrl/ScrollView/Contant/Item_Png/Contant/Btn5/BtnMDR/ItemDaoRu");
        tx_DuoJpg1 = Get<Text>("BottomContrl/ScrollView/Contant/Item_Jpg/Contant/Btn1/BtnMDR/ItemDaoRu");
        tx_DuoJpg2 = Get<Text>("BottomContrl/ScrollView/Contant/Item_Jpg/Contant/Btn2/BtnMDR/ItemDaoRu");
        tx_DuoJpg3 = Get<Text>("BottomContrl/ScrollView/Contant/Item_Jpg/Contant/Btn3/BtnMDR/ItemDaoRu");
        tx_DuoJpg4 = Get<Text>("BottomContrl/ScrollView/Contant/Item_Jpg/Contant/Btn4/BtnMDR/ItemDaoRu");
        tx_DuoJpg5 = Get<Text>("BottomContrl/ScrollView/Contant/Item_Jpg/Contant/Btn5/BtnMDR/ItemDaoRu");
        tx_DuoJiHe1 = Get<Text>("BottomContrl/ScrollView/Contant/Item_JiHe/Contant/Btn1/BtnMDR/ItemDaoRu");
        tx_DuoJiHe2 = Get<Text>("BottomContrl/ScrollView/Contant/Item_JiHe/Contant/Btn2/BtnMDR/ItemDaoRu");
        tx_DuoJiHe3 = Get<Text>("BottomContrl/ScrollView/Contant/Item_JiHe/Contant/Btn3/BtnMDR/ItemDaoRu");
        tx_DuoJiHe4 = Get<Text>("BottomContrl/ScrollView/Contant/Item_JiHe/Contant/Btn4/BtnMDR/ItemDaoRu");
        tx_DuoJiHe5 = Get<Text>("BottomContrl/ScrollView/Contant/Item_JiHe/Contant/Btn5/BtnMDR/ItemDaoRu");



        AddButtOnClick("BtnClose", Btn_OnCloseThis);
        // 序列图
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_XunLieTu/Contant/Item1/Btn", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G1Zheng, 0);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_XunLieTu/Contant/Item2/Btn", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G2Zheng_XiTong, 1);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_XunLieTu/Contant/Item3/Btn", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G3Zheng_Big, 2);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_XunLieTu/Contant/Item4/BtnHeng", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G4Two_Heng, 3);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_XunLieTu/Contant/Item4/BtnShu", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G4Two_Shu, 3);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_XunLieTu/Contant/Item5/BtnHeng", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G5Three_Heng, 4);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_XunLieTu/Contant/Item5/BtnShu", () =>
        {
            ManyBtn_XunLieTuDaoRu(EXunLieTu.G5Three_Shu, 4);
        });

        // 集合序列图
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_JiHeXuLie/Contant/Btn1/BtnMDR", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT1);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_JiHeXuLie/Contant/Btn2/BtnMDR", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT2);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_JiHeXuLie/Contant/Btn3/BtnMDR", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT3);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_JiHeXuLie/Contant/Btn4/BtnMDR", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT4);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_JiHeXuLie/Contant/Btn5/BtnMDR", () =>
        {
            ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType.JiHeXLT4);
        });

        // 透明图
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_Png/Contant/Btn1/BtnMDR", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.XiTong);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_Png/Contant/Btn2/BtnMDR", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.WenZi);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_Png/Contant/Btn3/BtnMDR", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.WuQi);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_Png/Contant/Btn4/BtnMDR", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.DaoJu);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_Png/Contant/Btn5/BtnMDR", () =>
        {
            ManyBtn_DaoRuTaoMingTu(ETaoMingType.ChengJi);
        });

        // Jpg
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_Jpg/Contant/Btn1/BtnMDR", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg1);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_Jpg/Contant/Btn2/BtnMDR", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg2);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_Jpg/Contant/Btn3/BtnMDR", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg3);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_Jpg/Contant/Btn4/BtnMDR", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg4);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_Jpg/Contant/Btn5/BtnMDR", () =>
        {
            ManyBtn_DaoRuJpgTu(ENormalTuType.Jpg5);
        });




        // 集合图
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_JiHe/Contant/Btn1/BtnMDR", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe1);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_JiHe/Contant/Btn2/BtnMDR", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe2);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_JiHe/Contant/Btn3/BtnMDR", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe3);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_JiHe/Contant/Btn4/BtnMDR", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe4);
        });
        AddButtOnClick("BottomContrl/ScrollView/Contant/Item_JiHe/Contant/Btn5/BtnMDR", () =>
        {
            ManyBtn_DaoRuJiHeTu(EJiHeType.JiHe5);
        });


        AddButtOnClick("Middle/BtnOpenFolder", Btn_OnOpenFolder);

    }



    #region 私有

    private GameObject mCuurentChooseBg;
    private GameObject moBan_Item;
    private RectTransform rt_Contant;
    private string mCurrentFolderPath;

    // 导入 Text
    private Text tx_DuoXLT222_1, tx_DuoXLT222_2, tx_DuoXLT222_3, tx_DuoXLT222_4, tx_DuoXLT222_5;
    private Text tx_DuoJHXuLie1, tx_DuoJHXuLie2, tx_DuoJHXuLie3, tx_DuoJHXuLie4, tx_DuoJHXuLie5;
    private Text tx_DuoTaoMing1, tx_DuoTaoMing2, tx_DuoTaoMing3, tx_DuoTaoMing4, tx_DuoTaoMing5;
    private Text tx_DuoJpg1, tx_DuoJpg2, tx_DuoJpg3, tx_DuoJpg4, tx_DuoJpg5;
    private Text tx_DuoJiHe1, tx_DuoJiHe2, tx_DuoJiHe3, tx_DuoJiHe4, tx_DuoJiHe5;


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


    public override string GetUIPathForRoot()
    {
        return "Right/DuoTuDaoRu";
    }



    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }


    #endregion



    private Dictionary<GameObject, ResultBean> itemSelectK_ResutltV = new Dictionary<GameObject, ResultBean>();     // item每行的作为 Key 结果为Value


    // UI事件
    private void Btn_OnCloseThis()                                   // 点击关闭
    {
        mUIGameObject.SetActive(false);
        if (itemSelectK_ResutltV.Count > 0)
        {
            List<GameObject> list = new List<GameObject>(itemSelectK_ResutltV.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                Object.Destroy(list[i]);
            }
            itemSelectK_ResutltV.Clear();
        }
    }

    private void Btn_OnOpenFolder()                                  // 打开文件夹
    {
        Application.OpenURL(mCurrentFolderPath);
    }


    //—————————————————— 每个 Item ——————————————————


    private void EachBtn_Delete(GameObject go)                        // 点击了 Item 的 Delete
    {
        itemSelectK_ResutltV.Remove(go);
        Object.Destroy(go);
    }


    private void EeachBtn_Up(GameObject go)                           // 点击了 Item 的 Up
    {

        ChangeDicIndex(go, -1);
    }


    private void EachBtn_Down(GameObject go)                         // 点击了 Item 的 Down
    {
        ChangeDicIndex(go, 1);
    }






    //—————————————————— 导入按钮 ——————————————————

    private void ManyBtn_XunLieTuDaoRu(EXunLieTu tuType, int index)           // 点击了序列图的导入
    {
        List<ResultBean> resultBeans = new List<ResultBean>(itemSelectK_ResutltV.Values);

        // 1. 发送导入的事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_XunLieTu, tuType, resultBeans);
        MyEventCenter.SendEvent(E_GameEvent.OnClickDuoTuDaoRu, resultBeans, index);

        // 2.关闭多选信息
        Btn_OnCloseThis();

    }


    // 导入按钮
    private void ManyBtn_DaoJiHeXuLieTu(EJiHeXuLieTuType type)           // 点击导入 集合序列图
    {
        List<ResultBean> resultBeans = new List<ResultBean>(itemSelectK_ResutltV.Values);
        // 1. 发送导入的事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_JiHeXuLieTu, type, resultBeans);
        MyEventCenter.SendEvent(E_GameEvent.OnClickDuoTuDaoRu, resultBeans, (int)type);
        // 2.关闭多选信息
        Btn_OnCloseThis();
    }

    private void ManyBtn_DaoRuTaoMingTu(ETaoMingType type)               // 点击导入 透明图
    {

        List<ResultBean> resultBeans = new List<ResultBean>(itemSelectK_ResutltV.Values);
        // 1. 发送导入的事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_TaoMingTu, type, resultBeans);
        MyEventCenter.SendEvent(E_GameEvent.OnClickDuoTuDaoRu, resultBeans, (int)type);
        // 2.关闭多选信息
        Btn_OnCloseThis();
    }

    private void ManyBtn_DaoRuJpgTu(ENormalTuType type)                  // 点击导入 Jpg
    {

        List<ResultBean> resultBeans = new List<ResultBean>(itemSelectK_ResutltV.Values);
        // 1. 发送导入的事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_NormalTu, type, resultBeans);
        MyEventCenter.SendEvent(E_GameEvent.OnClickDuoTuDaoRu, resultBeans, (int)type);
        // 2.关闭多选信息
        Btn_OnCloseThis();

    }


    private void ManyBtn_DaoRuJiHeTu(EJiHeType type)                     // 点击导入 集合
    {

        List<ResultBean> resultBeans = new List<ResultBean>(itemSelectK_ResutltV.Values);
        // 1. 发送导入的事件
        MyEventCenter.SendEvent(E_GameEvent.ResultDaoRu_JiHeTu, type, resultBeans);
        MyEventCenter.SendEvent(E_GameEvent.OnClickDuoTuDaoRu, resultBeans, (int)type);
        // 2.关闭多选信息
        Btn_OnCloseThis();

    }




    //—————————————————— 事件 ——————————————————

    private void E_Show(ResultBean[] resultBeans,string folderPath)
    {
        mCurrentFolderPath = folderPath;
        mUIGameObject.SetActive(true);

        tx_DuoXLT222_1.text = Ctrl_UserInfo.Instance.BottomXuLeTu222Name[0];
        tx_DuoXLT222_2.text = Ctrl_UserInfo.Instance.BottomXuLeTu222Name[1];
        tx_DuoXLT222_3.text = Ctrl_UserInfo.Instance.BottomXuLeTu222Name[2];
        tx_DuoXLT222_4.text = Ctrl_UserInfo.Instance.BottomXuLeTu222Name[3];
        tx_DuoXLT222_5.text = Ctrl_UserInfo.Instance.BottomXuLeTu222Name[4];

        tx_DuoJHXuLie1.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[0];
        tx_DuoJHXuLie2.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[1];
        tx_DuoJHXuLie3.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[2];
        tx_DuoJHXuLie4.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[3];
        tx_DuoJHXuLie5.text = Ctrl_UserInfo.Instance.BottomJiHeXLTName[4];

        tx_DuoTaoMing1.text = Ctrl_UserInfo.Instance.BottomTaoMingName[0];
        tx_DuoTaoMing2.text = Ctrl_UserInfo.Instance.BottomTaoMingName[1];
        tx_DuoTaoMing3.text = Ctrl_UserInfo.Instance.BottomTaoMingName[2];
        tx_DuoTaoMing4.text = Ctrl_UserInfo.Instance.BottomTaoMingName[3];
        tx_DuoTaoMing5.text = Ctrl_UserInfo.Instance.BottomTaoMingName[4];

        tx_DuoJpg1.text = Ctrl_UserInfo.Instance.BottomJpgName[0];
        tx_DuoJpg2.text = Ctrl_UserInfo.Instance.BottomJpgName[1];
        tx_DuoJpg3.text = Ctrl_UserInfo.Instance.BottomJpgName[2];
        tx_DuoJpg4.text = Ctrl_UserInfo.Instance.BottomJpgName[3];
        tx_DuoJpg5.text = Ctrl_UserInfo.Instance.BottomJpgName[4];

        tx_DuoJiHe1.text = Ctrl_UserInfo.Instance.BottomJiHeName[0];
        tx_DuoJiHe2.text = Ctrl_UserInfo.Instance.BottomJiHeName[1];
        tx_DuoJiHe3.text = Ctrl_UserInfo.Instance.BottomJiHeName[2];
        tx_DuoJiHe4.text = Ctrl_UserInfo.Instance.BottomJiHeName[3];
        tx_DuoJiHe5.text = Ctrl_UserInfo.Instance.BottomJiHeName[4];


        Ctrl_Coroutine.Instance.StartCoroutine(StartLoadDuoTu(resultBeans));

    }



    IEnumerator StartLoadDuoTu(ResultBean[] resultBeans)                                // 用线程 多张图的每个 Item
    {

        foreach (ResultBean bean in resultBeans)
        {
            Transform t = InstantiateMoBan(moBan_Item, rt_Contant);
            itemSelectK_ResutltV.Add(t.gameObject, bean);

            // 图标
            Transform btnIcon = t.Find("BtnIcon");
            btnIcon.GetComponent<Image>().sprite = bean.SP;
            FileInfo fileInfo = bean.File;
            btnIcon.GetComponent<Button>().onClick.AddListener(() =>
            {
                Application.OpenURL(fileInfo.FullName);
            });


            // 文件名
            t.Find("FileName").GetComponent<Text>().text = bean.File.Name;
            // 大小
            t.Find("Size").GetComponent<Text>().text = bean.Width + " x " + bean.Height;

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

}
