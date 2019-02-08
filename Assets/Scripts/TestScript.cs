using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    GameObject _goVoxel, _goTarget;
    [SerializeField]
    int _minimalConnections = 3;
    [SerializeField]
    Material _block, _connection;

    Vector3Int _target;
    Block test;
    Grid3d grid;



    void Start()
    {
        _target = _goTarget.transform.position.ToInt();

        grid = new Grid3d(30, 30, 30);
        var pattern = new Pattern(Vector3Int.up);
        test = new Block(pattern, new Vector3Int(15, 0, 15), new Vector3Int(0, 90, 0));
        grid.AddToGrid(test);
        CreateVoxels();
    }


    void CreateVoxels()
    {
        foreach (Voxel voxel in grid.Voxels)
        {
            if (voxel.Type != VoxelType.Empty)
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
