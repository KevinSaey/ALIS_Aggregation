using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Controller : MonoBehaviour
{
    [SerializeField]
    GameObject _goVoxel;
    public static GameObject GoVoxel;

    [SerializeField]
    GameObject _goTarget;
    public static GameObject GoTarget;

    [SerializeField]
    Material _matBlock, _matConnection;
    public static Material MatBlock, MatConnection;

    [SerializeField]
    Vector3Int _size;
    public static Vector3Int Size;

    [SerializeField]
    int _voxelSize, _minCon;
    public static int VoxelSize, MinCon;

    Block test;
    Grid3D _grid;


    void Start()
    {
        GoVoxel = _goVoxel;
        MatBlock = _matBlock;
        MatConnection = _matConnection;
        GoTarget = _goTarget;
        Size = _size;
        VoxelSize = _voxelSize;
        MinCon = _minCon;

        _grid = new Grid3D(Size);
        var pattern = new PatternA();
        test = new Block(pattern, new Vector3Int(15, 1, 15), new Vector3Int(0, 0, 0), _grid);
        _grid.AddBlockToGrid(test);

        // Temporary

        StartCoroutine(NextBlockOverTime());
    }

    IEnumerator NextBlockOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            var block = _grid.GenerateNextBlock();


            Debug.Log("NextBlock");
        }
    }

}
