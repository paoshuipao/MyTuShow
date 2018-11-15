using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PSPUtil;
using PSPUtil.Control;
using PSPUtil.StaticUtil;
using UnityEngine;
using UnityEngine.UI;

public class Game_Search : SubUI 
{

    protected override void OnStart(Transform root)
    {
        // 上
        mInputField = Get<InputField>("Top/InputField");
        mDropdown = Get<Dropdown>("Top/Dropdown");
        AddButtOnClick("Top/BtnSearch", () =>
        {
            Btn_SureSearch(true);
        });
        anim_ErrorTip = Get<DOTweenAnimation>("ErrorTip");
        anim_SearchNull = Get<DOTweenAnimation>("SearchNullTip");
        AddInputOnEndEdit(mInputField, (str) =>            // 输入完，按下回车
        {
            Btn_SureSearch(false);
        });

        // 内容
        go_MoBanDuoTu = GetGameObject("Bottom/MoBan_DuoTu");
        rt_Contant = Get<RectTransform>("Bottom/Contant");




    }

    
    #region 私有

    private string mCurrentInputStr;         // 当前搜索的字符


    // 上
    private InputField mInputField;
    private Dropdown mDropdown;
    private DOTweenAnimation anim_ErrorTip,anim_SearchNull;

    // 模版
    private GameObject go_MoBanDuoTu;

    private RectTransform rt_Contant;

    public override string GetUIPathForRoot()
    {
        return "Right/EachContant/Search";
    }



    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }

    #endregion



    private void Btn_SureSearch(bool isShowNullTip)                     //  点击 确定搜索
    {

        // 提示输入少于 2 位数
        string kName = mInputField.text;
        if (string.IsNullOrEmpty(kName) || kName.Length<=1)
        {
            if (isShowNullTip)
            {
                anim_ErrorTip.gameObject.SetActive(true);
                anim_ErrorTip.DORestart();
            }
            return;
        }

        // 当前的字符等于之前的字符
        if (kName == mCurrentInputStr)
        {
            return;
        }
        mCurrentInputStr = kName;
        // 提示找不到（TODO 暂停只找序列图）
        if (mDropdown.value>0)
        {
            anim_SearchNull.gameObject.SetActive(true);
            anim_SearchNull.DORestart();
            return;
        }

        // 先把之前的删除
        for (int i = 0; i < rt_Contant.childCount; i++)
        {
            Object.Destroy(rt_Contant.GetChild(i).gameObject);
        }
        // 搜索 序列图的
        Dictionary<string,ResultBean[]> dir = Ctrl_TextureInfo.SearchXLT(kName);
        if (dir.Count == 0)
        {
            anim_SearchNull.gameObject.SetActive(true);
            anim_SearchNull.DORestart();
            return;
        }
        Ctrl_Coroutine.Instance.StartCoroutine(CreateXuLieTu(dir));

    }



    IEnumerator CreateXuLieTu(Dictionary<string, ResultBean[]> dir)
    {

        foreach (string kName in dir.Keys)
        {
            ResultBean[] resultBeanse = dir[kName];

            Transform t = InstantiateMoBan(go_MoBanDuoTu, rt_Contant);
            // 大小
            t.Find("AnimTu").GetComponent<RectTransform>().sizeDelta = new Vector2(resultBeanse[0].SP.rect.width, resultBeanse[0].SP.rect.height);

            // 动图
            Sprite[] sps = new Sprite[resultBeanse.Length];
            for (int j = 0; j < resultBeanse.Length; j++)
            {
                sps[j] = resultBeanse[j].SP;
            }
            t.Find("AnimTu/Anim").GetComponent<UGUI_SpriteAnim>().ChangeAnim(sps);

            // 名称
            t.Find("TxName").GetComponent<Text>().text = kName;


            yield return new WaitForEndOfFrame();
        }
    }



}
