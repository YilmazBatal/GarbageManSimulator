using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Components")]
    [SerializeField] CharacterController characterController;
    [SerializeField] CinemachineCamera cinemachineCamera;
    [SerializeField] CinemachineBasicMultiChannelPerlin cinemachinePerlin;
    [SerializeField] AudioSource audioSource;

    [Header("Camera configs")]
    [SerializeField] float walkFOV = 60f;
    [SerializeField] float sprintFOV = 75f;
    private float targetFOV => sprintInput && energy >= 0.1f && playerSpeed >= walkSpeed ? sprintFOV : walkFOV;
    private float idleFrequencyGain => 1f;
    private float walkFrequencyGain => walkSpeed;
    private float sprintFrequencyGain => sprintSpeed;
    private float targetFrequencyGain => sprintInput && canSprint ? sprintFrequencyGain : walkFrequencyGain;

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
    [SerializeField] float energy = 100f;
    [SerializeField] float stepInterval = 0.5f;
    private float targetSpeed => sprintInput && energy >= 0.1f ? sprintSpeed : walkSpeed;
    private float currentSpeed;
    private float playerSpeed;
    private bool isGrounded => characterController.isGrounded;
    private bool justLanded = false;
    private float verticalVelocity = 0;
    private Vector3 previousPosition;

    [Header("Sprint configs")]
    [SerializeField] private float staminaCooldown = 3f;
    [SerializeField] private float regenDelay = 1f;
    [SerializeField] private float staminaDrain = 20f;
    [SerializeField] private float staminaRegen = 33f;
    private float cooldownTimer = 0f;
    private float regenTimer = 0f;
    private bool canSprint = true;

    [Header("Inputs")]
    public Vector2 moveInput;
    public Vector2 lookInput;
    public bool sprintInput;
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
        DisableMovementOnPanels();
        MovementUpdate();
        AudioUpdate();
        LookUpdate();
        AdjustCameraSettings();
    }

    private void DisableMovementOnPanels()
    {
        if (UIManager.Instance.isAnyPanelOpen)
        {
            // If any UI panel is open, disable player controls
            moveInput = new Vector3(0, 0, verticalVelocity); // Reset everyting except gravity (no idea why its works in z instead of y)
            lookInput = Vector2.zero;
            sprintInput = false;
        }
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

        // Bobbing
        cinemachinePerlin.enabled = isGrounded;

        // Landing check
        if (!justLanded && isGrounded)
        {
            Debug.Log("Landed");
            audioSource.PlayOneShot(AudioManager.Instance.land);
        }

        justLanded = isGrounded;

        // Handle gravity
        if (isGrounded && verticalVelocity <= 0.1f)
        {
            verticalVelocity = -3f;
        }
        verticalVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;

        Vector3 movementRaw = new Vector3(moveInput.x * currentSpeed, verticalVelocity, moveInput.y * currentSpeed);
        Vector3 movement = transform.TransformDirection(movementRaw) * Time.deltaTime;

        StaminaManagement();

        characterController.Move(movement);

        UpdateUI();
    }

    private void StaminaManagement()
    {
        if (!canSprint) // enerji bittiğinde bekleme
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= staminaCooldown)
            {
                canSprint = true;
                cooldownTimer = 0f;
            }
            return; // regen başlamadan çık
        }

        if (sprintInput && energy > 0f)
        {
            energy -= Time.deltaTime * staminaDrain;
            energy = Mathf.Max(energy, 0f);

            if (energy <= 0f)
            {
                canSprint = false;
                cooldownTimer = 0f;
            }
            regenTimer = 0f; // sprint sırasında regen timer sıfırlanır
        }
        else
        {
            // sprint iptal edildiğinde kısa bekleme
            if (regenTimer < regenDelay)
            {
                regenTimer += Time.deltaTime;
            }
            else
            {
                energy += Time.deltaTime * staminaRegen;
                energy = Mathf.Min(energy, 100f);
            }
        }
    }

    public void TryJump()
    {
        if (isGrounded == false) return;

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y * gravityScale);
        Debug.Log("Jumped");
        audioSource.PlayOneShot(AudioManager.Instance.jump);
        justLanded = true;
    }
    private void UpdateUI()
    {
        // Calculate the speed and update the UI text
        // V = X/T
        Vector3 velocity = (transform.position - previousPosition) / Time.deltaTime;
        playerSpeed = velocity.magnitude;
        // speedText.text = "Speed: " + playerSpeed.ToString("F2");

        previousPosition = transform.position;

        UIManager.Instance.staminaBar.fillAmount = energy / 100f;
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

    float stepTimer;

    void AudioUpdate(){
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                AudioClip clip = AudioManager.Instance.footstepClips[Random.Range(0, AudioManager.Instance.footstepClips.Length)];
                audioSource.PlayOneShot(clip);
                stepTimer = stepInterval / characterController.velocity.magnitude; 
            }
        }
    }
    #endregion
}
