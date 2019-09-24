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

    public UpdateScoreEvent onScoreUpdate;
    public UpdateLivesEvent onUpdateLives;
    public UnityEvent onGameOver;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    void Start()
    {
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
                        onUpdateLives.Invoke(curLives);

                        //Restar ship only for see in main menu
                        ship.ResetShip();
                        restarShip = false;
                    }


                    if (allAsteroidInScene == true)
                    {
                        var tempAsteroids = FindObjectsOfType<Asteroid>();

                        if (tempAsteroids.Length > 0)
                        {
                            for (int i = 0; i < tempAsteroids.Length; i++)
                            {
                                Destroy(tempAsteroids[i].gameObject);
                            }
                        }

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
        if(curLives > 0)
        {
            curLives--;
            onUpdateLives.Invoke(curLives);
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

            //First time ship reset
            ship.ResetShip();
            restarShip = false;
        }
    }

    public void SetGameState(GameState state)
    {
        gameState = state;
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    void CalculateScreenBounce()
    {
        minScreenBounce = Camera.main.ScreenToWorldPoint(Vector3.zero);
        maxScreenBounce = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    public int GetMaxLives()
    {
        return maxLives;
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

[System.Serializable]
public class UpdateLivesEvent : UnityEvent<int> { }
