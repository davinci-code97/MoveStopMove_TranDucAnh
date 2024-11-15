using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{

    [SerializeField] private List<CinemachineVirtualCamera> cameraList = new List<CinemachineVirtualCamera>();

    [SerializeField] private CinemachineVirtualCamera ActiveCamera;

    public bool IsActiveCamera(CinemachineVirtualCamera cam) => cam == ActiveCamera;

    public void SwitchCamera (CinemachineVirtualCamera newCamera) {
        newCamera.Priority = 10;
        ActiveCamera = newCamera;

        foreach (CinemachineVirtualCamera camera in cameraList)
        {
            if (camera != newCamera) {
                camera.Priority = 0;
            }
        }
    }
    
    public void SwitchCamera (int newCameraIndex) {
        CinemachineVirtualCamera newCamera = cameraList[newCameraIndex];
        newCamera.Priority = 10;
        ActiveCamera = newCamera;

        foreach (CinemachineVirtualCamera camera in cameraList)
        {
            if (camera != newCamera) {
                camera.Priority = 0;
            }
        }
    }



}