using UnityEngine;

public class GameManager : MonoBehaviour {
    public bool[] Objectives;

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
