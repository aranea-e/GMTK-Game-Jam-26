using UnityEngine;
using UnityEngine.UI;

public class AlarmManager : MonoBehaviour
{
    public bool alarmActive {get; private set;}
    public float alarmTimer {get; private set;}
    public float alarmDuration;
    public int alarmStage {get; private set;} // 0=calm, 1=alert, 2=lockdown, 3=fail

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

    [ContextMenu("Advance Alarm Stage")]
    void advanceAlarmStage()
    {
        alarmStage++;
        alarmTimer = alarmDuration;
        if (alarmStage > 2)
        {
            Debug.Log("Game over.");
            stopAlarm();
        }
    }

    void updateUI()
    {
        alarmImage.enabled = alarmStage > 0;
        alarmProgressBar.enabled = alarmActive;

        int stageImage = Mathf.Min(2, alarmStage);
        alarmStageImage.sprite = alarmStageImages[stageImage];
        alarmProgressBar.color = alarmStageColors[stageImage];

        if (!alarmActive) return;
        float barWidth = 158 * (1.0f - alarmTimer / alarmDuration);
        float barPosX = 17 + (barWidth) / 2;
        alarmProgressBar.rectTransform.anchoredPosition = new Vector2(barPosX, alarmProgressBar.rectTransform.anchoredPosition.y);
        alarmProgressBar.rectTransform.sizeDelta = new Vector2(barWidth, alarmProgressBar.rectTransform.sizeDelta.y);
    }
}
