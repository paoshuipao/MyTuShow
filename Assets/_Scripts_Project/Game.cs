﻿using System.Collections;
using PSPUtil;
using UnityEngine;

public class Game : MonoBehaviour
{

    public GameObject LOGO;

    void Awake()
    {
        Manager.Init();                      // 初始化所有的 Manager
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }


    void Start()
    {
        if (!LOGO.activeSelf)
        {
            LOGO.SetActive(true);
        }
        LOGO.transform.localPosition = Vector3.zero;
        LOGO.transform.localScale =Vector3.one;
        StartCoroutine(JumpScene());

    }

    void OnDestroy()
    {
        MyEventCenter.SendEvent(E_GameEvent.LogoExit);
    }



    IEnumerator JumpScene()
    {
        Ctrl_TextureInfo.Instance.OnInitData();
        yield return new WaitForSeconds(1f);
        Manager.Get<MySceneManager>(EF_Manager.MyScene).LoadScene(EF_Scenes._1_Start);

    }

}
