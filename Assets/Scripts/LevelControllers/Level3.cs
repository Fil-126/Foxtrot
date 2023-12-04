using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level3 : MonoBehaviour
{
    public List<EnemyController> enemies;
    public Door door;

    private bool doorOpened;

    private void Start()
    {
        SetFirstEnemy();
        SetSecondEnemy();
        SetThirdEnemy();
        SetFourthEnemy();
        SetFifthEnemy();
        SetSixthEnemy();
        SetSeventhEnemy();
        SetEighthEnemy();
        SetNinethEnemy();
        SetTenthEnemy();
    }

    private void Update()
    {
        if (Game.enemiesKilled >= 10 && !doorOpened)
        {
            door.Switch();
            doorOpened = true;
        }
    }
    
    public void OnEnemyDeath(int enemy_id)
    {
        
    }
    
    public void LeverAction(int lever_id)
    {
        
    }

    private void SetFirstEnemy()
    {
        EnemyController drone = enemies[0];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.MoveBetweenPoints(new List<Vector3>
        {
            startPos,
            startPos + new Vector3(11, 0, 0),
            startPos + new Vector3(11, 13, 0),
            startPos + new Vector3(0, 13, 0),
            startPos + new Vector3(5.5f, 6.5f, 0),
        }, 1));
    }
    
    private void SetSecondEnemy()
    {
        EnemyController drone = enemies[1];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.ChangeSpeed(4.1f));
        drone.StartCoroutine(drone.MoveAroundPoint(startPos, 3.1f));
    }
    
    private void SetThirdEnemy()
    {
        EnemyController drone = enemies[2];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.ChangeSpeed(4.1f));
        drone.StartCoroutine(drone.CycleMove(new List<Vector3>
        {
            startPos,
            startPos + new Vector3(0, 10, 0),
        }, 4));
    }

    private void SetFourthEnemy()
    {
        EnemyController drone = enemies[3];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.ChangeSpeed(3.4f));
        drone.StartCoroutine(drone.MoveAroundPoint(startPos, 4f));
    }
    
    private void SetFifthEnemy()
    {
        EnemyController drone = enemies[4];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.ChangeSpeed(3.6f));
        drone.StartCoroutine(drone.MoveAroundPoint(startPos, 2.9f));
    }
    
    private void SetSixthEnemy()
    {
        EnemyController drone = enemies[5];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.ChangeSpeed(2.9f));
        drone.StartCoroutine(drone.MoveAroundPoint(startPos, 2.8f));
    }
    
    private void SetSeventhEnemy()
    {
        EnemyController drone = enemies[6];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.MoveBetweenPoints(new List<Vector3>
        {
            startPos,
            startPos + new Vector3(7, 0, 0),
            startPos + new Vector3(7, 0, -20),
            startPos + new Vector3(0, 0, -20),
        }, 1));
    }
    
    private void SetEighthEnemy()
    {
        EnemyController drone = enemies[7];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.ChangeSpeed(2.9f));
        drone.StartCoroutine(drone.CycleMove(new List<Vector3>
        {
            startPos,
            startPos + new Vector3(12, -8, 0),
            startPos + new Vector3(0, -16, 0),
        }, 4));
    }
    
    private void SetNinethEnemy()
    {
        EnemyController drone = enemies[8];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.ChangeSpeed(2.1f));
        drone.StartCoroutine(drone.MoveAroundPoint(startPos, 5f));
    }
    
    private void SetTenthEnemy()
    {
        EnemyController drone = enemies[9];
        Vector3 startPos = drone.transform.position;
        drone.StartCoroutine(drone.ChangeSpeed(2.3f));
        drone.StartCoroutine(drone.CycleMove(new List<Vector3>
        {
            startPos,
            startPos + new Vector3(7, 0, 0),
            startPos + new Vector3(7, 0, -10),
            startPos + new Vector3(7, 0, 0),
        }, 1));
    }
}
