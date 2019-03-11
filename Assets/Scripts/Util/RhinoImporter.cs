using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Import a assembly pattern generated in grasshopper
/// </summary>
public class RhinoImporter//based on Vicente's code
{
    string _fileName = @"D:\Unity\ALIS_Aggregation\RhinoExporter\export.xml";
    Grid3D _grid;

    public RhinoImporter(Grid3D grid)
    {
        _grid = grid;
        Assembly.Import(_fileName).Generate(_grid);
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
            if(rotation.x == -90) rotation.x = 270;
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