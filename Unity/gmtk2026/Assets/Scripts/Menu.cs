using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour {
    public TextMeshProUGUI lastRoundText;
    

    void Start()
    {
        setLastRoundText(FindObjectOfType<TransitionInfo>().result);
    }

    void Update()
    {
        
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    void setLastRoundText(string text)
    {
        lastRoundText.text = text;
    }
}
