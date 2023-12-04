using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public List<Door> doors;
    public Transform knob;

    public float operatingSpeed = 2.1f;
    public int id;
    public bool active = true;

    private GameObject levelController;
    private bool canOperate;
    private bool on;

    private float onRotation = -90;
    private float offRotation = 38;
    
    private float relPosition;
    
    void Start()
    {
        levelController = GameObject.FindWithTag("GameController");
    }
    
    private void Update()
    {
        if (Game.state != Game.State.Play)
            return;
        
        if (active && canOperate && Input.GetKeyDown(KeyCode.E))
        {
            Switch();
            if (levelController != null)
            {
                levelController.SendMessage("LeverAction", id);
            }
            if (doors != null)
            {
                foreach (Door door in doors)
                {
                    door.Switch();
                }
            }
        }
    }
    
    public void Switch()
    {
        on = !on;
        if (on)
            StartCoroutine(TurnOn());
        else
            StartCoroutine(TurnOff());
    }

    private IEnumerator TurnOn()
    {
        while (on && relPosition < 1)
        {
            while (Game.state != Game.State.Play)
            {
                yield return null;
            }
            
            relPosition += operatingSpeed * TimeManager.deltaTime();
            if (relPosition > 1)
                relPosition = 1;
            
            Vector3 knobRotation = new Vector3(offRotation + (onRotation - offRotation) * relPosition, 0, 0);
            knob.transform.localRotation = Quaternion.Euler(knobRotation);

            yield return null;
        }
    }

    private IEnumerator TurnOff()
    {
        while (!on && relPosition > 0)
        {
            while (Game.state != Game.State.Play)
            {
                yield return null;
            }
            
            relPosition -= operatingSpeed * TimeManager.deltaTime();
            if (relPosition < 0)
                relPosition = 0;
            
            Vector3 knobRotation = new Vector3(offRotation + (onRotation - offRotation) * relPosition, 0, 0);
            knob.transform.localRotation = Quaternion.Euler(knobRotation);
            
            yield return null;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canOperate = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canOperate = false;
    }
}
