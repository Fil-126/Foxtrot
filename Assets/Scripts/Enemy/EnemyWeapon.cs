using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;

    private float delay;
    private Transform player;
    private EnemyController enemyController;
    private AudioSource audioSource;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        enemyController = GetComponentInParent<EnemyController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Game.state != Game.State.Play)
            return;
        
        delay -= TimeManager.deltaTime();
        
        if (enemyController.canShoot && delay <= 0 && PlayerVisible())
        {
            GameObject projectileObj = Instantiate(projectilePrefab);
            projectileObj.transform.position = transform.position;
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            projectile.ignoreEnemy = true;
            projectile.speed = enemyController.bulletSpeed;
            Vector3 dir = player.position - transform.position;
            projectileObj.transform.rotation = Quaternion.LookRotation(dir);
            projectileObj.transform.Rotate(90, 0, 0);
            delay = enemyController.shootDelay;
            if (Game.soundsOn)
                audioSource.Play();
        }
    }

    bool PlayerVisible()
    {
        Vector3 dir = player.position - transform.position;
        Ray ray = new Ray(transform.position, dir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Player"))
                return true;
        }

        return false;
    }
    
    
}
