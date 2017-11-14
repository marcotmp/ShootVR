using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanel : MonoBehaviour {

    public Transform duckIconContainer;
    public Transform bulletsContainer;

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

    public void SetBullets(int bullets)
    {
        if (bullets > bulletsContainer.childCount) bullets = bulletsContainer.childCount;

        for (var i = 0; i < bulletsContainer.childCount; i++)
        {
            var bullet = bulletsContainer.GetChild(i);
            bullet.gameObject.SetActive(i < bullets);
        }
    }
}
