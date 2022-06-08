using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public int score = 0;
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            score++;
            Debug.Log($"Score: {score}");
            Destroy(gameObject);
        }
    }
}
