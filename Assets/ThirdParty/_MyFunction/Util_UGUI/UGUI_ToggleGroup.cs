using System;
using System.Collections.Generic;
using PSPUtil.Attribute;
using PSPUtil.StaticUtil;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("我的组件/UI/UGUI_ToggleGroup(Toggle组扩展)", 10)]
public class UGUI_ToggleGroup : MonoBehaviour
{

    public Action<string> OnChangeValue;        // 不同的切换才调用

    public Action OnEachClick;                  // 只要点击就调用


    [ReadOnly]
    public string CurrentName;




    public void ChangeToggleOn(string changeName)
    {
        bool isChangeTure = false;
        foreach (Toggle subToggle in SubToggles)
        {
            subToggle.isOn = false;
        }


        for (int i = 0; i < SubToggles.Length; i++)
        {
            if (SubToggles[i].name.Equals(changeName))         // 如果是要改变的名称，那就切换成它
            {
                isChangeTure = true;
                SubToggles[i].isOn = true;
            }
        }

        if (!isChangeTure)
        {
            MyLog.Red(name + "  没有改变到 Toggle 的对应名称 ——"+ changeName);
        }
    }





    public Toggle[] SubToggles;



    void Awake()
	{
	    foreach (Toggle toggle in SubToggles)
	    {
	        string changeName = toggle.name;

            toggle.onValueChanged.AddListener((isOn) =>
	        {
	            if (null!= OnEachClick)
	            {
	                OnEachClick();
	            }
	            if (isOn&& CurrentName!= changeName)
	            {
	                if (null != OnChangeValue)
	                {
                        OnChangeValue(changeName);
	                    CurrentName = changeName;
	                }
                }
	        });

        }


	}


    [Button("重新 刷新 Toggle")]
    public void ResetSubToggle()                  // 用来刷新
    {
        SubToggles = GetComponentsInChildren<Toggle>();
    }



}
