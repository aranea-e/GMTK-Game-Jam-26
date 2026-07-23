using UnityEngine;

public class Controller : MonoBehaviour {
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _friction;
    [SerializeField] private float _jumpHeight;

    public Rigidbody rb {get; set;}

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        friction();
        
    }

    void Update()
    {
        move(readMoveInput(), _moveSpeed);
        jump();
    }

    Vector2 readMoveInput() {
        int x = 0;
        int y = 0;

        if (Input.GetKey(KeyCode.W)) {
            y = 1;
        } else if (Input.GetKey(KeyCode.S)) {
            y = -1;
        }

        if (Input.GetKey(KeyCode.A)) {
            x = -1;
        } else if (Input.GetKey(KeyCode.D)) {
            x = 1;
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
        if (Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, _jumpHeight);
        }
    }

}
