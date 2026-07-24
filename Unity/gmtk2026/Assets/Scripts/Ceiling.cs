using UnityEngine;
using UnityEngine.Tilemaps;

public class Ceiling : MonoBehaviour
{
    public Transform player;
    public Room room;
    public Tilemap tmp;
    public Cam cam;

    public float transparency;
    public float fadeRate;
    public bool hide;
    public float minTransparency;
    public bool AtWindow {get; set;}

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        room = GetComponentInParent<Room>();
        tmp = GetComponentInChildren<Tilemap>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Cam>();
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
        if (room.playerInside()) return true;
        if (Mathf.Abs(Util.mousePosition().y - transform.position.y) < room.roomSize.y * 0.5f &&
            Mathf.Abs(Util.mousePosition().x - transform.position.x) < room.roomSize.x * 0.5f &&
            player.position.z >= -0.90f &&
            cam.followType == 1 &&
            AtWindow) return true;
        return false;
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
