using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Controller : MonoBehaviour 
{
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
    private bool isPlaying = false;

    void Start()
    {
        HidePanels();
        startPanel.SetActive(true);
    }

    public void StartGame()
    {
        score = 0;
        bullets = 3;
        scorePanel.SetScore(score);
        scorePanel.SetBullets(bullets);
        AddDuck();
    }

    // Update is called once per frame
    void Update () 
    {
        if (GameInput.IsTriggered())
        {
            if (!isPlaying)
            {
                HidePanels();
                isPlaying = true;
                Invoke("StartGame", 1);
            }
            else // is playing
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
                            EndGame();
                    }
                }

                //bullets--;
                if (bullets <= 0)
                {
                    Invoke("ShowGameOver", 1);
                }

                //if (bullets <= 0)
                //    ShowGameOver();
            }
        }
    }

    private void AddDuck()
    {
        // create a duck in a random place
        var index = Random.Range(0, creators.Length);
        creators[index].CreateDuck();
    }

    private void EndGame()
    {
        Invoke("ShowYouWin", 1);
    }

    private void ShowYouWin()
    {
        isPlaying = false;

        // play win music
        youWinPanel.SetActive(true);
        youWinPanelScore.text = "Score: " + score;
    }

    private void ShowStartBtn()
    {
        HidePanels();
        startPanel.SetActive(true);
    }

    private void ShowGameOver()
    {
        isPlaying = false;
        HidePanels();
        gameOverPanel.SetActive(true);
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
