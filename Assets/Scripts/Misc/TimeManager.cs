using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static bool slow;
    public const float slowCoefficient = 0.3f;

    public static float deltaTime()
    {
        if (slow)
            return slowCoefficient * Time.deltaTime;

        return Time.deltaTime;
    }

    private void Update()
    {
        slow = Input.GetMouseButton(1);
    }
}
