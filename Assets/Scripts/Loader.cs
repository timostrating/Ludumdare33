using UnityEngine;
using System.Collections;

namespace Completed
{	
	public class Loader : MonoBehaviour 
	{
		public GameObject gameManager;			//GameManager
		//public GameObject soundManager;			//SoundManager
        //public GameObject playerInputManager;	//payerInputManager
		
		
		void Awake ()
		{
			if (GameManager.instance == null)
				Instantiate(gameManager);
			//if (SoundManager.instance == null)
			//	Instantiate(soundManager);
            //if (PlayerInputManager.instance == null)
            //    Instantiate(playerInputManager);
		}
	}
}