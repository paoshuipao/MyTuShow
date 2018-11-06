/*
using UnityEngine;

public class CameraManager : Manager
{

    public void SetOnePersonMode()                               // 设置成第 1 人称模式
    {

    }

    public void SetTheThirdPersonMode()                          // 设置成第 3 人称模式
    {

    }



    private Camera mCamera;
    private Transform mCameraTransform;
    private const string MAIN_CAMERA = "MainCamera";

    protected override void OnStart()
    {
        base.OnStart();
        GameObject cameraGo =new GameObject(MAIN_CAMERA);
        mCameraTransform = cameraGo.transform;
        mCameraTransform.tag = MAIN_CAMERA;
        mCameraTransform.SetParent(transform);
        mCameraTransform.localPosition = Vector3.zero;
        mCameraTransform.localScale = Vector3.one;
        mCamera = cameraGo.AddComponent<Camera>();

        cameraGo.AddComponent<FlareLayer>();
        mCamera.farClipPlane = 200f;


        cameraGo.AddComponent<AudioListener>();




    }
}
*/
