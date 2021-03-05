using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState{
    Walking,
    Attacking
}

public class Player : MonoBehaviour
{
    public static Player Instance{ get; private set;}

    //Animation states
    const string PLAYER_IDLE = "Player_Idle";
    const string PLAYER_WALKING = "Movement";
    const string PLAYER_ATTACK = "Attacking";
    const string PLAYER_MINING = "Mining";

    private Animator animator;
    private Equipment equipment;
    public GameObject hand;
    public PlayerState currentState;
    public Rigidbody2D rb;
    public GameObject camara;
    public HealthBar healthBar;
    public Joystick joystick;

    public int maxHealth = 100;
    public int currentHealth;
    public float moveSpeed = 5f;
    public float timerInmune;
    public bool inmune;
    private string currentAnimaton;

    Vector2 movement;
    public Vector2 lastMovement;

    void Awake()
    {
        Instance = this; 
        inmune = false;
        timerInmune = 0;
    }
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        hand = this.gameObject.transform.GetChild(0).gameObject;
        animator = gameObject.GetComponent<Animator>();
        equipment = gameObject.GetComponent<Equipment>();
        lastMovement = new Vector2(0, -1);
        currentState = PlayerState.Walking;
    }
    void Update()
    {
        if(timerInmune > 0)
            timerInmune -= Time.deltaTime;
         if(timerInmune <= 0)
         {
             inmune = false;
         }

        /*if(EventSystem.current.IsPointerOverGameObject())
            return;*/

        if(joystick.Horizontal >= .2f)
            movement.x = 1;
        else if(joystick.Horizontal <= -.2f)
            movement.x = -1;
        else movement.x = 0;
        if(joystick.Vertical >= .2f)
            movement.y = 1;
        else if(joystick.Vertical <= -.2f)
        movement.y = -1;
        else movement.y = 0;
        if(movement.x != 0 || movement.y != 0)
            lastMovement = new Vector2(movement.x, movement.y);

        if(Input.GetKeyDown(KeyCode.E))
            equipment.EquipWeapon(1);
        if(Input.GetKeyDown(KeyCode.Q))
            equipment.EquipWeapon(-1);

        /*if(Input.GetButtonDown("Fire1") && currentState != PlayerState.Attacking){
            animator.SetFloat("AccionHorizontal", lastMovement.x);
            animator.SetFloat("AccionVertical", lastMovement.y);
            currentState = PlayerState.Attacking;
            if(lastMovement.x == 0 && lastMovement.y == 1)
                hand.GetComponent<SpriteRenderer>().sortingLayerName = "BelowCharacter";
            PlayerAttack();
            ChangeAnimationState(PLAYER_ATTACK);
            Invoke("PlayerFinishAttack", 0.3333333f); //animator.GetCurrentAnimatorStateInfo(0).length
        }else if(Input.GetButtonDown("Fire2") && currentState != PlayerState.Attacking){
            animator.SetFloat("AccionHorizontal", lastMovement.x);
            animator.SetFloat("AccionVertical", lastMovement.y);
            currentState = PlayerState.Attacking;
            if(lastMovement.x == 0 && lastMovement.y == 1)
                hand.GetComponent<SpriteRenderer>().sortingLayerName = "BelowCharacter";
            PlayerMine();
            ChangeAnimationState(PLAYER_MINING);
            Invoke("PlayerFinishAttack", 0.3333333f);
            Invoke("PickaxeColliderOn", 0.3f);
        }else UpdateAnimationsAndMove();*/
        UpdateAnimationsAndMove();
    }

    void UpdateAnimationsAndMove(){    
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("LastHorizontal", lastMovement.x);
        animator.SetFloat("LastVertical", lastMovement.y);
        
        if(movement.sqrMagnitude > 0 && currentState == PlayerState.Walking)
            ChangeAnimationState(PLAYER_WALKING);
        else if(movement.sqrMagnitude == 0 && currentState == PlayerState.Walking){
            ChangeAnimationState(PLAYER_IDLE);
        }
        MoveCharacter();
    }

    void MoveCharacter()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage){
        if(inmune  == true)
            return;

        inmune = true;
        timerInmune = 2f;
        currentHealth -= damage;

        if(currentHealth <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        healthBar.SetHealth(currentHealth);
    }

    public void PlayerMine(){
        animator.SetFloat("AccionHorizontal", lastMovement.x);
        animator.SetFloat("AccionVertical", lastMovement.y);
        currentState = PlayerState.Attacking;
        if(lastMovement.x == 0 && lastMovement.y == 1)
            hand.GetComponent<SpriteRenderer>().sortingLayerName = "BelowCharacter";
        equipment.MineWithEquippedWeapon();
        ChangeAnimationState(PLAYER_MINING);
        Invoke("PlayerFinishAttack", 0.3333333f);
        Invoke("PickaxeColliderOn", 0.3f);
    }
    public void PlayerAttack(){
        animator.SetFloat("AccionHorizontal", lastMovement.x);
            animator.SetFloat("AccionVertical", lastMovement.y);
            currentState = PlayerState.Attacking;
            if(lastMovement.x == 0 && lastMovement.y == 1)
                hand.GetComponent<SpriteRenderer>().sortingLayerName = "BelowCharacter";
            equipment.AtackWithEquippedWeapon();
            ChangeAnimationState(PLAYER_ATTACK);
            Invoke("PlayerFinishAttack", 0.3333333f);
    }
    public void PlayerFinishAttack(){
        currentState = PlayerState.Walking;
        equipment.DestroyEquippedWeapon();
        hand.GetComponent<SpriteRenderer>().sortingLayerName = "AboveCharacter";
    }
    void PickaxeColliderOn(){
        if(lastMovement.x > 0)
            equipment.wep.GetComponent<Pickaxe>().p_RightCollider.enabled = true;
        else if(lastMovement.x < 0)
            equipment.wep.GetComponent<Pickaxe>().p_LeftCollider.enabled = true;
        else equipment.wep.GetComponent<Pickaxe>().p_MiddleCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Item"))
            other.gameObject.GetComponent<ItemPickup>().Interact();
        if(other.gameObject.CompareTag("Travel")){
            gameObject.transform.position = other.gameObject.GetComponent<TravelPoint>().playerPos.position;
            //camara.transform.position = other.gameObject.GetComponent<TravelPoint>().cameraPos.position;
        }
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
}