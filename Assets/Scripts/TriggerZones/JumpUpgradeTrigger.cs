using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUpgradeTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    private new Renderer renderer;
    private bool activated;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;
        
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.maxAdditionalJumps += 1;
            if (Game.soundsOn)
                audioSource.Play();
            activated = true;
            renderer.enabled = false;
            Destroy(gameObject, 1.5f);
        }
    }
}
