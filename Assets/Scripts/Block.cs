using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Block

{
    public Pattern Pattern;
    public Vector3Int ZeroIndex;
    public List<Voxel> BlockVoxels;
    //public Vector3Int X, Y, Z, MinX, MinY, MinZ;

    Vector3Int _rotation;

    public Block(Pattern pattern, Vector3Int zeroIndex, Vector3Int newRotation)
    {
        Pattern = pattern;
        ZeroIndex = zeroIndex;

        _rotation = newRotation;
        Debug.Log(_rotation);
        BlockVoxels = GetVoxels().ToList();
        /* X = new Vector3Int(1, 0, 0);
         Y = new Vector3Int(0, 1, 0);
         Z = new Vector3Int(0, 0, 1);

         MinX = new Vector3Int(-1, 0, 0);
         MinY = new Vector3Int(0, -1, 0);
         MinZ = new Vector3Int(-1, 0, -1);*/
    }

    IEnumerable<Voxel> GetVoxels()
    {
        foreach (var voxel in Pattern.Voxels)
        {
            

            yield return RotateVoxel(voxel);
        }
    }

    Voxel RotateVoxel(Voxel vox)
    {
        var copyVox = vox.ShallowClone();

        int x, y, z;
        x = copyVox.Index.x;
        y = copyVox.Index.y;
        z = copyVox.Index.z;

        switch (_rotation.x)
        {
            case 0:
                break;
            case 90:
                copyVox.Index = new Vector3Int(x, z, -y);
                break;
            case 180:
                copyVox.Index = new Vector3Int(x, -y, -z);
                break;
            case 270:
            case -90:
                copyVox.Index = new Vector3Int(x, -z, y);
                break;
            default:
                Debug.Log("X Angle should be either 90,180,270 or -90 degrees");
                break;
        }
        x = copyVox.Index.x;
        y = copyVox.Index.y;
        z = copyVox.Index.z;

        switch (_rotation.y)
        {
            case 0:

                break;
            case 90:
                copyVox.Index = new Vector3Int(-z, y, x);
                break;
            case 180:
                copyVox.Index = new Vector3Int(-x, y, -z);
                break;
            case 270:
            case -90:
                copyVox.Index = new Vector3Int(z, y, -x);
                break;
            default:
                Debug.Log("Y angle should be either 90,180,270 or -90 degrees");
                break;
        }
        x = copyVox.Index.x;
        y = copyVox.Index.y;
        z = copyVox.Index.z;

        switch (_rotation.z)
        {
            case 0:

                break;
            case 90:
                copyVox.Index = new Vector3Int(-y, x, z);
                break;
            case 180:
                copyVox.Index = new Vector3Int(-x, -y, z);
                break;
            case 270:
            case -90:
                copyVox.Index = new Vector3Int(y, -x, z);
                break;
            default:
                Debug.Log("Z-Angle should be either 90,180,270 or -90 degrees");
                break;


        }

        return copyVox;
    }


    //Vector3.SignedAngle(a, b, Vector3.up);


    /*public GameObject GoBlock;
    GameObject _block = new GameObject("Block");
    Vector3 _position;
    Vector3 _orientation;
    public List<Vector3> Points;
    public List<Vector3> Connections;
    int _xblock;
    int _yblock;
    int _zblock;
    float _voxelSize;

    public Block()
    {
    }

    public Block(Vector3 pos, Vector3 or, GameObject goBlock, int xblock, int yblock, int zblock, float voxelSize)

    {
        _position = pos;
        _orientation = or;
        GoBlock = goBlock;
        _xblock = xblock;
        _yblock = yblock;
        _zblock = zblock;
        _voxelSize = voxelSize;


        GeneratePoints();
        GenerateShape();
        _block.transform.rotation = Quaternion.Euler(or);
        //_block.transform.position = pos;
    }


    void GeneratePoints()
    {
        // Generate the points within the block
        Points = new List<Vector3>();
        for (int z = 0; z < _zblock; z++)
        {
            for (int y = 0; y < _yblock; y++)
            {
                for (int x = 0; x < _xblock; x++)
                {
                    Points.Add(new Vector3(x * _voxelSize, y * _voxelSize, z * _voxelSize) + 
                        new Vector3(_position.x-_voxelSize*_xblock/2+_voxelSize/2, _position.y, _position.z ));

                }
            }
        }
    }

    void GetPointFromIndex(int x, int y, int z)
    {

    }

    void GenerateShape()
    {
        
        foreach (var point in Points)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = new Vector3(_voxelSize, _voxelSize, _voxelSize);
            cube.transform.position = point;
            cube.transform.SetParent(_block.transform);
        }
    }

    void GenerateConnections()
    {
        // Generate the possible connection points
    }
    */

}
