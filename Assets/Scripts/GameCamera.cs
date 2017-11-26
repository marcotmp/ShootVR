using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game camera.
/// This script controls the camera when the game is running on a computer with mouse and keyboard
/// </summary>
public class GameCamera : MonoBehaviour {

    public int lineLength = 50;

    private float rotX;
    private float rotY;
	
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

	void Update () {

        // unlock cursor
        if (Input.GetMouseButton(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        // if locked, move the camera
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");
            rotX += mouseX * 4;
            rotY += mouseY * 4;

            transform.rotation = Quaternion.Euler(-rotY, rotX, 0f);
        }
	}
    
    void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * lineLength);
    }
}
