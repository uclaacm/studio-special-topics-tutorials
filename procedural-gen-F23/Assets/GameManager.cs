using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject restartText;
    int score;

    [SerializeField] float riseSpeed;
    [SerializeField] Transform cameraTransform, playerTransform;

    public static bool playerAlive;
    // Start is called before the first frame update
    void Start()
    {
        playerAlive = true;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerAlive)
        {
            cameraTransform.position += Vector3.up * riseSpeed;

            riseSpeed += .00002f;
        }
        else
        {
            restartText.SetActive(true);
        }

        if (playerTransform.position.y > score * 4)
        {
            score++;
            scoreText.text = "" + score;
        }
    }
}
