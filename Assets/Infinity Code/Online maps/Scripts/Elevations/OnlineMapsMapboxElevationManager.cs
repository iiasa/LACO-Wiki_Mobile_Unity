using System;
using UnityEngine;

[OnlineMapsPlugin("Mapbox Elevations", typeof(OnlineMapsControlBaseDynamicMesh), "Elevations")]
public class OnlineMapsMapboxElevationManager : OnlineMapsTiledElevationManager<OnlineMapsMapboxElevationManager>
{
    public string accessToken;

    protected override int tileWidth
    {
        get { return 256; }
    }

    protected override int tileHeight
    {
        get { return 256; }
    }

    private void OnTileDownloaded(Tile tile, OnlineMapsWWW www)
    {
        if (www.hasError)
        {
            Debug.Log("Download error");
            return;
        }

        Texture2D texture = new Texture2D(256, 256, TextureFormat.RGB24, false);
        texture.LoadImage(www.bytes);

        const int res = 256;

        if (texture.width != res || texture.height != res) return;

        Color[] colors = texture.GetPixels();

        tile.elevations = new short[tile.width, tile.height];

        short max = short.MinValue;
        short min = short.MaxValue;

        for (int y = 0; y < res; y++)
        {
            int py = (255 - y) * res;

            for (int x = 0; x < res; x++)
            {
                Color c = colors[py + x];

                double height = -10000 + (c.r * 255 * 256 * 256 + c.g * 255 * 256 + c.b * 255) * 0.1;
                short v = (short)Math.Round(height);
                tile.elevations[x, y] = v;
                if (v < min) min = v;
                if (v > max) max = v;
            }
        }

        tile.minValue = min;
        tile.maxValue = max;

        tile.loaded = true;
        needUpdateMinMax = true;

        map.Redraw();
    }

    protected override void StartDownloadElevationTile(Tile tile)
    {
        string url = "https://api.mapbox.com/v4/mapbox.terrain-rgb/" + tile.zoom + "/" + tile.x + "/" + tile.y + ".pngraw?access_token=" + accessToken;
        OnlineMapsWWW www = new OnlineMapsWWW(url);
        www.OnComplete += delegate { OnTileDownloaded(tile, www); };
    }
}