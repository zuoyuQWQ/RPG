using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("攻击信息")]
    public Transform attackCheck;
    public float attackCheckRadius;
    
    [Header("Collision info")]
    [SerializeField] protected GroundCheck groundCheck;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected Transform ledgeCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected float ledgeCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("击退信息")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFx fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    public bool IsBusy { get; private set; }

    #region 翻转
    public int facingDir { get; private set; } = 1;   
    protected bool facingRight = true;
    #endregion

    public System.Action onFlip;

    protected virtual void Awake()
    {
        var fxComponent = GetComponent<EntityFx>();
        if (fxComponent != null) fx = fxComponent;
        
        var rbComponent = GetComponent<Rigidbody2D>();
        if (rbComponent != null) rb = rbComponent;
        
        var animComponent = GetComponentInChildren<Animator>();
        if (animComponent != null) anim = animComponent;
        
        var srComponent = GetComponentInChildren<SpriteRenderer>();
        if (srComponent != null) sr = srComponent;
        
        var statsComponent = GetComponent<CharacterStats>();
        if (statsComponent != null) stats = statsComponent;
        
        var cdComponent = GetComponent<CapsuleCollider2D>();
        if (cdComponent != null) cd = cdComponent;
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {

    }
    
    public virtual void SlowEntityBY(float _slowPercentage, float _slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    #region 停止一段时间执行
    protected virtual IEnumerator BusyFor(float _seconds)
    {
        IsBusy = true;
        yield return new WaitForSeconds(_seconds);
        IsBusy = false;
    }
    #endregion

    #region 碰撞检测 - 改进的地面检测
    public virtual bool IsGroundDetected() 
    {
        if (groundCheck != null)
            return groundCheck.IsGrounded;
            
        Debug.LogWarning("GroundCheck脚本未在Entity上赋值！");
        return false;
    }
    
    public virtual bool IsWallDetetected()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    public virtual bool IsLedgeDetected()
    {
        return !Physics2D.Raycast(ledgeCheck.position, Vector2.down, ledgeCheckDistance, whatIsGround);
    }

    #endregion

    #region 伤害处理
    public virtual void DamageEffect()
    {
        StartCoroutine("HitKnockback");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }

    public virtual void Die()
    {

    }
    #endregion

    #region 可视化调试
    protected virtual void OnDrawGizmos()
    {
        // 墙面检测
        Gizmos.color = Color.blue;
        if (wallCheck != null)
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        
        // 悬崖检测
        Gizmos.color = Color.green;
        if (ledgeCheck != null)
            Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x, ledgeCheck.position.y - ledgeCheckDistance));

        // 攻击范围
        Gizmos.color = Color.red;
        if (attackCheck != null)
            Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region 翻转控制
    public virtual void Flip()
    {
        if (isKnocked)
            return;
        
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if(onFlip != null)
            onFlip();
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion

    #region 水平移动
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;
            
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public void ZeroVelocity()
    {
        if (isKnocked)
            return;
            
        rb.velocity = new Vector2(0, 0);
    }
    #endregion
}
