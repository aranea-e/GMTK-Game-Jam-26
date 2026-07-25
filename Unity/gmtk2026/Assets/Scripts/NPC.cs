using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    public float detectionSpeed = 1.0f;

    private SpriteRenderer _sprite;
    private Transform _spriteTransform;
    public Transform[] detectionBars;

    public string type;
    public Sprite[] sprites;
    public int tID;
    private bool _alive = true;

    GameManager gm;

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    private float _detection = 0.0f;

    public Vector2 targetPosition;
    public string aiState = "Idle";
    public float walkSpeed = 1.0f;
    public float investigateSpeed = 1.0f;
    public float runSpeed = 1.0f;
    [SerializeField] private float _alertProgress = 0.0f;
    [SerializeField] private float _fireDelay = 0.25f;
    public float fireRate = 0.5f;

    private float _swayTimer = 0.0f;
    public float swayRateIdle = 1.0f;
    public float swayAmountIdle = 5.0f;
    public float swayRateSearch = 1.0f;
    public float swayAmountSearch = 5.0f;
    private float _swayOffset = 0.0f;

    private float _wanderTimer = 0.0f;
    private float _wanderDelay = 1.0f;
    public int wanderTargetAttempts = 10;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();

        _sprite = GetComponentInChildren<SpriteRenderer>();
        _spriteTransform = _sprite.transform;

        visibleTargets = new List<Transform>();
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    void Start()
    {
        _swayTimer = Random.Range(0.0f, 2.0f * Mathf.PI);
        _wanderDelay = Random.Range(0.0f, 10.0f);
    }

    void LateUpdate()
    {
        if (!_alive)
        {
            aiState = "Dead";
        }
        else
        {
            FindVisibleTargets();
            DrawFieldOfView();
            DetectPlayer();
        }

        Move();
        Render();
    }

    void Move()
    {
        if (_alive)
        {
            switch (aiState)
            {
                case "Idle":
                    Sway(true, false, swayRateIdle, swayAmountIdle);
                    _wanderTimer += Time.deltaTime;
                    if (_wanderTimer >= _wanderDelay)
                    {
                        _wanderTimer = 0.0f;
                        targetPosition = FindWanderTarget();
                        _wanderDelay = Random.Range(4.0f, 6.0f);
                        aiState = "Wander";
                    }
                    break;
                case "Wander":
                    FaceTarget(targetPosition);
                    Sway(true, true, swayRateIdle, swayAmountIdle);
                    MoveTowardsTarget(targetPosition, walkSpeed);
                    _wanderTimer += Time.deltaTime;
                    if (Vector2.Distance(transform.position, targetPosition) < 0.1f || _wanderTimer >= _wanderDelay)
                    {
                        _wanderTimer = 0.0f;
                        _wanderDelay = Random.Range(8.0f, 12.0f);
                        aiState = "Idle";
                    }
                    break;
                case "Look":
                    FaceTarget(targetPosition);
                    Sway();
                    break;
                case "Investigate":
                    FaceTarget(targetPosition);
                    Sway();
                    MoveTowardsTarget(targetPosition, investigateSpeed);
                    break;
                case "Alert":
                    gm.alarmManager.startAlarm();
                    if (gm.alarmManager.alarmStage < 2)
                    {
                        _alertProgress += Time.deltaTime;
                        if (_alertProgress >= 1.0f)
                        {
                            gm.alarmManager.advanceAlarmTo(2);
                        }
                    }
                    else
                    {
                        if (visibleTargets.Count > 0)
                        {
                            _fireDelay += Time.deltaTime;
                            if (_fireDelay >= fireRate)
                            {
                                _fireDelay = 0.0f;
                                gm.bulletSpawner.SpawnBulletEnemy(transform, targetPosition);
                            }
                        }
                        else
                        {
                            aiState = "Search";
                            Move();
                            break;
                        }
                        FaceTarget(targetPosition);
                        Sway();
                    }
                    break;
                case "Search":
                    FaceTarget(targetPosition);
                    Sway(true, true, swayRateSearch, swayAmountSearch);

                    _fireDelay = 0.25f;
                    MoveTowardsTarget(targetPosition, runSpeed);
                    break;
                case "Dead":
                    Sway();
                    break;
            }
        }
    }

    void FaceTarget(Vector2 target)
    {
        Vector2 dirToTarget = (target - (Vector2)transform.position).normalized;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg - 90);
    }

    void MoveTowardsTarget(Vector2 target, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
    }

    void Sway(bool doSway = false, bool facingLocked = false, float swayRate = 1.0f, float swayAmount = 5.0f)
    {
        if (!doSway)
        {
            _swayTimer = 0.0f;
            _swayOffset = 0.0f;
            if (!facingLocked) transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - _swayOffset);
            return;
        }
        if (!facingLocked) transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - _swayOffset);
        _swayTimer += Time.deltaTime;
        float sway = Mathf.Sin(_swayTimer / swayRate) * swayAmount;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + sway);
        _swayOffset = sway;
    }

    Vector2 FindWanderTarget()
    {
        bool valid = false;
        int attempts = 0;
        Vector2 randomTarget = Vector2.zero;
        while (!valid)
        {
            randomTarget = new Vector2(transform.position.x + Random.Range(-4.0f, 4.0f) * Random.Range(-4.0f, 4.0f), transform.position.y + Random.Range(-4.0f, 4.0f) * Random.Range(-4.0f, 4.0f));

            Vector3 direction = randomTarget - (Vector2)transform.position;
            float distance = direction.magnitude;
            RaycastHit hit;
            valid = !Physics.Raycast(transform.position, direction, out hit, distance, obstacleMask);
            attempts++;
            if (attempts > wanderTargetAttempts)
            {
                return randomTarget;
            }
        }
        return randomTarget;
    }

    void Render()
    {
        if (_alive)
        {
            _sprite.sprite = sprites[0];
            detectionBars[0].localScale = new Vector3(Mathf.Clamp(_detection, 0, 1), 0.25f, 1);
            detectionBars[1].localScale = new Vector3(Mathf.Clamp(_detection - 1, 0, 1), 0.25f, 1);
            detectionBars[2].localScale = new Vector3(Mathf.Clamp(_detection - 2, 0, 1), 0.25f, 1);
            viewMeshFilter.gameObject.SetActive(true);
        }
        else
        {
            _sprite.sprite = sprites[1];
            detectionBars[0].localScale = new Vector3(0, 0.25f, 1);
            detectionBars[1].localScale = new Vector3(0, 0.25f, 1);
            detectionBars[2].localScale = new Vector3(0, 0.25f, 1);
            viewMeshFilter.gameObject.SetActive(false);
        }

        _spriteTransform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void DetectPlayer()
    {
        if (visibleTargets.Count > 0)
        {
            _detection += Time.deltaTime * detectionSpeed;
            if (_detection >= 1.0f)
            {
                targetPosition = visibleTargets[0].position;
                aiState = "Look";
            }
            if (_detection >= 2.0f)
            {
                aiState = "Investigate";
            }
            if (_detection >= 3.0f)
            {
                aiState = "Alert";
                _detection = 3.0f;
            }
        }
        else
        {
            if (_detection < 3.0f)
            {
                _detection -= Time.deltaTime;
                if (_detection < 2.0f && aiState == "Investigate")
                {
                    aiState = "Look";
                }
                if (_detection < 1.0f && (aiState == "Look" || aiState == "Investigate"))
                {
                    aiState = "Idle";
                }
                if (_detection <= 0.0f)
                {
                    _detection = 0.0f;
                }
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (!_alive) return;

        if (other.gameObject.CompareTag("Bullet") && other.gameObject.GetComponent<Bullet>().allegiance == "Player")
        {
            _alive = false;
            switch (type)
            {
                case "Target":
                    gm.completeObjective(tID);
                    gm.alarmManager.startAlarm();
                    break;
                case "Guard":
                    gm.alarmManager.startAlarm();
                    gm.alarmManager.advanceAlarmTimer(5.0f);
                    break;
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }



    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = -transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;
        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
