using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Crouching")]
    public float crouchScale = 0.6f;
    public float scaleSpeed = 10f;

    public float crouchSpeed;
    public bool isCrouching;

    [Header("Movement Lean")]
    public Transform visualRoot;
    public float leanAngle = 10f;
    public float leanDuration = 0.2f;

    [Header("Refernces")]
    public InputActionReference walkInput;
    public InputActionReference sprintInput;
    public InputActionReference jumpInput;
    public InputActionReference crouchInput;

    private CharacterController _charCTRL;
    private Vector3 _velocity;
    private Vector3 originalScale;
    private Tween leanTween;

    void Start()
    {
        _charCTRL = GetComponent<CharacterController>();

        originalScale = transform.localScale;
    }

    void Update()
    {
        HandleCrouch();

        Vector2 moveInput = walkInput.action.ReadValue<Vector2>();
        bool isSprinting = sprintInput.action.IsPressed();

        float currentSpeed = isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : walkSpeed);

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        float targetLean = -moveInput.x * leanAngle;

        if (visualRoot != null && Mathf.Abs(visualRoot.localEulerAngles.z - (targetLean < 0 ? 360f + targetLean : targetLean)) > 1f)
        {
            leanTween?.Kill();
            leanTween = visualRoot.DOLocalRotate(new Vector3(0f, 0f, targetLean), leanDuration).SetEase(Ease.OutQuad);
        }

        bool isGrounded = _charCTRL.isGrounded;

        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        if (isGrounded && jumpInput.action.IsPressed())
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _charCTRL.Move(move * currentSpeed * Time.deltaTime);

        _velocity.y += gravity * Time.deltaTime;

        _charCTRL.Move(_velocity * Time.deltaTime);
    }

    void HandleCrouch()
    {

        if (crouchInput.action.IsPressed())
        {
            Vector3 targetScale = new Vector3(originalScale.x, crouchScale, originalScale.z);
            transform.localScale = targetScale;
            isCrouching = true;
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, scaleSpeed * Time.deltaTime);
            isCrouching = false;
        }
    }
}
