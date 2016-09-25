using UnityEngine;
using System.Collections;

namespace Completed {
    public class Wall : MonoBehaviour {
        public static Wall instance = null;         // the public instance of the object for the PLAYER.CS script
        public Sprite dmgSprite;					// the second sprite of the wall object
        [SerializeField]
        private int hp = 3;							// hit points of the wall


        private SpriteRenderer spriteRenderer;		//Store a component reference to the attached SpriteRenderer.


        void Awake() {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            spriteRenderer = GetComponent<SpriteRenderer>();
        }


        public void DamageWall() {                  //DamageWall is called when the player attacks a wall.
            spriteRenderer.sprite = dmgSprite;      //Set spriteRenderer to the damaged wall sprite.
            hp--;
            if (hp <= 0)
                GameManager.instance.EndGame();
        }
    }
}

