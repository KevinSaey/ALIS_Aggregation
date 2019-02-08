using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    GameObject _goVoxel;
    [SerializeField]
    int _minimalConnections = 3;
    [SerializeField]
    Material _block, _connection;

    Block test;
    Grid3D grid;


    void Start()
    {
        grid = new Grid3D(30, 30, 30);
        var pattern = new Pattern(Vector3Int.up);
        test = new Block(pattern, new Vector3Int(15, 1, 15), new Vector3Int(0, 90, 0));
        grid.AddToGrid(test);
        grid.GenerateNextBlock();

        // Temporary
        foreach (var voxel in grid.Voxels)
        {
            if (voxel.Type == VoxelType.Block)
            {
                var vox = GameObject.Instantiate(_goVoxel, voxel.Index, Quaternion.identity);
                vox.name = voxel.Name;

                if (voxel.Type == VoxelType.Connection)
                {
                    var rend = vox.GetComponent<Renderer>();
                    rend.material = _connection;
                }
            }
        }

    }

    void Update()
    {

    }
}
