using UnityEngine;

public class TransitionInfo : MonoBehaviour {
    public string result;

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
