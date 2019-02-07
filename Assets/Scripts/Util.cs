using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

    public static class Util
    {
        public static Vector3Int ToInt(this Vector3 v)
        {
            int x = Mathf.RoundToInt(v.x);
            int y = Mathf.RoundToInt(v.y);
            int z = Mathf.RoundToInt(v.z);
            return new Vector3Int(x, y, z);
        }
    }