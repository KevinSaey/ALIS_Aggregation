using UnityEngine;
using UnityEditor;

public interface IGenerationAlgorithm

{
    Block GetNextBlock(Grid3D grid);
}