using System;
using System.Collections.Generic;
using System.IO;
using PSPUtil.Singleton;
using PSPUtil.StaticUtil;

[Serializable]
public class XunLieSaveBean
{
    public ushort TuType;
    public string KName;
    public string[] Paths;

}


public class Ctrl_TextureInfo : Singleton_Mono<Ctrl_TextureInfo> 
{


    public void DeleteAlll()                      // 删除所有
    {
        l_XunLieTuBean.Clear();

        foreach (EJiHeXuLieTuType type in Enum.GetValues(typeof(EJiHeXuLieTuType)))
        {
            DeleteJiHeXuLieOneLine(type);
        }

        foreach (ETaoMingType type in Enum.GetValues(typeof(ETaoMingType)))
        {
            DeleteTaoMingOneLine(type);
        }

        foreach (ENormalTuType type in Enum.GetValues(typeof(ENormalTuType)))
        {
            DeleteJpgOneLine(type);
        }

        foreach (EJiHeType type in Enum.GetValues(typeof(EJiHeType)))
        {
            DeleteJiHeOneLine(type);
        }

        // TODO 差音频删除

    }




    //—————————————————— 序列图 ——————————————————

    public List<string[]> GetXunLieTuPaths(EXunLieTu index)                // 获取
    {
        List<string[]> paths = new List<string[]>();
        foreach (XunLieSaveBean bean in l_XunLieTuBean)
        {
            if (bean.TuType == (ushort)index)
            {
                paths.Add(bean.Paths);
            }
        }
        return paths;

    }


    /// <summary>
    /// 保存序列图
    /// </summary>
    /// <param name="index"></param>
    /// <param name="paths"></param>
    /// <returns>true： 保存成功   false:之前已有，保存失败</returns>
    public bool SaveXunLieTu(EXunLieTu index,string[] paths)               // 保存
    {

        string kName = Path.GetFileNameWithoutExtension(paths[0]);
        if (!string.IsNullOrEmpty(kName))
        {
            kName = kName.Trim();
        }
        for (int i = 0; i < l_XunLieTuBean.Count; i++)
        {
            if (l_XunLieTuBean[i].KName == kName)
            {
                return false;
            }
        }
        XunLieSaveBean newBean = new XunLieSaveBean();
        newBean.TuType = (ushort)index;
        newBean.KName = kName;
        newBean.Paths = paths;
        l_XunLieTuBean.Add(newBean);
        return true;
    }


    public void DeleteXuLieTuSave(EXunLieTu index, string[] paths)         // 删除
    {
        string kName = Path.GetFileNameWithoutExtension(paths[0]);
        if (!string.IsNullOrEmpty(kName))
        {
            kName = kName.Trim();
        }
        for (int i = 0; i < l_XunLieTuBean.Count; i++)
        {
            XunLieSaveBean bean = l_XunLieTuBean[i];
            if (bean.KName == kName && bean.TuType == (ushort)index)
            {
                l_XunLieTuBean.RemoveAt(i);
                return;
            }
        }
    }



    public void DeleteXuLieTuOneLine(EXunLieTu index)                                     // 删除一行
    {
        for (int i = 0; i < l_XunLieTuBean.Count; i++)
        {
            if (l_XunLieTuBean[i].TuType == (ushort)index)
            {
                l_XunLieTuBean.RemoveAt(i);
            }
        }
    }


    #region 集合序列图 


    public List<string> GetJiHeXuLieTuPaths(EJiHeXuLieTuType index)                     // 获取
    {
        return jiHeXuLieTypeK_PathV[(ushort)index];
    }


    public bool SaveJiHeXuLieTu(EJiHeXuLieTuType index,string path)                    // 保存
    {

        if (!jiHeXuLieTypeK_PathV[(ushort)index].Contains(path))
        {
            jiHeXuLieTypeK_PathV[(ushort)index].Add(path);
            return true;
        }
        else
        {
            return false;
        }
    }


    public void DeleteJiHeXuLieSave(EJiHeXuLieTuType index,string path)                // 删除一个
    {
        if (jiHeXuLieTypeK_PathV[(ushort)index].Contains(path))
        {
            jiHeXuLieTypeK_PathV[(ushort) index].Remove(path);
        }
    }

    public void DeleteJiHeXuLieOneLine(EJiHeXuLieTuType index)                         // 删除整行
    {
        jiHeXuLieTypeK_PathV[(ushort)index].Clear();
    }


    #endregion


    #region 透明图


    public List<string> GetTaoMingTuPaths(ETaoMingType index)                      // 获取
    {
        return taoMingTypeK_PathV[(ushort)index];
    }


    public bool SaveTaoMingTu(ETaoMingType index, string path)                     // 保存
    {

        if (!taoMingTypeK_PathV[(ushort)index].Contains(path))
        {
            taoMingTypeK_PathV[(ushort)index].Add(path);
            return true;
        }
        else
        {
            return false;
        }
    }


    public void DeleteTaoMingSave(ETaoMingType index, string path)                 // 删除一个
    {
        if (taoMingTypeK_PathV[(ushort)index].Contains(path))
        {
            taoMingTypeK_PathV[(ushort)index].Remove(path);
        }
    }

    public void DeleteTaoMingOneLine(ETaoMingType index)                           // 删除整行
    {
        taoMingTypeK_PathV[(ushort)index].Clear();
    }

    #endregion

    #region Jpg


    public List<string> GetJpgTuPaths(ENormalTuType index)                      // 获取
    {
        return normalTypeK_PathV[(ushort)index];
    }


    public bool SaveJpgTu(ENormalTuType index, string path)                     // 保存
    {

        if (!normalTypeK_PathV[(ushort)index].Contains(path))
        {
            normalTypeK_PathV[(ushort)index].Add(path);
            return true;
        }
        else
        {
            return false;
        }
    }


    public void DeleteJpgSave(ENormalTuType index, string path)                 // 删除一个
    {
        if (normalTypeK_PathV[(ushort)index].Contains(path))
        {
            normalTypeK_PathV[(ushort)index].Remove(path);
        }
    }

    public void DeleteJpgOneLine(ENormalTuType index)                           // 删除整行
    {
        normalTypeK_PathV[(ushort)index].Clear();
    }

    #endregion


    #region 集合图


    public List<string> GetJiHeTuPaths(EJiHeType index)                      // 获取
    {
        return jiHeTypeK_PathV[(ushort)index];
    }


    public bool SaveJiHeTu(EJiHeType index, string path)                     // 保存
    {

        if (!jiHeTypeK_PathV[(ushort)index].Contains(path))
        {
            jiHeTypeK_PathV[(ushort)index].Add(path);
            return true;
        }
        else
        {
            return false;
        }
    }


    public void DeleteJiHeSave(EJiHeType index, string path)                 // 删除一个
    {
        if (jiHeTypeK_PathV[(ushort)index].Contains(path))
        {
            jiHeTypeK_PathV[(ushort)index].Remove(path);
        }
    }

    public void DeleteJiHeOneLine(EJiHeType index)                           // 删除整行
    {
        jiHeTypeK_PathV[(ushort)index].Clear();
    }

    #endregion


    //—————————————————— 音频 ——————————————————
    public List<string> GetAudioPaths(EAudioType index)                        // 获取
    {
        return audioTypeK_PathV[(ushort)index];
    }

    public bool SaveAudio(EAudioType index, string path)                       // 保存
    {

        if (!audioTypeK_PathV[(ushort)index].Contains(path))
        {
            audioTypeK_PathV[(ushort)index].Add(path);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DeleteAudioSave(EAudioType index, string path)                 // 删除
    {
        if (audioTypeK_PathV[(ushort)index].Contains(path))
        {
            audioTypeK_PathV[(ushort)index].Remove(path);
        }
    }




    #region 私有

    // 序列图
    private List<XunLieSaveBean> l_XunLieTuBean;
    private const string PP_XUN_LIE_TU = "PP_XUN_LIE_TU";
    private const string XunLieTuFile = "XunLieTu.es3";

    // 集合序列图
    private Dictionary<ushort, List<string>> jiHeXuLieTypeK_PathV;
    private const string PP_JIHE_XULIE_TU = "PP_JIHE_XULIE_TU";
    private const string JiHeXuLieTuFile = "JiHeXuLieTu.es3";

    // 透明图
    private Dictionary<ushort,List<string>>  taoMingTypeK_PathV;
    private const string PP_TAO_MING_TU = "PP_TAO_MING_TU";
    private const string TaoMingTuFile = "TaoMingTu.es3";

    // Jpg
    private Dictionary<ushort, List<string>> normalTypeK_PathV;
    private const string PP_JPG_TU = "PP_JPG_TU";
    private const string JpgTuFile = "JpgTu.es3";

    // 集合图
    private Dictionary<ushort, List<string>> jiHeTypeK_PathV;
    private const string PP_JI_HE_TU = "PP_JI_HE_TU";
    private const string JiHeTuFile = "JiHeTu.es3";



    // 音频
    private Dictionary<ushort, List<string>> audioTypeK_PathV;
    private const string PP_AUDIO = "PP_AUDIO";
    private const string AudioFile = "AudioFile.es3";



    #endregion

    public bool IsInitFinish =false;


    public void OnInitData()
    {
        // 序列图
        l_XunLieTuBean = ES3.Load(PP_XUN_LIE_TU, XunLieTuFile, new List<XunLieSaveBean>());

        #region 集合序列图

        if (!ES3.KeyExists(PP_JIHE_XULIE_TU, JiHeXuLieTuFile))
        {
            jiHeXuLieTypeK_PathV = new Dictionary<ushort, List<string>>();
            foreach (EJiHeXuLieTuType type in Enum.GetValues(typeof(EJiHeXuLieTuType)))
            {
                jiHeXuLieTypeK_PathV.Add((ushort)type, new List<string>());
            }
        }
        else
        {
            jiHeXuLieTypeK_PathV = ES3.Load(PP_JIHE_XULIE_TU, JiHeXuLieTuFile, new Dictionary<ushort, List<string>>());
        }

        #endregion

        #region 透明图

        if (!ES3.KeyExists(PP_TAO_MING_TU, TaoMingTuFile))
        {
            taoMingTypeK_PathV = new Dictionary<ushort, List<string>>();
            foreach (ETaoMingType type in Enum.GetValues(typeof(ETaoMingType)))
            {
                taoMingTypeK_PathV.Add((ushort)type,new List<string>());
            }
        }
        else
        {
            taoMingTypeK_PathV = ES3.Load(PP_TAO_MING_TU, TaoMingTuFile, new Dictionary<ushort, List<string>>());
        }

        #endregion

        #region Jpg

        if (!ES3.KeyExists(PP_JPG_TU, JpgTuFile))
        {
            normalTypeK_PathV = new Dictionary<ushort, List<string>>();
            foreach (ENormalTuType type in Enum.GetValues(typeof(ENormalTuType)))
            {
                normalTypeK_PathV.Add((ushort)type, new List<string>());
            }
        }
        else
        {
            normalTypeK_PathV = ES3.Load(PP_JPG_TU, JpgTuFile, new Dictionary<ushort, List<string>>());
        }

        #endregion

        #region 集合图

        if (!ES3.KeyExists(PP_JI_HE_TU, JiHeTuFile))
        {
            jiHeTypeK_PathV = new Dictionary<ushort, List<string>>();
            foreach (EJiHeType type in Enum.GetValues(typeof(EJiHeType)))
            {
                jiHeTypeK_PathV.Add((ushort)type, new List<string>());
            }
        }
        else
        {
            jiHeTypeK_PathV = ES3.Load(PP_JI_HE_TU, JiHeTuFile, new Dictionary<ushort, List<string>>());
        }

        #endregion

        // 音频
        if (!ES3.KeyExists(PP_AUDIO, AudioFile))
        {
            audioTypeK_PathV = new Dictionary<ushort, List<string>>();
            foreach (EAudioType type in Enum.GetValues(typeof(EAudioType)))
            {
                audioTypeK_PathV.Add((ushort)type, new List<string>());
            }
        }
        else
        {
            audioTypeK_PathV = ES3.Load(PP_AUDIO, AudioFile, new Dictionary<ushort, List<string>>());
        }

        IsInitFinish = true;
    }


    void OnApplicationQuit()
    {
        // 退出时保存
        ES3.Save<List<XunLieSaveBean>>(PP_XUN_LIE_TU, l_XunLieTuBean, XunLieTuFile);
        ES3.Save<Dictionary<ushort, List<string>>>(PP_JIHE_XULIE_TU, jiHeXuLieTypeK_PathV, JiHeXuLieTuFile);
        ES3.Save<Dictionary<ushort, List<string>>>(PP_TAO_MING_TU, taoMingTypeK_PathV, TaoMingTuFile);
        ES3.Save<Dictionary<ushort, List<string>>>(PP_JPG_TU, normalTypeK_PathV, JpgTuFile);
        ES3.Save<Dictionary<ushort, List<string>>>(PP_JI_HE_TU, jiHeTypeK_PathV, JiHeTuFile);
        ES3.Save<Dictionary<ushort, List<string>>>(PP_AUDIO, audioTypeK_PathV, AudioFile);


    }





}
