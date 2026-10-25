using UnityEngine;

public class CrosshairRecoil : MonoBehaviour
{
    public static CrosshairRecoil Instance;

    [Header("Crosshair Pieces")]
    public RectTransform top;
    public RectTransform bottom;
    public RectTransform left;
    public RectTransform right;

    [Header("Settings")]
    public float gapMultiplier = 10f;
    public float changeSpeed = 15f;

    private Vector2 topStart;
    private Vector2 bottomStart;
    private Vector2 leftStart;
    private Vector2 rightStart;

    private float targetSpread;

    private void Awake()
    {
        Instance = this;

        topStart = top.anchoredPosition;
        bottomStart = bottom.anchoredPosition;
        leftStart = left.anchoredPosition;
        rightStart = right.anchoredPosition;
    }

    private void Update()
    {
        float gap = targetSpread * gapMultiplier;

        MovePiece(top, topStart + Vector2.up * gap);
        MovePiece(bottom, bottomStart + Vector2.down * gap);
        MovePiece(left, leftStart + Vector2.left * gap);
        MovePiece(right, rightStart + Vector2.right * gap);
    }

    private void MovePiece(RectTransform piece, Vector2 target)
    {
        piece.anchoredPosition = Vector2.Lerp(
            piece.anchoredPosition,
            target,
            changeSpeed * Time.deltaTime
        );
    }

    public static void UpdateCrosshair(float spread)
    {
        if (Instance == null) return;

        Instance.targetSpread = spread;
    }

    public static void ResetCrosshair()
    {
        if (Instance == null) return;

        Instance.targetSpread = 0f;
    }
}
