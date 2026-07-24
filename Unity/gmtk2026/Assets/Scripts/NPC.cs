using UnityEngine;

public class NPC : MonoBehaviour {
    private SpriteRenderer _sprite;

    public string type;
    public Sprite[] sprites;
    public int tID;
    private bool _alive = true;

    GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();

        _sprite = GetComponentInChildren<SpriteRenderer>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (!_alive) return;

        if (other.gameObject.CompareTag("Bullet"))
        {
            _alive = false;
            _sprite.sprite = sprites[1];
            switch (type)
            {
                case "Target":
                    gm.completeObjective(tID);
                    gm.alarmManager.startAlarm();
                    break;
                case "Guard":
                    gm.alarmManager.startAlarm();
                    gm.alarmManager.advanceAlarmTimer(5.0f);
                    break;
            }
        }
    }
}
