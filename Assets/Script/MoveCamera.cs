using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    public Transform cameraPosition1;

    private void Update()
    {
        transform.position = cameraPosition.position;
        if (Input.GetMouseButton(1))
        {
            transform.position = cameraPosition1.position;
        }
    }
}
