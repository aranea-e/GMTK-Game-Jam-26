using UnityEngine;

public class NPC : MonoBehaviour {
    private SpriteRenderer _sprite;

    public string type;
    public Sprite[] sprites;
    public int tID;
    private bool _alive;

    GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();

        _sprite = GetComponentInChildren<SpriteRenderer>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            _alive = false;
            _sprite.sprite = sprites[1];
            gm.completeObjective(tID);
        }
    }
}
