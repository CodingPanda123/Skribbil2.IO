using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawingScript : MonoBehaviour
{
    [SerializeField] private GameObject square;

    public List<Vector3> squaresDrawnPosition = new List<Vector3>();

    Vector3 mousePosition;

    [SerializeField] ServerScript server;
    void Update()
    {


        if (!squaresDrawnPosition.Contains(Input.mousePosition))
        {
            if (Input.GetMouseButtonDown(0))
            {

                // print(Input.mousePosition);
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 10f;
                Instantiate(square, mousePosition, Quaternion.identity);
                

                server.positionBuffer.Add(mousePosition.x);
                server.positionBuffer.Add(mousePosition.y);
                server.positionBuffer.Add(mousePosition.z);

                squaresDrawnPosition.Add(mousePosition);
            }
        }


    }




}
