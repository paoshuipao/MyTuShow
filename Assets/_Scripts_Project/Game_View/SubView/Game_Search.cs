using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        AddButtOnClick("Top/BtnSearch", Btn_SureSearch);
        anim_ErrorTip = Get<DOTweenAnimation>("ErrorTip");
        anim_SearchNull = Get<DOTweenAnimation>("SearchNullTip");
        AddInputOnEndEdit(mInputField, (str) =>            // 输入完，按下回车
        {
            Btn_SureSearch();
        });

        // 内容
        go_MoBanDuoTu = GetGameObject("Bottom/MoBan_DuoTu");
        rt_Contant = Get<RectTransform>("Bottom/Contant");




    }

    
    #region 私有

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



    private void Btn_SureSearch()                     //  点击 确定搜索
    {
        string kName = mInputField.text;
        if (string.IsNullOrEmpty(kName) || kName.Length<=1)
        {
            anim_ErrorTip.gameObject.SetActive(true);
            anim_ErrorTip.DORestart();
            return;
        }

        if (mDropdown.value>0)
        {
            anim_SearchNull.gameObject.SetActive(true);
            anim_SearchNull.DORestart();
            return;
        }


        List<ResultBean[]> list = Ctrl_TextureInfo.SearchXLT(kName);
        if (list.Count == 0)
        {
            anim_SearchNull.gameObject.SetActive(true);
            anim_SearchNull.DORestart();
            return;
        }
        Ctrl_Coroutine.Instance.StartCoroutine(CreateXuLieTu(list));

    }



    IEnumerator CreateXuLieTu(List<ResultBean[]> list)
    {
        foreach (ResultBean[] resultBeanse in list)
        {
            Transform t = InstantiateMoBan(go_MoBanDuoTu, rt_Contant);
            // 大小
            t.Find("AnimTu").GetComponent<RectTransform>().sizeDelta = new Vector2(resultBeanse[0].SP.rect.width, resultBeanse[0].SP.rect.height);

            // 动图
            Sprite[] sps = new Sprite[resultBeanse.Length];
            for (int i = 0; i < resultBeanse.Length; i++)
            {
                sps[i] = resultBeanse[i].SP;
            }
            t.Find("AnimTu/Anim").GetComponent<UGUI_SpriteAnim>().ChangeAnim(sps);

            // 点击
            t.GetComponent<Button>().onClick.AddListener(() =>
            {
                
            });

            yield return new WaitForEndOfFrame();
        }

    }



}
