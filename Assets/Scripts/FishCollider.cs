using System;
using UnityEngine;

public class FishCollider:MonoBehaviour
{
    public FishController m_FishController;

    private void Awake()
    {
        m_FishController.totalScore++;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            m_FishController.score++;
            Debug.Log($"Score: {m_FishController.score}");
            Destroy(gameObject);
        }
    }
}