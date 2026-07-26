using UnityEngine;

public class TransitionInfo : MonoBehaviour {
    public int playerResult;
    public int targetResult;

    void Awake()
    {
        if (FindObjectsOfType<TransitionInfo>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
