using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤー操作を管理するクラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーの速度")]
    private float speed = 10f;

    [SerializeField, Tooltip("ジャンプ力")]
    private float jumpForce = 5f;

    [SerializeField, Tooltip("落下死亡Y座標")]
    private float deathY = -5f;

    private Rigidbody rb;
    private Vector2 movementInput;
    private bool jumpPressed = false;
    private bool isGrounded = false;
    private bool isDead = false;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 毎フレーム実行
    /// </summary>
    void Update()
    {
        if (isDead) return;

        CheckDeath();
    }

    /// <summary>
    /// 物理更新処理
    /// </summary>
    void FixedUpdate()
    {
        if (isDead) return;

        Move();
        Jump();
    }

    /// <summary>
    /// Input System の Move アクションから呼ばれる
    /// </summary>
    /// <param name="movementValue">移動入力値</param>
    private void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    /// <summary>
    /// Input System の Jump アクションから呼ばれる
    /// </summary>
    /// <param name="value">ジャンプ入力</param>
    private void OnJump(InputValue value)
    {
        if (value.isPressed)
            jumpPressed = true;
    }

    /// <summary>
    /// プレイヤー移動処理
    /// </summary>
    private void Move()
    {
        Vector3 movement =
            transform.forward * movementInput.y +
            transform.right * movementInput.x;

        Vector3 velocity = movement * speed;

        // y軸方向の速度は維持
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Jump()
    {
        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            jumpPressed = false;
        }
    }

    /// <summary>
    /// 落下死亡判定
    /// </summary>
    private void CheckDeath()
    {
        if (transform.position.y < deathY)
        {
            isDead = true;

            GameManager.Instance.GameOver();
        }
    }

    /// <summary>
    /// 床接触中の処理
    /// </summary>
    /// <param name="collision">衝突情報</param>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    /// <summary>
    /// 床から離れた時の処理
    /// </summary>
    /// <param name="collision">衝突情報</param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }
}