using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum Axis { X, Y, Z };

public static class Util
{
    public static Vector3Int[] NeighbourIndex = new Vector3Int[6] { new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0), new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0), new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1) };
    public static Vector3Int ToVector3Int(this Vector3 v)
    {
        int x = Mathf.RoundToInt(v.x);
        int y = Mathf.RoundToInt(v.y);
        int z = Mathf.RoundToInt(v.z);
        return new Vector3Int(x, y, z);
    }


    /// <summary>
    /// Get The neighbour indices from a given index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static Vector3Int[] GetNeighbourIndex(Vector3Int index)
    {
        return NeighbourIndex.Select(s => s + index).ToArray();
    }
}