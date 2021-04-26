using GameDevTV.Saving;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace RPG.Core
{
    public class CameraSettings : MonoBehaviour, ISaveable
    {
        CinemachineFreeLook freeLookCam;
        [SerializeField] float zoomSpeed = 1f;
        [SerializeField] float zoomAccelration = 2.5f;
        [SerializeField] float zoomInnerRange = 3;
        [SerializeField] float zoomOuterRange = 50f;
        [SerializeField] float zoomYAxis = 0f;

        float currentMiddleRigRadius = 10f;
        float newMiddleRigRadius = 10f;
        PlayerControls controls;

        private void Awake()
        {
            freeLookCam = GetComponent<CinemachineFreeLook>();
            controls = new PlayerControls();
        }

        private void OnEnable()
        {
            controls.Camera.Enable();
        }

        private void OnDisable()
        {
            controls.Camera.Disable();
        }

        private void AdjustCameraZoomIndex(float zoomYAxis)
        {
            if (zoomYAxis == 0) return;
            if (zoomYAxis < 0)
            {
                newMiddleRigRadius = currentMiddleRigRadius + zoomSpeed;
            }
            if (zoomYAxis > 0)
            {
                newMiddleRigRadius = currentMiddleRigRadius - zoomSpeed;
            }
        }

        private void LateUpdate()
        {
            if (Mouse.current.rightButton.isPressed)
            {
                UpdateZoomLevel();
                UpdateLookView();
            }
        }

        private void UpdateLookView()
        {

            Vector2 mousePosY = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
            Vector2 mousePosX = Mouse.current.position.ReadValue();
            mousePosY.y = Mathf.Clamp(mousePosY.y, 0.6f, 1);
            freeLookCam.m_XAxis.Value = mousePosX.x;
            freeLookCam.m_YAxis.Value = mousePosY.y;
        }

        private void UpdateZoomLevel()
        {
            AdjustCameraZoomIndex(controls.Camera.MouseZoom.ReadValue<float>());
            if (currentMiddleRigRadius == newMiddleRigRadius) return;

            currentMiddleRigRadius = Mathf.Lerp(currentMiddleRigRadius, newMiddleRigRadius, zoomAccelration * Time.deltaTime);
            currentMiddleRigRadius = Mathf.Clamp(currentMiddleRigRadius, zoomInnerRange, zoomOuterRange);

            freeLookCam.m_Orbits[1].m_Radius = currentMiddleRigRadius;
            freeLookCam.m_Orbits[0].m_Height = freeLookCam.m_Orbits[1].m_Radius;
            freeLookCam.m_Orbits[2].m_Height = -freeLookCam.m_Orbits[1].m_Radius;
        }

        [System.Serializable]
        struct CameraSaveData
        {
            public float camX;
            public float camY;
            public float middleRig;
            public float zoomValue;

        }

        public object CaptureState()
        {
            CameraSaveData camData = new CameraSaveData();
            camData.camX = freeLookCam.m_XAxis.Value;
            camData.camY = freeLookCam.m_YAxis.Value;
            camData.middleRig = currentMiddleRigRadius;
            return camData;
        }

        public void RestoreState(object state)
        {
            CameraSaveData camData = (CameraSaveData)state;
            freeLookCam = GetComponent<CinemachineFreeLook>();
            freeLookCam.m_Orbits[1].m_Radius = camData.middleRig;
            freeLookCam.m_Orbits[0].m_Height = freeLookCam.m_Orbits[1].m_Radius;
            freeLookCam.m_Orbits[2].m_Height = -freeLookCam.m_Orbits[1].m_Radius;
            freeLookCam.m_YAxis.Value = camData.camY;
            freeLookCam.m_XAxis.Value = camData.camX;
        }
    }
}
