using UnityEngine.UI;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public GameObject GameOverUI ;
    public Text currentCoinText;
    public int currentCoin = 0;
    public Text maxHealthText;
    public int maxHealth = 10;
    public float movement;
    public float speed = 7f;
    public float jumpForce = 10f;
    public Rigidbody2D rb;
    public Animator animator;
    private bool facingRight = true;
    private bool isGround = true;
    public Transform attackpoint;
    public float attackRadius = 1.5f;
    public LayerMask targetLayer;
    

    void Start() {
        
    }

    void Update() {
        if(maxHealth <= 0) {
            Die();
        }

        currentCoinText.text = currentCoin.ToString();
        maxHealthText.text = maxHealth.ToString();
        movement = Input.GetAxis("Horizontal");   
        
        if (movement > 0 && !facingRight) { // Move Right
            Flip();
        } else if (movement < 0 && facingRight) { // Move Left
            Flip();
        }

        if (Input.GetButtonDown("Jump") && isGround) { // Jump
            jump();
            animator.SetBool("Jump", true);
            isGround = false;
        }
        if (Mathf.Abs(movement) > .1f) {
            animator.SetFloat("Walk", 1f);
        } else if (movement < .1f) {
            animator.SetFloat("Walk", 0f);
        }
        if(Input.GetMouseButtonDown(0)){
            int randomIndex=Random.Range(0,3);
            if(randomIndex==0){
                animator.SetTrigger("Attack1");
            }else if(randomIndex==1){
                animator.SetTrigger("Attack2");
            }else{
                animator.SetTrigger("Attack3");
            }
        }
        

    }

    private void FixedUpdate() {
        transform.position += new Vector3(movement, 0f, 0f) * Time.deltaTime * speed;
    } 


    void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    
    void jump() {
        Vector2 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;
    }
    public void PlayerAttack(){

       Collider2D hitInfo =  Physics2D.OverlapCircle(attackpoint.position, attackRadius, targetLayer);
       if(hitInfo){
        if(hitInfo.GetComponent<Enemy>()!=null){
            hitInfo.GetComponent<Enemy>().EnemyTakeDamage(1);

        }
       }
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isGround = true;
            animator.SetBool("Jump", false);
        } 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            currentCoin++;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collect");
            Destroy(other.gameObject, 1f);
        }
        if (other.gameObject.tag == "Trap")
        {
            Die();
        }
    }

    public void PlayerTakeDamage(int damage){
        if(maxHealth <= 0) {
            return;
        }
        maxHealth -= damage;
    }

    private void OnDrawGizmosSelected() {
        
if (attackpoint == null) {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackpoint.position, attackRadius);
        
    }

    void Die(){
        Debug.Log(this.transform.name + " Dead");
        Destroy(this.gameObject);
        GameOverUI.SetActive(true);
    } 

}