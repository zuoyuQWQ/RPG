using UnityEngine;

/// <summary>
/// 这个脚本附加到一个子对象上，用于检测实体是否接触地面。
/// 它使用触发器来跟踪与“地面”层的所有接触点。
/// </summary>
public class GroundCheck : MonoBehaviour
{
    [Tooltip("设置哪些层被视作“地面”")]
    [SerializeField] private LayerMask whatIsGround;

    // 当前是否接触地面
    public bool IsGrounded { get; private set; }

    // 接触到的地面碰撞体数量
    private int groundContacts = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查进入的碰撞体是否在“地面”层中
        if ((whatIsGround.value & (1 << collision.gameObject.layer)) > 0)
        {
            groundContacts++;
            IsGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 检查离开的碰撞体是否在“地面”层中
        if ((whatIsGround.value & (1 << collision.gameObject.layer)) > 0)
        {
            groundContacts--;

            // 只有当不接触任何地面碰撞体时，才将状态设为“未在地面”
            if (groundContacts <= 0)
            {
                IsGrounded = false;
                groundContacts = 0; // 防止计数器变为负数
            }
        }
    }
}
