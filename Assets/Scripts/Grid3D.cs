using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid3D
{
    public Voxel[,,] Voxels { get; private set; }
    public Vector3Int Size;
    public List<Block> blocks = new List<Block>();           // The algorithm will try to approach this point
    IGenerationAlgorithm gen;

    public Face[][,,] Faces = new Face[3][,,];

    // Start is called before the first frame update
    public Grid3D(Vector3Int size)
    {
        gen = new GenerationAlgorithm(Controller.GoTarget.transform.position.ToVector3Int(), Controller.MinCon);
        Size = size;

        // make voxels
        Voxels = new Voxel[Size.x, Size.y, Size.z];

        for (int z = 0; z < Size.z; z++)
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                    Voxels[x, y, z] = new Voxel(x, y, z, VoxelType.Empty);

        MakeFaces();
    }

    public void MakeFaces()
    {
        // make faces
        Faces[0] = new Face[Size.x + 1, Size.y, Size.z];

        for (int z = 0; z < Size.z; z++)
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x + 1; x++)
                {
                    Faces[0][x, y, z] = new Face(x, y, z, Axis.X, this);
                }

        Faces[1] = new Face[Size.x, Size.y + 1, Size.z];

        for (int z = 0; z < Size.z; z++)
            for (int y = 0; y < Size.y + 1; y++)
                for (int x = 0; x < Size.x; x++)
                {
                    Faces[1][x, y, z] = new Face(x, y, z, Axis.Y, this);
                }

        Faces[2] = new Face[Size.x, Size.y, Size.z + 1];

        for (int z = 0; z < Size.z + 1; z++)
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                {
                    Faces[2][x, y, z] = new Face(x, y, z, Axis.Z, this);
                }
    }


    public Block GenerateNextBlock()
    {
        Block newBlock = gen.GetNextBlock(this);
        if (newBlock != null)
            AddBlockToGrid(newBlock);
        return newBlock;
    }

    public void AddBlockToGrid(Block block)
    {
        blocks.Add(block);
        block.DrawBlock(this);
        foreach (var vox in block.BlockVoxels)
        {
            if (!(vox.Index.x < 0 || vox.Index.y < 0 || vox.Index.z < 0 ||
                vox.Index.x >= Size.x || vox.Index.y >= Size.y || vox.Index.z >= Size.z)
                && GetVoxelAt(vox.Index).Type != VoxelType.Block)
            {
                AddVoxel(vox);
            }
        }


        Debug.Log( $"{GetClimableFaces().Count()} Climable faces");

    }

    public bool CanBlockExist(Block block)
    {
        foreach (var vox in block.BlockVoxels.Where(s => s.Type == VoxelType.Block))
        {
            if (vox.Index.x < 0 || vox.Index.y < 0 || vox.Index.z < 0 ||
                vox.Index.x >= Size.x || vox.Index.y >= Size.y || vox.Index.z >= Size.z)
                return false;

            if (GetVoxelAt(vox.Index).Type == VoxelType.Block)
                return false;
        }

        return true;
    }

    public IEnumerable<Face> GetClimableFaces()
    {
        for (int n = 0; n < 3; n++)
        {
            int xSize = Faces[n].GetLength(0);
            int ySize = Faces[n].GetLength(1);
            int zSize = Faces[n].GetLength(2);

            for (int z = 0; z < zSize; z++)
                for (int y = 0; y < ySize; y++)
                    for (int x = 0; x < xSize; x++)
                    {
                        if (Faces[n][x, y, z].Climable)
                        {
                            yield return Faces[n][x, y, z];
                        }
                    }
        }
    }

    


    public Voxel GetVoxelAt(Vector3Int index)
    {
        return Voxels[index.x, index.y, index.z];
    }

    private void AddVoxel(Voxel vox)
    {
        Voxels[vox.Index.x, vox.Index.y, vox.Index.z].Copy(vox);
    }


}
