using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //RUNECAT!

    //Remember to disable event system in the RDG Main scene when playing in the Lobby

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
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject victoryMenu;
    [SerializeField] GameObject defeatMenu;
    [SerializeField] GameObject hintMenu;


    private bool gameStarted = false;
    private bool gamePaused = false;
    private bool gameEnded = false;
    private bool gameLost = false;

    public int runeNum = 15;

    //[SerializeField] Camera lobbyCamera;
    //[SerializeField] Camera RDGCamera;

    void Start() {

        //lobbyCamera.enabled = false;
        //RDGCamera.enabled = true;

        CanvasReset();

        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Canvas"));
        //DontDestroyOnLoad(GameObject.FindGameObjectWithTag("EventSystem"));

        timeRem = gameTime;
    }

    private void CanvasReset(){
        scoreCounter.text = "Runes: 0/" + runeNum;
        scoreCounter.color = Color.white;
        countdownTimer.text = "Time: " + gameTime;
        countdownTimer.color = Color.white;
        
        introMenu.SetActive(true);
        inGameMenu.SetActive(false);
        victoryMenu.SetActive(false);
        defeatMenu.SetActive(false);
        hintMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void ActivateRune(){
        score++;
        scoreCounter.text = "Runes: " + score + "/" + runeNum;
        if (runeNum == score){
            scoreCounter.color = Color.cyan;
        }
    }

    //timer function for countdown
    public void StartCountdown(){
        if (!gameStarted) {
            gameStarted = true;
            introMenu.SetActive(false);
            inGameMenu.SetActive(true);
            StartCoroutine(Countdown());
        }
    }

    private IEnumerator Countdown(){
        while (gameStarted && !gameEnded){
            yield return new WaitForSeconds(1f);
            if (!gamePaused) timeRem--;
            countdownTimer.text = "Time: " + timeRem;
            if (timeRem < 0.5*gameTime) countdownTimer.color = Color.red;
            //intro menu stuff
            if (timeRem < 0){
                gameEnded = true;
                Defeat();
            }
        }
    }

    public void PauseUnpause(){

        if (gameStarted && !gameEnded){

            gamePaused = !gamePaused;

            inGameMenu.SetActive(!gamePaused);
            pauseMenu.SetActive(gamePaused);
            hintMenu.SetActive(false);
            
            if (gamePaused){
                Time.timeScale = 0f;
            } else {
                Time.timeScale = 1f;
            }
        }
    }

    public void Defeat(){
        
        Time.timeScale = 0f;

        gameEnded = true;
        gameLost = true;
        inGameMenu.SetActive(false);
        defeatMenu.SetActive(true);
    }

    public void Victory(){

        if (score != runeNum){
            hintMenu.SetActive(true);
            StartCoroutine(hintMenuCountdown());
            return;
        }

        gameEnded = true;

        Time.timeScale = 0f;

        finalScore.text = "Score: " + timeRem;

        inGameMenu.SetActive(false);
        victoryMenu.SetActive(true);
        
        //UPLOAD SCORE TO LOBBY
    }

    private IEnumerator hintMenuCountdown(){
        yield return new WaitForSeconds(3f);
        hintMenu.SetActive(false);
    }

    public bool GameLost(){
        return gameLost;
    }

    public bool GameStarted(){
        return gameStarted;
    }

    public bool GameEnded(){
        return gameEnded;
    }

    private void DestroyEverything(){
        // Destroy(GameObject.FindGameObjectWithTag("Player"));
        // Destroy(GameObject.FindGameObjectWithTag("MainCamera"));
        // Destroy(GameObject.FindGameObjectWithTag("RoomController"));
        // Destroy(GameObject.FindGameObjectWithTag("GameManager"));
        gameStarted = false;
        gamePaused = false;
        gameEnded = false;
        gameLost = false;
    }

    public void Lobby(int lobbyID){
        Debug.Log("return to lobby");
        //upload score if not zero (if not defeated)
        //Time.timeScale = 1f;
        //DestroyEverything()
        //SceneManager.LoadScene(lobbyID);

        //REMEMBER TO SWITCH BACK TO THE LOBBY EVENT SYSTEM
    }

    public void Restart(){
        Time.timeScale = 1f;
        DestroyEverything();
        CanvasReset();
        gameEnded = false;
        gameStarted = false;
        gamePaused = false;
        gameLost = false;
        timeRem = gameTime;
        score = 0;
        SceneManager.LoadScene("Main");
    }

}
