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
    int _minimalConnections = 3;

    [SerializeField]
    GameObject _goTarget;
    public static GameObject GoTarget;

    [SerializeField]
    Material _matBlock, _matConnection;
    public static Material MatBlock, MatConnection;

    Block test;
    Grid3D _grid;


    void Start()
    {
        GoVoxel = _goVoxel;
        MatBlock = _matBlock;
        MatConnection = _matConnection;
        GoTarget = _goTarget;

        _grid = new Grid3D(30, 50, 30);
        var pattern = new PatternA();
        test = new Block(pattern, new Vector3Int(15, 1, 15), new Vector3Int(0, 0, 0));
        _grid.AddToGrid(test);

        // Temporary




    }

    public void DrawGrid()
    {

    }

    public void Update()
    {
        StartCoroutine(NextBlockOverTime());
    }

    IEnumerator NextBlockOverTime()
    {
        yield return new WaitForSeconds(1f);
        var block = _grid.GenerateNextBlock();


        Debug.Log("NextBlock");
    }

}
