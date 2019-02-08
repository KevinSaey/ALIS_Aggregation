using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//using System.Diagnostics;

public class Grid3d
{
    public Voxel[,,] Voxels { get; private set; }
    public Corner[,,] Corners;
    public Face[][,,] Faces = new Face[3][,,];
    public Edge[][,,] Edges = new Edge[3][,,];
    public float VoxelSize;
    public Vector3 Corner;
    public Vector3Int Size;

    public Grid3d(int sizeX, int sizeY, int sizeZ)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        Size = new Vector3Int(sizeX, sizeY, sizeZ);

        MakeVoxels();
        
        Debug.Log($"Grid took: {watch.ElapsedMilliseconds} ms to create.\r\nGrid size: {Size}, {Size.x * Size.y * Size.z} voxels.");
    }

    public void Initialize()
    {
        //MakeCorners(); maybe later
        MakeFaces();
        MakeEdges();
    }

    public Grid3d(Vector3Int gridSize) : this(gridSize.x, gridSize.y, gridSize.z) { }

    //________________________________________________________________________________MAKE

    public void MakeVoxels()
    {
        // make voxels
        Voxels = new Voxel[Size.x, Size.y, Size.z];

        for (int z = 0; z < Size.z; z++)
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                    Voxels[x, y, z] = new Voxel(x, y, z, VoxelType.Empty);
    }

    public void MakeCorners()
    {
        // make corners 
        Corners = new Corner[Size.x + 1, Size.y + 1, Size.z + 1];

        for (int z = 0; z < Size.z + 1; z++)
            for (int y = 0; y < Size.y + 1; y++)
                for (int x = 0; x < Size.x + 1; x++)
                {
                    Corners[x, y, z] = new Corner(new Vector3Int(x, y, z), this);
                }
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

    public void MakeEdges()
    {
        // make edges
        Edges[2] = new Edge[Size.x + 1, Size.y + 1, Size.z];

        for (int z = 0; z < Size.z; z++)
            for (int y = 0; y < Size.y + 1; y++)
                for (int x = 0; x < Size.x + 1; x++)
                {
                    Edges[2][x, y, z] = new Edge(x, y, z, Axis.Z, this);
                }

        Edges[0] = new Edge[Size.x, Size.y + 1, Size.z + 1];

        for (int z = 0; z < Size.z + 1; z++)
            for (int y = 0; y < Size.y + 1; y++)
                for (int x = 0; x < Size.x; x++)
                {
                    Edges[0][x, y, z] = new Edge(x, y, z, Axis.X, this);
                }

        Edges[1] = new Edge[Size.x + 1, Size.y, Size.z + 1];

        for (int z = 0; z < Size.z + 1; z++)
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x + 1; x++)
                {
                    Edges[1][x, y, z] = new Edge(x, y, z, Axis.Y, this);
                }
    }

    //________________________________________________________________________________GET
    public IEnumerable<Voxel> GetVoxels()
    {
        for (int z = 0; z < Size.z; z++)
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                {
                    yield return Voxels[x, y, z];
                }
    }

    public IEnumerable<Corner> GetCorners()
    {
        for (int z = 0; z < Size.z + 1; z++)
            for (int y = 0; y < Size.y + 1; y++)
                for (int x = 0; x < Size.x + 1; x++)
                {
                    yield return Corners[x, y, z];
                }
    }

    public IEnumerable<Face> GetFaces()
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
                        yield return Faces[n][x, y, z];
                    }
        }
    }

    public IEnumerable<Edge> GetEdges()
    {
        for (int n = 0; n < 3; n++)
        {
            int xSize = Edges[n].GetLength(0);
            int ySize = Edges[n].GetLength(1);
            int zSize = Edges[n].GetLength(2);

            for (int z = 0; z < zSize; z++)
                for (int y = 0; y < ySize; y++)
                    for (int x = 0; x < xSize; x++)
                    {
                        yield return Edges[n][x, y, z];
                    }
        }
    }


    public void GenerateNextBlock()
    {

    }



    public void AddToGrid(Block block)
    {
        foreach (var vox in block.BlockVoxels)
        {
            if (!(vox.Index.x < 0 || vox.Index.y < 0 || vox.Index.z < 0 ||
                vox.Index.x >= Size.x || vox.Index.y >= Size.y || vox.Index.z >= Size.z)
                && GetVoxelAt(vox.Index).Type == VoxelType.Empty)
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
                vox.Index.x >= Size.x || vox.Index.y >= Size.y || vox.Index.z >= Size.z)
                return false;

            if (GetVoxelAt(vox.Index).Type == VoxelType.Block)
                return false;
        }

        return true;
    }

    private Voxel GetVoxelAt(Vector3Int index)
    {
        return Voxels[index.x, index.y, index.z];
    }

    private void AddVoxel(Voxel vox)
    {
        Debug.Log(vox.Index);
        Voxels[vox.Index.x, vox.Index.y, vox.Index.z] = vox;
    }


}
