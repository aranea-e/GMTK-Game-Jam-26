using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public bool[] Objectives;

    private float _transtimer;

    public AlarmManager alarmManager;
    public BulletSpawner bulletSpawner;

    void Awake()
    {
        alarmManager = GameObject.FindGameObjectWithTag("AlarmManager").GetComponent<AlarmManager>();
        bulletSpawner = GameObject.FindGameObjectWithTag("BulletSpawner").GetComponent<BulletSpawner>();
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

    public void endGame(string result)
    {
        FindObjectOfType<TransitionInfo>().result = result;
        SceneManager.LoadScene(0);
    }
}
