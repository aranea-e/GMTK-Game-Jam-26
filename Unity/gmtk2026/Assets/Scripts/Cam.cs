using UnityEngine;

public class Cam : MonoBehaviour {
    public int followType = 0;
    public Transform player;
    public float zoomMod;
    public float camDepth;

    void Update()
    {
        if (Input.GetMouseButton(1)) {
            followType = 1;
        } else {
            followType = 0;
        }
        follow();
    }

    void follow()
    {
        switch (followType)
        {
            case 0:
                transform.position = new Vector3(player.position.x, player.position.y, player.position.z - camDepth);
                break;
            case 1:
                float xMod = ((Input.mousePosition.x / Screen.width) - 0.5f) * zoomMod;
                float yMod = ((Input.mousePosition.y / Screen.height) - 0.5f) * zoomMod;
                transform.position = new Vector3(player.position.x + xMod, player.position.y + yMod, player.position.z - camDepth);
                break;
        }
    }
}
