using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour {
    public Image playerPlacard;
    public Image targetPlacard;

    public Sprite[] placardSprites;
    

    void Start()
    {
        playerPlacard.sprite = placardSprites[FindObjectOfType<TransitionInfo>().playerResult];
        targetPlacard.sprite = placardSprites[FindObjectOfType<TransitionInfo>().targetResult];
    }

    void Update()
    {
        
    }

    public void LoadScene(int sceneIndex)
    {
        FindObjectOfType<audioManager>().play("Select");
        SceneManager.LoadScene(sceneIndex);
    }
}
