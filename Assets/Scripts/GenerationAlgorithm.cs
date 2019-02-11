using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class GenerationAlgorithm : IGenerationAlgorithm
{
    Vector3Int _target;


    public GenerationAlgorithm(Vector3Int target)
    {

        _target = target;
    }


    public Block GetNextBlock(Grid3D grid)
    {
        // The connection points, sorted by distance.
        IEnumerable<Voxel> byDistance = SortByDistance(GetConnectionPoints(grid));
        IEnumerable<Block> blocksFromDistance = byDistance.SelectMany(v => new Block[] { new Block(new PatternA(), v, grid), new Block(new PatternB(), v, grid) });


        return blocksFromDistance.FirstOrDefault(bl => Validate(bl, grid));
    }

    private List<Voxel> GetConnectionPoints(Grid3D grid)
    {
        List<Voxel> lst = new List<Voxel>();
        foreach (var vox in grid.Voxels)
        {
            if (vox.Type == VoxelType.Connection)
                lst.Add(vox);
        }
        return lst;
    }

    private IEnumerable<Voxel> SortByDistance(List<Voxel> toSort)
    {
        return toSort.OrderBy(vx => Vector3Int.Distance(vx.Index, _target));
    }


    // These functions are meant to eliminate blocks that cannot / should not exist

    private bool Validate(Block block, Grid3D grid)
    {
        bool valid = true;
        valid = valid && grid.CanBlockExist(block);
        valid = valid && CheckMinConnPoints(block, grid);
        return valid;
    }

    private bool CheckMinConnPoints(Block block, Grid3D grid)
    {
        IEnumerable<Vector3Int> connPoints = GetConnectionPoints(grid).Select(pt => pt.Index);
        IEnumerable<Vector3Int> blocks = block.BlockVoxels.Where(vx => vx.Type == VoxelType.Block).Select(pt => pt.Index);

        return connPoints.Intersect(blocks).Count() >= Controller.MinCon;
    }
}