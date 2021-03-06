﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanel : MonoBehaviour {

    public Transform duckIconContainer;
    public Transform bulletsContainer;
    public Transform scopeBulletsContainer;

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
        SetBullets(bulletsContainer, bullets);
        SetBullets(scopeBulletsContainer, bullets);
    }

    private void SetBullets(Transform container, int bullets)
    {
        if (bullets > container.childCount) bullets = container.childCount;

        for (var i = 0; i < container.childCount; i++)
        {
            var bullet = container.GetChild(i);
            bullet.gameObject.SetActive(i < bullets);
        }
    }

}
