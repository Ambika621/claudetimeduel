using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI loopCountText;
    public TextMeshProUGUI timerText;
    public Image timerFillBar;
    public Slider healthBar;
    
    [Header("Effects")]
    public GameObject rewindEffectPanel;
    
    private TimeLoopManager loopManager;
    
    void Start()
    {
        loopManager = FindObjectOfType<TimeLoopManager>();
        
        if (loopManager)
        {
            loopManager.onLoopStart.AddListener(UpdateLoopCount);
            loopManager.onTimeUpdate.AddListener(UpdateTimer);
            loopManager.onRewind.AddListener(ShowRewindEffect);
        }
    }
    
    void UpdateLoopCount(int count)
    {
        if (loopCountText)
        {
            loopCountText.text = $"LOOP {count}";
        }
    }
    
    void UpdateTimer(float normalizedTime)
    {
        if (timerFillBar)
        {
            timerFillBar.fillAmount = 1f - normalizedTime;
        }
        
        if (timerText && loopManager)
        {
            float remaining = loopManager.GetRemainingTime();
            timerText.text = $"{remaining:F1}s";
        }
    }
    
    void ShowRewindEffect()
    {
        if (rewindEffectPanel)
        {
            rewindEffectPanel.SetActive(true);
            Invoke("HideRewindEffect", 0.5f);
        }
    }
    
    void HideRewindEffect()
    {
        if (rewindEffectPanel)
        {
            rewindEffectPanel.SetActive(false);
        }
    }
    
    public void UpdateHealth(float normalizedHealth)
    {
        if (healthBar)
        {
            healthBar.value = normalizedHealth;
        }
    }
}

