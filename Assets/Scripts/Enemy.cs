using UnityEngine;

public class Enemy : MonoBehaviour
{   
    public int maxHealth = 3;
    public Animator animator;
    private bool playerInRange = false;
    public Transform player;
    public float attackRange = 10f;
    public float WalkSpeed = 1.5f;
    public Transform detectPoint;
    public float distance;
    public LayerMask detectLayer; 

    private bool facingLeft = true;
    public float chaseSpeed = 2.5f;
    public float retriveDistance = 2.5f;
    public Transform attackPoint;
    public float attackRadius = 2f;
    public LayerMask attackLayer;
   

    void Update()
    {
        if(maxHealth <= 0) {
            Die();
        }


        if(player == null){
             animator.SetBool("PlayerDead", true);
       return;
        }


        if (Vector2.Distance(transform.position, player.position) <= attackRange) {
            playerInRange = true;
        } 
        else {
            playerInRange = false;
        }


        if (playerInRange) {
            if (transform.position.x < player.position.x && facingLeft) {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            } else if (transform.position.x > player.position.x && !facingLeft) {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }



            if (Vector2.Distance(transform.position, player.position) > retriveDistance) {
                animator.SetBool("Attack", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            } else {
                animator.SetBool("Attack", true);
            }
        } else{
            transform.Translate(Vector2.left * WalkSpeed * Time.deltaTime);
            RaycastHit2D hit = Physics2D.Raycast(detectPoint.position, Vector2.down, distance, detectLayer);
            if (hit == false) {
                if (facingLeft) {
                    transform.Translate(Vector2.left * WalkSpeed * Time.deltaTime);
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    facingLeft = false;
                } else if (!facingLeft) {
                    transform.Translate(Vector2.right * WalkSpeed * Time.deltaTime);
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    facingLeft = true;
                }
            }
        }
    }
    public void Attack(){
       Collider2D collInfo =  Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
       if (collInfo) {
        if(collInfo.GetComponent<PlayerMovements>() !=null){
collInfo.GetComponent<PlayerMovements>().PlayerTakeDamage(1);
        }
    }
    }
     public void EnemyTakeDamage(int damage){
        if(maxHealth <= 0) {
            return;
        }
        maxHealth -= damage;
     }

    private void OnDrawGizmosSelected() {
        if (detectPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(detectPoint.position, Vector2.down * distance);

        if (attackPoint != null){
            Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }

    void Die(){
        Debug.Log(this.gameObject.name + "Dead");
        Destroy(this.gameObject);
    }

}