using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public float dreamSpawnInterval;
    float canDreamSpawn;
    public float dreamPosXmax = 2.7f;
    public float dreamPosYmax = 6f;
    public Transform dreamTarget;
    public GameObject dreamObject;
    public Player player;
    float currentDreamSpawnInterval;
    float currentDreamValue = 0.5f;
    public float dreamGain = 0.1f;
    int currentHours;
    int currentMinutes = 0;
    public float clockChangeInterval = 10;
    float canClockChange;
    public int startHour = 20;
    public int endHour = 8;
    public bool isPlaying;
    public float dreamSpawnIntervalChange = 0.5f;
    public float dreamSpeedChange = 0.5f;
    float currentDreamSpeed;
    public float startDreamSpeed = 2f;
    int nightsSurvived = 0;
    public List<GameObject> spawnedDreams = new List<GameObject>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying == false)
        {
            return;
        }

        canDreamSpawn -= Time.deltaTime;

        if (canDreamSpawn <= 0)
        {
            SpawnDream();
        }

        canClockChange -= Time.deltaTime;

        if (canClockChange <= 0)
        {
            HandleClockChange();
            canClockChange = clockChangeInterval;
            CheckIfNightOver();
        }
    }

    private void SpawnDream()
    {
        float dreamPosX = Random.Range(-dreamPosXmax, dreamPosXmax);
        Vector2 dreamPos = new Vector2(dreamPosX, dreamPosYmax);
        GameObject dreamObj = Instantiate(dreamObject, dreamPos, Quaternion.identity);
        dreamObj.GetComponent<Dream>().Init(dreamTarget, player, currentDreamSpeed);
        canDreamSpawn = currentDreamSpawnInterval;
        spawnedDreams.Add(dreamObj);
    }

    public void UpdateCurrentDreamValue(float value)
    {
        currentDreamValue += value;
        if(currentDreamValue > 1)
        {
            currentDreamValue = 1;
        }
        else if (currentDreamValue <= 0.01f)
        {
            currentDreamValue = 0;
            GameOver();
        }
        UIManager.instance.UpdateDreamStateImage(currentDreamValue);
    }

    void HandleClockChange()
    {
        currentMinutes += 15;
        if(currentMinutes >= 60)
        {
            currentHours++;
            currentMinutes = 0;
        }
        if (currentHours >= 24)
        {
            currentHours = 0;
        }
        UIManager.instance.UpdateClockText(currentHours, currentMinutes);
        
    }

    void CheckIfNightOver()
    {
        if (currentHours == endHour)
        {
            UIManager.instance.nightOverPanel.SetActive(true);
            isPlaying = false;
            nightsSurvived++;
        }
    }

    public void ResetNight()
    {
        currentDreamSpeed += dreamSpeedChange;
        currentDreamSpawnInterval -= dreamSpawnIntervalChange;
        if (currentDreamSpawnInterval <= 0.2f)
        {
            currentDreamSpawnInterval = 0.2f;
        }
        UIManager.instance.nightOverPanel.SetActive(false);
        SetNightStartValues();
    }

    void StartGame()
    {
        isPlaying = true;
    }

    void GameOver()
    {
        UIManager.instance.gameOverPanel.SetActive(true);
        isPlaying = false;
        UIManager.instance.nightsSurvivedText.text = "Nights slept: " + nightsSurvived.ToString();
    }

    public void ResetGame()
    {
        currentDreamSpawnInterval = dreamSpawnInterval;
        canDreamSpawn = currentDreamSpawnInterval;
        canClockChange = clockChangeInterval;
        currentDreamSpeed = startDreamSpeed;
        nightsSurvived = 0;
        SetNightStartValues();
    }

    void SetNightStartValues()
    {
        currentHours = startHour;
        currentMinutes = 0;
        HandleClockChange();
        StartGame();
        currentDreamValue = 0.5f;
        UIManager.instance.UpdateDreamStateImage(currentDreamValue);
        foreach (GameObject obj in spawnedDreams)
        {
            Destroy(obj);
        }
        spawnedDreams.Clear();
    }
}
