using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSpawner : MonoBehaviour
{
    public GameObject[] obstacleModel;
    [HideInInspector]
    public GameObject[] obstaclePrefab = new GameObject[4];

    public GameObject winPrefab;

    private GameObject temp1Obtacle, temp2Obtacle;

    private int level = 1, addNumber = 7;

    float obstacleNumber;

    void Awake()
    {
        level = PlayerPrefs.GetInt("Level", 1);

        RandomObstacleGenerator();
        float randomNumber = Random.value;
        for (obstacleNumber = 0; obstacleNumber > -level - addNumber; obstacleNumber -= 0.5f)
        {

            if (level <= 20)
            {
                temp1Obtacle = Instantiate(obstaclePrefab[Random.Range(0, 2)]);
            }
            if (level > 20 && level < 50)
            {
                temp1Obtacle = Instantiate(obstaclePrefab[Random.Range(1, 3)]);
            }
            if (level >= 50 && level < 100)
            {
                temp1Obtacle = Instantiate(obstaclePrefab[Random.Range(2, 4)]);
            }
            if (level >= 100)
            {
                temp1Obtacle = Instantiate(obstaclePrefab[Random.Range(3, 4)]);
            }


            temp1Obtacle.transform.position = new Vector3(0, obstacleNumber - 0.01f, 0);
            temp1Obtacle.transform.eulerAngles = new Vector3(0, obstacleNumber * 8, 0);

            if (Mathf.Abs(obstacleNumber) >= level * 0.3f && Mathf.Abs(obstacleNumber) <= level * 0.06f)
            {
                temp1Obtacle.transform.eulerAngles = new Vector3(0, obstacleNumber * 8, 0);
                temp1Obtacle.transform.eulerAngles += Vector3.up * 180;
            }
            else if (Mathf.Abs(obstacleNumber) > level * 0.8f)
            {
                temp1Obtacle.transform.eulerAngles = new Vector3(0, obstacleNumber * 8, 0);
                if (randomNumber > 0.75f)
                {
                    temp1Obtacle.transform.eulerAngles += Vector3.up * 180;
                }

            }


            temp1Obtacle.transform.parent = FindObjectOfType<RotateManager>().transform;
        }

        temp2Obtacle = Instantiate(winPrefab);
        temp2Obtacle.transform.position = new Vector3(0, obstacleNumber - 0.01f, 0);
    }


    public void RandomObstacleGenerator()
    {
        int random = Random.Range(0,5);

        switch (random)
        {
            case 0:
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i];
                }
                break;
            case 1:
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i + 4];
                }
                break;
            case 2:
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i + 8];
                }
                break;
            case 3:
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i + 12];
                }
                break;
            case 4:
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i + 16];
                }
                break;
            default:
                break;
        }
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);

        Destroy(temp2Obtacle);
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadScene(0,LoadSceneMode.Additive);
    }

    public void RestartLevel() 
    {
        Destroy(temp2Obtacle);
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
    }
}
