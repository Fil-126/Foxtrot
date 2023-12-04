using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 30.0f;
    public float timeToLive = 10f;
    public bool ignorePlayer;
    public bool ignoreEnemy;

    void Update()
    {
        if (Game.state != Game.State.Play)
            return;

        timeToLive -= TimeManager.deltaTime();
        if (timeToLive <= 0)
            Destroy(gameObject);

        // Need to change y-coordinate because of the rotation
        transform.Translate(0,speed * TimeManager.deltaTime(), 0);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
            return;
        
        if (other.CompareTag("Player"))
        {
            if (ignorePlayer)
                return;
            
            if (!other.GetComponent<PlayerCharacter>().Hurt())
                return;
        }

        if (other.CompareTag("Enemy"))
        {
            if (ignoreEnemy)
                return;
            
            other.GetComponent<EnemyController>().Die();
        }
        
        
        Destroy(gameObject);
    }
}
