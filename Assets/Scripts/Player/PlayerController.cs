using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Components")]
    [SerializeField] CharacterController characterController;
    [SerializeField] CinemachineCamera cinemachineCamera;
    [SerializeField] CinemachineBasicMultiChannelPerlin cinemachinePerlin;

    [Header("Camera configs")]
    [SerializeField] float walkFOV = 60f;
    [SerializeField] float sprintFOV = 75f;
    private float targetFOV => sprintInput && playerSpeed >= walkSpeed ? sprintFOV : walkFOV;
    private float idleFrequencyGain => 1f;
    private float walkFrequencyGain => walkSpeed;
    private float sprintFrequencyGain => sprintSpeed;
    private float targetFrequencyGain => sprintInput ? sprintFrequencyGain : walkFrequencyGain;

    [Header("Look configs")]
    [SerializeField] float mouseSensivity = 2f;
    [SerializeField] float clampValue = 85f;
    private float cameraPitch = 0f;

    [Header("Movement configs")]
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 7f;
    [SerializeField] float smoothTime = 5f;
    [SerializeField] float gravityScale = 3f;
    private float targetSpeed => sprintInput ? sprintSpeed : walkSpeed;
    private float currentSpeed;
    private float playerSpeed;
    private bool isGrounded => characterController.isGrounded;
    private float verticalVelocity = 0;
    private Vector3 previousPosition;

    [Header("Inputs")]
    public Vector2 moveInput;
    public Vector2 lookInput;
    public bool sprintInput;

    [Header("UI")]
    [SerializeField] TMP_Text speedText;
    #endregion

    #region Unity Methods
    void Start()
    {
        previousPosition = transform.position;
        currentSpeed = walkSpeed; // Initialize current speed to walk speed

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (UIManager.Instance.isAnyPanelOpen)
        {
            // If any UI panel is open, disable player controls
            moveInput = new Vector3(0,0, verticalVelocity); // Reset everyting except gravity (no idea why its works in z instead of y)
            lookInput = Vector2.zero;
            sprintInput = false;
        }
        MovementUpdate();
        LookUpdate();
        AdjustCameraSettings();
    }
    #endregion

    #region Methods
    private void LookUpdate()
    {
        // Horizontal rotation (yaw) - rotate the player
        transform.Rotate(Vector3.up * lookInput.x * mouseSensivity);

        // Vertical rotation (pitch) - rotate the camera
        cameraPitch -= lookInput.y * mouseSensivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -clampValue, clampValue); // Clamp to avoid flipping

        // Assuming your camera is a child of this GameObject
        if (cinemachineCamera != null)
        {
            cinemachineCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
        }


    }

    void MovementUpdate()
    {
        // Transition for the Current Speed
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * smoothTime);
        // normalize the input vector to ensure consistent movement speed
        moveInput = moveInput.normalized;

        // Disable bobbing depending on grounded state
        if (isGrounded)
        {
            cinemachinePerlin.enabled = true;
        }
        else
        {
            cinemachinePerlin.enabled = false;

        }

        // Handle gravity
        if (isGrounded && verticalVelocity <= 0.1f)
        {
            verticalVelocity = -3f;
        }
        verticalVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;

        Vector3 movementRaw = new Vector3(moveInput.x * currentSpeed, verticalVelocity, moveInput.y * currentSpeed);
        Vector3 movement = transform.TransformDirection(movementRaw) * Time.deltaTime;

        characterController.Move(movement);

        UpdateUI();
    }
    public void TryJump()
    {
        if (isGrounded == false) return;

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y * gravityScale);
    }
    private void UpdateUI()
    {
        // Calculate the speed and update the UI text
        // V = X/T
        Vector3 velocity = (transform.position - previousPosition) / Time.deltaTime;
        playerSpeed = velocity.magnitude;
        // speedText.text = "Speed: " + playerSpeed.ToString("F2");

        previousPosition = transform.position;
    }

    void AdjustCameraSettings()
    {
        cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(cinemachineCamera.Lens.FieldOfView, targetFOV, Time.deltaTime * smoothTime);
        if (playerSpeed >= walkSpeed)
        {
            cinemachinePerlin.FrequencyGain = Mathf.Lerp(cinemachinePerlin.FrequencyGain, targetFrequencyGain, Time.deltaTime * smoothTime);
        }
        else
        {
            cinemachinePerlin.FrequencyGain = Mathf.Lerp(cinemachinePerlin.FrequencyGain, idleFrequencyGain, Time.deltaTime * smoothTime);
        }
    }
    #endregion
}
