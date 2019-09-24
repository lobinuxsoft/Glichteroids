using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameState gameState = GameState.MAIN;

    [SerializeField] Ship ship;
    [SerializeField] float restarShipTimer = 3;
    float lastRestar = 0;
    bool restarShip = false;

    [Tooltip("Initial amount of large asteroids on screen")]
    [SerializeField] int starAsteroidsAmount = 4;
    [SerializeField] Asteroid asteroidPref;
    [SerializeField] int totalAsteroidsInScene = 0;
    bool allAsteroidInScene = false;

    [SerializeField] int maxLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] int highscore = 0;

    int curLives = 0;

    [SerializeField] Vector2 minScreenBounce = Vector2.zero;
    [SerializeField] Vector2 maxScreenBounce = Vector2.zero;

    [SerializeField] UpdateScoreEvent onScoreUpdate;
    [SerializeField] UnityEvent onGameOver;
    void Start()
    {
        if (!instance)
        {
            instance = this;
        }

        ship = (Ship)FindObjectOfType<Ship>();

        CalculateScreenBounce();
    }

    private void Update()
    {
        if (Time.frameCount % 3 == 0)
        {
            switch (gameState)
            {
                case GameState.MAIN:

                    if (curLives != maxLives)
                    {
                        curLives = maxLives;
                    }

                    if (allAsteroidInScene == true)
                    {
                        allAsteroidInScene = false;
                    }

                    break;

                case GameState.GAMEPLAY:

                    CreateAsteroids();

                    break;
            }
        }

        if (restarShip)
        {
            if ((Time.time - lastRestar) > restarShipTimer)
            {
                ship.ResetShip();
                restarShip = false;
            }
        }
    }

    public void DamageShip()
    {
        if(maxLives > 0)
        {
            maxLives--;
            lastRestar = Time.time;
            restarShip = true;
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        SetGameState(GameState.GAMEOVER);
        onGameOver.Invoke();
    }

    public void DestroyAsteroid()
    {
        score += 10;

        if(score > highscore)
        {
            highscore = score;
        }
        totalAsteroidsInScene--;
        onScoreUpdate.Invoke(score, highscore);

        if(totalAsteroidsInScene <= 0)
        {
            //TODO WIN
        }
    }

    void CreateAsteroids()
    {
        if (allAsteroidInScene == false)
        {
            for (int i = 0; i < starAsteroidsAmount; i++)
            {
                Asteroid tempAsteroid = Instantiate(
                    asteroidPref,
                    new Vector3(Random.Range(minScreenBounce.x, maxScreenBounce.x), Random.Range(minScreenBounce.y, maxScreenBounce.y), 0f),
                    Quaternion.identity
                );

                Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                tempAsteroid.ForceToDirection(dir);
            }

            allAsteroidInScene = true;
            totalAsteroidsInScene = starAsteroidsAmount + (starAsteroidsAmount * 2) + (starAsteroidsAmount * 4);
        }
    }

    public void SetGameState(GameState state)
    {
        gameState = state;
    }

    void CalculateScreenBounce()
    {
        minScreenBounce = Camera.main.ScreenToWorldPoint(Vector3.zero);
        maxScreenBounce = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }
}

public enum GameState
{
    MAIN,
    GAMEPLAY,
    GAMEOVER
}

[System.Serializable]
public class UpdateScoreEvent : UnityEvent<int, int>{ }
