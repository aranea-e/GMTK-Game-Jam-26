using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool[] Objectives;

    private float _transtimer;
    private float _endGameTimer;
    private bool _endingGame;

    public AlarmManager alarmManager;
    public BulletSpawner bulletSpawner;
    public Controller player;

    public Animator vanAnim;
    public Animator copsAnim;

    void Awake()
    {
        alarmManager = GameObject.FindGameObjectWithTag("AlarmManager").GetComponent<AlarmManager>();
        bulletSpawner = GameObject.FindGameObjectWithTag("BulletSpawner").GetComponent<BulletSpawner>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
    }

    void Start()
    {
        FindObjectOfType<audioManager>().play("music");

    }

    void Update()
    {
        if (_endingGame)
        {
            _endGameTimer -= Time.deltaTime;
            if (_endGameTimer <= 0.0f)
            {
                exitToMenu();
            }
        }
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

    public void endGame(int result)
    {
        if (_endingGame) return;
        FindObjectOfType<audioManager>().stop("music");
        FindObjectOfType<TransitionInfo>().playerResult = result;
        FindObjectOfType<TransitionInfo>().targetResult = objectivesCompleted() ? 1 : 0;
        _endGameTimer = 3.0f;
        _endingGame = true;

        switch (result)
        {
            case 0:
                player.GetComponentInChildren<SpriteRenderer>().enabled = false;
                FindObjectOfType<audioManager>().play("Exit");
                break;
            case 1:
                player.alive = false;
                player.render();
                FindObjectOfType<audioManager>().play("Hit");
                break;
            case 2:
                FindObjectOfType<audioManager>().play("Police");
                break;
        }

        player.rb.velocity = Vector3.zero;
        player.enabled = false;

        vanAnim.SetTrigger("Exit");
        if (result > 0)
        {
            copsAnim.SetTrigger("Exit");
        }
    }

    public void exitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
