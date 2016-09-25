using UnityEngine;
using System.Collections;
using UnityEngine.UI; // ??? maybe neded

namespace Completed {
    using System.Collections.Generic;		// Allows us to use Lists. 

    public class GameManager : MonoBehaviour {
        public float turnDelay = 0.5f;							// Delay between each Player turn.
        public static GameManager instance = null;				// Static instance of GameManager which allows it to be accessed by any other script.
        [HideInInspector]                                          
        public bool playersTurn = true;		                    // Boolean to check if it's players turn, hidden in inspector but public.

        private BoardManager boardScript;						// Store a reference to our BoardManager which will set up the level.
        private int wave = 1;
        [SerializeField]                                           
        private List<Enemy> enemies;							// List of all Enemy units, used to issue them move commands.
        private bool enemiesMoving;								// Boolean to check if enemies are moving.

        private Text coinsText;                 // coin text


        void Awake() {
            if (instance == null)                               // Check if this instance
                instance = this;                                   

            else if (instance != this)                          // If instance already exists and it's not this:
                Destroy(gameObject);                            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                                                                   
            DontDestroyOnLoad(gameObject);                      // Sets this to not be destroyed when reloading scene
            enemies = new List<Enemy>();                        
            boardScript = GetComponent<BoardManager>();         // Get a component reference to the attached BoardManager script
            InitGame();                                         // Call the InitGame function to initialize the first level 
        }

        void InitGame() {  //Initializes the game for each level.
            enemies.Clear();                                    // Clear any Enemy objects in our List to prepare for next level.
            boardScript.SetupScene(wave);                      // Call the SetupScene function of the BoardManager script, pass it current level number.
        }


        void Start() {
            coinsText = GameObject.FindGameObjectWithTag("CoinsText").GetComponent<Text>();
        }


        void Update() {
            if (playersTurn || enemiesMoving)                   // Check that playersTurn or enemiesMoving or doingSetup are not currently true.
                return;

            StartCoroutine(MoveEnemies());                      // Start moving enemies.
        }


        public void AddEnemyToList(Enemy script) {
            enemies.Add(script);
        }

        public void RemoveEnemyFromList(Enemy script) {
            enemies.Remove(script);
        }


        public void EndGame() {
            enemiesMoving = false;
            playersTurn = false;

            coinsText.text = "MONSTERS WIN";
        }


        IEnumerator MoveEnemies() {                             // Coroutine to move enemies in sequence.
            enemiesMoving = true;                               // While enemiesMoving is true player is unable to move.

            yield return new WaitForSeconds(turnDelay);         // Wait for turnDelay seconds, defaults to .1 (100 ms).

            if (enemies.Count <= 0) {
                EndGame();
                yield return true;
            }

            for (int i = 0; i < enemies.Count; i++) { // loop enemy
                enemies[i].MoveEnemy();
                yield return new WaitForSeconds(0.2f); // Wait for Enemy's moveTime before moving next Enemy,
            }

            playersTurn = true; // players turn
            enemiesMoving = false; 
        }
    }
}





//using UnityEngine;
//using System.Collections;
//
//namespace Completed
//{
//	using System.Collections.Generic;		//Allows us to use Lists. 
//	using UnityEngine.UI;					//Allows us to use UI.
//	
//	public class GameManager : MonoBehaviour
//	{
//		public float levelStartDelay = 1f;						//Time to wait before starting level, in seconds.
//		public float turnDelay = 0.5f;							//Delay between each Player turn.
//		public int playerFoodPoints = 100;						//Starting value for Player food points.
//		public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
//		[HideInInspector] public bool playersTurn = true;		//Boolean to check if it's players turn, hidden in inspector but public.
//		
//		
//		private BoardManager boardScript;						//Store a reference to our BoardManager which will set up the level.
//		private int wave = 1;									//Current level number, expressed in game as "Day 1".
//		private List<Enemy> enemies;							//List of all Enemy units, used to issue them move commands.
//		private bool enemiesMoving;								//Boolean to check if enemies are moving.
//		private bool doingSetup = true;							//Boolean to check if we're setting up board, prevent Player from moving during setup.
//		
//		
//		
//		void Awake() {
//			if (instance == null)
//				instance = this;
//			
//			else if (instance != this)
//				Destroy(gameObject);	
//			
//			DontDestroyOnLoad(gameObject);
//			enemies = new List<Enemy>();
//			boardScript = GetComponent<BoardManager>();
//			InitGame();
//		}
//		
//		void OnLevelWasLoaded(int index) {
//			wave++;
//			InitGame();
//		}
//		
//		void InitGame() {
//            doingSetup = true; //While doingSetup is true the player can't move, prevent player from moving while title card is up.
//			enemies.Clear();
//			boardScript.SetupScene(wave);
//			
//		}
//		
//		
//		void HideLevelImage() {			
//			doingSetup = false;
//		}
//
//		
//		void Update() {
//            if (playersTurn || enemiesMoving || doingSetup)                     //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
//				return;
//
//            StartCoroutine(MoveEnemies());                                      //Start moving enemies.
//		}
//
//		
//		//Call this to add the passed in Enemy to the List of Enemy objects.
//		public void AddEnemyToList(Enemy script) {
//			enemies.Add(script);
//		}
//		
//		
//		//GameOver is called when the player reaches 0 food points
//		public void GameOver() {
//			enabled = false;
//		}
//		
//
//		//Coroutine to move enemies in sequence.
//		IEnumerator MoveEnemies() {
//            enemiesMoving = true;                                               //While enemiesMoving is true player is unable to move.
//			
//            yield return new WaitForSeconds(turnDelay); // (100 ms)
//
//            if (enemies.Count == 0) {                                           //If there are no enemies spawned (IE in first level):
//                yield return new WaitForSeconds(turnDelay);                     //Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
//			}
//
//
//            for (int i = 0; i < enemies.Count; i++) {                           //Loop through List of Enemy objects.
//                enemies[i].MoveEnemy();                                         //Call the MoveEnemy function of Enemy at index i in the enemies List.
//                yield return new WaitForSeconds(enemies[i].moveTime);           //Wait for Enemy's moveTime before moving next Enemy, 
//			}
//
//            playersTurn = true;                                                 //Once Enemies are done moving, set playersTurn to true so player can move.
//            enemiesMoving = false;                                              //Enemies are done moving, set enemiesMoving to false.
//		}
//	}
//}
//
//