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
    private AudioSource punch;
    private AudioSource pickup;
    private AudioSource[] audios;



    private float speed = 15.0f;
    private float rotateSpeed = 60.0f;
    private float jogTime = 0;
    private string currentlyFighting = "";
    private GameObject currentAlien;
    private bool canHit = false;
    private bool canAttack = true;
    private bool canMove = false;
    private bool level2 = true;
    private bool level3 = true;
    private float attackTime = 0;
    

    private Vector3 initialPos;
    private EveHealth healthInfo;
    private AlienHealth alienHealth;
    private GameObject ship;
    public Text gameText;
    private float gameTimer = 0;
    private float findShip = 0;





    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        healthInfo = GetComponent<EveHealth>();

        audios = GetComponents<AudioSource>();
        pickup = audios[0];
        punch = audios[1];
 

        ship = GameObject.Find("SpaceShip");
        

        initialPos = new Vector3(292, 0.46f, 76);
        transform.position = initialPos;
        gameText = GameObject.Find("GameText").GetComponent<Text>();


    }
    
    void Update()
    {
        gameTimer += Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate() {

        getDistanceFromShip();

        if(healthInfo.getEnergy() >= ENERGY_NEEDED)
        {
            findShip += Time.deltaTime;
            gameText.text = "This should be enough energy. I should find my ship.";
            if (findShip >= 3.0f)
            {
                gameText.text = "";
            }
        }
        

        if(!canMove)
        {
            if (gameTimer >= 1.5f && gameTimer < 4.0f)
            {
                gameText.text = "What is this place? Where is my ship?";
            }
            else if (gameTimer >= 4.0f && gameTimer < 7.0f)
            {
                gameText.text = "Eve this is your commander. You crash landed on this planet.";
                gameText.color = Color.cyan;
            }
            else if (gameTimer >= 7.0f && gameTimer < 11.0f)
            {
                gameText.text = "Analyzing the planet we've noticed there's a lot of energy.";
            }
            else if (gameTimer >= 11.0f && gameTimer < 15.0f)
            {
                gameText.text = "Collect enough energy and find your ship then you will be able to leave.";
            }
            else if (gameTimer >= 15.0f && gameTimer < 19.0f)
            {
                gameText.text = "Be careful of the enemies here. But you may be able to harness their energy as well.";
            }
            else if (gameTimer >= 19.0f && gameTimer < 21.0f)
            {
                gameText.text = "Good luck!";
            }
            else if (gameTimer >= 21.0f && gameTimer < 24.0f)
            {
                gameText.text = "Okay I need to collect enough energy to get out of here.";
                gameText.color = Color.white;
            }
            else if (gameTimer >= 24.0f)
            {
                gameText.text = "";
                canMove = true;
            }
        }
        
        if(isFighting())
        {
            getFightDistance();
            if(currentAlien != null && Input.GetKeyUp(KeyCode.A) && canHit)
            {
                if (healthInfo.getStamina() >= 1)
                {
                    healthInfo.hitEnemy();
                    Debug.Log("calling alien damage");
                    alienHealth.Damage();
                    canAttack = false;
                }
            }
        }

        if (!canAttack)
        {
            attackTime += Time.deltaTime;
        }

        if (attackTime >= 2.0f)
        {
            canAttack = true;
        }


        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                anim.SetTrigger("Punch");
                punch.Play();
                healthInfo.decreaseStamina();
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += transform.forward * Time.deltaTime * speed;
                anim.SetBool("isJogging", true);

            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
                anim.SetBool("isJogging", true);

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(-Vector3.up * rotateSpeed * Time.deltaTime);
                anim.SetBool("isJogging", true);

            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += -transform.forward * Time.deltaTime * speed;
                anim.SetBool("isJogging", true);
                
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                anim.SetBool("isJogging", false);
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                anim.SetBool("isJogging", false);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                anim.SetBool("isJogging", false);
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                anim.SetBool("isJogging", false);
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (anim.GetBool("isJogging"))
                {
                    anim.SetTrigger("jumpFromJog");
                }
                else
                {
                    anim.SetTrigger("jumpFromIdle");
                }
            }
        }
        

        if(healthInfo.getEnergy() >= 15 && healthInfo.getEnergy() < 30 && level2)
        {
            healthInfo.setLevel(2);
            level2 = false;
        } else if (healthInfo.getEnergy() >= 30 && level3)
        {
            healthInfo.setLevel(3);
            level3 = false;
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
            currentAlien = null;
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
            if(healthInfo.getEnergy() >= ENERGY_NEEDED)
            {
                
                gameText.text = "Press Enter to start ship.";
                Debug.Log(gameText.text);
                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    gameText.text = "YOU WIN!";
                }
            } else
            {
                gameText.text = "I need more energy.";
            }
        } else
        {
            gameText.text = "";
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name.Contains("Energy"))
        {
            Destroy(col.gameObject);
            healthInfo.addEnergy(1);
            pickup.Play();
            
            
        }
    }
}
