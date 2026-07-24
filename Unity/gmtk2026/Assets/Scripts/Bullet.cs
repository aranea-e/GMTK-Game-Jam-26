using UnityEngine;

public class Bullet : MonoBehaviour {
    Rigidbody rb;
    public float speed;
    [SerializeField] private float _groundCheckDistance = 0.1f;
    private Collider _collider;
    private SpriteRenderer _sprite;
    private Transform _spriteTransform;

    public float fallSpeed;
    public float heightDiff;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _spriteTransform = _sprite.transform;
    }

    void Start()
    {
        rb.velocity = speed * transform.right;
    }

    void Update()
    {
        fall();
        render();
    }

    void fall() {
        if (!isGrounded() && transform.position.z < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,
                                             transform.position.z + 1);
            heightDiff = 1;
        }

        if (heightDiff != 0)
        {
            heightDiff *= fallSpeed;
            if (heightDiff < 0.1f)
            {
                heightDiff = 0.1f;
            }
        }
    }

    void render()
    {
        if (transform.position.z == 0)
        {
            _sprite.sortingLayerID = SortingLayer.NameToID("Below Roof");
        }

        if (transform.position.z == -1)
        {
            _sprite.sortingLayerID = SortingLayer.NameToID("Above Roof");
        }

        _spriteTransform.localPosition = new Vector3(0, 0, -heightDiff);
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
}
