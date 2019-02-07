using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VoxelType { Empty, Connection, Block };
public class Voxel
{
    public bool IsActive { get; private set; } = false;
    public Vector3Int Index;
    public VoxelType Type;
    public Vector3Int Orientation;
    public Pattern ParentPattern { get; private set; }
    public Block ParentBlock { get; private set; }
    public string Name;
    Vector3Int[] _neighbours = new Vector3Int[6]; //X+,X-,Y+,Y-,Z+,Z-

        

    /// <summary>
    /// Instantiate a voxel. When instantiated, a voxel is turned of
    /// </summary>
    /// <param name="x">X index of the voxel</param>
    /// <param name="y">Y index of the voxel</param>
    /// <param name="z">Z index of the voxel</param>
    /// /// <param name="type">The type of the voxel (Empty, Connection, Block) </param>
    public Voxel(int x, int y, int z, VoxelType type)
    {
        Index = new Vector3Int(x, y, z);
        Type = type;
    }

    /// <summary>
    /// Instantiate a block or connection voxel
    /// </summary>
    /// <param name="x">X index of the voxel</param>
    /// <param name="y">Y index of the voxel</param>
    /// <param name="z">Z index of the voxel</param>
    /// <param name="type">The type of the voxel (Empty, Connection, Block) </param>
    /// <param name="normal">Direction normal in the direction from the Parrent Pattern to the connection voxel eg. (1,0,0)</param>
    /// <param name="parentPattern">Parrent Pattern of the connection</param>
    public Voxel(int x, int y, int z, VoxelType type, Vector3Int normal, Pattern parentPattern)
    {
        Index = new Vector3Int(x, y, z);
        Type = type;
        Orientation = normal;
        ParentPattern = parentPattern;
        Name = $"x {x}, y {y}, z {z}";
    }


    /// <summary>
    /// Turn the voxel on or off
    /// </summary>
    /// <returns>the changed state of the voxel</returns>
    public bool SwitchActive(bool isActive)
    {
        IsActive = isActive;
        if (isActive)
        {
            Type = VoxelType.Block;
        }
        else
        {
            Type = VoxelType.Empty;
        }
        return IsActive;
    }

    public Voxel ShallowClone()
    {
        return MemberwiseClone() as Voxel;
    }

    public Voxel DeepClone() {
        Voxel clone = ShallowClone();
        clone._neighbours = new Vector3Int[6];
        _neighbours.CopyTo(clone._neighbours,0);
        return clone;
    }

    

}









