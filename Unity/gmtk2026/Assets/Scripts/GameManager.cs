using UnityEngine;

public class GameManager : MonoBehaviour {
    public bool[] Objectives;

    private float _transtimer;

    public AlarmManager alarmManager;

    void Awake()
    {
        alarmManager = GameObject.FindGameObjectWithTag("AlarmManager").GetComponent<AlarmManager>();
    }

    public bool objectivesCompleted()
    {
        foreach (bool o in Objectives)
        {
            if (!o) return false;
        }
        return true;
    }

    public void completeObjective(int i)
    {
        Objectives[i] = true;
    }
}
