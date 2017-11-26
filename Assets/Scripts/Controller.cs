using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour 
{
    enum GameState
    {
        Idle,
        Start,
        Playing,
        Scoring,
        GameOver,
        YouWin
    }

    public DuckCreator[] creators;
    public GameObject target;
    public GameObject theCamera;
    public ScorePanel scorePanel;
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject youWinPanel;
    public Text youWinPanelScore;

    public int hitDistance = 50;
    public int totalDucksPerRound = 10;

    public Sounds sounds;

    private int score = 0;
    private int bullets = 3;

    private GameState gameState;

    void Start()
    {
        ReadyToStart();
    }

    void Update () 
    {
        switch (gameState)
        {
            case GameState.Start:
                if (GameInput.IsTriggered())
                {
                    score = 0;
                    scorePanel.SetScore(score);
                    HidePanels();

                    sounds.PlayStartRound();

                    //Note: this code is not recommended because it uses reflection.
                    Invoke("StartRound", 1);

                    gameState = GameState.Idle;
                }

                break;

            case GameState.Playing:
                if (GameInput.IsTriggered())
                {
                    sounds.PlayShoot();

                    var start = theCamera.transform.position;
                    var end = start + theCamera.transform.forward * hitDistance;
                    var hitInfo = new RaycastHit();
                    var hit = Physics.Linecast(start, end, out hitInfo);

                    bullets--;
                    scorePanel.SetBullets(bullets);

                    if (hit)
                    {
                        var colliderHit = hitInfo.collider;
                        if (colliderHit.tag == "Duck")
                        {
                            score++;
                            var duck = colliderHit.GetComponent<Duck>();
                            duck.Hit();
                            sounds.StopFly();
                            //sounds.PlayFall();
                            scorePanel.SetScore(score);

                            sounds.PlayScoring();

                            if (score < totalDucksPerRound)
                            {
                                gameState = GameState.Scoring;
                                Invoke("StartRound", 1);    
                            }
                            else
                            {
                                gameState = GameState.YouWin;
                                Invoke("ShowYouWin", 1);
                            }
                        }
                    }
                    else if (bullets <= 0)
                    {
                        gameState = GameState.GameOver;

                        FlyAway();

                        sounds.StopFly();
                        sounds.PlayLose();

                        Invoke("ShowGameOver", 1);
                    }

                }

                break;
        }
    }

    private void StartRound()
    {
        // reset bullets
        bullets = 3;
        scorePanel.SetBullets(bullets);
        // add duck
        AddDuck();
        // set playing state
        gameState = GameState.Playing;

    }

    private void AddDuck()
    {
        // create a duck in a random place
        var index = Random.Range(0, creators.Length);
        creators[index].CreateDuck();

        sounds.PlayFly();
    }

    private void ShowYouWin()
    {
        gameState = GameState.YouWin;

        // play win music
        youWinPanel.SetActive(true);
        youWinPanelScore.text = "Score: " + score;

        sounds.PlayWin();

        FlyAway();

        Invoke("ReadyToStart", 2);
    }

    private void ShowGameOver()
    {
        gameState = GameState.GameOver;

        // cooldown for 3 seconds
        Invoke("ReadyToStart", 3);

        HidePanels();
        gameOverPanel.SetActive(true);
    }

    private void ReadyToStart()
    {
        sounds.PlayQuack();

        gameState = GameState.Start;
        HidePanels();
        startPanel.SetActive(true);
    }

    private void HidePanels()
    {
        youWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        startPanel.SetActive(false);
    }

    private void FlyAway()
    {
        // tell ducks to fly away
        for (var index = 0; index < creators.Length; index++)
        {
            var duckContainer = creators[index].transform;
            for (var i = 0; i < duckContainer.childCount; i++)
            {
                var duckInstance = duckContainer.GetChild(i);
                var duck = duckInstance.GetComponent<Duck>();
                if (duck != null)
                    duck.FlyAway();
            }
        }
    }
}
