using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject livePref;
    [SerializeField] Transform liveContainer;
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        animator = GetComponent<Animator>();

        scoreText.text = "Score: 0";
        highScoreText.text = "Highscore: 0";

        for (int i = 0; i < gameManager.GetMaxLives(); i++)
        {
            GameObject live = Instantiate(livePref, liveContainer);
            live.SetActive(false);
        }

        gameManager.onScoreUpdate.AddListener(UpdateScore);
        gameManager.onUpdateLives.AddListener(UpdateLive);
        gameManager.onGameOver.AddListener(GameOver);
    }

    private void LateUpdate()
    {
        if(gameManager.GetGameState() == GameState.MAIN)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameStart();
            }
        }
    }

    void UpdateScore(int score, int highScore)
    {
        scoreText.text = string.Format("Score: {0:0}", score);
        highScoreText.text = string.Format("Highscore: {0:0}", highScore);
    }

    void UpdateLive(int curLive)
    {
        for (int i = 0; i < liveContainer.childCount; i++)
        {
            liveContainer.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < curLive; i++)
        {
            liveContainer.GetChild(i).gameObject.SetActive(true);
        }
    }

    void GameStart()
    {
        animator.SetTrigger("GameStart");
    }

    public void GameStartAnimationEvent()
    {
        gameManager.SetGameState(GameState.GAMEPLAY);
    }

    void GameOver()
    {
        animator.SetTrigger("GameOver");
    }

    public void GameOverAnimationEvent()
    {
        gameManager.SetGameState(GameState.MAIN);
    }
}
