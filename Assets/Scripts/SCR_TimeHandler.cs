using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SCR_TimeHandler : MonoBehaviour
{
    public static SCR_TimeHandler Instance;
    private void Awake() { Instance = this; }

    bool hasStarted;

    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] int originalTimeInMinutes;
    [SerializeField] GameObject gameOverCanvas;

    float totalTimePassed = 0;

    Color currentColor = Color.white;

    void Update()
    {
        if (!hasStarted) return;

        totalTimePassed += Time.deltaTime;

        currentColor = Color.Lerp(Color.white, Color.red, totalTimePassed / (originalTimeInMinutes * 60));

        DisplayTime();
    }

    void DisplayTime()
    {
        int timeLeft = originalTimeInMinutes - Mathf.FloorToInt(totalTimePassed / 60F);
        if (timeLeft <= 0) GameOver();

        timeText.text = timeLeft.ToString(); 
        timeText.color = currentColor;  
    }

    void GameOver()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        hasStarted = true;
    }
}
