using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{

    Vector3 oldPosition;
    HexComponent[] hexes;

    void Start()
    {

        oldPosition = transform.position;
    }

    void Update()
    {
        CheckIfCameraMoved();
    }

    void CheckIfCameraMoved()
    {
        if (oldPosition != transform.position)
        {
            oldPosition = transform.position;
            if (hexes == null)
            {
                hexes = GameObject.FindObjectsOfType<HexComponent>();
            }
            foreach (var hex in hexes)
            {
                hex.UpdatePosition();
            }
        }
    }
}
