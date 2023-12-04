using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseLookX : MonoBehaviour
{
    public float sensitivity = 3.0f;
    private float horizontalRot;


    void Update()
    {
        if (Game.state != Game.State.Play)
            return;
        
        horizontalRot += Input.GetAxis("Mouse X") * sensitivity;
        float verticalRot = transform.localEulerAngles.x;
        transform.localEulerAngles = new Vector3(verticalRot, horizontalRot, 0);
    }
}