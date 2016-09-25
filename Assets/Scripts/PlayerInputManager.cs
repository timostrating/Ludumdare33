using UnityEngine;
using System.Collections;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random; 		// Tells Random to use the Unity Engine random number generator.
using UnityEngine.UI;                   // Allows us to UNITY 4.6 UI 

namespace Completed {
    public class PlayerInputManager : MonoBehaviour {

        public static PlayerInputManager instance = null;

        public int playerCoins = 100;           // coin amount
        private Text coinsText;                 // coin text

        public GameObject[] floorTiles;		    // Array of floor 
        public GameObject[] spawnTiles;	        // Array of floor 

        private Transform respawnPoint;
        private GameObject boardHolder;

        private List<Player> players;
        



        private void Awake() {
            if (instance == null)
                instance = this;

            else if (instance != this)
                Destroy(gameObject);

            respawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
            respawnPoint.position = new Vector3(2, 0, 0);
            players = new List<Player>();
            coinsText = GameObject.FindGameObjectWithTag("CoinsText").GetComponent<Text>();
        }

        void Start() {
            coinsText.text = "COINS: " + playerCoins; 
            Setup();
        }


        private void Update() {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit != null && hit.collider != null && hit.collider.transform.position.y == 0) {
                respawnPoint.position = hit.transform.position;
            }

            //if (!GameManager.instance.playersTurn) return; //If it's not the player's turn, exit the function.

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) { MoveLeft(); } // LEFT
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) { MoveUp(); } // UP
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) { MoveRight(); } // RIGHT
        }


        private void Setup() {
            boardHolder = GameObject.Find("Board");
            for (int x = 2; x < 19; x++) {  // loop x
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, 0, 0f), Quaternion.identity) as GameObject;
                instance.AddComponent<BoxCollider2D>();
                instance.GetComponent<BoxCollider2D>().isTrigger = true;
                instance.tag = "SpawnPoint";
                instance.transform.SetParent(boardHolder.transform);
            }
        }

        public void AddCoins(int amount) {
            playerCoins += amount;
            coinsText.text = "COINS: " + playerCoins;
        }

        public void AddPlayerToList(Player script) {
            players.Add(script);
        }

        public void RemovePlayerFromList(Player script) {
            players.Remove(script);
        }


        public void SpawnPlayer(int witchUnit) {
            if (playerCoins - ((witchUnit + 1) * 10) > -1)
                playerCoins = playerCoins - ((witchUnit + 1) * 10);
            else 
                return;
            
            coinsText.text = "COINS: " + playerCoins;
            Instantiate(spawnTiles[witchUnit], respawnPoint.position, Quaternion.identity);
        }

        public void MoveLeft() { // LEFT
            Debug.Log("MoveLeft : " + players.Count + " Listener");
            for (int i = 0; i < players.Count; i++) { // loop Players
                players[i].AttemptMoveConstructor(-1, 0);
            }
        }
        public void MoveUp() { // UP
            Debug.Log("MoveUp : " + players.Count + " Listener");
            for (int i = 0; i < players.Count; i++) { // loop Players
                players[i].AttemptMoveConstructor(0, 1);
            }
        }
        public void MoveRight() { // RIGHT
            Debug.Log("MoveRight : " + players.Count + " Listener");
            for (int i = 0; i < players.Count; i++) { // loop Players
                players[i].AttemptMoveConstructor(1, 0);
            }
        }
    }
}
