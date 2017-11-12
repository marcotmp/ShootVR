using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour 
{
    public DuckCreator[] creators;
    public GameObject target;
    public GameObject theCamera;
    public ScorePanel scorePanel;
    public int hitDistance = 50;
    public int totalDucksPerRound = 10;
    public GameObject startBtn;

    private int score = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartGame()
    {
        score = 0;

        AddDuck();
    }

    // Update is called once per frame
    void Update () 
    {
        if (IsTriggered())
        {
            var start = theCamera.transform.position;
            var end = start + theCamera.transform.forward * hitDistance;
            var hitInfo = new RaycastHit();
            var hit = Physics.Linecast(start, end, out hitInfo);

            if (hit)
            {
                var colliderHit = hitInfo.collider;
                Debug.Log(colliderHit.tag);
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
                else if (colliderHit.tag == "StartDuck")
                {
                    colliderHit.gameObject.SetActive(false);

                    Invoke("StartGame", 1);
                }
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
        startBtn.SetActive(true);
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
