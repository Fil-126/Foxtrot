using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Vector3 normal = Vector3.right;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        normal = normal.normalized;
    }

    public Vector3 GetNormal()
    {
        Vector3 rotatedNormal = transform.rotation * normal;

        Vector3 playerRelPos = transform.InverseTransformPoint(player.position);
        playerRelPos.y = 0;
        if (Vector3.Angle(normal, playerRelPos) > 90)
            rotatedNormal = -rotatedNormal;

        return rotatedNormal;
    }
}
