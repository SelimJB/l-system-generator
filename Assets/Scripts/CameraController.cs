using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float distance = 200f;
    private float mooveSpeed = 200f;
    private float orbitalSpeed = 70f;
    private float zoomSpeed = 60f;
    private Vector3 center = Vector3.zero;
    public bool Orbit { get; set; } = true;
    public static CameraController Instance;

    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public void Reset()
    {
        Instance.transform.position = new Vector3(0, 0, -Instance.distance);
        Instance.transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
            Zoom(Input.mouseScrollDelta.y);
        if (Input.GetKey(KeyCode.KeypadPlus))
            Zoom(true);
        if (Input.GetKey(KeyCode.KeypadMinus))
            Zoom(false);
        if (Orbit)
            AutomaticOrbitalCam();
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
                OrbitalCam(Direction.Right);
            if (Input.GetKey(KeyCode.LeftArrow))
                OrbitalCam(Direction.Left);
            if (Input.GetKey(KeyCode.UpArrow))
                OrbitalCam(Direction.Up);
            if (Input.GetKey(KeyCode.DownArrow))
                OrbitalCam(Direction.Down);
        }
    }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OrbitalCam(Direction d)
    {
        var up = (transform.rotation * Vector3.up).normalized;
        var left = (transform.rotation * Vector3.left).normalized;
        float step = mooveSpeed * Time.deltaTime;
        float orbitCircumfrance = 2F * distance * Mathf.PI;
        float distanceDegrees = (mooveSpeed / orbitCircumfrance) * 360;
        float distanceRadians = (mooveSpeed / orbitCircumfrance) * 2 * Mathf.PI;
        if (d == Direction.Right)
        {
            transform.RotateAround(center, up, -distanceRadians);
        }
        else if (d == Direction.Left)
            transform.RotateAround(center, up, distanceRadians);
        else if (d == Direction.Up)
            transform.RotateAround(center, left, distanceRadians);
        else if (d == Direction.Down)
            transform.RotateAround(center, left, -distanceRadians);
    }

    private void AutomaticOrbitalCam()
    {
        var up = (transform.rotation * Vector3.up).normalized;
        float step = orbitalSpeed * Time.deltaTime;
        float orbitCircumfrance = 2F * distance * Mathf.PI;
        float distanceDegrees = (orbitalSpeed / orbitCircumfrance) * 360;
        float distanceRadians = (orbitalSpeed / orbitCircumfrance) * 2 * Mathf.PI;
        transform.RotateAround(center, up, -distanceRadians);
    }

    private void Zoom(float scrollDelta)
    {
        transform.Translate(0, 0, scrollDelta * zoomSpeed, Space.Self);
    }

    private void Zoom(bool zoomIn)
    {
        if (zoomIn)
            transform.Translate(0, 0, zoomSpeed * 10 * Time.deltaTime, Space.Self);
        else
            transform.Translate(0, 0, -zoomSpeed * 10 * Time.deltaTime, Space.Self);
    }
}