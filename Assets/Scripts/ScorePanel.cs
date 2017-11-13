using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanel : MonoBehaviour {

    public Transform duckIconContainer;

    private void Start()
    {
        // Hide all ducks
        for (var i = 0; i < duckIconContainer.childCount; i++)
        {
            var duck = duckIconContainer.GetChild(i);
            duck.gameObject.SetActive(false);
        }
    }

    public void SetScore(int score)
    {
        if (score > duckIconContainer.childCount) score = duckIconContainer.childCount;

        for (var i = 0; i < duckIconContainer.childCount; i++)
        {
            var duck = duckIconContainer.GetChild(i);
            duck.gameObject.SetActive(i < score);
        }
    }
}
