using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckCreator : MonoBehaviour {

    public GameObject duckPrefab;
    public Transform topLeft;
    public Transform bottomRight;

	public void CreateDuck()
    {
        var duckInstance = Instantiate(duckPrefab, transform.position, transform.rotation);

        // make sure the instance is active
        duckInstance.SetActive(true);

        var duck = duckInstance.GetComponent<Duck>();
        duck.topLeft = topLeft.position;
        duck.bottomRight = bottomRight.position;
    }
}
