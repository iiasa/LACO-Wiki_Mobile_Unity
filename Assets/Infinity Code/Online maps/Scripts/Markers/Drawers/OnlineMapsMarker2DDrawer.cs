using System;
using UnityEngine;

public abstract class OnlineMapsMarker2DDrawer : OnlineMapsMarkerDrawerBase
{
    public virtual OnlineMapsMarker GetMarkerFromScreen(Vector2 screenPosition)
    {
        Vector2 coords = OnlineMapsControlBase.instance.GetCoords(screenPosition);
        if (coords == Vector2.zero) return null;

        OnlineMapsMarker marker = null;
        double lng = double.MinValue, lat = double.MaxValue;
        double mx, my;
        int zoom = map.zoom;

        foreach (OnlineMapsMarker m in OnlineMapsMarkerManager.instance)
        {
            if (!m.enabled || !m.range.InRange(zoom)) continue;
            if (m.HitTest(coords, zoom))
            {
                m.GetPosition(out mx, out my);
                if (my < lat || (Math.Abs(my - lat) < double.Epsilon && mx > lng))
                {
                    marker = m;
                    lat = my;
                    lng = mx;
                }
            }
        }

        return marker;
    }
}