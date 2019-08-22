using UnityEngine;

public abstract class OnlineMapsTooltipDrawerBase: OnlineMapsGenericBase<OnlineMapsTooltipDrawerBase>
{
    public static string tooltip;
    public static OnlineMapsDrawingElement tooltipDrawingElement;
    public static OnlineMapsMarkerBase tooltipMarker;
    private static OnlineMapsMarkerBase rolledMarker;

    protected OnlineMaps map;
    protected OnlineMapsControlBase control;

    /// <summary>
    /// Checks if the marker in the specified screen coordinates, and shows him a tooltip.
    /// </summary>
    /// <param name="screenPosition">Screen coordinates</param>
    public void ShowMarkersTooltip(Vector2 screenPosition)
    {
        if (map.showMarkerTooltip != OnlineMapsShowMarkerTooltip.onPress)
        {
            tooltip = string.Empty;
            tooltipDrawingElement = null;
            tooltipMarker = null;
        }

        IOnlineMapsInteractiveElement el = control.GetInteractiveElement(screenPosition);
        OnlineMapsMarkerBase marker = el as OnlineMapsMarkerBase;

        if (map.showMarkerTooltip == OnlineMapsShowMarkerTooltip.onHover)
        {
            if (marker != null)
            {
                tooltip = marker.label;
                tooltipMarker = marker;
            }
            else
            {
                OnlineMapsDrawingElement drawingElement = map.GetDrawingElement(screenPosition);
                if (drawingElement != null)
                {
                    tooltip = drawingElement.tooltip;
                    tooltipDrawingElement = drawingElement;
                }
            }
        }

        if (rolledMarker != marker)
        {
            if (rolledMarker != null && rolledMarker.OnRollOut != null) rolledMarker.OnRollOut(rolledMarker);
            rolledMarker = marker;
            if (rolledMarker != null && rolledMarker.OnRollOver != null) rolledMarker.OnRollOver(rolledMarker);
        }
    }
}