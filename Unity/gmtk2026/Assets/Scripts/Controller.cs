using UnityEngine;

public class Controller : MonoBehaviour {
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _friction;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _groundCheckDistance = 0.1f;

    public Rigidbody rb {get; set;}
    private Collider _collider;
    private BulletSpawner _bulletSpawner;

    GameManager gm;

    void Awake() {
        gm = FindObjectOfType<GameManager>();

        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _bulletSpawner = FindObjectOfType<BulletSpawner>();
    }

    void FixedUpdate() {
        friction();
        
    }

    void Update()
    {
        move(readMoveInput(), _moveSpeed);
        jump();
        fire();
    }

    Vector2 readMoveInput() {
        int x = 0;
        int y = 0;

        if (Input.GetKey(KeyCode.W)) {
            y = 1;
        } else if (Input.GetKey(KeyCode.S)) {
            y = -1;
        }

        if (y > 0 && wallInDirection(Vector3.up)) {
            y = 0;
        } else if (y < 0 && wallInDirection(Vector3.down)) {
            y = 0;
        }

        if (Input.GetKey(KeyCode.A)) {
            x = -1;
        } else if (Input.GetKey(KeyCode.D)) {
            x = 1;
        }

        if (x > 0 && wallInDirection(Vector3.right)) {
            x = 0;
        } else if (x < 0 && wallInDirection(Vector3.left)) {
            x = 0;
        }

        return new Vector2(x, y).normalized;
    }

    void friction() {
        Vector3 velocity = rb.velocity;

        if (velocity.x != 0) {
            velocity.x *= _friction;
            if (Mathf.Abs(velocity.x) < 0.1f) velocity.x = 0;
        }

        if (velocity.y != 0) {
            velocity.y *= _friction;
            if (Mathf.Abs(velocity.y) < 0.1f) velocity.y = 0;
        }

        rb.velocity = velocity;
    }

    void move(Vector2 direction, float speed) {
        Vector3 velocity = rb.velocity;

        if (direction.x != 0) velocity.x = direction.x * speed;
        if (direction.y != 0) velocity.y = direction.y * speed;

        rb.velocity = velocity;
    }

    void jump() {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded()) {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, _jumpHeight);
        }
    }

    void fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _bulletSpawner.SpawnBulletPlayer();
        }
    }

    bool isGrounded() {
        Bounds bounds = _collider.bounds;
        Vector3 center = bounds.center;
        Vector3 halfExtents = bounds.extents * 0.95f;

        return Physics.BoxCast(
            center,
            halfExtents,
            Vector3.forward,
            out _,
            transform.rotation,
            _groundCheckDistance,
            ~0,
            QueryTriggerInteraction.Ignore
        );
    }

    bool wallInDirection(Vector3 direction)
    {
        Bounds bounds = _collider.bounds;
        Vector3 directionNormalized = direction.normalized;
        Vector3 directionMask = new Vector3(
            Mathf.Abs(directionNormalized.x),
            Mathf.Abs(directionNormalized.y),
            Mathf.Abs(directionNormalized.z)
        );

        Vector3 halfExtents = bounds.extents * 0.95f;
        Vector3 surfaceOffset = Vector3.Scale(bounds.extents, directionMask);
        Vector3 center = bounds.center + new Vector3(
            surfaceOffset.x * directionNormalized.x,
            surfaceOffset.y * directionNormalized.y,
            surfaceOffset.z * directionNormalized.z
        ) + directionNormalized * _groundCheckDistance;

        Collider[] hits = Physics.OverlapBox(
            center,
            halfExtents,
            transform.rotation,
            ~0,
            QueryTriggerInteraction.Ignore
        );

        foreach (Collider hit in hits)
        {
            if (hit == _collider)
            {
                continue;
            }

            if (Physics.GetIgnoreLayerCollision(gameObject.layer, hit.gameObject.layer))
            {
                continue;
            }

            if (hit != _collider)
            {
                return true;
            }
        }

        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Van":
                if (gm.objectivesCompleted())
                {
                    gm.endGame("Target eliminated. You escaped successfully.");
                }
                break;
            case "Window":
                other.gameObject.GetComponent<Window>().EnterWindow();
                break;
            case "Bullet":
                if (other.gameObject.GetComponent<Bullet>().allegiance == "Enemy")
                {
                    gm.endGame("You were shot, dumbahh");
                }
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Window":
                other.gameObject.GetComponent<Window>().ExitWindow();
                break;
        }
    }
}
