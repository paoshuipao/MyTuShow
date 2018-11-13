
public enum E_GameEvent                           // 这里写事件
{

    LogoExit,                    // 离开Logo事件     logo -> 动画 -> 离开Logo -> 动画 -> RealJumpIntoScene 最终进入场景
    RealJumpIntoScene,           // 动画全部播放完成，进入了场景 （带 EF_Scenes scene 参数）

    ShowStartGameUI,             //进入 开始游戏 UI
    HideStartGameUI,             //隐藏


    ShowLog,                    // 显示 Log UI
    HideLog,                    // 隐藏



    OnClickDown_Shift,              // 按下 Shift
    OnClickUp_Shift,               //  松开 Shift

    OnClickDown_Ctrl,              // 按下 Ctrl
    OnClickUp_Ctrl,                // 松开 Ctrl

    OnClickMouseLeftDown,          // 按下鼠标右键


    ChangGameToggleType,            // 切换左边总的选项 （EGameType,int）
    DaoRuResult,                    // 返回导入结果（EGameType 导入那一个, bool 是否成功,List<FileInfo> 错误集合）




    //————————————————————————————————————


    OpenFileContrl,                // 打开 文件 资源管理器
    OpenFolderContrl,              // 打开 文件夹 资源管理器
    CloseFileOrFolderContrl,       // 关闭 文件或者文件夹资源管理器





    //————————————————————————————————————


    OnClickEscOrOnPause,       // 点击了 Esc 退出键 或者点击了 暂停的按钮


    ShowPauseUI,               // 显示 PauseUI


    HidePauseUI,               // Esc(马上游戏暂停)  -> PauseUI 动画 -> PauseUI  ->  Esc(还是暂停)   -> PauseUI 动画隐藏结束 -> 游戏才继续

    

    GameGoHead,                // 游戏继续


    OnQuitGame,                // 点击退出游戏


    //——————————————————— 导入 —————————————————


    DaoRu_XunLieTu,                    // 导入 序列图（EXunLieTu 类型 ，List<FileInfo> 文件集合 , bool 是否保存)
    ResultDaoRu_XunLieTu,              // （已加载）导入序列图(EXunLieTu 类，List<ResultBean> 结果集合)


    DaoRu_XunLieTu222,                    // 导入 序列图222（EXunLieTu 类型 ，List<FileInfo> 文件集合 , bool 是否保存)
    ResultDaoRu_XunLieTu222,              //（已加载）导入序列图222(EXunLieTu 类，List<ResultBean> 结果集合)



    DaoRu_JiHeXuLieTu,                   // 导入 集合序列图 (EJiHeXuLieTuType 类型, List<FileInfo> 多文件，bool 是否保存)
    ResultDaoRu_JiHeXuLieTu,             // （已加载）导入 集合序列图（EJiHeXuLieTuType 类型 ， List<ResultBean> 结果集合）



    DaoRu_TaoMingTu,                   // 导入 透明图 (ETaoMingType 类型, List<FileInfo> 多文件，bool 是否保存)
    ResultDaoRu_TaoMingTu,             // （已加载）导入 透明图（ETaoMingType 类型 ， List<ResultBean> 结果集合）


    DaoRu_NormalTu,                   // 导入 Jpg图 (ETaoMingType 类型, List<FileInfo> 多文件，bool 是否保存)
    ResultDaoRu_NormalTu,             // （已加载）导入 Jpg图（ENormalTuType 类型 ， List<ResultBean> 结果集合）


    DaoRu_JiHeTu,                    // 导入 集合图 (EJiHeType 类型, List<FileInfo> 多文件，bool 是否保存)
    ResultDaoRu_JiHeTu,              // （已加载）导入 集合图（EJiHeType 类型 ， List<ResultBean> 结果集合）

    ResultDaoRu_Audio,                    // 导入音频（EAudioType 类型,AudioResBean 结果 ）




    //————————————————————————————————————


    ShowMusicInfo,         // 显示音乐信息(Text 用于导入变绿色,FileInfo,bool true:需要导入)
    CloseMusicInfo,        // 关闭音乐信息




    ShowIsSure,            // 显示 确定是否的界面（EGameType 标记,string 标题）
    ClickTrue,             // 点击确定(EGameType)，
    ClickFalse,            // 点击取消(EGameType)


    ShowSingleTuInfo,      // 显示单图信息（EGameType 标记，ResultBean 文件）
    CloseSingleTuInfo,     // 关闭（EGameType 标记）
    OnClickNoSaveThis,     // 点击了 不保存这个（EGameType 标记）



    ShowDuoTuInfo,           // 显示多图信息(EGameType 标记，ResultBean[] 文件集合)
    CloseDuoTuInfo,          // 关闭（EGameType 标记）
    OnClickNoSaveThisDuoTu,  // 点击了 不保存这个多图（EGameType 标记,string[] 删除路径）



    DelteAll,               // 所有重置



    ShowChangeSizeSlider,                // 显示能改变大小的Slider

    ShowGeiMingUI,               // 显示改名UI(EGameType,string 原名)
    SureGeiMing,                 // 确定改名（EGameType，string 修改后的名称）


    ChangeAudioVolumeing,                   // 改变音量大小ing(float 大小)
    ChangeAudioVolumeEnd,                   // 结束改变音量大小(float 大小)





}
