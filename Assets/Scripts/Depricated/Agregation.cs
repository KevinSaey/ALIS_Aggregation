using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// (depricated) Random aggregation of blocks
/// </summary>
public class Agregation : MonoBehaviour
{

    [SerializeField]
    GameObject _goVoxel;
    [SerializeField]
    int _boxX, _boxY, _boxZ;

    List<GameObject> _voxels = new List<GameObject>();
    List<List<List<bool>>> _voxelBool = new List<List<List<bool>>>();
    List<Vector3> _connections = new List<Vector3>();
    List<Vector3> _direction = new List<Vector3> { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

    [SerializeField]
    int _xAmount, _yAmount, _zAmount;

    void Start()
    {
        BuildVoxelGrid();
        CreateFirstBlock();
        /*for (int i = 0; i < 2000; i++)
        {
            GenerateNextBlok();
        }*/
        

    }

    void BuildVoxelGrid()
    {
        for (int y = 0; y < _yAmount; y++)
        {
            _voxelBool.Add(new List<List<bool>>());
            for (int z = 0; z < _zAmount; z++)
            {
                _voxelBool[y].Add(new List<bool>());
                for (int x = 0; x < _xAmount; x++)
                {
                    _voxelBool[y][z].Add(true);
                }
            }
        }
    }

    void CreateFirstBlock()
    {
        Vector3 firstBlock = new Vector3((int)_xAmount / 2, 0, (int)_zAmount/2);
        CreateVoxels(CreateBlock(firstBlock, new Vector3(0, 1, 0), true));
    }

    List<Vector3> CreateBlock(Vector3 origin, Vector3 direction, bool create)
    {
        List<Vector3> boxVoxels = new List<Vector3>();
        if (direction == Vector3.up)
        {
            Debug.Log("up");
            for (int y = 0; y < _boxY; y++)
            {
                for (int z = 0; z < _boxZ; z++)
                {
                    for (int x = 0; x < _boxX; x++)
                    {
                        boxVoxels.Add(new Vector3(origin.x + x, origin.y + y, origin.z + z));
                    }
                }
            }
            if (create)
            {
                foreach (var voxel in boxVoxels.GetRange(0, 3))
                {
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z + 1));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z - 1));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y - 1, voxel.z));
                }
                foreach (var voxel in boxVoxels.GetRange(15, 3))
                {
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z + 1));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z - 1));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y + 1, voxel.z));
                }
                for (int i = 0; i < 18; i += 3)
                {
                    AddSingleConnection(new Vector3(boxVoxels[i].x - 1, boxVoxels[i].y, boxVoxels[i].z));

                }
                for (int i = 2; i < 18; i += 3)
                {
                    AddSingleConnection(new Vector3(boxVoxels[i].x + 1, boxVoxels[i].y, boxVoxels[i].z));
                }
            }
        }
        else if (direction == Vector3.down)
        {
            Debug.Log("down");
            for (int y = 0; y < _boxY; y++)
            {
                for (int z = 0; z < _boxZ; z++)
                {
                    for (int x = 0; x < _boxX; x++)
                    {
                        boxVoxels.Add(new Vector3(origin.x - x, origin.y - y, origin.z + z));
                    }
                }
            }
            if (create)
            {
                foreach (var voxel in boxVoxels.GetRange(0, 3))
                {
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z + 1));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z - 1));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y + 1, voxel.z));
                }
                foreach (var voxel in boxVoxels.GetRange(15, 3))
                {
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z + 1));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z - 1));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y - 1, voxel.z));
                }
                for (int i = 0; i < 18; i += 3)
                {
                    AddSingleConnection(new Vector3(boxVoxels[i].x - 1, boxVoxels[i].y, boxVoxels[i].z));

                }
                for (int i = 2; i < 18; i += 3)
                {
                    AddSingleConnection(new Vector3(boxVoxels[i].x + 1, boxVoxels[i].y, boxVoxels[i].z));
                }
            }
        }
        else if (direction == Vector3.right)
        {
            Debug.Log("right");
            for (int y = 0; y < _boxY; y++)
            {
                for (int z = 0; z < _boxZ; z++)
                {
                    for (int x = 0; x < _boxX; x++)
                    {
                        boxVoxels.Add(new Vector3(origin.x + y, origin.y - x, origin.z + z));
                    }
                }
            }
            if (create)
            {
                foreach (var voxel in boxVoxels.GetRange(0, 3))
                {
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z + 1));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z - 1));
                    AddSingleConnection(new Vector3(voxel.x - 1, voxel.y, voxel.z));
                }
                foreach (var voxel in boxVoxels.GetRange(15, 3))
                {
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z + 1));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z - 1));
                    AddSingleConnection(new Vector3(voxel.x + 1, voxel.y, voxel.z));
                }
                for (int i = 0; i < 18; i += 3)
                {
                    AddSingleConnection(new Vector3(boxVoxels[i].x, boxVoxels[i].y + 1, boxVoxels[i].z));

                }
                for (int i = 2; i < 18; i += 3)
                {
                    AddSingleConnection(new Vector3(boxVoxels[i].x, boxVoxels[i].y - 1, boxVoxels[i].z));
                }
            }
        }
        else if (direction == Vector3.left)
        {
            Debug.Log("left");
            for (int y = 0; y < _boxY; y++)
            {
                for (int z = 0; z < _boxZ; z++)
                {
                    for (int x = 0; x < _boxX; x++)
                    {
                        boxVoxels.Add(new Vector3(origin.x - y, origin.y + x, origin.z + z));
                    }
                }
            }
            if (create)
            {
                foreach (var voxel in boxVoxels.GetRange(0, 3))
                {
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z + 1));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z - 1));
                    AddSingleConnection(new Vector3(voxel.x + 1, voxel.y, voxel.z));
                }
                foreach (var voxel in boxVoxels.GetRange(15, 3))
                {
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z + 1));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z - 1));
                    AddSingleConnection(new Vector3(voxel.x - 1, voxel.y, voxel.z));
                }
                for (int i = 0; i < 18; i += 3)
                {
                    AddSingleConnection(new Vector3(boxVoxels[i].x, boxVoxels[i].y - 1, boxVoxels[i].z));

                }
                for (int i = 2; i < 18; i += 3)
                {
                    AddSingleConnection(new Vector3(boxVoxels[i].x, boxVoxels[i].y + 1, boxVoxels[i].z));
                }
            }
        }
        else if (direction == Vector3.forward)
        {
            Debug.Log("forward");
            for (int y = 0; y < _boxY; y++)
            {
                for (int z = 0; z < _boxZ; z++)
                {
                    for (int x = 0; x < _boxX; x++)
                    {
                        boxVoxels.Add(new Vector3(origin.x + x, origin.y + z, origin.z + y));
                    }
                }
            }
            if (create)
            {
                foreach (var voxel in boxVoxels.GetRange(0, 3))
                {
                    AddSingleConnection(new Vector3(voxel.x, voxel.y + 1, voxel.z));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y - 1, voxel.z));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z - 1));
                }
                foreach (var voxel in boxVoxels.GetRange(15, 3))
                {
                    AddSingleConnection(new Vector3(voxel.x, voxel.y + 1, voxel.z));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y - 1, voxel.z));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z + 1));
                }
                for (int i = 0; i < 18; i += 3)
                {
                    AddSingleConnection(new Vector3(boxVoxels[i].x - 1, boxVoxels[i].y, boxVoxels[i].z));

                }
                for (int i = 2; i < 18; i += 3)
                {
                    AddSingleConnection(new Vector3(boxVoxels[i].x + 1, boxVoxels[i].y, boxVoxels[i].z));
                }
            }
        }
        else if (direction == Vector3.back)
        {
            Debug.Log("back");
            for (int y = 0; y < _boxY; y++)
            {
                for (int z = 0; z < _boxZ; z++)
                {
                    for (int x = 0; x < _boxX; x++)
                    {
                        boxVoxels.Add(new Vector3(origin.x - x, origin.y + z, origin.z - y));
                    }
                }
            }
            if (create)
            {
                foreach (var voxel in boxVoxels.GetRange(0, 3))
                {
                    AddSingleConnection(new Vector3(voxel.x, voxel.y + 1, voxel.z));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y - 1, voxel.z));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z + 1));
                }
                foreach (var voxel in boxVoxels.GetRange(15, 3))
                {
                    AddSingleConnection(new Vector3(voxel.x, voxel.y + 1, voxel.z));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y - 1, voxel.z));
                    AddSingleConnection(new Vector3(voxel.x, voxel.y, voxel.z - 1));
                }
                for (int i = 0; i < 18; i += 3)
                {
                    AddSingleConnection(new Vector3(boxVoxels[i].x + 1, boxVoxels[i].y, boxVoxels[i].z));

                }
                for (int i = 2; i < 18; i += 3)
                {
                    AddSingleConnection(new Vector3(boxVoxels[i].x - 1, boxVoxels[i].y, boxVoxels[i].z));
                }
            }
        }
        return boxVoxels;
    }

    void CreateVoxels(List<Vector3> indices)
    {
        GameObject daddy = new GameObject("Block");
        daddy.transform.position = indices.First();
        foreach (var index in indices)
        {
            _voxelBool[(int)index.y][(int)index.z][(int)index.x] = false;
            Vector3 voxel_location = index;
            _voxels.Add(GameObject.Instantiate(_goVoxel, voxel_location, Quaternion.identity, daddy.transform));
        }
    }

    void AddSingleConnection(Vector3 index)
    {
        if (index.y < _yAmount && index.z < _zAmount && index.x < _xAmount && index.y >= 0 && index.z >= 0 && index.x >= 0)
        {
            if (_voxelBool[(int)index.y][(int)index.z][(int)index.x])
            {
                _connections.Add(index);
            }
        }
    }

    void GenerateNextBlok()
    {
        int indexConnection = Random.Range(0, _connections.Count);
        int indexDirection = Random.Range(0, _direction.Count);


        bool possible = true;

        var nextVoxels = CreateBlock(_connections[indexConnection], _direction[indexDirection], false);
        foreach (var voxel in nextVoxels)
        {
            if (voxel.y >= _yAmount || voxel.z >= _zAmount || voxel.x >= _xAmount || voxel.y <= 0 || voxel.z <= 0 || voxel.x <= 0)
            {
                possible = false;
            }
            else if (_voxelBool[(int)voxel.y][(int)voxel.z][(int)voxel.x] == false)
            {
                possible = false;
            }
        }


        if (possible)
        {

            CreateVoxels(CreateBlock(_connections[indexConnection], _direction[indexDirection], true));
            foreach (var voxel in nextVoxels)
            {
                _connections.Remove(voxel);
            }
        }
    }

    private void Update()
    {
        StartCoroutine(CreateBlock());
    }

    private void OnGUI()
    {
        /*if (GUI.Button(new Rect(10, Screen.height - 60, 200, 50), "NEXT"))
        {
            GenerateNextBlok();
        }*/
    }

    IEnumerator CreateBlock()

    {

        yield return new WaitForSeconds(0.1f);
        GenerateNextBlok();
        Debug.Log("NextBlock");


    }
}
