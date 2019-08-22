using System;
using System.Collections.Generic;

public abstract class OnlineMapsTiledElevationManager<T> : OnlineMapsElevationManager<T>
    where T : OnlineMapsTiledElevationManager<T>
{
    public int zoomOffset = 3;

    protected Dictionary<ulong, Tile> tiles;
    protected bool needUpdateMinMax = true;
    private int prevTileX;
    private int prevTileY;

    protected abstract int tileWidth { get; }
    protected abstract int tileHeight { get; }

    public override bool hasData
    {
        get { return true; }
    }

    protected override float GetElevationValue(double x, double z, float yScale, double tlx, double tly, double brx, double bry)
    {
        if (tiles == null) tiles = new Dictionary<ulong, Tile>();
        x = x / -sizeInScene.x;
        z = z / sizeInScene.y;

        double cx = (brx - tlx) * x + tlx;
        double cz = (bry - tly) * z + tly;

        int zoom = map.zoom - zoomOffset;
        double tx, ty;
        map.projection.CoordinatesToTile(cx, cz, zoom, out tx, out ty);
        int ix = (int) tx;
        int iy = (int) ty;

        ulong key = OnlineMapsTile.GetTileKey(zoom, ix, iy);
        bool finded = false;
        Tile tile;
        finded = tiles.TryGetValue(key, out tile);
        if (finded && !tile.loaded) finded = false;

        if (!finded)
        {
            int nz = zoom;
            
            while (!finded && nz < OnlineMaps.MAXZOOM)
            {
                nz++;
                map.projection.CoordinatesToTile(cx, cz, nz, out tx, out ty);
                ix = (int)tx;
                iy = (int)ty;
                key = OnlineMapsTile.GetTileKey(zoom, ix, iy);

                finded = tiles.TryGetValue(key, out tile) && tile.loaded;
            }
        }

        if (!finded)
        {
            int nz = zoom;

            while (!finded && nz > 1)
            {
                nz--;
                map.projection.CoordinatesToTile(cx, cz, nz, out tx, out ty);
                ix = (int)tx;
                iy = (int)ty;
                key = OnlineMapsTile.GetTileKey(zoom, ix, iy);

                finded = tiles.TryGetValue(key, out tile) && tile.loaded;
            }
        }

        if (!finded) return 0;

        map.projection.CoordinatesToTile(cx, cz, tile.zoom, out tx, out ty);
        short v = tile.GetElevation(tx, ty);

        if (bottomMode == OnlineMapsElevationBottomMode.minValue) v -= minValue;
        return v * yScale * scale;
    }

    private void OnChangePosition()
    {
        double tx, ty;
        map.GetTilePosition(out tx, out ty);

        if (needUpdateMinMax || prevTileX != (int)tx || prevTileY != (int)ty)
        {
            UpdateMinMax();
            prevTileX = (int)tx;
            prevTileY = (int)ty;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (OnlineMaps.instance != null)
        {
            OnlineMaps.instance.OnChangePosition -= OnChangePosition;
            OnlineMaps.instance.OnLateUpdateBefore -= OnLateUpdateBefore;
        }

        OnlineMapsTileManager.OnPrepareDownloadTile -= OnStartDownloadTile;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (tiles == null) tiles = new Dictionary<ulong, Tile>();
    }

    private void OnLateUpdateBefore()
    {
        if (needUpdateMinMax) UpdateMinMax();
    }

    private void OnStartDownloadTile(OnlineMapsTile tile)
    {
        int zoom = map.zoom - zoomOffset;
        if (tile.zoom < zoom || zoom < 1) return;

        int s = 1 << (tile.zoom - zoom);
        int x = tile.x / s;
        int y = tile.y / s;

        ulong key = OnlineMapsTile.GetTileKey(zoom, x, y);
        if (tiles.ContainsKey(key)) return;

        Tile t = new Tile
        {
            x = x,
            y = y,
            zoom = zoom,
            width = tileWidth,
            height = tileHeight
        };
        tiles.Add(key, t);

        StartDownloadElevationTile(t);
    }

    protected void Start()
    {
        OnlineMaps.instance.OnChangePosition += OnChangePosition;
        OnlineMaps.instance.OnLateUpdateBefore += OnLateUpdateBefore;

        OnlineMapsTileManager.OnPrepareDownloadTile += OnStartDownloadTile;
    }

    protected abstract void StartDownloadElevationTile(Tile tile);

    protected override void UpdateMinMax()
    {
        double tlx, tly, brx, bry;
        map.GetTileCorners(out tlx, out tly, out brx, out bry);

        int zoom = map.zoom - zoomOffset;
        if (zoom < 1)
        {
            minValue = maxValue = 0;
            return;
        }

        int itlx = (int) tlx;
        int itly = (int) tly;
        int ibrx = (int) brx;
        int ibry = (int) bry;

        int s = 1 << zoomOffset;

        itlx /= s;
        itly /= s;
        ibrx /= s;
        ibry /= s;

        minValue = short.MaxValue;
        maxValue = short.MinValue;

        for (int x = itlx; x <= ibrx; x++)
        {
            for (int y = itly; y <= ibry; y++)
            {
                ulong key = OnlineMapsTile.GetTileKey(zoom, x, y);
                Tile tile;
                if (!tiles.TryGetValue(key, out tile)) continue;

                if (tile.minValue < minValue) minValue = tile.minValue;
                if (tile.maxValue > maxValue) maxValue = tile.maxValue;
            }
        }
    }

    protected class Tile
    {
        public bool loaded = false;
        public int x;
        public int y;
        public int zoom;

        public short minValue;
        public short maxValue;

        public int width;
        public int height;
        public short[,] elevations;

        public short GetElevation(double tx, double ty)
        {
            if (!loaded) return 0;

            double rx = tx - Math.Floor(tx);
            double ry = ty - Math.Floor(ty);
            int x = (int)Math.Round(rx * 256);
            int y = (int)Math.Round(ry * 256);
            if (x > 255) x = 255;
            if (y > 255) y = 255;
            return elevations[x, y];
        }
    }
}