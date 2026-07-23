using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform player;
    public AlarmManager alarmManager;
    public bool alarmed;
    public Vector2 roomSize = new Vector2(8.0f, 8.0f);

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        alarmManager = GameObject.FindGameObjectWithTag("AlarmManager").GetComponent<AlarmManager>();
    }

    void Update()
    {
        playerInside();
        checkAlarm();
    }

    public bool playerInside()
    {
        return (Mathf.Abs(player.position.y - transform.position.y) < roomSize.y * 0.5f &&
            Mathf.Abs(player.position.x - transform.position.x) < roomSize.x * 0.5f &&
            player.position.z >= -0.90f);
    }

    void checkAlarm()
    {
        if (alarmed && playerInside() && !alarmManager.alarmActive)
        {
            alarmManager.startAlarm();
        }
    }
}
