using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Controller : MonoBehaviour 
{
    enum GameState
    {
        Start,
        Playing,
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
                    bullets = 3;
                    scorePanel.SetScore(score);
                    scorePanel.SetBullets(bullets);

                    HidePanels();

                    Invoke("AddDuck", 1);
                    //Note: this code is recommended
                    //Wait(1, AddDuck);
                    gameState = GameState.Playing;

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

                    if (hit)
                    {
                        var colliderHit = hitInfo.collider;
                        if (colliderHit.tag == "Duck")
                        {
                            score++;

                            bullets = 3;
                            scorePanel.SetBullets(bullets);
                            var duck = hitInfo.collider.GetComponent<Duck>();
                            duck.Hit();
                            scorePanel.SetScore(score);

                            if (score < totalDucksPerRound)
                                Invoke("AddDuck", 1);
                            else
                                Invoke("ShowYouWin", 1);
                        }
                    }

                    bullets--;
                    if (bullets <= 0)
                    {
                        //Invoke("ShowGameOver", 1);
                    }
                }

                break;
        }
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

        Invoke("ReadyToStart", 2);
    }

    private void ShowGameOver()
    {
        gameState = GameState.GameOver;

        // tell ducks to fly away

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

    private void OnGUI()
    {
        Debug.DrawLine(theCamera.transform.position, theCamera.transform.position + theCamera.transform.forward * 50);
    }
}
