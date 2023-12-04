using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level2 : MonoBehaviour
{
    public List<EnemyController> group1;
    public List<EnemyController> group2;

    private bool group1_released;

    private void Start()
    {
        foreach (EnemyController drone in group2)
        {
            drone.StartCoroutine(drone.ActionSequence(new List<IEnumerator>
            {
                drone.ChangeSpeed(2),
                drone.ChangeBulletSpeed(110),
                drone.ChangeShotingDelay(0.35f),
                drone.MoveAroundPoint(drone.transform.position, 3)
            }));
        }
    }

    public void LeverAction(int lever_id)
    {
        if (lever_id == 1 && !group1_released)
        {
            StartCoroutine(ActivateDrones());
            group1_released = true;
        }
    }

    public void OnEnemyDeath(int enemy_id)
    {
        
    }

    private IEnumerator ActivateDrones()
    {
        foreach (EnemyController drone in group1)
        {
            SendOneDrone(drone);
            float t = 0;
            while (t < 3)
            {
                if (Game.state != Game.State.Play)
                    yield return null;

                t += TimeManager.deltaTime();
                
                yield return null;
            }
        }
    }

    private void SendOneDrone(EnemyController drone)
    {
        Vector3 startPos = drone.transform.position;
        int sign = Random.value > 0.5f ? 1 : -1;
        List<Vector3> path = new List<Vector3>
        {
            new Vector3(6 * sign, 0, 0) + startPos,
            new Vector3(12 * sign, 5, 0) + startPos,
            new Vector3(9 * sign, 8, 0) + startPos,
        };
        
        List<Vector3> circularPath = new List<Vector3>
        {
            new Vector3(3 * sign, 12, 5) + startPos,
            new Vector3(2 * sign, 6, 0) + startPos,
            new Vector3(-5 * sign, 9, 3) + startPos,
            new Vector3(-8 * sign, 7, 2) + startPos,
            new Vector3(4 * sign, 11, 8) + startPos,
            new Vector3(9 * sign, 8, 0) + startPos,
        };

        drone.StartCoroutine(drone.ActionSequence(new List<IEnumerator>
        {
            drone.AllowShooting(false),
            drone.ChangeSpeed(5),
            drone.PathMove(path),
            drone.AllowShooting(true),
            drone.ChangeSpeed(3),
            drone.CycleMove(circularPath),
        }));
    }
}
