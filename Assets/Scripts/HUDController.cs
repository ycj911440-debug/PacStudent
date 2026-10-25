using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text countdownText;
    public TMP_Text gameTimerText;
    public GameObject blockingImage;
    public PacStudentController pacStudentController;
    public Collider2D pacStudentCollider;
    public AudioSource introMusic;
    public AudioSource normalMusic;
    private int score = 0;
    private float countdownTimer = 0f;
    private int countdownStage = 0;
    private float secondTimer = 0f;
    private int seconds = 0;
    private int minutes = 0;
    private int hours = 0;
    private bool gameStarted = false;

    void Start()
    {
        UpdateScore();
        UpdateGameTimer();
        blockingImage.SetActive(true);
        countdownText.gameObject.SetActive(true);
        countdownText.text = "3";
        pacStudentController.enabled = false;
        pacStudentCollider.enabled = false;
        normalMusic.Stop();
        introMusic.Play();
    }

    void Update()
    {
        if (gameStarted == false)
        {
            UpdateCountdown();
        }
        else
        {
            {
                secondTimer += Time.deltaTime;

                if (secondTimer >= 1f)
                {
                    secondTimer -= 1f;
                    seconds += 1;

                    if (seconds >= 60)
                    {
                        seconds = 0;
                        minutes += 1;
                    }

                    if (minutes >= 60)
                    {
                        minutes = 0;
                        hours += 1;
                    }
                }
                UpdateGameTimer();
            }
        }
    }

    void UpdateCountdown()
    {
        countdownTimer += Time.deltaTime;
        
        if (countdownTimer >= 1f)
        {
            countdownTimer = 0f;
            countdownStage += 1;

            if (countdownStage == 1)
            {
                countdownText.text = "2";
            }
            else if (countdownStage == 2)
            {
                countdownText.text = "1";
            }
            else if (countdownStage == 3)
            {
                countdownText.text = "GO!";
            }
            else if (countdownStage == 4)
            {
                StartGame();
            }
        }
    }

    void StartGame()
    {
        countdownText.gameObject.SetActive(false);
        blockingImage.SetActive(false);
        pacStudentController.enabled = true;
        pacStudentCollider.enabled = true;
        introMusic.Stop();
        normalMusic.loop = true;
        normalMusic.Play();
        gameStarted = true;
    }

    void UpdateGameTimer()
    {
        gameTimerText.text = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = score.ToString("D6");
    }
    
    public void ReturnToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
