using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Eve : MonoBehaviour {

    private static int ENERGY_NEEDED = 50;

    private Animator anim;
    private NavMeshAgent nav;
    private Rigidbody rb;
    public Slider energyBar;
    public Text energyText;

    private float speed = 15.0f;
    private float rotateSpeed = 50.0f;
    private float jogTime = 0;
    private int energy;
    private string currentlyFighting = "";
    private GameObject currentAlien;
    private bool canHit = false;
    

    private Vector3 initialPos;
    private EveHealth healthInfo;
    private AlienHealth alienHealth;
    private GameObject ship;
    

    


    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        healthInfo = GetComponent<EveHealth>();
 

        ship = GameObject.Find("SpaceShip");
        energyBar = GameObject.Find("EnergyBar").GetComponent<Slider>();
        energyBar.maxValue = 50;
        energyBar.value = 0;

        energyText = GameObject.Find("EnergyText").GetComponent<Text>();
        energyText.text = "0";

        initialPos = new Vector3(292, 0.46f, 76);
        transform.position = initialPos;
        energy = 0;

    }

    // Update is called once per frame
    void FixedUpdate() {
        getDistanceFromShip();
        
        if(isFighting())
        {
            getFightDistance();
            if(currentAlien != null && anim.GetBool("Punch") && canHit)
            {
                Debug.Log("caling alien damage");
                alienHealth.Damage();
            }
        }


        if (Input.GetKey(KeyCode.UpArrow)) {
            transform.position += transform.forward * Time.deltaTime * speed;
            anim.SetBool("isJogging", true);
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
            anim.SetBool("isJogging", true);
        } else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(-Vector3.up * rotateSpeed * Time.deltaTime);
            anim.SetBool("isJogging", true);
        } else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += -transform.forward * Time.deltaTime * speed;
            anim.SetBool("isJogging", true);
        }

        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            anim.SetBool("isJogging", false);
        } else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            anim.SetBool("isJogging", false);
        } else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim.SetBool("isJogging", false);
        } else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            anim.SetBool("isJogging", false);
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            if (healthInfo.getStamina() >= 1)
            {
                healthInfo.hitEnemy();
            }
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("Punch", false);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(anim.GetBool("isJogging"))
            {
                anim.SetTrigger("jumpFromJog");
            } else
            {
                anim.SetTrigger("jumpFromIdle");
            }
        }

        if(energy >= 15 && energy < 30)
        {
            healthInfo.setLevel(2);
        } else if (energy >= 30)
        {
            healthInfo.setLevel(3);
        }
    }

    bool isFighting()
    {
        if(!healthInfo.getFighting().Equals(""))
        {
            currentlyFighting = healthInfo.getFighting();
            currentAlien = GameObject.Find(currentlyFighting);
            alienHealth = currentAlien.GetComponent<AlienHealth>();
            
            healthInfo.inRange(true);
            return true;
        } else
        {
            currentlyFighting = healthInfo.getFighting();
            healthInfo.inRange(false);
            return false;
        }

    }

    void getFightDistance()
    {
        double distance = Vector3.Distance(transform.position, currentAlien.transform.position);
        if(distance > 8.0)
        {
            healthInfo.setFighting("");
            currentAlien = null;
            alienHealth = null;
        }

        if(distance <= 1.2)
        {
            canHit = true;
        } else
        {
            canHit = false;
        }
    }

    void getDistanceFromShip()
    {
        double distance = Vector3.Distance(transform.position, ship.transform.position);

        if(distance <= 7)
        {
            if(energy >= ENERGY_NEEDED)
            {
                Debug.Log("Press Enter to start ship");
            } else
            {
                Debug.Log("You don't have enough energy");
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name.Contains("Energy"))
        {
            Destroy(col.gameObject);
            energy++;
            energyText.text = energy.ToString();
            energyBar.value++;
        }
    }
}
