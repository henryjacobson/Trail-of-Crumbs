using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheOnCheckpoint : MonoBehaviour
{
    private static List<CacheOnCheckpoint> cache = new List<CacheOnCheckpoint>();

    public void OnCache(float destroyDelay)
    {
        cache.Add(this);

        this.gameObject.SetActive(false);

        this.gameObject.SendMessage("OnDestroy", destroyDelay);
    }

    public void Revert()
    {
        this.gameObject.SetActive(true);
    }

    public static void ClearCache()
    {
        cache = new List<CacheOnCheckpoint>();
    }

    public static void ResetCache()
    {
        foreach(CacheOnCheckpoint coc in cache)
        {
            coc.Revert();
        }

        ClearCache();
    }
}
