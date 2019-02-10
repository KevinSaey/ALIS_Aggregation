using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Rotates and translates a pattern
/// </summary>
public class Block

{
    Grid3D _grid;
    public Pattern Pattern;
    public Vector3Int ZeroIndex;
    public List<Voxel> BlockVoxels;
    public GameObject goBlockParent;

    //public Vector3Int X, Y, Z, MinX, MinY, MinZ;

    Vector3Int _rotation;

    public Block(Pattern pattern, Vector3Int zeroIndex, Vector3Int newRotation, Grid3D grid)
    {

        Pattern = pattern;
        ZeroIndex = zeroIndex;
        _grid = grid;

        _rotation = newRotation;

        BlockVoxels = GetVoxels().ToList();
        BlockVoxels.ForEach(f => f.Index += zeroIndex);
        BlockVoxels.ForEach(f => f.ParentBlock = this);//translation
        RotateConnections();
    }

    public Block(Pattern pattern, Voxel connPoint, Grid3D grid) : this(pattern, connPoint.Index, connPoint.Orientation, grid)
    {

    }

    IEnumerable<Voxel> GetVoxels()
    {
        foreach (var voxel in Pattern.Voxels)
        {
            var copyVox = voxel.ShallowClone();
            copyVox.Index = RotateVector(copyVox.Index);
            copyVox.WalkableFaces?.ForEach(s => RotateVector(s));
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


    public void InstantiateGoBlock()
    {
        goBlockParent = new GameObject($"Block {ZeroIndex}");
        goBlockParent.transform.position = ZeroIndex;
    }

    public void DrawBlock(Grid3D grid)
    {
        InstantiateGoBlock();
        foreach (var vox in BlockVoxels)
        {
            if (!(vox.Index.x < 0 || vox.Index.y < 0 || vox.Index.z < 0 ||
                vox.Index.x >= Controller.Size.x || vox.Index.y >= Controller.Size.y || vox.Index.z >= Controller.Size.z))
            {
                var gridVox = grid.GetVoxelAt(vox.Index);
                if ((vox.Type == VoxelType.Block || vox.Type == VoxelType.Connection) && gridVox.Type != VoxelType.Block)
                {
                    if (gridVox.Go == null)
                    {
                        gridVox.Go = GameObject.Instantiate(Controller.GoVoxel, vox.Index, Quaternion.identity, vox.ParentBlock.goBlockParent.transform);
                        gridVox.Go.name = vox.Name;
                    }

                    if (vox.Type == VoxelType.Connection)
                    {
                        GameObject go = gridVox.Go;
                        var rend = go.GetComponent<Renderer>();
                        go.transform.SetParent(vox.ParentBlock.goBlockParent.transform);
                        rend.material = Controller.MatConnection;
                    }
                    else if (vox.Type == VoxelType.Block)
                    {
                        GameObject go = gridVox.Go;
                        var rend = go.GetComponent<Renderer>();
                        go.transform.SetParent(vox.ParentBlock.goBlockParent.transform);
                        rend.material = Controller.MatBlock;
                    }
                }
            }
        }
    }




}
