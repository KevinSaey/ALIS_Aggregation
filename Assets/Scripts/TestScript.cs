using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    GameObject _goVoxel;
    Block test;
    

    void Start()
    {
        var pattern = new Pattern(Vector3Int.up);
        test = new Block(pattern, Vector3Int.zero, new Vector3Int(90,20,0));
        foreach (var voxel in test.BlockVoxels/*.Where(s=>s.Type==VoxelType.Block)*/)
        {
            var vox = GameObject.Instantiate(_goVoxel, voxel.Index, Quaternion.identity);
            vox.name = voxel.Name;
        }
        
    }

    void Update()
    {
        
    }
}
