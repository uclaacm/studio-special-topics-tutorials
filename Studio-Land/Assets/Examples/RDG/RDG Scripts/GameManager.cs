using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{

    //RUNECAT!

    public static GameManager instance { get; private set; }

    private void Awake()
    {
        //instatiating singleton instance
        if (instance == null)
        {
            Debug.Log("game manager instance created");
            instance = this; //sets it to this instance if script is running for the first time
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); //keeps only the first instance
        }
    }

    private int score = 0;
    [SerializeField] int gameTime;
    private int timeRem;

    [SerializeField] TMP_Text scoreCounter;
    [SerializeField] TMP_Text countdownTimer;
    [SerializeField] TMP_Text finalScore;

    [SerializeField] GameObject introMenu;
    [SerializeField] GameObject inGameMenu;
    [SerializeField] GameObject victoryMenu;
    [SerializeField] GameObject defeatMenu;
    [SerializeField] GameObject hintMenu;

    private bool gameStarted = false;
    private bool gamePaused = false;
    private bool gameEnded = false;
    private bool gameLost = false;
    private bool gameWon = false;

    public int runeNum = 15;

    private StudioLand.MinigameController minigameController;
    [SerializeField] RenderPipelineAsset renderer2D;
    [SerializeField] RenderPipelineAsset renderer3D;

    void Start()
    {
        CanvasReset();
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Canvas"));

        timeRem = gameTime;

        minigameController = GameObject.Find("Minigame Controller").GetComponent<StudioLand.MinigameController>();
        minigameController.SetGameScore(timeRem);
        
        //Sets renderer settings to 2D
        GraphicsSettings.renderPipelineAsset = renderer2D;
        QualitySettings.SetQualityLevel(3,true);
        Debug.Log("renderer changed to 2d");

        //Resets camera
        Destroy(CameraController.instance.gameObject.GetComponent<UnityEngine.Video.VideoPlayer>());
        CameraController.instance.gameObject.GetComponent<AudioListener>().enabled = true;
    }

    private void CanvasReset()
    {
        scoreCounter.text = "Runes: 0/" + runeNum;
        scoreCounter.color = Color.white;
        countdownTimer.text = "Time: " + gameTime;
        countdownTimer.color = Color.white;
        
        introMenu.SetActive(true);
        inGameMenu.SetActive(false);
        victoryMenu.SetActive(false);
        defeatMenu.SetActive(false);
        hintMenu.SetActive(false);

        gameStarted = false;
        gamePaused = false;
        gameEnded = false;
        gameLost = false;
        gameWon = false;
    }

    public void ActivateRune()
    {
        score++;
        scoreCounter.text = "Runes: " + score + "/" + runeNum;
        if (runeNum == score){
            scoreCounter.color = Color.cyan;
        }
    }

    //timer function for countdown
    public void StartCountdown()
    {
        if (!gameStarted) {
            gameStarted = true;
            introMenu.SetActive(false);
            inGameMenu.SetActive(true);
            StartCoroutine(Countdown());
        }
    }

    private IEnumerator Countdown()
    {
        while (gameStarted && !gameEnded){
            yield return new WaitForSeconds(1f);
            if (!gamePaused) timeRem--;
            countdownTimer.text = "Time: " + timeRem;
            minigameController.SetGameScore(timeRem);
            if (timeRem < 0.5*gameTime) countdownTimer.color = Color.red;
            if (timeRem < 0){
                gameEnded = true;
                Defeat();
            }
        }
    }

    public void Defeat()
    {
        if (!gameWon) {
            Time.timeScale = 0f;

            gameEnded = true;
            gameLost = true;
            inGameMenu.SetActive(false);
            defeatMenu.SetActive(true);

            minigameController.SetGameScore(0);
            minigameController.EndGame();
            End();
        }
    }

    public void Victory()
    {
        if (score != runeNum){
            hintMenu.SetActive(true);
            StartCoroutine(hintMenuCountdown());
            return;
        }

        gameEnded = true;
        gameWon = true;

        Time.timeScale = 0f;

        finalScore.text = "Score: " + timeRem;

        inGameMenu.SetActive(false);
        victoryMenu.SetActive(true);
        
        minigameController.SetGameScore(timeRem);
        minigameController.EndGame();
        End();
    }

    public void omo()
    {
        gameWon = true;
        minigameController.SetGameScore(99999);
        minigameController.EndGame();
        End();
    }

    private IEnumerator hintMenuCountdown()
    {
        yield return new WaitForSeconds(3f);
        hintMenu.SetActive(false);
    }

    public bool GameLost()
    {
        return gameLost;
    }

    public bool GameStarted()
    {
        return gameStarted;
    }

    public bool GameEnded()
    {
        return gameEnded;
    }

    public void End()
    {
        Time.timeScale = 1f;
        
        //Sets rendere back to 3D
        GraphicsSettings.renderPipelineAsset = renderer3D;
        QualitySettings.SetQualityLevel(2,true);
        Debug.Log("renderer changed back to 3d");

        //Destroy game objects    
        Destroy(PlayerMovement.instance.gameObject);
        Destroy(GameManager.instance.gameObject);
        Destroy(RoomLoader.instance.gameObject);
        Destroy(GameObject.FindGameObjectWithTag("Canvas"));
        
        //Reset camera
        CameraController.instance.gameObject.GetComponent<AudioListener>().enabled = false;
    }






}
