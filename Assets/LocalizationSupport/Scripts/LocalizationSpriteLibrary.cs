using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpriteEntry
{
    public string name;
    public Sprite sprite;
}

public class LocalizationSpriteLibrary : MonoBehaviour
{
    public SpriteEntry[] sprites;

    private static Dictionary<string, Sprite> spritesLibrary;

    public static Sprite GetSprite(string name)
    {
        if (spritesLibrary == null)
            throw new Exception("Localization Support: Sprite library has not been initialized. Please create a sprite library gameobject from prefab, add your sprites and ensure its Awake method gets called if you wish to use images.");

        if (!SpriteExists(name))
            return null;

        return spritesLibrary[name];
    }

    public static bool SpriteExists(string name)
    {
        if (spritesLibrary == null)
            throw new Exception("Localization Support: Sprite library has not been initialized. Please create a sprite library gameobject from prefab, add your sprites and ensure its Awake method gets called if you wish to use images.");

        return spritesLibrary.ContainsKey(name);
    }

    void Awake()
    {
        if (spritesLibrary != null)
            return;

        spritesLibrary = new Dictionary<string, Sprite>();

        foreach (SpriteEntry entry in sprites)
        {
            spritesLibrary.Add(entry.name, entry.sprite);
        }
    }
}
