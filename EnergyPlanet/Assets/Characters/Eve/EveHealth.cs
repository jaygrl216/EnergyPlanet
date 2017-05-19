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
    private int energy;
    private bool isDead;
    private bool isHit;
    public Slider healthBar;
    public Slider staminaBar;
    public Slider energyBar;
    public Text energyText;
    public Text staminaText;
    public Text healthText;
    public Animator anim;
    private float timer;
    private float timer2;
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

        energyBar = GameObject.Find("EnergyBar").GetComponent<Slider>();
        energyBar.maxValue = 50;
        energyBar.value = 0;

        energyText = GameObject.Find("EnergyText").GetComponent<Text>();
        energyText.text = "0";
        energy = 0;

        staminaText = GameObject.Find("StaminaText").GetComponent<Text>();
        staminaText.text = stamina.ToString();

        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        healthText.text = currentHealth.ToString();

    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;


        if (currentHealth <= 0)
        {
            isDead = true;
            anim.SetBool("isDead", true);
        }


        if(timer >= 7 && !nearEnemy)
        {
            if (currentHealth < maxHealth) {
                currentHealth++;
                healthBar.value = currentHealth;
                healthText.text = currentHealth.ToString();
            }
            timer = 0;
        }

        if (timer2 >= 7)
        {
            if (stamina < maxStamina)
            {
                stamina++;
                staminaBar.value = stamina;
                staminaText.text = stamina.ToString();
            }
            timer2 = 0;
        }


    }

    public void decreaseStamina()
    {
        stamina--;
        staminaBar.value--;
        staminaText.text = stamina.ToString();
    }

    public void addEnergy(int num)
    {
        energy += num;
        energyText.text = energy.ToString();
        energyBar.value++;
    }

    public int getEnergy()
    {
        return energy;
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
        anim.SetTrigger("Punch");
        stamina--;
        staminaBar.value--;
        staminaText.text = stamina.ToString();
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
        healthText.text = currentHealth.ToString();


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
            currentHealth = maxHealth;
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
            healthText.text = currentHealth.ToString();
        }
        else if (level == 3)
        {
            maxHealth = 40;
            currentHealth = maxHealth;
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
            healthText.text = currentHealth.ToString();
        }
    }
}
