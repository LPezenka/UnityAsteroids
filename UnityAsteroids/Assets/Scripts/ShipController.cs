using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{

    public InputActionReference accelerate;   // Button
    public InputActionReference rotateCW;     // Button (clockwise)
    public InputActionReference rotateCCW;    // Button (counter-clockwise)
    public Camera camera;

    [Header("Tuning")]
    [Tooltip("Degrees per second")]
    public float rotationSpeedDeg = 180f;
    [Tooltip("Units per second^2")]
    public float acceleration = 6f;
    [Tooltip("Max linear speed (units/second)")]
    public float maxSpeed = 8f;
    [Tooltip("Simple linear damping (units/second)")]
    public float linearDrag = 1f;

    [Header("State (debug)")]
    public Vector2 heading = Vector2.up;  // forward direction in world space
    public Vector2 velocity;              // current velocity

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure the sprite visually points along the heading at start
        transform.rotation = Quaternion.LookRotation(Vector3.forward, heading);
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        // ----- Rotation via Quaternion around Z -----
        bool cw = rotateCW && rotateCW.action.IsPressed();
        bool ccw = rotateCCW && rotateCCW.action.IsPressed();
        float signed = (cw ? -1f : 0f) + (ccw ? 1f : 0f); // CW is negative Z in Unity 2D

        if (signed != 0f)
        {
            float deltaAngle = signed * rotationSpeedDeg * dt;
            Quaternion zRot = Quaternion.AngleAxis(deltaAngle, Vector3.forward);
            heading = (Vector2)(zRot * (Vector3)heading);
            heading.Normalize();

            // Keep sprite aligned with heading
            transform.rotation = Quaternion.LookRotation(Vector3.forward, heading);
        }

        // ----- Thrust / acceleration -----
        bool thrust = accelerate && accelerate.action.IsPressed();
        if (thrust)
        {
            velocity += heading * (acceleration * dt);
            float speed = velocity.magnitude;
            if (speed > maxSpeed)
                velocity = velocity.normalized * maxSpeed;
        }

        // ----- Linear drag (space-y damping) -----
        //if (velocity.sqrMagnitude > 0f)
        //{
        //    float speed = velocity.magnitude;
        //    speed = Mathf.Max(0f, speed - linearDrag * dt);
        //    velocity = (speed > 0f) ? velocity.normalized * speed : Vector2.zero;
        //}

        // ----- Integrate position (manual kinematics) -----
        transform.position += (Vector3)(velocity * dt);
        WrapToCameraBounds();
    }

    void WrapToCameraBounds()
    {
        var cam = camera;//Camera.main;
        var pos = transform.position;
        var z = pos.z;

        Vector3 min = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 max = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        if (pos.x < min.x) pos.x = max.x;
        else if (pos.x > max.x) pos.x = min.x;

        if (pos.y < min.y) pos.y = max.y;
        else if (pos.y > max.y) pos.y = min.y;

        pos.z = z;
        transform.position = pos;
    }

    void OnEnable()
    {
        if (accelerate) accelerate.action.Enable();
        if (rotateCW) rotateCW.action.Enable();
        if (rotateCCW) rotateCCW.action.Enable();
    }

    void OnDisable()
    {
        if (accelerate) accelerate.action.Disable();
        if (rotateCW) rotateCW.action.Disable();
        if (rotateCCW) rotateCCW.action.Disable();
    }

}
