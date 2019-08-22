using UnityEngine;

public abstract class OnlineMapsVectorTile : OnlineMapsTile
{
    public OnlineMapsVectorTile(int x, int y, int zoom, OnlineMaps map, bool isMapTile = true) : base(x, y, zoom, map, isMapTile)
    {
    }
}