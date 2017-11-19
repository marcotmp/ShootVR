using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckCreator : MonoBehaviour {

    public GameObject duckPrefab;
    public Transform topLeft;
    public Transform bottomRight;

	public void CreateDuck()
    {
        // calculate random positions
        var position = new Vector3();
        position.x = Random.Range(topLeft.position.x, bottomRight.position.x);
        position.y = bottomRight.position.y;
        position.z = transform.position.z;

        var duckInstance = Instantiate(duckPrefab, position, transform.rotation);
        duckInstance.transform.parent = transform;

        // make sure the instance is active
        duckInstance.SetActive(true);

        // set movement limits limits
        var duck = duckInstance.GetComponent<Duck>();
        duck.topLeft = topLeft;
        duck.bottomRight = bottomRight;
    }

    private void OnDrawGizmos()
    {
        var size = topLeft.position - bottomRight.position;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
