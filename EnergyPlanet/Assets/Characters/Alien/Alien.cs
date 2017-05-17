using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour {

    GameObject eve;
    Animator anim;
    private float speed = 2.0f;
    private float rotateSpeed = 3.0f;
    private EveHealth eveHealth;
    private float attackTimer = 0;
    private bool canAttack = true;
    private double distance;
    private Vector3 initialPos;


    void Start () {
		eve = GameObject.Find("EvePrefab");
        anim = GetComponent<Animator>();
        eveHealth = eve.GetComponent<EveHealth>();
    }
	

	void Update () {
		
	}

    void FixedUpdate()
    {
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
            anim.SetBool("punch", false);
            anim.SetBool("isWalking", false);
        }
        else
        {
            if (canAttack)
            {
                anim.SetBool("punch", true);
                canAttack = false;

                eveHealth.Damage();


            }
            else
            {
                anim.SetBool("punch", false);
                attackTimer += Time.deltaTime;
                if (attackTimer >= 3.0f)
                {
                    canAttack = true;
                    attackTimer = 0;
                }
            }
        }
    }
}
