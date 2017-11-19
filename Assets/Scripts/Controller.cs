using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Controller : MonoBehaviour 
{
    enum GameState
    {
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

    private int score = 0;
    private int bullets = 3;

    private GameState gameState;

    void Start()
    {
        HidePanels();
        startPanel.SetActive(true);
    }

    // Update is called once per frame
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

                    //bullets = 3;
                    //scorePanel.SetBullets(bullets);

                    ////Note: this code is not recommended because it uses reflection.
                    //Invoke("AddDuck", 1);

                    //gameState = GameState.Playing;

                    //Note: this code is not recommended because it uses reflection.
                    Invoke("StartRound", 1);   

                    Debug.Log("GameState.Start");
                }

                break;

            case GameState.Playing:
                if (GameInput.IsTriggered())
                {
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
                            scorePanel.SetScore(score);

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
        AddDuck();
        gameState = GameState.Playing;
    }

    private void AddDuck()
    {
        // create a duck in a random place
        var index = Random.Range(0, creators.Length);
        creators[index].CreateDuck();
    }

    private void ShowYouWin()
    {
        gameState = GameState.YouWin;

        // play win music
        youWinPanel.SetActive(true);
        youWinPanelScore.text = "Score: " + score;

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

    void OnDrawGizmos()
    {
        Debug.DrawLine(theCamera.transform.position, theCamera.transform.position + theCamera.transform.forward * 50);
    }
}
