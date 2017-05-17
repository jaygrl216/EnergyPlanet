using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AlienHealth : MonoBehaviour
{
    private int maxHealth;
    private int level;
    private int currentHealth;
    private bool isDead;
    private bool isHit;
    public Slider healthBar;
    public Animator anim;

    // level 1 max health = 8, level 2 max health = 15, level 3 max health = 25

    void Start()
    {
        level = 1;
        maxHealth = 8;
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        healthBar = GameObject.Find("AlienHealth").GetComponent<Slider>();
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
            //anim.SetBool("isDead", true);
        }

    }

    public void Damage()
    {
        int damage;

        switch (level)
        {
            case 1:
                damage = Random.Range(1, 4);
                break;
            case 2:
                damage = Random.Range(1, 6);
                break;
            case 3:
                damage = Random.Range(1, 11);
                break;
            default:
                damage = 1;
                break;
        }

        
        currentHealth -= damage;
        Debug.Log("Alien was hit with " + damage + "and current health is" + currentHealth);
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

    public void setLevel(int level)
    {
        this.level = level;


        if (level == 1)
        {
            maxHealth = 8;
        }
        else if (level == 2)
        {
            maxHealth = 15;
            //healthBar.maxValue = maxHealth;
            //healthBar.value = maxHealth;
        }
        else if (level == 3)
        {
            maxHealth = 25;
            //healthBar.maxValue = maxHealth;
            //healthBar.value = maxHealth;
        }
    }
}
