using UnityEngine;
using System.Collections;

namespace Completed {
    [RequireComponent(typeof(Collider2D))]
    public class Player : MovingObject {
        public int lifePoints = 2;					// lifes of the player




        protected override void Start() {
            PlayerInputManager.instance.AddPlayerToList(this);
            base.Start();
        }


        public void AttemptMoveConstructor(int xDir, int yDir) {
            AttemptMove<Wall>(xDir, yDir);
        }


        //AttemptMove overrides the AttemptMove function in the base class MovingObject
        //AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
        protected override void AttemptMove<T>(int xDir, int yDir) {

            base.AttemptMove<T>(xDir, yDir);        // Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.

            RaycastHit2D hit;                       // Hit allows us to reference the result of the Linecast done in Move.

            if (Move(xDir, yDir, out hit)) {        // If Move returns true, meaning Player was able to move into an empty space.
                //Debug.Log("move");
            }

            //Set the playersTurn boolean of GameManager to false now that players turn is over.
            GameManager.instance.playersTurn = false;
        }


        protected override void OnCantMove<T>(T component) {
            Enemy hitEnemy = component as Enemy;
            LoseLife(1);
            GameManager.instance.RemoveEnemyFromList(hitEnemy);
            Destroy(hitEnemy.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Gold_Small") { 
                PlayerInputManager.instance.AddCoins(10); 
                other.gameObject.SetActive(false); 
            } 
            
            else if (other.tag == "Gold_Medium") { 
                PlayerInputManager.instance.AddCoins(25); 
                other.gameObject.SetActive(false); 
            } 
            
            else if (other.tag == "Gold_Large") { 
                PlayerInputManager.instance.AddCoins(50); 
                other.gameObject.SetActive(false); 
            } 
            
            else if (other.tag == "Finish") { 
                Wall.instance.DamageWall(); 
                Die(); 
                Debug.Log("die"); }
        }

        public void LoseLife(int amount) {
            lifePoints -= amount;
            CheckIfDead();
        }

        private void CheckIfDead() {
            if (lifePoints < 0) {
                CheckIfDead();
            }
        }

        private void Die() {
            PlayerInputManager.instance.RemovePlayerFromList(this);
            Destroy(this.gameObject);
        }
    }
}



//using UnityEngine;
//using System.Collections;
//
//public class Player : MonoBehaviour {
//
//    public float moveTime = 0.1f;			//Time it will take object to move, in seconds.
//    public LayerMask blockingLayer;			//Layer on which collision will be checked.
//
//
//    private BoxCollider2D boxCollider; 		//The BoxCollider2D component attached to this object.
//    private Rigidbody2D rb2D;				//The Rigidbody2D component attached to this object.
//    private float inverseMoveTime;			//Used to make movement more efficient
//
//    public void AttemptMoveConstructor(int xDir, int yDir) {
//        AttemptMove(xDir, yDir);
//    }
//
//
//    void Start() {
//        boxCollider = GetComponent<BoxCollider2D>();                //Get a component reference to this object's BoxCollider2D
//        rb2D = GetComponent<Rigidbody2D>();                         //Get a component reference to this object's Rigidbody2D
//        inverseMoveTime = 1f / moveTime;                            //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
//    }
//
//
//    // Move returns true if it is able to move and false if not.       Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
//    bool Move(int xDir, int yDir, out RaycastHit2D hit) {
//
//        Vector2 start = transform.position;                         // Store start position to move from, based on objects current transform position.
//        Vector2 end = start + new Vector2(xDir, yDir);              // Calculate end position based on the direction parameters passed in when calling Move.
//        boxCollider.enabled = false;                                // Disable the boxCollider so that linecast doesn't hit this object's own collider.
//        hit = Physics2D.Linecast(start, end, blockingLayer);        // Cast a line from start point to end point checking collision on blockingLayer.
//        boxCollider.enabled = true;                                 // Re-enable boxCollider after linecast
//
//
//        if (hit.transform == null) {                                // Check if anything was hit
//            StartCoroutine(SmoothMovement(end));                    // If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
//            return true;                                            // Return true to say that Move was successful
//        }
//
//
//        return false;                                               // If something was hit, return false, Move was unsuccesful.
//    }
//
//
//
//    IEnumerator SmoothMovement(Vector3 end) { //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
//            
//        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;                                   //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
//                                                                                                                //Square magnitude is used instead of magnitude because it's computationally cheaper.
//
//        while (sqrRemainingDistance > float.Epsilon) {                                                          //While that distance is greater than a very small amount (Epsilon, almost zero):
//            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);     //Find a new position proportionally closer to the end, based on the moveTime
//            rb2D.MovePosition(newPostion);                                                                      //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
//            sqrRemainingDistance = (transform.position - end).sqrMagnitude;                                     //Recalculate the remaining distance after moving
//            yield return null;                                                                                  //Return and loop until sqrRemainingDistance is close enough to zero to end the function
//        }
//    }
//
//
//    //The virtual keyword means AttemptMove can be overridden by inheriting classes using the override keyword.
//    //AttemptMove takes a generic parameter T to specify the type of component we expect our unit to interact with if blocked (Player for Enemies, Wall for Player).
//    //void AttemptMove<T>(int xDir, int yDir)  where T : Component{
//    public void AttemptMove(int xDir, int yDir) {
//
//        RaycastHit2D hit;                                       //Hit will store whatever our linecast hits when Move is called.
//        bool canMove = Move(xDir, yDir, out hit);               //Set canMove to true if Move was successful, false if failed.
//
//        if (hit.transform == null)                              //Check if nothing was hit by linecast
//            return;                                             //If nothing was hit, return and don't execute further code.
//
//        //T hitComponent = hit.transform.GetComponent<T>();       //Get a component reference to the component of type T attached to the object that was hit
//
//        //If canMove is false and hitComponent is not equal to null, meaning MovingObject is blocked and has hit something it can interact with.
//        //if(!canMove && hitComponent != null)
//
//        //Call the OnCantMove function and pass it hitComponent as a parameter.
//        //OnCantMove (hitComponent);
//    }
//}