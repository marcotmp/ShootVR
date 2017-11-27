using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game camera.
/// This script controls the camera when the game is running on a computer with mouse and keyboard
/// </summary>
public class GameCamera : MonoBehaviour {

    public int lineLength = 50;
    public float rotationSpeed = 4;
    private float rotX = 0;
    private float rotY = 0;
	
    void Start()
    {
        // lock cursor at the beginning
        Cursor.lockState = CursorLockMode.Locked;
    }

	void Update () {
        
        // lock on mouse click
        if (Input.GetMouseButton(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) // unlock cursor on press escape key
        {
            Cursor.lockState = CursorLockMode.None;
        }

        // if locked, move the camera
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");
            rotX += mouseX * rotationSpeed;
            rotY += mouseY * rotationSpeed;

            transform.rotation = Quaternion.Euler(-rotY, rotX, 0f);
        }
	}
    
    void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * lineLength);
    }
}
