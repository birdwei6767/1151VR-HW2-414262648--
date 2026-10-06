using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移動與跳躍設定")]
    public float moveSpeed = 5f;    // 左右移動速度
    public float jumpPower = 10f;   // 跳躍力度
    public float gravity = 20f;     // 模擬重力（數字越大掉越快）

    private float moveInput;
    private bool isFacingRight = true;
    
    // 用來儲存 Y 軸的速度與地板高度
    private float velocityY = 0f;
    private float groundY;

    void Start()
    {
        // 遊戲開始時，記錄角色現在的高度作為「地板基準線」
        groundY = transform.position.y;
    }

    void Update()
    {
        // 1. 取得左右輸入 (-1, 0, 1)
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2. 處理左右轉向圖片
        if (moveInput > 0 && !isFacingRight) Flip();
        else if (moveInput < 0 && isFacingRight) Flip();

        // 3. 判斷跳躍 (如果按下空白鍵，且角色在地板高度)
        if (Input.GetButtonDown("Jump") && transform.position.y <= groundY)
        {
            velocityY = jumpPower; // 給予向上的速度
        }

        // 4. 模擬重力
        if (transform.position.y > groundY || velocityY > 0)
        {
            // 如果在空中，受到重力影響往下掉
            velocityY -= gravity * Time.deltaTime;
        }
        else
        {
            // 如果掉回地板，速度歸零，並把位置對齊地板
            velocityY = 0f;
            transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
        }

        // 5. 執行 Vector 移動 (左右移動 + 垂直跳躍/掉落)
        Vector3 moveVector = new Vector3(moveInput * moveSpeed, velocityY, 0f);
        transform.position += moveVector * Time.deltaTime;
    }

    // 翻轉角色圖片
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
}