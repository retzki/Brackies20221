using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public Image dreamStateImage;
    public TMP_Text clockText;
    public GameObject nightOverPanel;
    public GameObject gameOverPanel;
    public TMP_Text nightsSurvivedText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        nightOverPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDreamStateImage(float value)
    {
        dreamStateImage.transform.localScale = new Vector2(value, dreamStateImage.transform.localScale.y);
        if (value > .5f)
        {
            dreamStateImage.color = new Color(2 - 2 * value, 1, 0);
        }
        else if (value <= .5f)
        {
            dreamStateImage.color = new Color(1, 2 * value, 0);
        }
    }

    public void UpdateClockText(int hours, int minutes)
    {
        string hoursString = hours.ToString();
        string minutesString = minutes.ToString();

        if (minutes == 0)
        {
            minutesString = "00";
        }

        if(hours < 10)
        {
            hoursString = "0" + hours.ToString();
        }
        if (hours==0)
        {
            hoursString = "00";
        }

        clockText.text = hoursString + ":" + minutesString;
    }

    public void ResetNight()
    {
        GameManager.instance.ResetNight();
    }

    public void ResetGame()
    {
        gameOverPanel.SetActive(false);
        GameManager.instance.ResetGame();
    }
}
