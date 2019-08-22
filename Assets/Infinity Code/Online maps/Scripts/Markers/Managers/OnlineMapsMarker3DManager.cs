using System;
using UnityEngine;

[Serializable]
[DisallowMultipleComponent]
public class OnlineMapsMarker3DManager : OnlineMapsMarkerManagerBase<OnlineMapsMarker3DManager, OnlineMapsMarker3D>
{
    /// <summary>
    /// Specifies whether to create a 3D marker by pressing N under the cursor.
    /// </summary>
    public bool allowAddMarker3DByN = true;

    /// <summary>
    /// Default 3D marker.
    /// </summary>
    public GameObject defaultPrefab;

    /// <summary>
    /// Scaling of 3D markers by default.
    /// </summary>
    public float defaultScale = 1;

    public OnlineMapsMarker3D Create(double longitude, double latitude, GameObject prefab)
    {
        OnlineMapsMarker3D marker = _CreateItem(longitude, latitude);
        marker.prefab = prefab;
        OnlineMapsControlBase3D control = marker.control = OnlineMapsControlBase3D.instance;
        marker.scale = defaultScale;
        marker.Init(control.transform);
        Redraw();
        return marker;
    }
    public OnlineMapsMarker3D Create_2(double longitude, double latitude, GameObject prefab, GameObject prefab2, float value, int type)
    {
        OnlineMapsMarker3D marker = _CreateItem(longitude, latitude);
        marker.prefab = prefab;
        OnlineMapsControlBase3D control = marker.control = OnlineMapsControlBase3D.instance;
        marker.scale = defaultScale;
       // marker.Init(control.transform);
        marker.Init_2(control.transform, prefab2, value, type);
        Redraw();
        return marker;
    }
    public OnlineMapsMarker3D Create_3(double longitude, double latitude, GameObject prefab, GameObject prefab2, int nrpins)
    {
        OnlineMapsMarker3D marker = _CreateItem(longitude, latitude);
        marker.prefab = prefab;
        OnlineMapsControlBase3D control = marker.control = OnlineMapsControlBase3D.instance;
        marker.scale = defaultScale;
       // marker.Init(control.transform);
        marker.Init_3(control.transform, prefab2, nrpins);
        Redraw();
        return marker;
    }


    public OnlineMapsMarker3D CreateFromExistGameObject(double longitude, double latitude, GameObject markerGameObject)
    {
        OnlineMapsMarker3D marker = _CreateItem(longitude, latitude);
        marker.prefab = marker.instance = markerGameObject;
        marker.control = OnlineMapsControlBase3D.instance;
        marker.scale = defaultScale;
        markerGameObject.AddComponent<OnlineMapsMarker3DInstance>().marker = marker;
        marker.inited = true;

        Update();

        if (marker.OnInitComplete != null) marker.OnInitComplete(marker);
        Redraw();
        return marker;
    }

    public static OnlineMapsMarker3D CreateItem(Vector2 location, GameObject prefab)
    {
        return instance.Create(location.x, location.y, prefab);
    }

    public static OnlineMapsMarker3D CreateItem(double lng, double lat, GameObject prefab)
    {
        return instance.Create(lng, lat, prefab);
    }

    public static OnlineMapsMarker3D CreateItem_2(double lng, double lat, GameObject prefab, GameObject prefab2, float value, int type)
    {
        return instance.Create_2(lng, lat, prefab, prefab2, value, type);
    }

    public static OnlineMapsMarker3D CreateItem_3(double lng, double lat, GameObject prefab, GameObject prefab2, int nrpins)
    {
        return instance.Create_3(lng, lat, prefab, prefab2, nrpins);
    }

    public static OnlineMapsMarker3D CreateItemFromExistGameObject(double longitude, double latitude, GameObject markerGameObject)
    {
        return instance.CreateFromExistGameObject(longitude, latitude, markerGameObject);
    }

    public override OnlineMapsSavableItem[] GetSavableItems()
    {
        if (savableItems != null) return savableItems;

        savableItems = new[]
        {
            new OnlineMapsSavableItem("markers3D", "3D Markers", SaveSettings)
            {
                priority = 90,
                loadCallback = LoadSettings
            }
        };

        return savableItems;
    }

    public void LoadSettings(OnlineMapsJSONItem json)
    {
        OnlineMapsJSONItem jitems = json["items"];
        RemoveAll();
        foreach (OnlineMapsJSONItem jitem in jitems)
        {
            OnlineMapsMarker3D marker = new OnlineMapsMarker3D();

            double mx = jitem.ChildValue<double>("longitude");
            double my = jitem.ChildValue<double>("latitude");

            marker.SetPosition(mx, my);

            marker.range = jitem.ChildValue<OnlineMapsRange>("range");
            marker.label = jitem.ChildValue<string>("label");
            marker.prefab = OnlineMapsUtils.GetObject(jitem.ChildValue<int>("prefab")) as GameObject;
            marker.rotationY = jitem.ChildValue<float>("rotationY");
            marker.scale = jitem.ChildValue<float>("scale");
            marker.enabled = jitem.ChildValue<bool>("enabled");
            Add(marker);
        }

        (json["settings"] as OnlineMapsJSONObject).DeserializeObject(this);
    }

    protected override OnlineMapsJSONItem SaveSettings()
    {
        OnlineMapsJSONItem jitem = base.SaveSettings();
        jitem["settings"].AppendObject(new
        {
            allowAddMarker3DByN,
            defaultPrefab = defaultPrefab != null? defaultPrefab.GetInstanceID(): -1,
            defaultScale
        });
        return jitem;
    }

    protected override void Update()
    {
        base.Update();

      /*  if (allowAddMarker3DByN && Input.GetKeyUp(KeyCode.N))
        {
            OnlineMapsMarker3D marker3D = CreateItem(OnlineMapsControlBase.instance.GetCoords(), defaultPrefab);
            marker3D.scale = defaultScale;
        }*/
    }
}
 