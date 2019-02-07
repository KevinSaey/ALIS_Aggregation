using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

/*public class EditorScript : MonoBehaviour
{
    [MenuItem("Window/KS_ImportPattern")]
    static void KS_ImportPattern()
    {
        EditorWindow.GetWindow(typeof(KS_ImportPattern));
    }
}

public class KS_ImportPattern : EditorWindow
{
    string _fileName = @"C:\Users\vicen\Documents\Repositories\RhinoToUnity\Rhino files\export.xml";
    bool _addJoints = true;
    bool _makeKinematic = true;

    void OnGUI()
    {
        GUILayout.Label("Import from Rhino", EditorStyles.boldLabel);
        _fileName = EditorGUILayout.TextField("Path of XML file", _fileName);
        _addJoints = EditorGUILayout.Toggle("Add joints", _addJoints);
        _makeKinematic = EditorGUILayout.Toggle("Set to kinematic", _makeKinematic);

        if (GUILayout.Button("Import", GUILayout.Width(120)))
        {
            Assembly.Import(_fileName).Generate(_addJoints, _makeKinematic);
        }
    }
}

public class Assembly
{
    public List<Tile> Tiles { get; set; }
    public List<Instance> Instances { get; set; }
    public float AngleLimit { get; set; }
    public float BreakForce { get; set; }

    public static Assembly Import(string fileName)
    {
        var serializer = new XmlSerializer(typeof(Assembly));
        using (var reader = XmlReader.Create(fileName))
        {
            return serializer.Deserialize(reader) as Assembly;
        }
    }

    public void Generate(bool addJoints, bool makeKinematic)
    {
        for (int i = 0; i < Tiles.Count; i++)
        {
            Tiles[i].CompactIndex = i;
            Tiles[i].MakeKinematic = makeKinematic;
            Tiles[i].Furthest = Tiles[i].Faces.Max(f => f.sqrMagnitude);
        }

        var anchor = new GameObject("Assembly").transform;

        foreach (var instance in Instances)
        {
            var tile = Tiles.First(t => t.Index == instance.DefinitionIndex);
            instance.Tile = tile;

            var go = tile.Instantiate(instance.Pose);
            go.transform.parent = anchor;
            instance.GameObject = go;
        }

        foreach (var tile in Tiles)
            tile.Destroy();

        if (addJoints)
            MakeJoints();
    }

    void MakeJoints()
    {
        for (int i = 0; i < Instances.Count - 1; i++)
        {
            for (int j = i + 1; j < Instances.Count; j++)
            {
                MakeJoint(Instances[i], Instances[j]);
            }
        }
    }

    void MakeJoint(Instance a, Instance b)
    {
        const float tol = 0.001f;
        const float tolSq = tol * tol;
        float size = a.Tile.Furthest + b.Tile.Furthest;

        var v = (a.Pose.position - b.Pose.position);
        if (Mathf.Abs(v.x) > size || Mathf.Abs(v.y) > size || Mathf.Abs(v.z) > size)
            return;

        foreach (var la in a.Tile.Faces)
        {
            var pa = a.Transform(la);

            foreach (var lb in b.Tile.Faces)
            {
                var pb = b.Transform(lb);
                var distance = (pb - pa).sqrMagnitude;

                if (distance < tolSq)
                {
                    var joint = a.GameObject.AddComponent<FixedJoint>();
                    joint.anchor = pa;
                    joint.connectedBody = b.GameObject.GetComponent<Rigidbody>();
                    joint.connectedAnchor = pb;
                    joint.breakForce = BreakForce;
                    joint.breakTorque = BreakForce;
                    joint.enableCollision = false;
                    joint.enablePreprocessing = false;
                    return;
                }
            }
        }
    }
}

public class Tile
{
    public int Index { get; set; }
    public float Mass { get; set; }
    public Vector3 Centroid { get; set; }
    public List<MeshExport> Renderers { get; set; }
    public List<MeshExport> Colliders { get; set; }
    public List<Vector3> Faces { get; set; }

    internal int CompactIndex;
    internal bool MakeKinematic;
    internal float Furthest;
    GameObject _gameObject;

    void MakeGameObject()
    {
        int num = CompactIndex + 1;
        var renderMesh = Renderers.First().ToMesh($"RenderMesh{num:00}");
        var colliderMeshes = Colliders.Select((c, i) => c.ToMesh($"ColliderMesh{num:00}_{i + 1:00}"));
        var material = Resources.Load($"Material{num:00}") as Material;

        var go = new GameObject($"Tile{num}", typeof(MeshFilter), typeof(MeshRenderer), typeof(Rigidbody), typeof(TileInstance));
        go.SetActive(false);
        go.layer = 9;

        var filter = go.GetComponent<MeshFilter>();
        filter.mesh = renderMesh;

        var renderer = go.GetComponent<MeshRenderer>();
        renderer.material = material;

        var rb = go.GetComponent<Rigidbody>();
        rb.isKinematic = MakeKinematic;
        rb.centerOfMass = Centroid;
        rb.mass = Mass;
        // rb.drag = 1f;
        // rb.angularDrag = 1f;

        foreach (var mesh in colliderMeshes)
        {
            var collider = go.AddComponent<MeshCollider>();
            collider.convex = true;
            collider.sharedMesh = mesh;
        }

        //var collider = go.AddComponent<BoxCollider>();
        _gameObject = go;
    }

    public void Destroy()
    {
        GameObject.DestroyImmediate(_gameObject);
        _gameObject = null;
    }

    public GameObject Instantiate(Pose pose)
    {
        if (_gameObject == null)
            MakeGameObject();

        var go = GameObject.Instantiate(_gameObject);
        go.transform.position = pose.position;
        go.transform.rotation = pose.rotation;
        go.SetActive(true);

        return go;
    }
}

public class Instance
{
    public int DefinitionIndex;
    public Pose Pose;

    internal Tile Tile;
    internal GameObject GameObject;

    public Vector3 Transform(Vector3 point)
    {
        return Pose.rotation * point + Pose.position;
    }
}

public class MeshExport
{
    public List<Vector3> Vertices { get; set; }
    public List<Vector2> TextureCoordinates { get; set; }
    public List<int> Faces { get; set; }

    public Mesh ToMesh(string name)
    {
        var mesh = new Mesh()
        {
            name = name,
            vertices = Vertices.ToArray(),
            uv = TextureCoordinates.ToArray(),
            triangles = Faces.ToArray()
        };

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();

        return mesh;
    }
}*/