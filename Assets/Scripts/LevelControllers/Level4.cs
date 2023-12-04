using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level4 : MonoBehaviour
{
    public GameObject dronePrefab;
    public List<EnemyController> enemies;
    public List<Lever> levers;
    public TMP_Text greaterHint;
    public TMP_Text evenHint;
    public TMP_Text colorHint;
    public Door door;

    private int properLever;
    private int leverPushes;
    private int dronesInRoom;
    private int wrongLeverId;  // Id of the wrong lever that was pushed last 

    private List<Vector3> spawningPositions = new List<Vector3>
    {
        new Vector3(2.5f, 25, 3),
        new Vector3(8.5f, 25, 15),
        new Vector3(-3.5f, 25, 15),
        new Vector3(2.5f, 25, 27),
        new Vector3(14.5f, 25, 27),
        new Vector3(-9.5f, 25, 27),
    };

    public void LeverAction(int lever_id)
    {
        if (lever_id == properLever)
            door.Switch();
        else
        {
            leverPushes += 1;
            wrongLeverId = lever_id;
            foreach (Lever lever in levers)
                lever.active = false;
            SpawnSeveralDrones(leverPushes);
        }
    }
    
    public void OnEnemyDeath(int enemy_id)
    {
        if (enemy_id == 1)
        {
            dronesInRoom -= 1;
            if (dronesInRoom == 0)
            {
                foreach (Lever lever in levers)
                {
                    lever.active = true;
                    if (lever.id == wrongLeverId)
                        lever.Switch();
                }
            }
        }
    }

    private void Start()
    {
        properLever = Random.Range(1, 9);
        if (properLever > 4)
            greaterHint.text = "Lever's number is greater than 4";
        else
            greaterHint.text = "Lever's number is less than 5";
        if (properLever % 2 == 0)
            evenHint.text = "Lever's number is even";
        else
            evenHint.text = "Lever's number is odd";
        if (properLever is 1 or 2 or 5 or 6)
            colorHint.text = "Lever's color is red";
        else
            colorHint.text = "Lever's color is blue";
        
        SetFirstEnemy();
        SetSecondEnemy();
        SetThirdEnemy();
        SetFourthEnemy();
        SetFifthEnemy();
    }

    private void SpawnSeveralDrones(int n)
    {
        n = Mathf.Min(n, 6);
        dronesInRoom = n;
        List<int> indexes = new List<int>
        {
            0, 1, 2, 3, 4, 5
        };
        
        for (int i = 0; i < n; i++)
        {
            int ind = indexes[Random.Range(0, indexes.Count)];
            indexes.Remove(ind);
            SpawnDrone(spawningPositions[ind]);
        }
    }

    private void SpawnDrone(Vector3 pos)
    {
        GameObject droneObject = Instantiate(dronePrefab);
        droneObject.transform.position = pos;
        EnemyController drone = droneObject.GetComponent<EnemyController>();
        drone.id = 1;
        drone.StartCoroutine(drone.ActionSequence(new List<IEnumerator>
        {
            drone.AllowShooting(false),
            drone.ChangeSpeed(5),
            drone.MoveTo(pos + new Vector3(0, -11, 0)),
            drone.ChangeSpeed(2 + (float) leverPushes / 3),
            drone.AllowShooting(true),
            drone.MoveAroundPoint(pos + new Vector3(0, -11, 0), 4)
        }));
    }

    private void SetFirstEnemy()
    {
        EnemyController drone = enemies[0];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.MoveAroundPoint(startPos, 3));
    }
    
    private void SetSecondEnemy()
    {
        EnemyController drone = enemies[1];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.MoveAroundPoint(startPos, 3));
    }
    
    private void SetThirdEnemy()
    {
        EnemyController drone = enemies[2];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.MoveAroundPoint(startPos, 2.8f));
    }
    
    private void SetFourthEnemy()
    {
        EnemyController drone = enemies[3];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.MoveAroundPoint(startPos, 2.8f));
    }
    
    private void SetFifthEnemy()
    {
        EnemyController drone = enemies[4];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.MoveAroundPoint(startPos, 2.5f));
    }

}
