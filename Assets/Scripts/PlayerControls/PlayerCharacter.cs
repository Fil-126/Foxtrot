using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public int hp = 30;
    public int max_hp = 30;

    private InGameMenu menu;
    private PlayerMovement playerMovement;

    void Start()
    {
        menu = GameObject.FindWithTag("Menu").GetComponent<InGameMenu>();
        playerMovement = GetComponent<PlayerMovement>();
        Game.enemiesKilled = 0;
    }
    
    public bool Hurt()
    {
        // Returns true if player was hurt
        
        // Player is invulnerable during stunts
        if (playerMovement.movementType != PlayerMovement.MovementType.Normal)
            return false;
        
        // Player is invulnerable after jumping
        if (playerMovement.invulnerabilityTime > 0)
            return false;
        
        hp -= 1;
        menu.hp.value = (float) hp / max_hp;
        if (hp <= 0)
            Die();

        return true;
    }

    public void Die()
    {
        Game.state = Game.State.Death;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        menu.deathMenu.SetActive(true);
    }
}
