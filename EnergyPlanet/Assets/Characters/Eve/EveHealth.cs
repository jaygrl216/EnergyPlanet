using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EveHealth : MonoBehaviour {
    private int maxHealth;
    private int level;
    private int currentHealth;
    private int maxStamina;
    private int stamina;
    private bool isDead;
    private bool isHit;
    public Slider healthBar;
    public Slider staminaBar;
    public Animator anim;
    private float timer;
    private bool nearEnemy;
    private string enemy;

    // level 1 max health = 10, level 2 max health = 20, level 3 max health = 40

    void Start () {
        level = 1;
        maxHealth = 10;
        currentHealth = maxHealth;
        maxStamina = 20;
        stamina = maxStamina;
        nearEnemy = false;
        enemy = "";
        anim = GetComponent<Animator>();
        staminaBar = GameObject.Find("StaminaBar").GetComponent<Slider>();
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        
        if(currentHealth <= 0)
        {
            isDead = true;
            anim.SetBool("isDead", true);
        }


        if(timer >= 7 && !nearEnemy)
        {
            if (currentHealth < maxHealth) {
                currentHealth++;
                healthBar.value = currentHealth;
            }
            timer = 0;
        }
       
		
	}

    public void setFighting(string enemyName)
    {
        enemy = enemyName;
    }

    public string getFighting()
    {
        return enemy;
    }

    public void inRange(bool isInRange)
    {
        nearEnemy = isInRange;
    }

    public void hitEnemy()
    {
        anim.SetBool("Punch", true);
        stamina--;
        staminaBar.value--;
    }

    public void Damage()
    {
        int damage;

        switch (level)
        {
            case 1:
                damage = Random.Range(1, 3);
                break;
            case 2:
                damage = Random.Range(1, 5);
                break;
            case 3:
                damage = Random.Range(1, 9);
                break;
            default:
                damage = 1;
                break;
        }

        Debug.Log(damage);
        currentHealth -= damage;
        healthBar.value = currentHealth;


    }

    public bool Dead()
    {
        return isDead;
    }

    public bool Hit()
    {
        return isHit;
    }

    public int getStamina()
    {
        return stamina;
    }

    public void setLevel(int level)
    {
        this.level = level;


        if (level == 1)
        {
            maxHealth = 10;
        }
        else if (level == 2)
        {
            maxHealth = 20;
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }
        else if (level == 3)
        {
            maxHealth = 40;
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }
    }
}
