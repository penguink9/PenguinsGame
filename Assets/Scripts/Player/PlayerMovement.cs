using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }
    private bool facingLeft = false;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform attackCollider;

    public static PlayerMovement Instance;
    private PlayerController playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private GameObject slashAnim;
    [SerializeField] private float attackCooldown = 0.5f; 
    private float lastAttackTime = 0f;
    private bool isAttacking = false; // Biến kiểm tra trạng thái tấn công



    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerController();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
    }

   

    private void FixedUpdate()
    {
        //Chỉ cho phép di chuyển nếu không đang tấn công
        if (!isAttacking)
        {
            AdjustPlayerFacingDirection();
            Move();
        }

    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void Attack()
    {
        if ((Time.time - lastAttackTime >= attackCooldown ) && !isAttacking )
            


        {
            isAttacking = true; 
            rb.linearVelocity = Vector2.zero; 
            AdjustPlayerFacingDirection(); // Cập nhật FacingLeft ngay lập tức
            myAnimator.SetTrigger("Attack");
            attackCollider.gameObject.SetActive(true);

            Vector3 spawnPosition = slashAnimSpawnPoint.position;

            if (FacingLeft)
            {
                spawnPosition.x -= 1f; 

            }
            else
            {
                spawnPosition.x += 0.5f; 

            }
            slashAnim = Instantiate(slashAnimPrefab, spawnPosition, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;
            FlipSlashAnim();

            lastAttackTime = Time.time;
            StartCoroutine(AttackCooldownCoroutine());

        }
    }

    public void FlipSlashAnim()
    {

        if (slashAnim != null)
        {
            if (FacingLeft)
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
                Debug.Log("Facingleft");
            }
            else
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = false;
                Debug.Log("Facingright");

            }
        }
    }

    public void DoneAttackingAnimEvent()
    {
        attackCollider.gameObject.SetActive(false);
    }

    private IEnumerator AttackCooldownCoroutine()
    {
        // Đợi cho đến khi cooldown kết thúc
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false; 
    }


    private void AdjustPlayerFacingDirection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
            FacingLeft = true;
            attackCollider.transform.rotation = Quaternion.Euler(0, -180, 0);

        }
        else
        {
            mySpriteRender.flipX = false;
            FacingLeft = false;
            attackCollider.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
    }

    private void Start()
    {

        playerControls.Combat.Attack.started += _ => Attack();

    }



}
