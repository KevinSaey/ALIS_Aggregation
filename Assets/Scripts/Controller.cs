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

    Block startBlock;
    Grid3D _grid;
    
    bool _showPath;

    public RhinoImporter RhinoImport;

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
        //startBlock = new Block(pattern, new Vector3Int(Size.x / 2, 1, Size.y / 2), new Vector3Int(0, 180, 0), _grid);

        //_grid.AddBlockToGrid(startBlock);

        RhinoImport = new RhinoImporter(_grid);
        _grid.IniPathFinding();

        //StartCoroutine(NextBlockOverTime()); //To generate blocks over time
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
            _grid.PathFinding.DrawMesh();
        }
    }

    /// <summary>
    /// Generate the next block
    /// </summary>
    public void NextBlock()
    {
        var block = _grid.GenerateNextBlock();
        _grid.SwitchBlockVisibility(!_showPath);
        _grid.PathFinding.Regenerate();
        Debug.Log("NextBlock");
    }

    /// <summary>
    /// Generate a next block over time
    /// </summary>
    /// <returns></returns>
    IEnumerator NextBlockOverTime(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            var block = _grid.GenerateNextBlock();
            _grid.SwitchBlockVisibility(!_showPath);
            _grid.PathFinding.Regenerate();
            Debug.Log("NextBlock");
        }
    }
}
