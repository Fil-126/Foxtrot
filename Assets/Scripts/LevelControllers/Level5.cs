using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level5 : MonoBehaviour
{
    public GameObject dronePrefab;
    public List<EnemyController> enemies;

    private int dronesInRoom;

    private List<Vector3> spawningPositions = new List<Vector3>
    {
        new Vector3(-9.5f, 45, 9),
        new Vector3(-9.5f, 45, -3),
    };

    public void LeverAction(int lever_id)
    {
        
    }
    
    public void TriggerAction(int trigger_id)
    {
        switch (trigger_id)
        {
            case 1:
                SetSecondEnemy();
                break;
            case 2:
                StartCoroutine(SpawnDrones());
                break;
            case 3:
                SetFifthEnemy();
                break;
        }
    }
    
    public void OnEnemyDeath(int enemy_id)
    {
        switch (enemy_id)
        {
            case 1:
                SetFirstEnemy();
                break;
            case -1:
                dronesInRoom -= 1;
                break;
        }
    }

    private void Start()
    {
        SetThirdEnemy();
        SetFourthEnemy();
    }

    private IEnumerator SpawnDrones()
    {
        float t = 6.5f;
        while (true)
        {
            while (Game.state != Game.State.Play || dronesInRoom > 0)
            {
                yield return null;
            }
            
            t -= TimeManager.deltaTime();
            if (t <= 0)
            {
                SpawnDrone(spawningPositions[0]);
                SpawnDrone(spawningPositions[1]);
                dronesInRoom = 2;
                t = 7.7f;
            }

            yield return null;
        }
    }

    private void SpawnDrone(Vector3 pos)
    {
        GameObject droneObject = Instantiate(dronePrefab);
        droneObject.transform.position = pos;
        EnemyController drone = droneObject.GetComponent<EnemyController>();
        drone.id = -1;
        drone.StartCoroutine(drone.ActionSequence(new List<IEnumerator>
        {
            drone.AllowShooting(false),
            drone.ChangeSpeed(6),
            drone.MoveTo(pos + new Vector3(0, -10, 0)),
            drone.ChangeSpeed(3.7f),
            drone.AllowShooting(true),
            drone.CycleMove(new List<Vector3>
            {
                pos + new Vector3(0, -20, 0),
                pos + new Vector3(0, -10, 0),
            }, 0.5f)
        }));
    }

    private void SetFirstEnemy()
    {
        EnemyController drone = enemies[0];
        Vector3 startPos = drone.transform.position;
        Vector3 newPos = startPos + new Vector3(0, 10, 0);
        drone.movementSpeed = 6f;
        drone.StartCoroutine(drone.MoveTo(newPos));
    }
    
    private void SetSecondEnemy()
    {
        EnemyController drone = enemies[1];
        Vector3 startPos = drone.transform.position;
        Vector3 newPos = startPos + new Vector3(7, 0, 0);
        drone.movementSpeed = 6f;
        drone.StartCoroutine(drone.MoveTo(newPos));
    }
    
    private void SetThirdEnemy()
    {
        EnemyController drone = enemies[2];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.MoveAroundPoint(startPos, 1.8f));
    }
    
    private void SetFourthEnemy()
    {
        EnemyController drone = enemies[3];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.MoveAroundPoint(startPos, 1.8f));
    }
    
    private void SetFifthEnemy()
    {
        EnemyController drone = enemies[4];
        Vector3 startPos = drone.transform.position;
        Vector3 newPos = startPos + new Vector3(0, -8, 0);
        drone.movementSpeed = 6f;
        drone.StartCoroutine(drone.MoveTo(newPos));
    }

}
