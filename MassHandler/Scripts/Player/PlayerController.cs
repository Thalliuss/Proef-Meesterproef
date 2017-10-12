using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("Camera Settings.")]
    private Camera _camera;
    [SerializeField, Range(0, 3)]
    private float _scrollSpeed;
    [SerializeField] private float _xSensitivity = 2f;
    [SerializeField] private float _ySensitivity = 2f;
    [SerializeField] private float _minimumY = -90F;
    [SerializeField] private float _maximumY = 90F;

    [SerializeField, Header("Movement Settings.")]
    private float _moveSpead = 15f;
    [SerializeField] private float _jumpForce = 220f;
    [SerializeField] private LayerMask _groundedMask;

    private bool _clampVerticalRotation = true;
    private bool _grounded = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update ()
    {
        Rigidbody t_rigidbody = GetComponent<Rigidbody>();

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (_grounded)
                t_rigidbody.AddForce(transform.up * _jumpForce);
        }

        _grounded = false;
        Ray t_ray = new Ray(transform.position, -transform.up);
        RaycastHit t_hit;
        if (Physics.Raycast(t_ray, out t_hit, 1.5f, _groundedMask))
            _grounded = true;

        if (!Cursor.visible) LookRotation(transform, _camera.transform);
        UpdateZoom();
    }

    private void FixedUpdate()
    {
        Rigidbody t_rigidbody = GetComponent<Rigidbody>();
        Vector3 t_moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKey(KeyCode.LeftShift)) t_rigidbody.MovePosition(t_rigidbody.position + transform.TransformDirection(t_moveDir) * _moveSpead * 2 * Time.deltaTime);
        else t_rigidbody.MovePosition(t_rigidbody.position + transform.TransformDirection(t_moveDir) * _moveSpead * Time.deltaTime);
    }

    public void LookRotation(Transform p_character, Transform p_camera)
    {
        Quaternion t_cameraRotation = _camera.transform.localRotation;

        float zRot = CrossPlatformInputManager.GetAxis("Mouse Y") * _ySensitivity;
        float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * _xSensitivity;

        t_cameraRotation *= Quaternion.Euler(-zRot, 0f, 0f);

        if (_clampVerticalRotation)
            t_cameraRotation = ClampRotationAroundXAxis(t_cameraRotation);

        p_character.Rotate(new Vector3(0, yRot, 0));
        p_camera.localRotation = t_cameraRotation;
    }

    Quaternion ClampRotationAroundXAxis(Quaternion p_q)
    {
        p_q.x /= p_q.w;
        p_q.y /= p_q.w;
        p_q.z /= p_q.w;
        p_q.w = 1.0f;

        float t_angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(p_q.x);

        t_angleX = Mathf.Clamp(t_angleX, _minimumY, _maximumY);

        p_q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * t_angleX);

        return p_q;
    }

    private void UpdateZoom()
    {
        Vector3 t_pos = _camera.transform.localPosition;
        float t_ypos = t_pos.y;

        t_ypos -= Input.mouseScrollDelta.y * _scrollSpeed;
        _camera.transform.localPosition = new Vector3(t_pos.x, Mathf.Clamp(t_ypos, 5, 100), t_pos.z);
    }
}
