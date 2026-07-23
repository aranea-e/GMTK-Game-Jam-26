using UnityEngine;
using UnityEngine.Tilemaps;

public class Ceiling : MonoBehaviour
{
    public Transform player;
    public Room room;
    public Tilemap tmp;

    public float transparency;
    public float fadeRate;
    public bool hide;
    public float minTransparency;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        room = GetComponentInParent<Room>();
        tmp = GetComponentInChildren<Tilemap>();
    }

    void Start()
    {
        transparency = 1.0f;
        hide = false;
    }

    void Update()
    {
        hide = playerInside();
        updateTransparency();
    }

    bool playerInside()
    {
        return room.playerInside();
    }

    void updateTransparency()
    {
        if (hide)
        {
            transparency *= fadeRate;
            if (transparency < minTransparency)
            {
                transparency = minTransparency;
            }
        }
        else
        {
            float inv = 1.0f - transparency;
            inv *= fadeRate;
            transparency = 1.0f - inv;
            if (transparency > 0.9f)
            {
                transparency = 1.0f;
            }
        }
        tmp.color = new Color(1.0f, 1.0f, 1.0f, transparency);
    }
}
