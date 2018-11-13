using System.Collections;
using System.Collections.Generic;
using System.IO;
using PSPUtil;
using PSPUtil.Control;
using UnityEngine;
using UnityEngine.UI;

public class Game_DuoTuInfo : SubUI
{


    protected override void OnStart(Transform root)
    {
        MyEventCenter.AddListener<EGameType, ResultBean[]>(E_GameEvent.ShowDuoTuInfo, E_Show);
        AddButtOnClick("Contant/BtnClose", Btn_OnCloseShowInfo);

        //  左边
        rtAnimTu = Get<RectTransform>("Contant/Left/Contant/Tu/AnimTu");
        anim_Tu = Get<UGUI_SpriteAnim>("Contant/Left/Contant/Tu/AnimTu/Anim");
        tx_WidthSize = Get<Text>("Contant/Left/Contant/SliderWidth/TxValue");
        tx_HeightSize = Get<Text>("Contant/Left/Contant/SliderHeight/TxValue");
        slider_Width = Get<Slider>("Contant/Left/Contant/SliderWidth/Slider");
        slider_Height = Get<Slider>("Contant/Left/Contant/SliderHeight/Slider");
        AddButtOnClick("Contant/Left/Contant/Tu/AnimTu", Btn_OnAnimTuClick);
        AddSliderOnValueChanged(slider_Width, (value) =>
        {
            SetTuSize(value);
        });
        AddSliderOnValueChanged(slider_Height, (value) =>
        {
            SetTuSize(0, value);
        });
        AddButtOnClick("Contant/Left/Contant/BtnSize/BtnFirst", () =>
        {
            SetTuSize(yuanLaiWidth, yuanLaiHidth);
        });
        AddButtOnClick("Contant/Left/Contant/BtnSize/BtnPlusHalf", () =>
        {
            SetTuSize(yuanLaiWidth * 0.5f, yuanLaiHidth * 0.5f);
        });
        AddButtOnClick("Contant/Left/Contant/BtnSize/BtnAddHalf", () =>
        {
            SetTuSize(yuanLaiWidth * 1.5f, yuanLaiHidth * 1.5f);
        });
        AddButtOnClick("Contant/Left/Contant/BtnSize/BtnAddTwo", () =>
        {
            SetTuSize(yuanLaiWidth * 2f, yuanLaiHidth * 2f);
        });

        // 右边
        tx_InfoName = Get<Text>("Contant/Right/InfoName/Name");
        tx_InfoNum = Get<Text>("Contant/Right/InfoNum/TxNum");
        go_ItemMoBan = GetGameObject("Contant/Right/Item/ScrollRect/Contant/MoBan");
        rt_ItemContant = Get<RectTransform>("Contant/Right/Item/ScrollRect/Contant");
        AddButtOnClick("Contant/Right/BtnOpenFolder/Btn", Btn_OnOpenFolder);
        AddButtOnClick("Contant/Right/BtnDelete/Btn", Btn_OnNoSaveThis);


    }




    #region 私有

    private EGameType mCurrentType;
    private ResultBean[] l_CurrentResultBeans; // 当前选择的文件集合


    // 双击显示信息
    private readonly List<GameObject> l_InfoItems = new List<GameObject>(); // 双击 右边的 Item
    private float yuanLaiWidth, yuanLaiHidth;

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


    public override string GetUIPathForRoot()
    {
        return "Right/DuoTuInfo";
    }



    public override void OnEnable()
    {

    }

    public override void OnDisable()
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



    private Sprite[] GetSpriteList(ResultBean[] beans)
    {
        Sprite[] sps = new Sprite[beans.Length];
        for (int i = 0; i < beans.Length; i++)
        {
            sps[i] = beans[i].SP;
        }
        return sps;
    }


    #endregion



    private void Btn_OnCloseShowInfo() // 关闭打开的信息
    {
        mUIGameObject.SetActive(false);
        Ctrl_Coroutine.Instance.StopAllCoroutines();
        for (int i = 0; i < l_InfoItems.Count; i++)
        {
            Object.Destroy(l_InfoItems[i]);
        }
        l_InfoItems.Clear();
        l_CurrentResultBeans = null;
        MyEventCenter.SendEvent(E_GameEvent.CloseDuoTuInfo, mCurrentType);
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
        MyEventCenter.SendEvent(E_GameEvent.OnClickNoSaveThisDuoTu, mCurrentType, paths);
        Btn_OnCloseShowInfo();
    }



    //—————————————————— 事件 ——————————————————


    private void E_Show(EGameType type, ResultBean[] resultBeans)
    {
        mCurrentType = type;
        l_CurrentResultBeans = resultBeans;
        mUIGameObject.SetActive(true);
        tx_InfoName.text = resultBeans[0].SP.name;
        tx_InfoNum.text = resultBeans.Length.ToString();
        anim_Tu.ChangeAnim(GetSpriteList(resultBeans));
        yuanLaiWidth = resultBeans[0].Width;
        yuanLaiHidth = resultBeans[0].Height;
        SetTuSize(yuanLaiWidth, yuanLaiHidth);
        Ctrl_Coroutine.Instance.StartCoroutine(LoadInfoItem(resultBeans));

    }


}
