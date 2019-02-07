using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pattern
{
    public List<Voxel> Voxels { get; private set; }
    public Voxel BaseVoxel { get; private set; }
    public List<Vector3Int> BlockIndices, ConnectionIndices, ConnectionNormals, Dissabled; // list of activated indexes compared to 0,0,0
    public Vector3Int PatternNormal { get; private set; }

    public Pattern(Vector3Int patternNormal)
    {
        Voxels = new List<Voxel>();
        PatternNormal = patternNormal;

        PatternNormal = Vector3Int.up;// temporary hard coded
        CreatePattern();
    }

    void CreatePattern()
    {
        //Create basevoxel
        //Voxels.Add(new Voxel(0, 0, 0, VoxelType.Block, PatternNormal, this));

        // temporary hard coded block
        for (int y = 0; y < 6; y++)
            for (int x = -1; x <= 1; x++)
            {
                Voxels.Add(new Voxel(x, y, 0, VoxelType.Block, PatternNormal, this));
            }

        var conVox = new List<Voxel>();
        foreach (var voxel in Voxels.Where(s => s.Type == VoxelType.Block && s.Index.y != 2 && s.Index.y != 3))
        {

            conVox.Add(new Voxel(voxel.Index.x, voxel.Index.y, voxel.Index.z + 1, VoxelType.Connection, new Vector3Int(0, 0, 1), this));
            conVox.Add(new Voxel(voxel.Index.x, voxel.Index.y, voxel.Index.z - 1, VoxelType.Connection, new Vector3Int(0, 0, -1), this));
        }
        Voxels.AddRange(conVox);

        Voxels.Add(new Voxel(-1, 6, 0, VoxelType.Connection, Vector3Int.up, this));
        Voxels.Add(new Voxel(0, 6, 0, VoxelType.Connection, Vector3Int.up, this));
        Voxels.Add(new Voxel(1, 6, 0, VoxelType.Connection, Vector3Int.up, this));

        Voxels.Add(new Voxel(-1, -1, 0, VoxelType.Connection, Vector3Int.down, this));
        Voxels.Add(new Voxel(0, -1, 0, VoxelType.Connection, Vector3Int.down, this));
        Voxels.Add(new Voxel(1, -1, 0, VoxelType.Connection, Vector3Int.down, this));
        

    }
}

