using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Face
{
    public enum BoundaryType { Inside = 0, Left = -1, Right = 1, Outside = 2 }

    public bool Climable
    {
        get
        {
            return Normal != Vector3Int.down && HasOneBlockParent() && (WalkablePattern() || Normal == Vector3Int.up);
        }
    }

    public BoundaryType Boundary
    {
        get
        {
            bool left = ParentVox[0]?.Type == VoxelType.Block;
            bool right = ParentVox[1]?.Type == VoxelType.Block;

            if (!left && right) return BoundaryType.Left;
            if (left && !right) return BoundaryType.Right;
            if (left && right) return BoundaryType.Inside;
            return BoundaryType.Outside;
        }
    }

    public Vector3Int Normal
    {
        get
        {
            int f = (int)Boundary;
            if (Boundary == BoundaryType.Outside) f = 0;

            if (Index.y == 0 && Direction == Axis.Y)
            {
                f = Boundary == BoundaryType.Outside ? 1 : 0;
            }

            switch (Direction)
            {
                case Axis.X:
                    return Vector3Int.right * f;
                case Axis.Y:
                    return Vector3Int.up * f;
                case Axis.Z:
                    return new Vector3Int(0, 0, 1) * f;
                default:
                    throw new Exception("Wrong direction.");
            }
        }
    }

    public Axis Direction;
    public Voxel[] ParentVox;
    public Vector3Int Index;
    public Vector3 Center;
    Grid3D _grid;
    public int DistanceFromZero = 0;

    public Face(int x, int y, int z, Axis direction, Grid3D grid)
    {
        _grid = grid;
        Index = new Vector3Int(x, y, z);
        Direction = direction;
        ParentVox = GetVoxels();

        foreach (var v in ParentVox.Where(v => v != null))
            v.Faces.Add(this);

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

    public bool WalkablePattern()
    {
        return ParentVox.First(f => f?.Type == VoxelType.Block).WalkableFaces.Count(s => s == Normal)==1;
    }

    public bool HasOneBlockParent()
    {
        return ParentVox.Count(s => s?.Type == VoxelType.Block) == 1;
    }
}
