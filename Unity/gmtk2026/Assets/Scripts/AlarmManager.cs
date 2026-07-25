using UnityEngine;
using UnityEngine.UI;

public class AlarmManager : MonoBehaviour
{
    public bool alarmActive { get; private set; }
    public float alarmTimer { get; private set; }
    public float alarmDuration;
    public int alarmStage { get; private set; } // 0=calm, 1=alert, 2=lockdown, 3=fail

    public Image alarmImage;
    public Image alarmStageImage;
    public Image alarmProgressBar;
    public Sprite[] alarmStageImages;
    public Color[] alarmStageColors;

    void Update()
    {
        updateAlarm();
        updateUI();
    }

    void updateAlarm()
    {
        if (!alarmActive) return;

        alarmTimer -= Time.deltaTime;
        if (alarmTimer <= 0)
        {
            advanceAlarmStage();
        }
    }

    [ContextMenu("Start Alarm")]
    public void startAlarm()
    {
        if (alarmActive || alarmStage > 2) return;
        alarmActive = true;
        alarmTimer = alarmDuration;
    }

    public void stopAlarm()
    {
        alarmActive = false;
    }

    public void advanceAlarmTimer(float amount)
    {
        if (!alarmActive) return;
        alarmTimer -= amount;
        if (alarmTimer <= 0)
        {
            advanceAlarmStage();
        }
    }

    [ContextMenu("Advance Alarm Stage")]
    void advanceAlarmStage()
    {
        alarmStage++;
        alarmTimer += alarmDuration;
        if (alarmTimer <= 0)
        {
            advanceAlarmStage();
            return;
        }
        if (alarmStage > 2)
        {
            alarmTimer = 0;
            stopAlarm();

            FindObjectOfType<GameManager>().endGame("Police arrived. You failed to escape.");
        }
    }

    public void advanceAlarmTo(int stage)
    {
        if (stage <= alarmStage) return;
        alarmStage = stage;
        alarmTimer = alarmDuration;
        if (alarmStage > 2)
        {
            FindObjectOfType<GameManager>().endGame("Police arrived. You failed to escape.");
            stopAlarm();
        }
    }

    void updateUI()
    {
        alarmImage.enabled = alarmStage > 0;
        alarmProgressBar.enabled = alarmActive || alarmStage > 2;

        int stageImage = Mathf.Min(2, alarmStage);
        alarmStageImage.sprite = alarmStageImages[stageImage];
        alarmProgressBar.color = alarmStageColors[stageImage];

        if (!alarmActive) return;

        float percent = alarmTimer / alarmDuration;
        if (alarmStage > 2) percent = 1;

        float barWidth = 158 * (1.0f - percent);
        float barPosX = 17 + (barWidth) / 2;
        alarmProgressBar.rectTransform.anchoredPosition = new Vector2(barPosX, alarmProgressBar.rectTransform.anchoredPosition.y);
        alarmProgressBar.rectTransform.sizeDelta = new Vector2(barWidth, alarmProgressBar.rectTransform.sizeDelta.y);
    }
}
