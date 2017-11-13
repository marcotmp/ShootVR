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

    public int hitDistance = 50;
    public int totalDucksPerRound = 10;

    private int score = 0;
    private bool isPlaying = false;

    void Start()
    {
        HidePanels();
        startPanel.SetActive(true);
    }

    public void StartGame()
    {
        score = 0;
        scorePanel.SetScore(score);

        AddDuck();
    }

    // Update is called once per frame
    void Update () 
    {
        if (IsTriggered())
        {

            if (!isPlaying)
            {
                HidePanels();
                isPlaying = true;
                Invoke("StartGame", 1);
            }

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

                    var duck = hitInfo.collider.GetComponent<Duck>();
                    duck.Hit();
                    scorePanel.SetScore(score);

                    if (score < totalDucksPerRound)
                        Invoke("AddDuck", 1);
                    else
                        EndGame();
                }
            }

            //if (bullets <= 0)
            //    ShowGameOver();
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
    }

    private void ShowStartBtn()
    {
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

    private bool IsTriggered()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
            return true;
        else
            return false;
    }
}
