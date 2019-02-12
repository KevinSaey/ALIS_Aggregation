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
    PathFinding _pathFinding;
    bool _showPath;


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
        test = new Block(pattern, new Vector3Int(Size.x / 2, 1, Size.y / 2), new Vector3Int(0, 0, 0), _grid);
        _grid.AddBlockToGrid(test);

        _pathFinding = new PathFinding(_grid);


        //StartCoroutine(NextBlockOverTime());
    }

    void OnGUI()
    {
        int i = 1;
        int s = 25;

        if (GUI.Button(new Rect(s, s * i++, 100, 20), "Switch graph"))
        {
            _showPath = !_showPath;
            _grid.SwitchBlockVisibility(!_showPath);
        }
        if (GUI.Button(new Rect(s, s * i++, 100, 20), "Generate Next Block"))
        {
            NextBlock();
        }

    }

    void Update()
    {
        if (_showPath)
        {
            _pathFinding.DrawMesh();
        }
    }

    public void NextBlock()
    {
        var block = _grid.GenerateNextBlock();
        _grid.SwitchBlockVisibility(!_showPath);
        _pathFinding.Regenerate();
        Debug.Log("NextBlock");
    }


    IEnumerator NextBlockOverTime()
    {
        while (true)
        {
            //yield return new WaitForSeconds(1f);

            var block = _grid.GenerateNextBlock();
            _grid.SwitchBlockVisibility(!_showPath);
            _pathFinding.Regenerate();
            Debug.Log("NextBlock");
        }
    }

}
