using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public TMP_Text m_CounterText;
    public FishController m_FishController;

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Level"))
        {
            m_CounterText.text = $"Fish {m_FishController.score}/{m_FishController.totalScore}";
            if (m_FishController.score >= m_FishController.totalScore)
                CompletedFirstLevel();
        }
    }

    public void StartGame()
    {
        ChangeScene("Level1");
    }

    public void CompletedFirstLevel()
    {
        Cursor.lockState = CursorLockMode.None;
        ChangeScene("Ending", 2f);
    }

    public void ReturnToStarting()
    {
        ChangeScene("Starting");
    }

    public void ChangeScene(string scene, float delay = 0f)
    {
        StartCoroutine(_ChangeScene(scene, delay));
    }

    private IEnumerator _ChangeScene(string scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }
}