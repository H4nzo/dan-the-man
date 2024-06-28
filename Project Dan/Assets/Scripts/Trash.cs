using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Trash
{
    public static List<GameObject> trash = new List<GameObject>();

    public static void Dump(GameObject obj)
    {
        trash.Add(obj);

        if(trash.Count > 50)
        {
            foreach (var item in trash)
            {
                GameObject.Destroy(item);
            }
            trash.Clear();
        }
    }
}
