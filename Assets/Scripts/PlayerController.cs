using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;

    bool hit;

    float currentTime;

    bool invincible;

    public GameObject fireShield;

    [SerializeField]
    AudioClip win, death, idestory, destory, bounce; // idestory = Invincible destory

    [SerializeField] GameUI sliderUI;
    public int currentObstacleNumber;
    public int totalObstacleNumber;

    public Image InvincibleSlider;
    public GameObject InvincibleOBJ;
    public GameObject gameOverUI;
    public GameObject finishUI;

    public enum PlayerState
    {
        Preper,
        Playing,
        Died,
        Finish
    }

    [HideInInspector]
    public PlayerState playerState = PlayerState.Preper;

    void Start()
    {
        totalObstacleNumber = FindObjectsOfType<ObstacleController>().Length;

        rb = GetComponent<Rigidbody>();
        fireShield.SetActive(false);
    }
    void Awake()
    {
        currentObstacleNumber = 0;
    }
    void Update()
    {
        if (playerState == PlayerState.Finish)
        {
            if (Input.GetMouseButton(0))
            {
                FindObjectOfType<LevelSpawner>().NextLevel();
            }
        }

        if (playerState == PlayerState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                hit = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                hit = false;
            }


            if (invincible)
            {
                currentTime -= Time.deltaTime * 0.35f;
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
                if (hit)
                {
                    currentTime += Time.deltaTime * 0.8f;
                }
                else
                {
                    currentTime -= Time.deltaTime * 0.5f;
                }
            }

            if (currentTime >= 0.15 || InvincibleSlider.color == Color.red)
            {
                InvincibleOBJ.SetActive(true);
            }
            else
            {
                InvincibleOBJ.SetActive(false);
            }

            if (currentTime >= 1)
            {
                currentTime = 1;
                invincible = true;
                InvincibleSlider.color = Color.red;
            }
            else if (currentTime <= 0)
            {
                currentTime = 0;
                invincible = false;
                InvincibleSlider.color = Color.white;
            }

            if (InvincibleOBJ.activeInHierarchy)
            {
                InvincibleSlider.fillAmount = currentTime / 1;
            }



        }

        /*if (playerState == PlayerState.Preper)
        {
            if (Input.GetMouseButton(0))
            {
                playerState = PlayerState.Playing;
            }
        }*/
       
    }

    public void ShatterObstacles()
    {
        if (invincible)
        {
            ScoreManager.Instance.AddScore(1);
        }
        else
        {
            ScoreManager.Instance.AddScore(2);
        }
    }
    public void SetPlayerStateFinish() 
    {
        playerState = PlayerState.Finish;
    }
    private void FixedUpdate()
    {
        if (playerState == PlayerState.Playing)
        {
            if (hit)
            {
                rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }
        }
 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hit)
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }
        else
        {

            if (invincible)
            {
                if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "plane")
                {
                    //Destroy(collision.transform.parent.gameObject);
                    collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles();
                    ShatterObstacles();
                    SoundManager.Instance.playSoundFX(idestory, 0.5f);
                    currentObstacleNumber++;
                }
                
            }
            else
            {
                if (collision.gameObject.tag == "enemy")
                {
                    //Destroy(collision.transform.parent.gameObject);
                    collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles();
                    ShatterObstacles();
                    SoundManager.Instance.playSoundFX(destory, 0.5f);
                    currentObstacleNumber++;
                }
                else if (collision.gameObject.tag == "plane")
                {
                    playerState = PlayerState.Died;
                    rb.isKinematic = true;
                    gameOverUI.SetActive(true);
                    Debug.Log("GAME OVER");
                    ScoreManager.Instance.ResetScore();
                    SoundManager.Instance.playSoundFX(death,0.5f);
                }
            }
        }

        sliderUI.LevelSliderFill(currentObstacleNumber / (float)totalObstacleNumber);

        if (collision.gameObject.tag == "Finish" && playerState == PlayerState.Playing)
        {
            sliderUI.UpdateLevelUI();
            playerState = PlayerState.Finish;
            SoundManager.Instance.playSoundFX(win, 0.5f);
            finishUI.SetActive(true);
            finishUI.transform.GetChild(0).GetComponent<Text>().text = "Level" + PlayerPrefs.GetInt(key: "Level", defaultValue: 1);
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if(!hit || collision.gameObject.tag == "Finish")
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            SoundManager.Instance.playSoundFX(bounce, 0.5f);
        }
    }
}
