using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid3D : MonoBehaviour
{
    public Voxel[,,] Voxels { get; private set; }
    public Vector3Int Size;

    // Start is called before the first frame update
    void Start()
    {
        Size = new Vector3Int(20, 20, 20);

        // make voxels
        Voxels = new Voxel[Size.x, Size.y, Size.z];

        for (int z = 0; z < Size.z; z++)
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                {
                    Voxels[x, y, z] = new Voxel(x, y, z, VoxelType.Empty);
                }

    }

    List<Voxel> GetNeighbourghs(Voxel baseVoxel)
    {
        List<Voxel> neighbourghs = new List<Voxel>();



        return neighbourghs;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
