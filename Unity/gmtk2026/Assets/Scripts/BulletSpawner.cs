using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public List<GameObject> bullets;
    public Transform player;

    void Awake()
    {
        bullets = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SpawnBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullets.Add(bullet);

        Vector3 position = player.position;
        position.z = Mathf.Ceil(position.z - 0.1f);
        bullet.transform.position = position;

        Vector2 direction = new Vector2(Util.mousePosition().x - player.position.x,
                                        Util.mousePosition().y - player.position.y);

        float angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
