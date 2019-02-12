using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms;

public class PathFinding
{
    Grid3D _grid;
    Mesh _mesh;

    public PathFinding(Grid3D grid)
    {
        _grid = grid;
        Regenerate();
    }

    public void Regenerate()
    {
        GenerateClimableMeshes(CreateGraph());
    }

    public void Update()
    {
        if (_grid == null||_mesh == null) return;
        Drawing.DrawMesh(false, _mesh);
    }

    public TryFunc<Face, IEnumerable<TaggedEdge<Face, Edge>>> CreateGraph()
    {
        // select edges of boundary faces
        var edges = _grid.GetEdges().Where(e => e.ClimbableFaces.Length == 2);

        // create graph from edges -- library quickgraph
        var graphEdges = edges.Select(e => new TaggedEdge<Face, Edge>(e.ClimbableFaces[0], e.ClimbableFaces[1], e));
        var graph = graphEdges.ToUndirectedGraph<Face, TaggedEdge<Face, Edge>>();

        // start face for shortest path
        var start = _grid.GetVoxelAt(_grid.blocks[0].ZeroIndex).Faces.First(f=>f!=null&&f.Climable);

        // calculate shortest path from start face to all boundary faces
        return graph.ShortestPathsDijkstra(e => 1.0, start);
    }

    public void GenerateClimableMeshes(TryFunc<Face,IEnumerable<TaggedEdge<Face,Edge>>> shortest)
    {
        var faceMeshes = new List<CombineInstance>();

        foreach (var face in _grid.GetClimableFaces())
        {
            float t = 1;

            if (shortest(face, out var path))
            {
                t = path.Count() * 0.04f;
                t = Mathf.Clamp01(t);
            }

            Mesh faceMesh;
            faceMesh = Drawing.MakeFace(face.Center, face.Direction, 1, t);

            faceMeshes.Add(new CombineInstance() { mesh = faceMesh });
        }
        var mesh = new Mesh()
        {
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        };

        mesh.CombineMeshes(faceMeshes.ToArray(), true, false, false);

        _mesh = mesh;
    }
}
