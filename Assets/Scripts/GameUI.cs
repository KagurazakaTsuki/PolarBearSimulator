using TMPro;
using UnityEngine;


public class GameUI : MonoBehaviour
{
    public TMP_Text m_CounterText;
    public FishController m_FishController;
    
    private void Update()
    {
        m_CounterText.text = $"Fish {m_FishController.score}/{m_FishController.totalScore}";
    }
}