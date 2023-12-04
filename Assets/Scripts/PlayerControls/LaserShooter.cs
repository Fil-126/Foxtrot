using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    public float shootDelay = 0.1f;
    private float delay;

    private new Camera camera;
    private AudioSource audioSource;

    void Start()
    {
        camera = Camera.main;
        audioSource = transform.Find("ShotAudio").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Game.state != Game.State.Play)
            return;

        delay -= TimeManager.deltaTime();
        
        if (delay <= 0 && Input.GetMouseButtonDown(0))
        {
            GameObject projectileObj = Instantiate(projectilePrefab);
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            projectile.ignorePlayer = true;
            projectileObj.transform.position = camera.transform.position + camera.transform.forward;
            projectileObj.transform.Rotate(0, 0, -transform.eulerAngles.y);
            projectileObj.transform.Rotate(camera.transform.localEulerAngles.x, 0, 0);
            delay = shootDelay;
            if (Game.soundsOn)
                audioSource.Play();
        }

        
    }
}
