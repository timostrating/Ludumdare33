using UnityEngine;
using System;
using System.Collections.Generic; 		// Allows us to use Lists.
using Random = UnityEngine.Random; 		// Tells Random to use the Unity Engine random number generator.

namespace Completed {
	
	public class BoardManager : MonoBehaviour {

		// Using Serializable allows us to embed a class with sub properties in the inspector.
		[Serializable]
		public class Count {
			public int minimum; 			// Minimum value for our Count class.
			public int maximum; 			// Maximum value for our Count class.

            public Count(int min, int max) { // Assignment constructor.
				minimum = min;
				maximum = max;
			}
		}
		
		int columns = 20; 										        // Number of columns of the board.
		int rows = 11;											        // Number of rows of the board.
		Count treeCount = new Count (25, 30);
        Count treasureCount = new Count(5, 10);
        Count enemyCount = new Count(5, 10);

        public GameObject exit;											// exit Prefab
		public GameObject[] floorTiles;									// Array of floor 
		public GameObject[] treeTiles;									// Array of wall 
		public GameObject[] treasureTiles;								// Array of food 
		public GameObject[] enemyTiles;									// Array of enemy 
		public GameObject[] outerWallTiles;								// Array of outerWall 
		
		private Transform boardHolder;									// A variable to store a reference to the transform of our Board object.
		private List <Vector3> gridPositions = new List <Vector3> ();	// A list of possible locations to place tiles.
		
		
		// Clears our list gridPositions and prepares it to generate a new board.
		void InitialiseList () {
			gridPositions.Clear ();
			for(int x = 1; x < columns-1; x++) { // loop x
                for (int y = 1; y < rows - 1; y++) {  // loop y
                    gridPositions.Add(new Vector3(x, y, 0f));          // At each index add a new Vector3 to our list with the x and y coordinates of that position.
				}
			}
		}
		
		
		//Sets up the outer walls and floor (background) of the game board.
		void BoardSetup () {
			//Instantiate Board and set boardHolder to its transform.
			boardHolder = new GameObject ("Board").transform;
			
			for(int x = 1; x < columns; x++) { // loop x
                for (int y = 1; y < rows; y++) {  // loop y
                    GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];                                      //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.

                    if (x == 1 || x == columns-1 || y == rows-1) 
                        toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

					GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;     //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                    instance.transform.SetParent(boardHolder);                                                                      //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				}
			}
		}
		
		
		//RandomPosition returns a random position from our list gridPositions.
		Vector3 RandomPosition () {
            int randomIndex = Random.Range(0, gridPositions.Count);                                                                 //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
            Vector3 randomPosition = gridPositions[randomIndex];                                                                    //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
            gridPositions.RemoveAt(randomIndex);                                                                                    //Remove the entry at randomIndex from the list so that it can't be re-used.
            return randomPosition;                                                                                                  //Return the randomly selected Vector3 position.
		}
		
		
		//LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
        void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {
            int objectCount = Random.Range(minimum, maximum + 1);
            for (int i = 0; i < objectCount; i++) {
                Vector3 randomPosition = RandomPosition();
                GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
                GameObject instance = Instantiate(tileChoice, randomPosition, Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);   
            }
        }

        void RemoveIdexItemsFromListByVector(params Vector2[] indexVector) {
            int indexCount = indexVector.Length;
            gridPositions.RemoveAt(5);
            //for (int i = 0; i <= indexCount; i++) {
            //    int indexNumber = (int)indexVector[i].y * columns + (int)indexVector[i].x;
            //    gridPositions.RemoveAt( 5 );
            //    Debug.Log("gridPositions.RemoveAt " + indexVector);
            //}
        }
		
		
		//SetupScene initializes our level and calls the previous functions to lay out the game board
		public void SetupScene (int level) {
            BoardSetup();       // floor
            InitialiseList();                                             
            LayoutObjectAtRandom(treeTiles,     treeCount.minimum,      treeCount.maximum);
            LayoutObjectAtRandom(treasureTiles, treasureCount.minimum,  treasureCount.maximum);                                    
            LayoutObjectAtRandom(enemyTiles,    enemyCount.minimum,     enemyCount.maximum);
            Instantiate(exit, new Vector3(columns - 2, rows - 2, 0f), Quaternion.identity);
        }
	}
}
