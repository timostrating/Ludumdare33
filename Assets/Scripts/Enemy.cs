using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace Completed {
    //Enemy inherits from MovingObject, our base class for objects that can move, Player also inherits from this.
    public class Enemy : MovingObject {
        public int playerDamage; 							//The amount of food points to subtract from the player when attacking.
        //public AudioClip attackSound1;						//First of two audio clips to play when attacking the player.
        //public AudioClip attackSound2;						//Second of two audio clips to play when attacking the player.

        private Transform target;							//Transform to attempt to move toward each turn.
        private bool skipMove;								//Boolean to determine whether or not enemy should skip a turn or move this turn.



        //Start overrides the virtual Start function of the base class.
        protected override void Start() {
            GameManager.instance.AddEnemyToList(this); //Register this enemy with our instance of GameManager by adding it to a list of Enemy objects.    This allows the GameManager to issue movement commands.
            GetRandomTarget();
            base.Start();
        }


        //Override the AttemptMove function of MovingObject to include functionality needed for Enemy to skip turns.
        //See comments in MovingObject for more on how base AttemptMove function works.
        protected override void AttemptMove<T>(int xDir, int yDir) {
            if (skipMove) {                                 //Check if skipMove is true, if so set it to false and skip this turn.
                skipMove = false;
                return;
            }

            base.AttemptMove<T>(xDir, yDir);                //Call the AttemptMove function from MovingObject.
            skipMove = true; // skip next move
        }

        private void CheckIfTargetIsAllive() {
            if (target == null)
                GetRandomTarget();
            //else
            //    Debug.Log(gameObject.name + " : target = " + target.position);
        }

        private void GetRandomTarget() {
            GameObject[] arrayOfPlayers = GameObject.FindGameObjectsWithTag("Player");
            if (arrayOfPlayers.Length <= 0)
                return;
            target = arrayOfPlayers[Random.Range(0, arrayOfPlayers.Length)].transform;
        }


        public void MoveEnemy() {
            CheckIfTargetIsAllive();
            if (target == null) {
                Debug.Log("help");
                return;
            }

            int xDir = 0; //Declare variables for X and Y axis move directions, these range from -1 to 1.
            int yDir = 0;



            //If the difference in positions is approximately zero (Epsilon) do the following:
            if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
                yDir = target.position.y > transform.position.y ? 1 : -1;

            else
                xDir = target.position.x > transform.position.x ? 1 : -1;

            //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
            AttemptMove<Player>(xDir, yDir);
        }


        //OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
        //and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
        protected override void OnCantMove<T>(T component) {
            Player hitPlayer = component as Player;
            hitPlayer.LoseLife(playerDamage);


            //dead
            GameManager.instance.RemoveEnemyFromList(this);
            Destroy(this.gameObject);

            //Set the attack trigger of animator to trigger Enemy attack animation.
            //animator.SetTrigger("enemyAttack");

            //Call the RandomizeSfx function of SoundManager passing in the two audio clips to choose randomly between.
            //SoundManager.instance.RandomizeSfx(attackSound1, attackSound2);
        }
    }
}

//using UnityEngine;
//using System.Collections;
//
//namespace Completed
//{
//	//Enemy inherits from MovingObject, our base class for objects that can move, Player also inherits from this.
//	public class Enemy : MovingObject
//	{
//		public int playerDamage; 							//The amount of food points to subtract from the player when attacking.
//		
//		private Animator animator;							//Variable of type Animator to store a reference to the enemy's Animator component.
//		private Transform target;							//Transform to attempt to move toward each turn.
//		private bool skipMove;								//Boolean to determine whether or not enemy should skip a turn or move this turn.
//		
//		
//		//Start overrides the virtual Start function of the base class.
//		protected override void Start () {
//			GameManager.instance.AddEnemyToList (this);
//			animator = GetComponent<Animator> ();
//
//            target = GameObject.FindGameObjectWithTag("Player").transform;
//			
//			base.Start ();
//		}
//		
//		
//		//Override the AttemptMove function of MovingObject to include functionality needed for Enemy to skip turns.
//		//See comments in MovingObject for more on how base AttemptMove function works.
//		protected override void AttemptMove <T> (int xDir, int yDir) {
//
//            if (skipMove) {                                                                      
//                skipMove = false;
//				return;
//			}
//            base.AttemptMove<T>(xDir, yDir);                                                     //Call the AttemptMove function from MovingObject.
//            skipMove = true;                                                                     //Now that Enemy has moved, set skipMove to true to skip next move.
//		}
//		
//		
//		//MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
//		public void MoveEnemy () {
//			
//            int xDir = 0;                                                                        //Declare variables for X and Y axis move directions, these range from -1 to 1.
//            int yDir = 0;                                                                        //These values allow us to choose between the cardinal directions: up, down, left and right.
//
//
//            if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)            //If the difference in positions is approximately zero (Epsilon) do the following:
//                yDir = target.position.y > transform.position.y ? 1 : -1;                       //If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
//
//            else                                                                                //If the difference in positions is not approximately zero (Epsilon) do the following:
//                xDir = target.position.x > transform.position.x ? 1 : -1;                       //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
//
//            AttemptMove<Player>(xDir, yDir);                                                    //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
//		}
//		
//		
//		//OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
//		//and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
//		//protected override void OnCantMove <T> (T component) {
//        //    Player hitPlayer = component as Player;                                             //Declare hitPlayer and set it to equal the encountered component.
//        //    animator.SetTrigger("enemyAttack");                                                 //Set the attack trigger of animator to trigger Enemy attack animation.
//        //}
//	}
//}
