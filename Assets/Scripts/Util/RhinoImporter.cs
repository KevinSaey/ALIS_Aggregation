using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// Import a assembly pattern generated in grasshopper
/// </summary>
public class RhinoImporter//based on Vicente's code
{

    string _path = @"D:\Unity\School\ALIS_Aggregation\RhinoExporter\Sample_1.xml";
    Grid3D _grid;

    public RhinoImporter(Grid3D grid)
    {
        //var files = LoadFiles();
        //Debug.Log(files);
        _grid = grid;
        //List<RhinoSample> assemblies = new List<RhinoSample>();

        /*for (int i = 0; i < files.Count; i++)
        {
            assemblies.Add(RhinoSample.Import(files[i]));
        }*/
        Assembly.Import(_path).Generate(_grid);
    }

    public List<string> LoadFiles()
    {
        return Directory.GetFiles(_path, "*.xml").ToList();
    }
}



/// <summary>
/// Assembly pattern existing of blocks 
/// </summary>
public class Assembly //VS
{
    public List<Instance> Instances { get; set; }

    public static Assembly Import(string fileName)
    {
        var serializer = new XmlSerializer(typeof(Assembly));
        using (var reader = XmlReader.Create(fileName))
        {
            return serializer.Deserialize(reader) as Assembly;
        }
    }

    public void Generate(Grid3D grid)
    {
        var pattern = new PatternC();
        foreach (var instance in Instances)
        {
            var rotation = instance.Pose.rotation.eulerAngles.ToVector3Int();
            Debug.Log("imported rotation " + rotation);
            if (rotation.x == -90) rotation.x = 270;
            if (rotation.y == -90) rotation.x = 270;
            if (rotation.z == -90) rotation.x = 270;
            Debug.Log("adjusted rotation " + rotation);

            var block = new Block(pattern, instance.Pose.position.ToVector3Int(), rotation, grid);
            grid.AddBlockToGrid(block);
        }
    }
}

public class Instance
{
    public int DefinitionIndex;
    public Pose Pose;
}