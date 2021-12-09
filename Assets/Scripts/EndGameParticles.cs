using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameParticles : MonoBehaviour
{
    private void OnEnable()
    {
        TouchAndGo.EndGameParticles += End;
    }

    private void End()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        GameValues.IsPlayerWin = true;
        GameValues.IsGameOver = true;
    }
}