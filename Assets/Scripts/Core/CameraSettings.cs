using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;
using Cinemachine;

public class CameraSettings : MonoBehaviour, ISaveable
{
    CinemachineFreeLook cameraFreeLook;

    private void Start() 
    {
        cameraFreeLook = GetComponent<CinemachineFreeLook>();
    }

    [System.Serializable]
        struct CameraSaveData
        {
            public SerializableVector3 camPosition;
            public float zoomValue;
        }
    
    public object CaptureState()
        {
            CameraSaveData camData = new CameraSaveData();
            camData.camPosition = new SerializableVector3(transform.position);
            camData.zoomValue = cameraFreeLook.m_YAxis.Value;
            return camData;
        }

        public void RestoreState(object state)
        {
            CameraSaveData camData = (CameraSaveData) state;
            transform.position = camData.camPosition.ToVector();
            cameraFreeLook = GetComponent<CinemachineFreeLook>();
            cameraFreeLook.m_YAxis.Value = camData.zoomValue;
        }
}
