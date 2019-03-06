using UnityEngine;
using UnityEditor;

/// <summary>
/// Implementation to use different generation algorithms (for future use)
/// </summary>
public interface IGenerationAlgorithm

{
    Block GetNextBlock(Grid3D grid);
}