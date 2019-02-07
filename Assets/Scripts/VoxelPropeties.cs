using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelPropeties : MonoBehaviour
{

    public bool available;
    public bool ocupied;
    public bool connection;

    public Material matAvailable, matOccupied, matConnection;
    Material CurrentMat;
    // Use this for initialization
    void Start()
    {
        available = true;
        ocupied = false;
        connection = false;
        CurrentMat = matAvailable;

    }

    // Update is called once per frame
    void Update()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null && available)
        {
            rend.material = matAvailable;
        }

        if (rend != null && ocupied)
        {
            rend.material = matOccupied;
        }

        if (rend != null && connection)
        {
            rend.material = matConnection;
        }
        
    }
}