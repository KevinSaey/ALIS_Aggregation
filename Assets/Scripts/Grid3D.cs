using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid3D
{
    public Voxel[,,] Voxels { get; private set; }
    Vector3Int _size;
    public List<Block> blocks = new List<Block>();           // The algorithm will try to approach this point
    IGenerationAlgorithm gen = new GenerationAlgorithm(Controller.GoTarget.transform.position.ToVector3Int(), 3);

    // Start is called before the first frame update
    public Grid3D(int sizeX, int sizeY, int sizeZ)
    {
        _size = new Vector3Int(sizeX, sizeY, sizeZ);

        // make voxels
        Voxels = new Voxel[_size.x, _size.y, _size.z];

        for (int z = 0; z < _size.z; z++)
            for (int y = 0; y < _size.y; y++)
                for (int x = 0; x < _size.x; x++)
                    Voxels[x, y, z] = new Voxel(x, y, z, VoxelType.Empty);

    }

    public Block GenerateNextBlock()
    {
        Block newBlock = gen.GetNextBlock(this);
        if (newBlock != null)
            AddToGrid(newBlock);
        return newBlock;
    }

    public void AddToGrid(Block block)
    {
        blocks.Add(block);
        block.DrawBlock(this);
        foreach (var vox in block.BlockVoxels)
        {
            if (!(vox.Index.x < 0 || vox.Index.y < 0 || vox.Index.z < 0 ||
                vox.Index.x >= _size.x || vox.Index.y >= _size.y || vox.Index.z >= _size.z)
                && GetVoxelAt(vox.Index).Type != VoxelType.Block)
            {
                AddVoxel(vox);
            }
        }
    }

    public bool CanBlockExist(Block block)
    {
        foreach (var vox in block.BlockVoxels.Where(s => s.Type == VoxelType.Block))
        {
            if (vox.Index.x < 0 || vox.Index.y < 0 || vox.Index.z < 0 ||
                vox.Index.x >= _size.x || vox.Index.y >= _size.y || vox.Index.z >= _size.z)
                return false;

            if (GetVoxelAt(vox.Index).Type == VoxelType.Block)
                return false;
        }

        return true;
    }

    public Voxel GetVoxelAt(Vector3Int index)
    {
        return Voxels[index.x, index.y, index.z];
    }

    private void AddVoxel(Voxel vox)
    {
        Debug.Log(vox.Index);
        Voxels[vox.Index.x, vox.Index.y, vox.Index.z] = vox;
    }


}
