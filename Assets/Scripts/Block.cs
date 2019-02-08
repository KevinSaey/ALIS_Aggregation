using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Rotates and translates a pattern
/// </summary>
public class Block

{
    public Pattern Pattern;
    public Vector3Int ZeroIndex;
    public List<Voxel> BlockVoxels;
    //public Vector3Int X, Y, Z, MinX, MinY, MinZ;

    Vector3Int _rotation;

    public Block(Pattern pattern, Vector3Int zeroIndex, Vector3Int newRotation)
    {
        Pattern = pattern;
        ZeroIndex = zeroIndex;

        _rotation = newRotation;

        BlockVoxels = GetVoxels().ToList();
        BlockVoxels.ForEach(f => f.Index += zeroIndex);//translation
        RotateConnections();
    }

    public Block(Pattern pattern, Voxel connpoint): this(pattern, connpoint.Index, connpoint.Orientation) {

    }

    IEnumerable<Voxel> GetVoxels()
    {
        foreach (var voxel in Pattern.Voxels)
        {
            var copyVox = voxel.ShallowClone();
            copyVox.Index = RotateVector(copyVox.Index);

            yield return copyVox;
        }
    }

    Vector3Int RotateVector(Vector3Int vec)
    {
        // x rotation
        Vector3Int[] rotation_x = new Vector3Int[]
        {
            vec,
            new Vector3Int(vec.x, vec.z, -vec.y),
            new Vector3Int(vec.x, -vec.y, -vec.z),
            new Vector3Int(vec.x, -vec.z, vec.y)
        };

        vec = rotation_x[_rotation.x / 90 % 4];

        // y rotation
        Vector3Int[] rotation_y = new Vector3Int[]
        {
            vec,
            new Vector3Int(-vec.z, vec.y, vec.x),
            new Vector3Int(-vec.x, vec.y, -vec.z),
            new Vector3Int(vec.z, vec.y, -vec.x)
        };

        vec = rotation_y[_rotation.y / 90 % 4];

        // z rotation
        Vector3Int[] rotation_z = new Vector3Int[]
        {
            vec,
            new Vector3Int(-vec.y, vec.x, vec.z),
            new Vector3Int(-vec.x, -vec.y, vec.z),
            new Vector3Int(vec.y, -vec.x, vec.z)
        };

        vec = rotation_z[_rotation.z / 90 % 4];

        return vec;
    }

    void RotateConnections()
    {
        BlockVoxels.ForEach(f => f.Orientation += _rotation);
    }

    void SetConnectionVectors()
    {
        foreach (Voxel vox in BlockVoxels.Where(s => s.Type == VoxelType.Connection))
        {
            Voxel neighbour = null;
            int cnt = 0;
            while ((neighbour == null || neighbour.Type != VoxelType.Block) && cnt < 6)
            {
                neighbour = GetVoxelAt(Util.GetNeighbourIndex(vox.Index)[cnt]);
                cnt++;
            }

            if (neighbour == null)
            {
                Debug.Log("Something went wrong, No neighbourgh found!");
                return;
            }
            else if (neighbour.Type != VoxelType.Block)
            {
                Debug.Log("Something went wrong, No neighbourh found with type Block!");
                return;
            }

            vox.Orientation = vox.Index - neighbour.Index;
            //Debug.Log(vox.Orientation);
        }
    }

    Voxel GetVoxelAt(Vector3Int index)
    {
        return BlockVoxels.FirstOrDefault(s => s.Index == index);
    }


    //Vector3.SignedAngle(a, b, Vector3.up);




}
