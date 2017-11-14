using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckCreator : MonoBehaviour {

    public GameObject duckPrefab;
    public Transform topLeft;
    public Transform bottomRight;
    public Transform duckContainer;

	public void CreateDuck()
    {
        var duckInstance = Instantiate(duckPrefab, transform.position, transform.rotation);
        duckInstance.transform.parent = duckContainer;

        // make sure the instance is active
        duckInstance.SetActive(true);

        var duck = duckInstance.GetComponent<Duck>();
        duck.topLeft = topLeft.position;
        duck.bottomRight = bottomRight.position;
    }

    private void OnDrawGizmos()
    {
        var size = topLeft.position - bottomRight.position;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
