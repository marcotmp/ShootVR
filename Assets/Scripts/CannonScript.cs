using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour {

	public GameObject ballPrefab;
	public Vector3 minForce;
	public float duration = 3f;
	public Vector3 min = new Vector3(-300, 1000, 500);
	public Vector3 max = new Vector3(300, 1000, 700);
	public float minTime = 1;
	public float maxTime = 2;

	private float dt;
	private float time;

	// Use this for initialization
	void Start () {
		time = Random.Range(minTime, maxTime);
	}
	
	// Update is called once per frame
	void Update () {
		dt += Time.deltaTime;

		if (dt > time)
		{
			time = Random.Range(minTime, maxTime);

			dt = 0;
			Shoot();
		}
	}

	private void Shoot()
	{
		var ballInstance = Instantiate(ballPrefab, transform.position, transform.rotation);
		var rigidBody = ballInstance.GetComponent<Rigidbody>();
		rigidBody.AddForce(new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z)));

		Destroy(ballInstance, duration);
	}

}
