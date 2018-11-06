using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NAudio.Wave;
using PSPUtil.Control;
using PSPUtil.Singleton;
using PSPUtil.StaticUtil;
using UnityEngine;
using UnityEngine.Networking;

public class LoadAudioClip : Singleton_Mono<LoadAudioClip>
{


    public void StartLoadAudioClip(FileInfo fileInfo, Action<AudioClip> callBack)
    {

        foreach (string key in pathK_ClipV.Keys)
        {
            if (fileInfo.FullName == key)
            {
                callBack(pathK_ClipV[key]);
                return;
            }
        }

        if (fileInfo.Extension == ".mp3")
        {
            AudioResBean resBean = null;
            for (int i = 0; i < l_HasLoadMp3.Count; i++)
            {
                if (l_HasLoadMp3[i].YuanPath == fileInfo.FullName)
                {
                    resBean = l_HasLoadMp3[i];
                    break;
                }
            }
            if (null == resBean)
            {
                resBean = new AudioResBean();
                string savePath = dirPath + "/" + Path.GetFileNameWithoutExtension(fileInfo.FullName) + ".wav";
                new Thread(() =>
                {
                    try
                    {
                        FileStream stream = File.Open(fileInfo.FullName, FileMode.Open);
                        Mp3FileReader reader = new Mp3FileReader(stream);
                        WaveFileWriter.CreateWaveFile(savePath, reader);
                        resBean.YuanPath = fileInfo.FullName;
                        resBean.isOk = true;
                        resBean.SavePath = savePath;
                        l_HasLoadMp3.Add(resBean);
                    }
                    catch (Exception e)
                    {
                        MyLog.Red("有错 —— " + e);
                        throw;
                    }
                }).Start();
            }
            Ctrl_Coroutine.Instance.StartCoroutine(LoadMp3(resBean, callBack));
        }
        else if (fileInfo.Extension == ".ogg")
        {
            Ctrl_Coroutine.Instance.StartCoroutine(LoadOtherGeShi(fileInfo.FullName, AudioType.OGGVORBIS, callBack));
        }
        else if (fileInfo.Extension == ".aiff")
        {
            Ctrl_Coroutine.Instance.StartCoroutine(LoadOtherGeShi(fileInfo.FullName, AudioType.AIFF, callBack));
        }
        else if (fileInfo.Extension == ".wav")
        {
            Ctrl_Coroutine.Instance.StartCoroutine(LoadOtherGeShi(fileInfo.FullName, AudioType.WAV, callBack));
        }
        else
        {
            MyLog.Red("还有其他格式？");
        }
    }


    #region 私有



    class AudioResBean
    {
        public string YuanPath; // 原路径
        public string SavePath; // 保存的路径
        public bool isOk; // 是否转完成
    }

    private const string SAVE_FOLDER_NAME = "/Mp3SavePath";
    private string dirPath;
    private readonly List<AudioResBean> l_HasLoadMp3 = new List<AudioResBean>(); // 已经下载过的 mp3
    private readonly Dictionary<string, AudioClip> pathK_ClipV = new Dictionary<string, AudioClip>();



    IEnumerator LoadOtherGeShi(string path, AudioType audioType, Action<AudioClip> callBack)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + path, audioType))
        {
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                MyLog.Red(request.error);
                yield break;
            }
            AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
            clip.name = Path.GetFileNameWithoutExtension(path);
            pathK_ClipV.Add(path,clip);
            if (null != callBack)
            {
                callBack(clip);
            }
        }
    }

    IEnumerator LoadMp3(AudioResBean res, Action<AudioClip> callBack)
    {
        while (!res.isOk)
        {
            yield return new WaitForSeconds(0.1f);
        }
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + res.SavePath, AudioType.WAV))
        {
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                MyLog.Red(request.error);
                yield break;
            }
            AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
            clip.name = Path.GetFileNameWithoutExtension(res.SavePath);
            pathK_ClipV.Add(res.YuanPath, clip);
            if (null != callBack)
            {
                callBack(clip);
            }
        }

    }

    #endregion


    protected override void OnAwake()
    {
        base.OnAwake();
        dirPath = Application.persistentDataPath + SAVE_FOLDER_NAME;
        DirectoryInfo dir = new DirectoryInfo(dirPath);
        if (!dir.Exists)
        {
            dir.Create();
        }
    }

    void OnApplicationQuit() // 退出把加载的所有音乐删除
    {
        DirectoryInfo dir = new DirectoryInfo(dirPath);
        dir.Delete(true);
    }




}