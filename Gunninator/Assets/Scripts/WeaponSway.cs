using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    [Header("Variables")]
    public float smoothAmount;
    public float swayMultiplier;

    [Space]

    public float swayAimMultiplier;

    [Header("Checks")]
    public bool isAiming;

    [Header("Refernces")]
    public InputActionReference lookInput;

    public static WeaponSway Instance;

    private Quaternion initialRotation;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (isAiming)
        {
            Vector2 lookVector = lookInput.action.ReadValue<Vector2>();

            float mouseX = lookVector.x * swayAimMultiplier;
            float mouseY = lookVector.y * swayAimMultiplier;

            Quaternion swayRotation = Quaternion.Euler(-mouseY, mouseX, -mouseX);

            Quaternion targetRotation = initialRotation * swayRotation;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothAmount * Time.deltaTime);
        }
        else
        {
            Vector2 lookVector = lookInput.action.ReadValue<Vector2>();

            float mouseX = lookVector.x * swayMultiplier;
            float mouseY = lookVector.y * swayMultiplier;

            Quaternion swayRotation = Quaternion.Euler(-mouseY, mouseX, -mouseX);

            Quaternion targetRotation = initialRotation * swayRotation;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothAmount * Time.deltaTime);
        }
    }
}
