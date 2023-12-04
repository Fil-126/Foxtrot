using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    public float openSpeed = 0.5f;

    private bool open;

    private Vector3 leftClosedPosition = new Vector3(0, 0, 0);
    private Vector3 leftOpenPosition = new Vector3(0, 0, 1.9f);
    private Vector3 rightClosedPosition = new Vector3(0, -0.15f, -1.43f);
    private Vector3 rightOpenPosition = new Vector3(0, -0.15f, -3.33f);

    private float openPart = 0;

    public void Switch()
    {
        open = !open;
        if (open)
            StartCoroutine(Open());
        else
            StartCoroutine(Close());
    }

    private IEnumerator Open()
    {
        while (open && openPart < 1)
        {
            while (Game.state != Game.State.Play)
            {
                yield return null;
            }
            
            openPart += openSpeed * TimeManager.deltaTime();
            if (openPart > 1)
                openPart = 1;
            
            Vector3 leftPosition = leftClosedPosition + (leftOpenPosition - leftClosedPosition) * openPart;
            leftDoor.transform.localPosition = leftPosition;
            Vector3 rightPosition = rightClosedPosition + (rightOpenPosition - rightClosedPosition) * openPart;
            rightDoor.transform.localPosition = rightPosition;
            
            yield return null;
        }
    }

    private IEnumerator Close()
    {
        while (!open && openPart > 0)
        {
            while (Game.state != Game.State.Play)
            {
                yield return null;
            }
            
            openPart -= openSpeed * TimeManager.deltaTime();
            if (openPart < 0)
                openPart = 0;
            
            Vector3 leftPosition = leftClosedPosition + (leftOpenPosition - leftClosedPosition) * openPart;
            leftDoor.transform.localPosition = leftPosition;
            Vector3 rightPosition = rightClosedPosition + (rightOpenPosition - rightClosedPosition) * openPart;
            rightDoor.transform.localPosition = rightPosition;
            
            yield return null;
        }
    }
    
}
