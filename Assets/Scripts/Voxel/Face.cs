using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Face
{
    bool _climable;
    public bool Climable
    {
        get
        {
            return (_climable || _normal == Vector3Int.up) && _normal != Vector3Int.down && ParentVox.Count(s => s != null) == 1;
        }
    }

    Vector3Int _normal;
    Axis Direction;
    public Voxel[] ParentVox;
    public Vector3Int Index;
    public Vector3 Center;
    Grid3D _grid;

    public Face(int x, int y, int z, Axis direction, Grid3D grid)
    {
        _grid = grid;
        Index = new Vector3Int(x, y, z);
        Direction = direction;
        ParentVox = GetVoxels();

        /*foreach (var v in ParentVox.Where(v => v != null))
            v.Faces.Add(this);*/

        Center = GetCenter();
    }

    Voxel[] GetVoxels()
    {
        int x = Index.x;
        int y = Index.y;
        int z = Index.z;

        switch (Direction)
        {
            case Axis.X:
                return new[]
                {
                   x == 0 ? null : _grid.Voxels[x - 1, y, z],
                   x == _grid.Size.x ? null : _grid.Voxels[x, y, z]
                };
            case Axis.Y:
                return new[]
                {
                   y == 0 ? null : _grid.Voxels[x, y - 1, z],
                   y == _grid.Size.y ? null : _grid.Voxels[x, y, z]
                };
            case Axis.Z:
                return new[]
                {
                   z == 0 ? null : _grid.Voxels[x, y, z - 1],
                   z == _grid.Size.z ? null : _grid.Voxels[x, y, z]
                };
            default:
                throw new Exception("Wrong direction, Bitch!");
        }
    }

    Vector3 GetCenter()
    {
        int x = Index.x;
        int y = Index.y;
        int z = Index.z;

        switch (Direction)
        {
            case Axis.X:
                return new Vector3(x, y + 0.5f, z + 0.5f) * Controller.VoxelSize;
            case Axis.Y:
                return new Vector3(x + 0.5f, y, z + 0.5f) * Controller.VoxelSize;
            case Axis.Z:
                return new Vector3(x + 0.5f, y + 0.5f, z) * Controller.VoxelSize;
            default:
                throw new Exception("Wrong direction, Bitch!");
        }
    }
}
