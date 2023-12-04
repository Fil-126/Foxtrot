using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public float movementSpeed = 1f;
    public bool canShoot = true;
    public float shootDelay = 0.1f;
    public float bulletSpeed = 60;
    public int id;

    private GameObject levelController;
    private Transform player;
    private Quaternion facing;

    void Start()
    {
        levelController = GameObject.FindWithTag("GameController");
        player = GameObject.FindWithTag("Player").transform;
        facing = new Quaternion(-0.5f, -0.5f, -0.5f, 0.5f);
    }


    void Update()
    {
        if (Game.state != Game.State.Play)
            return;

        LookAtPlayer();
    }

    public void Die()
    {
        canShoot = false;
        Game.enemiesKilled += 1;
        if (levelController != null)
        {
            levelController.SendMessage("OnEnemyDeath", id);
        }
        StopAllCoroutines();
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        Destroy(gameObject);
        yield return null;
    }

    void LookAtPlayer()
    {
        Vector3 dir = player.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(dir) * facing;
        transform.rotation = rot;
    }

    public IEnumerator MoveTo(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            while (Game.state != Game.State.Play)
            {
                yield return null;
            }
            
            Vector3 dir = destination - transform.position;
            dir = dir.normalized;
            Vector3 newPos = transform.position + dir * movementSpeed * TimeManager.deltaTime();
            transform.position = newPos;
            yield return null;
        }
    }

    public IEnumerator CycleMove(List<Vector3> points, float radius = 0)
    {
        int ind = 0;

        while (true)
        {
            Vector3 displacement = Random.insideUnitSphere * radius;
            yield return MoveTo(points[ind] + displacement);
            ind += 1;
            ind %= points.Count;
        }
    }
    
    public IEnumerator PathMove(List<Vector3> points)
    {
        foreach (Vector3 point in points)
        {
            yield return MoveTo(point);
        }
    }

    public IEnumerator ActionSequence(List<IEnumerator> actions)
    {
        foreach (IEnumerator action in actions)
        {
            yield return action;
        }
    }

    public IEnumerator AllowShooting(bool allow)
    {
        canShoot = allow;
        yield return null;
    }
    
    public IEnumerator ChangeSpeed(float speed)
    {
        movementSpeed = speed;
        yield return null;
    }
    
    public IEnumerator ChangeBulletSpeed(float speed)
    {
        bulletSpeed = speed;
        yield return null;
    }
    
    public IEnumerator ChangeShotingDelay(float delay)
    {
        shootDelay = delay;
        yield return null;
    }

    public IEnumerator MoveAroundPoint(Vector3 centerPoint, float radius)
    {
        Vector3 point = Random.insideUnitSphere * radius + centerPoint;
        while (true)
        {
            yield return MoveTo(point);
            point = Random.insideUnitSphere * radius + centerPoint;
        }
    }

    public IEnumerator MoveBetweenPoints(List<Vector3> points, float radius = 0)
    {
        Vector3 point = points[Random.Range(0, points.Count)];
        point += Random.insideUnitSphere * radius;
        while (true)
        {
            yield return MoveTo(point);
            point = points[Random.Range(0, points.Count)];
            point += Random.insideUnitSphere * radius;
        }
    }

}
