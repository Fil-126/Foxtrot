using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private InGameMenu menu;
    
    void Start()
    {
        menu = GameObject.FindWithTag("Menu").GetComponent<InGameMenu>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Game.state = Game.State.Win;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            menu.winMenu.SetActive(true);
        }
    }
}