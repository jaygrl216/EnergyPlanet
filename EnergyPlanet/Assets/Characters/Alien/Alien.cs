using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour {

    GameObject eve;
    Animator anim;
    private float speed = 2.0f;
    private float rotateSpeed = 3.0f;
    private EveHealth eveHealth;
    private AlienHealth myHealth;
    private float attackTimer = 0;
    private bool canAttack = true;
    private double distance;
    private Vector3 initialPos;
    private bool deathCounter = false;
    private float deathTime = 0;
    private AudioSource punch;


    void Start () {
		eve = GameObject.Find("EvePrefab");
        anim = GetComponent<Animator>();
        eveHealth = eve.GetComponent<EveHealth>();
        myHealth = GetComponent<AlienHealth>();
        punch = GetComponent<AudioSource>();
    }
	

	void Update () {
		
	}

    void FixedUpdate()
    {
        if(myHealth.Dead())
        {
            deathCounter = true;
            eveHealth.setFighting("");
            
        }
        if (deathCounter)
        {
            deathTime += Time.deltaTime;
        }

        if (deathTime >= 4.0f)
        {
            eveHealth.addEnergy(2);
            Destroy(gameObject);
        }

        distance = Vector3.Distance(transform.position, eve.transform.position);
        if (distance <= 8.0f && distance >= 1.2f)
        {
            eveHealth.setFighting(gameObject.name);
            anim.SetBool("isWalking", true);
            Vector3 direction = (eve.transform.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
            transform.position += transform.forward * speed * Time.deltaTime;

        }

        if(distance > 8.0)
        {
            anim.SetBool("isWalking", false);
        }

        //Debug.Log(distance);

        if(distance <= 1.2f)
        {
            
            Attack();
        }

    }

    public void Attack()
    {
        if (eveHealth.Dead())
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            if (canAttack)
            {
                anim.SetTrigger("punch");
                punch.Play();
                canAttack = false;

                eveHealth.Damage();


            }
            else
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= 2.0f)
                {
                    canAttack = true;
                    attackTimer = 0;
                }
            }
        }
    }
}
