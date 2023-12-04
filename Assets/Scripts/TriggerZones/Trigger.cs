using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public int id;
    
    private GameObject levelController;
    
    void Start()
    {
        levelController = GameObject.FindWithTag("GameController");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelController.SendMessage("TriggerAction", id);
            Destroy(gameObject);
        }
    }
}
