using System.Collections.Generic;
using PSPUtil.Extensions;
using PSPUtil.StaticUtil;
using UnityEngine;

public class LogManager_Old : Manager
{
    public LogPosition Position = LogPosition.FullScreen; //Log位置
    public int TextSize = 35; //Log字体大小

    #region 私有

    private bool IsShowLog = true;
    private float m_X, m_Y, m_Width, m_Height;
    private Vector2 m_Position;
    private readonly Dictionary<string, int> logK_NumV = new Dictionary<string, int>();

    protected override void OnAwake()
    {
        base.OnAwake();
        Application.logMessageReceived += HandleLog;
    }


    void OnApplicationQuit()
    {
        logK_NumV.Clear();
        Application.logMessageReceived -= HandleLog;
    }

    private void SetBigBuJu()
    {
        switch (Position)
        {
            case LogPosition.FullScreen:
                SetPostion(0, 0, Screen.width, Screen.height);
                break;
            case LogPosition.HalfTop:
                SetPostion(0, 0, Screen.width, Screen.height / 2);
                break;
            case LogPosition.HalfBottom:
                SetPostion(0, Screen.height / 2, Screen.width, Screen.height / 2);
                break;
            case LogPosition.TopLeft:
                SetPostion(0, 0, Screen.width / 2, Screen.height / 2);
                break;
            case LogPosition.TopRight:
                SetPostion(Screen.width / 2, 0, Screen.width / 2, Screen.height / 2);
                break;
            case LogPosition.BottomLeft:
                SetPostion(0, Screen.height / 2, Screen.width / 2, Screen.height / 2);
                break;
            case LogPosition.BottomRight:
                SetPostion(Screen.width / 2, Screen.height / 2, Screen.width / 2, Screen.height / 2);
                break;
        }
    }

    private void SetPostion(float x, float y, float width, float height)
    {
        m_X = x;
        m_Y = y;
        m_Width = width;
        m_Height = height;
    }


    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        //不处理警告和断言的
        if (type == LogType.Warning || type == LogType.Assert)
        {
            return;
        }
        if (logK_NumV.ContainsKey(logString))
        {
            logK_NumV[logString] += 1;
        }
        else
        {
            logK_NumV.Add(logString, 0);
        }
    }


    private void CreateLogItem(string logString, int num)
    {
        MyGUI.Heng(() =>
        {
            MyGUI.Text(logString.AddSize(TextSize));
            if (num != 0)
            {
                MyGUI.AddSpace();
                MyGUI.Text(num.ToString());
            }
        });
    }




    #endregion

    private float updateInterval = 1;
    private float seconds = 0;
    private float frames = 0;

    private string text = string.Empty;

    private MyEnumColor TextColor;

    protected override void OnUpdate()
    {
        base.OnUpdate();
        seconds += Time.deltaTime;
        frames++;

        if (seconds >= updateInterval)
        {
            float fps = frames / seconds;
            text = System.String.Format("{0:F2} FPS", fps);

            if (fps < 30)
            {
                TextColor = MyEnumColor.Yellow;
            }
            else if (fps < 10)
            {
                TextColor = MyEnumColor.Red;
            }
            else
            {
                TextColor = MyEnumColor.Green;
            }
            seconds = 0;
            frames = 0;
        }

    }


    void OnGUI()
    {
        SetBigBuJu();
        MyGUI.BuJu(m_X, m_Y, m_Width, m_Height, () =>
        {
            MyGUI.Heng(() =>
            {
                MyGUI.Button(IsShowLog ? "关闭".AddSize(TextSize) : "显示".AddSize(TextSize),
                    () => { IsShowLog = !IsShowLog; });
                if (IsShowLog)
                {
                    MyGUI.Button("清除Log".AddSize(TextSize), () =>
                    {
                        logK_NumV.Clear();
                    });
                }
                MyGUI.Text(text.AddColorAndSize(TextColor,TextSize,false));

            });
            if (IsShowLog)
            {
                MyGUI.CreateScrollView(ref m_Position, () =>
                {
                    MyGUI.AddSpace(2);
                    foreach (string logString in logK_NumV.Keys)
                    {
                        CreateLogItem(logString, logK_NumV[logString]);
                    }
                });
            }
        });
    }
}


public enum LogPosition
{
    #region Log的位置

    FullScreen, //全屏
    HalfTop, //上面一部分
    HalfBottom, //下面一部分
    TopLeft, //上面左边 四分之一
    TopRight, //上面右边 四分之一
    BottomLeft, //下面左边 四分之一
    BottomRight //下面右边 四分之一

    #endregion
}