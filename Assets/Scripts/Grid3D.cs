﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Global grid
/// </summary>
public class Grid3D
{
    public Voxel[,,] Voxels;
    public Vector3Int Size;
    public List<Block> Blocks = new List<Block>();           // The algorithm will try to approach this point
    IGenerationAlgorithm gen;

    public Face[][,,] Faces = new Face[3][,,];
    public Edge[][,,] Edges = new Edge[3][,,];

    public PathFinding PathFinding;
    
    /// <summary>
    /// Initialise a voxel grid
    /// </summary>
    /// <param name="size">The number of voxels in x,y,z</param>
    public Grid3D(Vector3Int size)
    {
        gen = new GenerationAlgorithm(Controller.GoTarget.transform.position.ToVector3Int());
        Size = size;

        // make voxels
        Voxels = new Voxel[Size.x, Size.y, Size.z];

        for (int z = 0; z < Size.z; z++)
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                    Voxels[x, y, z] = new Voxel(x, y, z, VoxelType.Empty);

        MakeFaces();
        MakeEdges();
    }

    /// <summary>
    /// initialise the pathfinding
    /// </summary>
    public void IniPathFinding()
    {
        PathFinding = new PathFinding(this);
    }

    /// <summary>
    /// Generate the faces of the voxelgrid
    /// </summary>
    public void MakeFaces() // stolen from Vicente
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

    /// <summary>
    /// Generate the edges of the voxel grid
    /// </summary>
    public void MakeEdges() //stolen from Vicente
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

    /// <summary>
    /// Generate the next possible block
    /// </summary>
    /// <returns>the next possible block (outside global grid)</returns>
    public Block GenerateNextBlock()
    {
        Block newBlock = gen.GetNextBlock(this);
        if (newBlock != null)
            AddBlockToGrid(newBlock);
        else
            Debug.Log("No next block found");
        return newBlock;
    }

    /// <summary>
    /// Add the generated Block to the grid
    /// </summary>
    /// <param name="block">the Block to add</param>
    public void AddBlockToGrid(Block block)
    {
        Blocks.Add(block);
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

        Debug.Log($"{GetClimableFaces().Count()} Climable faces");
    }

    /// <summary>
    /// Check if a given block can exist within the voxel grid
    /// </summary>
    /// <param name="block">The Block to check</param>
    /// <returns>if the block can exist or not</returns>
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

    /// <summary>
    /// Get all the climable faces within the grid
    /// </summary>
    /// <returns>List of climable faces</returns>
    public IEnumerable<Face> GetClimableFaces() //Stolen from Vicente
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

    /// <summary>
    /// Get a flattened list of all the edges
    /// </summary>
    /// <returns>flattened list of all the edges</returns>
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

    /// <summary>
    /// Get a Voxel at a given index within the grid
    /// </summary>
    /// <param name="index">Index of the Voxel</param>
    /// <returns>The voxel at the index</returns>
    public Voxel GetVoxelAt(Vector3Int index)
    {
        return Voxels[index.x, index.y, index.z];
    }
    
    /// <summary>
    /// Add a voxel to the grid
    /// </summary>
    /// <param name="vox">Voxel existing outside the grid</param>
    public void AddVoxel(Voxel vox)
    {
        Voxels[vox.Index.x, vox.Index.y, vox.Index.z].Copy(vox);
    }

    /// <summarty>
    /// Switch the visibility of the blocks within the grid
    /// </summary>
    /// <param name="vis">True = visible, false = hideden</param>
    public void SwitchBlockVisibility(bool vis)
    {
        Blocks.ForEach(b => b.goBlockParent.SetActive(vis));
    }
}
