using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    bool carpma;
    
    float currentTÝme;

    bool invincible;

    public GameObject fireShield;
    [SerializeField]
    AudioClip win, death, idestroy, destroy, bounce;

    public int currentObstacleNumber;
    public int totalObstacleNumber;

    public Image ÝnvincibleSlider;
    public GameObject InvincibleOBJ;

    public GameObject gameOverUI;
    public GameObject finishUI;

    public enum PlayerState
    {
        Prepare,
        Playing,
        Died,
        Finish
    }
    [HideInInspector]
    public PlayerState playerstate=PlayerState.Prepare;


    void Start()
    {
        totalObstacleNumber = FindObjectsOfType<ObstacleController>().Length;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentObstacleNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerstate == PlayerState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                carpma = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                carpma = false;
            }


            if (invincible)
            {
                currentTÝme -= Time.deltaTime * .35f;
                if (!fireShield.activeInHierarchy)
                {
                    fireShield.SetActive(true);
                }

            }
            else
            {
                if (fireShield.activeInHierarchy)
                {
                    fireShield.SetActive(false);
                }

                if (carpma)
                {
                    currentTÝme += Time.deltaTime * 0.8f;
                }
                else
                {
                    currentTÝme -= Time.deltaTime * 0.5f;
                }
            }

            if(currentTÝme >= 0.15f || ÝnvincibleSlider.color==Color.red )
            {
                InvincibleOBJ.SetActive(true);
            }
            else
            {
                InvincibleOBJ.SetActive(false);
            }


            if (currentTÝme >= 1)
            {
                invincible = true;
                currentTÝme = 1;
                Debug.Log("Ýnvincible");
                ÝnvincibleSlider.color = Color.red;
            }
            else if (currentTÝme <= 0)
            {
                currentTÝme = 0;
                invincible = false;
                ÝnvincibleSlider.color = Color.white;
            }

            if (InvincibleOBJ.activeInHierarchy)
            {
                ÝnvincibleSlider.fillAmount = currentTÝme / 1;
            }

        }


        /*if (playerstate == PlayerState.Prepare)
        {
            if (Input.GetMouseButton(0))
            {
                playerstate = PlayerState.Playing;
            }
        }
        */
        if(playerstate == PlayerState.Finish)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FindObjectOfType<LevelSpawner>().NextLevel();
            }
        }


    }
 
    public void shatterObstacles()
    {

        if (invincible)
        {
            ScoreManager.instance.addScore(1);
        }
        else
        {
            ScoreManager.instance.addScore(2);
        }
        
    }



    public void FixedUpdate()
    {
        if(playerstate == PlayerState.Playing)
        {
            if (carpma)
            {
                rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }

        }


    }
    public void OnCollisionEnter(Collision collision)
    {
        if (!carpma)
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime*5, 0);
        }
        else
            {
            if (invincible)
            {
                if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "plane")
                {
                    //Destroy(collision.transform.parent.gameObject);
                    collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles();
                    shatterObstacles();
                    SoundManager.instance.playSoundFX(idestroy, 0.5f);
                    currentObstacleNumber++;

                }

            }
            else
            {
                if (collision.gameObject.tag == "enemy")
                {
                    //Destroy(collision.transform.parent.gameObject);
                    collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles();
                    shatterObstacles();
                    SoundManager.instance.playSoundFX(destroy, 0.5f);
                    currentObstacleNumber++;

                }
                else if (collision.gameObject.tag == "plane")
                {
                    Debug.Log("Game Over");
                    gameOverUI.SetActive(true);
                    playerstate = PlayerState.Finish;
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    ScoreManager.instance.ResetScore();
                    SoundManager.instance.playSoundFX(death, 0.5f);

                }
            }

            FindObjectOfType<GameUI>().LevelSliderFill(currentObstacleNumber / (float)totalObstacleNumber);

                if(collision.gameObject.tag=="Finish" && playerstate == PlayerState.Playing)
            {
                
                playerstate = PlayerState.Finish;
                SoundManager.instance.playSoundFX(win, 0.5f);
                finishUI.SetActive(true);
            }



        }
        
    }
    public void OnCollisionStay(Collision collision)
    {
        if (!carpma || collision.gameObject.tag=="Finish")
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            SoundManager.instance.playSoundFX(bounce, 0.5f);

        }
    }

}
