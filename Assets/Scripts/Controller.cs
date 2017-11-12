using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour 
{
    public Text textField;
    public GameObject target;
    public GameObject theCamera;

    private float rotX;
    private float rotY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");
            //textField.text = "mouseX " + mouseX + "\n";
            rotX += mouseX * 4;
            rotY += mouseY * 4;

            theCamera.transform.rotation = Quaternion.Euler(-rotY, rotX, 0f);
        }

        if (IsTriggered())
        {
            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;

            //target.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            var start = theCamera.transform.position;
            var end = start + theCamera.transform.forward * 50;
            var hitInfo = new RaycastHit();
            var hit = Physics.Linecast(start, end, out hitInfo);

            if (hit)
            {
                Debug.Log(hitInfo.collider.tag);
                if (hitInfo.collider.tag == "Duck")
                {
                    var duck = hitInfo.collider.GetComponent<Duck>();
                    duck.Hit();
                }
            }
            else
            {
                //textField.text += ("hit with nothing \n"); 
            }
        }
        else
        {
            //target.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    private void OnGUI()
    {
        Debug.DrawLine(theCamera.transform.position, theCamera.transform.position + theCamera.transform.forward * 50);
    }

    private bool IsTriggered()
    {
        var value = false;

        if (Input.GetMouseButton(0)) return true;

        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            textField.text += "Touch " + touch.phase + "\n";
            return true;
        }

        return value;
    }
}
