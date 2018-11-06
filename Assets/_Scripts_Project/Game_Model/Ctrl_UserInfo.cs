using System;
using System.Collections.Generic;
using System.IO;
using PSPUtil.Singleton;
using UnityEngine;


[Serializable]
public class GridSizeBean
{
    public Vector2 YuanSize;          // 原来大小
    public Vector2 CurrentSize;        // 当前大小
    public int ChangeValue;            // 改变的大小

}



public class Ctrl_UserInfo : Singleton_Mono<Ctrl_UserInfo>
{



    public string DaoRuFirstPath { get; set; }                     // 导入时 打开的路径（导入框）

    public List<string> L_FavoritesPath { get; set; }              // 收藏的路径集合


    public string ShowFirstPath { get; set; }                     // 点击导入的大项，一开始显示的路径



    public bool IsXuLieTuShowTip { get; set; }                   // 序列图是否需要提示




    //——————————————————— 大小 —————————————————
    public bool IsCanChangeSize { get; set; }                       // 是否可改大小


    public GridSizeBean[] L_JiHeXuLieTuSize { get; set; }             // 集合序列图 Grid大小

    public GridSizeBean[] L_TaoMingTuSize { get; set; }               // 透明图 Grid大小

    public GridSizeBean[] L_JPGTuSize { get; set; }                   // Jpg图 Grid大小

    public GridSizeBean[] L_JiHeTuSize { get; set; }                  // 集合图 Grid大小

    



    #region 私有


    private const string PP_DAORU_PATH = "PP_DAORU_PATH";
    private const string PP_FAVORITES_PATH = "PP_FAVORITES_PATH";
    private const string PP_SHOW_FIRST_PATH = "PP_SHOW_FIRST_PATH";
    private const string PP_IS_XLT_SHOW_TIP = "PP_IS_XLT_SHOW_TIP";


    // 大小
    private const string PP_IS_CHANGE_SIZE = "PP_IS_CHANGE_SIZE";
    private const string PP_JIHE_XLT_SIZES = "PP_JIHE_XLT_SIZES";
    private const string PP_TAO_MING_SIZE = "PP_TAO_MING_SIZE";
    private const string PP_JPG_SIZE = "PP_JPG_SIZE";
    private const string PP_JI_HE_SIZE = "PP_JI_HE_SIZE";



    private string GetPath(string pp)    // 判断路径是否存在，不存在返回桌面的路径
    {
        string path = ES3.LoadStr(pp, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)  // 不存在的情况
        {
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
        return path;
    }


    #endregion


    protected override void OnAwake()
    {
        base.OnAwake();
        L_FavoritesPath = ES3.Load(PP_FAVORITES_PATH,new List<string>());
        DaoRuFirstPath = GetPath(PP_DAORU_PATH);
        ShowFirstPath = GetPath(PP_SHOW_FIRST_PATH);
        IsXuLieTuShowTip = ES3.Load(PP_IS_XLT_SHOW_TIP, true);
        IsCanChangeSize = ES3.Load(PP_IS_CHANGE_SIZE, false);

        // 集合序列图
        if (!ES3.KeyExists(PP_JIHE_XLT_SIZES))
        {
            L_JiHeXuLieTuSize = new GridSizeBean[5];
            L_JiHeXuLieTuSize[0] = GetGridSizeBean(128, 128);
            L_JiHeXuLieTuSize[1] = GetGridSizeBean(325, 325);
            L_JiHeXuLieTuSize[2] = GetGridSizeBean(325, 325);
            L_JiHeXuLieTuSize[3] = GetGridSizeBean(325, 325);
            L_JiHeXuLieTuSize[4] = GetGridSizeBean(325, 325);
        }
        else
        {
            L_JiHeXuLieTuSize = ES3.Load<GridSizeBean[]>(PP_JIHE_XLT_SIZES);
        }


        // 透明图
        if (!ES3.KeyExists(PP_TAO_MING_SIZE))
        {
            L_TaoMingTuSize = new GridSizeBean[5];
            L_TaoMingTuSize[0] = GetGridSizeBean(128, 128);
            L_TaoMingTuSize[1] = GetGridSizeBean(128, 128);
            L_TaoMingTuSize[2] = GetGridSizeBean(128, 128);
            L_TaoMingTuSize[3] = GetGridSizeBean(128, 128);
            L_TaoMingTuSize[4] = GetGridSizeBean(128, 128);
        }
        else
        {
            L_TaoMingTuSize = ES3.Load<GridSizeBean[]>(PP_TAO_MING_SIZE);
        }

        // Jpg图
        if (!ES3.KeyExists(PP_JPG_SIZE))
        {
            L_JPGTuSize = new GridSizeBean[5];
            L_JPGTuSize[0] = GetGridSizeBean(128, 128);
            L_JPGTuSize[1] = GetGridSizeBean(128, 128);
            L_JPGTuSize[2] = GetGridSizeBean(128, 128);
            L_JPGTuSize[3] = GetGridSizeBean(128, 128);
            L_JPGTuSize[4] = GetGridSizeBean(128, 128);
        }
        else
        {
            L_JPGTuSize = ES3.Load<GridSizeBean[]>(PP_JPG_SIZE);
        }

        // 集合图
        if (!ES3.KeyExists(PP_JI_HE_SIZE))
        {
            L_JiHeTuSize = new GridSizeBean[5];
            L_JiHeTuSize[0] = GetGridSizeBean(325, 325);
            L_JiHeTuSize[1] = GetGridSizeBean(325, 325);
            L_JiHeTuSize[2] = GetGridSizeBean(325, 325);
            L_JiHeTuSize[3] = GetGridSizeBean(325, 325);
            L_JiHeTuSize[4] = GetGridSizeBean(325, 325);
        }
        else
        {
            L_JiHeTuSize = ES3.Load<GridSizeBean[]>(PP_JI_HE_SIZE);
        }
    }

    private GridSizeBean GetGridSizeBean(float x,float y)
    {
        GridSizeBean bean = new GridSizeBean();
        bean.YuanSize = new Vector2(x,y);
        bean.CurrentSize = bean.YuanSize;
        bean.ChangeValue = 0;
        return bean;
    }









    void OnApplicationQuit()
    {
        ES3.Save<string>(PP_DAORU_PATH, DaoRuFirstPath);
        ES3.Save<List<string>>(PP_FAVORITES_PATH, L_FavoritesPath);
        ES3.Save<string>(PP_SHOW_FIRST_PATH, ShowFirstPath);
        ES3.Save<bool>(PP_IS_XLT_SHOW_TIP, IsXuLieTuShowTip);
        // 大小
        ES3.Save<bool>(PP_IS_CHANGE_SIZE, IsCanChangeSize);
        ES3.Save<GridSizeBean[]>(PP_JIHE_XLT_SIZES, L_JiHeXuLieTuSize);
        ES3.Save<GridSizeBean[]>(PP_TAO_MING_SIZE, L_TaoMingTuSize);
        ES3.Save<GridSizeBean[]>(PP_JPG_SIZE, L_JPGTuSize);
        ES3.Save<GridSizeBean[]>(PP_JI_HE_SIZE, L_JiHeTuSize);



    }





    //——————————————————  不保存的 ——————————————————


    public static float DoubleClickTime = 0.5f;                  // 双击的控制时间（少于这个时间就算是双击）

}
