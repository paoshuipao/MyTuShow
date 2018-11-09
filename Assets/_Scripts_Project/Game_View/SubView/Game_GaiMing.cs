using PSPUtil;
using UnityEngine;
using UnityEngine.UI;

public class Game_GaiMing : SubUI 
{



    #region 私有

    private Text tx_YuanName;


    public override string GetUIPathForRoot()
    {
        return "Right/GaiNing";
    }


    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }

    #endregion



    protected override void OnStart(Transform root)
    {

        MyEventCenter.AddListener<string>(E_GameEvent.ShowGeiMingUI, E_OnShow);

        tx_YuanName = Get<Text>("Contant/Grid/Middle/TxYuan");




        AddButtOnClick("Contant/Grid/Bottom/BtnSure",Btn_OnSure);
        AddButtOnClick("Contant/Grid/Bottom/BtnFalse", Btn_OnFalse);


    }


    private void Btn_OnSure()                     // 点击确定
    {
        mUIGameObject.SetActive(false);
    }

    private void Btn_OnFalse()                     // 点击取消
    {
        mUIGameObject.SetActive(false);
    }


    //—————————————————— 事件——————————————————

    private void E_OnShow(string yuanName)        // 显示
    {
        tx_YuanName.text = yuanName;
        mUIGameObject.SetActive(true);
    }





}
