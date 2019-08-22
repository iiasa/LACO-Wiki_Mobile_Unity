#if (!UNITY_ANDROID && !UNITY_IPHONE) || UNITY_EDITOR
#define USE_MOUSE_ROTATION
#endif

using System;
using UnityEngine;

[AddComponentMenu("Infinity Code/Online Maps/Plugins/Camera Orbit")]
[OnlineMapsPlugin("Camera Orbit", typeof(OnlineMapsControlBase3D))]
public class OnlineMapsCameraOrbit : MonoBehaviour, IOnlineMapsSavableComponent
{
    public Action OnCameraControl;
    public OnlineMapsCameraAdjust adjustTo = OnlineMapsCameraAdjust.maxElevationInArea;
    public float distance = 1000;
    public float maxRotationX = 80;
    public Vector2 rotation = Vector2.zero;
    public Vector2 speed = Vector2.one;

    private static OnlineMapsCameraOrbit _instance;
    private bool isCameraControl;

    private Vector2 lastInputPosition;
    private OnlineMapsSavableItem[] savableItems;

    public static OnlineMapsCameraOrbit instance
    {
        get { return _instance; }
    }

    private Camera activeCamera
    {
        get { return control.activeCamera; }
    }

    private OnlineMapsControlBaseDynamicMesh control
    {
        get { return OnlineMapsControlBaseDynamicMesh.instance; }
    }

    private OnlineMaps map
    {
        get { return OnlineMaps.instance; }
    }

    private Vector2 sizeInScene
    {
        get { return control.sizeInScene; }
    }

    public OnlineMapsSavableItem[] GetSavableItems()
    {
        if (savableItems != null) return savableItems;

        savableItems = new[]
        {
            new OnlineMapsSavableItem("cameraOrbit", "Camera Orbit", SaveSettings)
            {
                loadCallback = LoadSettings
            }
        };

        return savableItems;
    }

    private void LoadSettings(OnlineMapsJSONObject obj)
    {
        obj.DeserializeObject(this);
    }

    private OnlineMapsJSONItem SaveSettings()
    {
        return OnlineMapsJSON.Serialize(this);
    }

    private void OnEnable()
    {
        _instance = this;
    }

    private void Update()
    {
#if USE_MOUSE_ROTATION
        if (Input.GetMouseButton(1))
        { 
            Vector2 inputPosition = control.GetInputPosition();
#else
        if (Input.touchCount == 2)
        {
            Vector2 p1 = Input.GetTouch(0).position;
            Vector2 p2 = Input.GetTouch(1).position;

            Vector2 inputPosition = Vector2.Lerp(p1, p2, 0.5f);
#endif
            if (control.IsCursorOnUIElement(inputPosition)) return;

            isCameraControl = true;
            if (lastInputPosition == Vector2.zero) lastInputPosition = inputPosition;
            if (lastInputPosition != inputPosition && lastInputPosition != Vector2.zero)
            {
                Vector2 offset = lastInputPosition - inputPosition;
                rotation.x -= offset.y / 10f * speed.x;
                rotation.y -= offset.x / 10f * speed.y;
            }
            lastInputPosition = inputPosition;
        }
        else if (isCameraControl)
        {
            lastInputPosition = Vector2.zero;
            isCameraControl = false;
        }

        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        if (rotation.x > maxRotationX) rotation.x = maxRotationX;
        else if (rotation.x < 0) rotation.x = 0;

        float rx = 90 - rotation.x;
        if (rx > 89.9) rx = 89.9f;

        double px = Math.Cos(rx * Mathf.Deg2Rad) * distance;
        double py = Math.Sin(rx * Mathf.Deg2Rad) * distance;
        double pz = Math.Cos(rotation.y * Mathf.Deg2Rad) * px;
        px = Math.Sin(rotation.y * Mathf.Deg2Rad) * px;

        Vector3 targetPosition = transform.position;
        Vector3 offset = new Vector3(sizeInScene.x / -2, 0, sizeInScene.y / 2);

        if (OnlineMapsElevationManagerBase.useElevation)
        {
            double tlx, tly, brx, bry;
            map.GetCorners(out tlx, out tly, out brx, out bry);
            float yScale = OnlineMapsElevationManagerBase.GetBestElevationYScale(tlx, tly, brx, bry);
            if (adjustTo == OnlineMapsCameraAdjust.maxElevationInArea) offset.y = OnlineMapsElevationManagerBase.instance.GetMaxElevation(yScale);
            else offset.y = OnlineMapsElevationManagerBase.GetElevation(targetPosition.x, targetPosition.z, yScale, tlx, tly, brx, bry);
        }

        offset.Scale(transform.lossyScale);
        targetPosition += transform.rotation * offset;

        Vector3 oldPosition = activeCamera.transform.position;
        Vector3 newPosition = transform.rotation * new Vector3((float)px, (float)py, (float)pz) + targetPosition;

        activeCamera.transform.position = newPosition;
        activeCamera.transform.LookAt(targetPosition);

        if (oldPosition != newPosition && OnCameraControl != null) OnCameraControl();
    }

    
}