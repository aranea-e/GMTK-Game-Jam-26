using UnityEngine;

public class Window : MonoBehaviour {
    public Ceiling ceiling;
    private int _enterCount;

    void Awake() {
        ceiling = GetComponentInParent<Ceiling>();
    }

    public void EnterWindow()
    {
        _enterCount++;
        if (_enterCount >= 1) ceiling.AtWindow = true;
    }

    public void ExitWindow()
    {
        _enterCount--;
        if (_enterCount < 1) ceiling.AtWindow = false;
    }
}
