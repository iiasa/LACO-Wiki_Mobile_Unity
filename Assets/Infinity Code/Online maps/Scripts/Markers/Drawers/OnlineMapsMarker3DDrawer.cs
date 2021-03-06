using UnityEngine;

public class OnlineMapsMarker3DDrawer : OnlineMapsMarkerDrawerBase
{
    private OnlineMapsControlBase3D control;

    public OnlineMapsMarker3DDrawer(OnlineMapsControlBase3D control)
    {
        this.control = control;
        control.OnUpdate3DMarkers += Update3DMarkers;
    }

    public override void Dispose()
    {
        control.OnUpdate3DMarkers -= Update3DMarkers;
        control = null;
    }

    private void Update3DMarkers()
    {
        if (OnlineMapsMarker3DManager.instance == null) return;
        if (control.cl == null) return;

        int zoom = map.zoom;

        double tlx, tly, brx, bry;
        map.GetCorners(out tlx, out tly, out brx, out bry);

        double ttlx, ttly, tbrx, tbry;
        map.projection.CoordinatesToTile(tlx, tly, zoom, out ttlx, out ttly);
        map.projection.CoordinatesToTile(brx, bry, zoom, out tbrx, out tbry);

    //    Debug.Log("Update3DMarkers");

        Bounds bounds = control.cl.bounds;
       // float bestYScale = OnlineMapsElevationManagerBase.GetBestElevationYScale(tlx, tly, brx, bry);

        foreach (OnlineMapsMarker3D marker in OnlineMapsMarker3DManager.instance)
        {
            marker.Update(map, control, bounds, tlx, tly, brx, bry, zoom, ttlx, ttly, tbrx, tbry/*, bestYScale*/);
        }
    }
}