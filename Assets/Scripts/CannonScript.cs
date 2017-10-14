using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour {

	public GameObject ballPrefab;
	public Vector3 minForce;
	public float duration = 3f;

	// Use this for initialization
	void Start () {
		InvokeRepeating("Shoot", 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void Shoot()
	{
		var ballInstance = Instantiate(ballPrefab, transform.position, transform.rotation);
		var rigidBody = ballInstance.GetComponent<Rigidbody>();
		rigidBody.AddForce(new Vector3(Random.Range(-500, 500), Random.Range(1000, 1900), Random.Range(500, 1000)));

		Destroy(ballInstance, duration);
	}

}
