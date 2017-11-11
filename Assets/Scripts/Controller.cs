using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
	public Text scoreText;
    public GameObject target;
    public GameObject theCamera;
	public GameObject explosionPrefab;
	public float raycastLenght = 90;

    private float rotX;
    private float rotY;
	private int score = 0;

    private void Start()
    {
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

		scoreText.text = "Score: 0";
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;

        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        rotX += mouseX*4;
        rotY += mouseY*4;

		if (Cursor.lockState == CursorLockMode.Locked)
	        theCamera.transform.rotation = Quaternion.Euler(-rotY, rotX, 0f);

        if (IsTriggered())
        {
            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;

            target.transform.localScale = new Vector3(0.11f, 0.11f, 0.11f);

            var start = theCamera.transform.position;
            var end = start + theCamera.transform.forward * raycastLenght;
            var hitInfo = new RaycastHit();
            var hit = Physics.Linecast(start, end, out hitInfo);

            if (hit)
            {
				var obj = hitInfo.collider.gameObject;
				print("Collide with" + obj.tag);
				if (obj.tag == "Ball")
				{
					var objTransform = obj.transform;
					var explosion = Instantiate(explosionPrefab, objTransform.position, objTransform.rotation);
					Destroy(obj);
					Destroy(explosion, 3);

					score++;

					scoreText.text = "Score: " + score;
				}
            }
        }
        else
        {
            target.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    private void OnGUI()
    {
        Debug.DrawLine(theCamera.transform.position, theCamera.transform.position + theCamera.transform.forward * 50);
    }

    private bool IsTriggered()
    {
        var value = false;

        if (Input.GetMouseButtonDown(0)) return true;

        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            return true;
        }

        return value;
    }
}
