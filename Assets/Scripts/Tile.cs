/* A component that encapsulates tile-related data and behavior */

using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    static Sprite[]         spriteArray;

    public Texture2D        spriteTexture;
    public int				x, y;
    public int				tileNum;
    private BoxCollider		bc;
    private Material        mat;

    private SpriteRenderer  sprend;

    void Awake() {
        if (spriteArray == null) {
            spriteArray = Resources.LoadAll<Sprite>(spriteTexture.name);
        }

        bc = GetComponent<BoxCollider>();

        sprend = GetComponent<SpriteRenderer>();
        //Renderer rend = gameObject.GetComponent<Renderer>();
        //mat = rend.material;
    }

    public void SetTile(int eX, int eY, int eTileNum = -1) {
        if (x == eX && y == eY) return; // Don't move this if you don't have to. - JB

        x = eX;
        y = eY;
        transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D3")+"x"+y.ToString("D3");

        tileNum = eTileNum;
        if (tileNum == -1 && ShowMapOnCamera.S != null) {
            tileNum = ShowMapOnCamera.MAP[x,y];
            if (tileNum == 0) {
                ShowMapOnCamera.PushTile(this);
            }
        }

        sprend.sprite = spriteArray[tileNum];

        if (ShowMapOnCamera.S != null) Customize();
        //TODO: Add something for destructibility - JB

        gameObject.SetActive(true);
        if (ShowMapOnCamera.S != null) {
            if (ShowMapOnCamera.MAP_TILES[x,y] != null) {
                if (ShowMapOnCamera.MAP_TILES[x,y] != this) {
                    ShowMapOnCamera.PushTile( ShowMapOnCamera.MAP_TILES[x,y] );
                }
            } else {
                ShowMapOnCamera.MAP_TILES[x,y] = this;
            }
        }
    }

	void OnCollisionEnter(Collision coll) {
		//print ("entered collision thingie for tile");
		//print ("tile tag " + this.gameObject.tag);
		//print ("collision tag " + coll.gameObject.tag);
		if ((this.gameObject.tag == "Wall" || this.gameObject.tag == "DoorDown"
		    || this.gameObject.tag == "DoorUp" || this.gameObject.tag == "DoorLeft"
		    || this.gameObject.tag == "DoorRight") && coll.gameObject.tag == "Sword") {
			//print ("omg a wall nooo");
			Destroy (coll.gameObject);
			//print ("destroyed!");
		}
	}

	void OnTriggerEnter(Collider coll) {
		if (this.gameObject.tag == "DoorDown" && coll.gameObject.tag == "Player" 
			&& coll.gameObject.GetComponent<PlayerController>().current_direction == Direction.SOUTH) {
//			print ("tag is " + this.gameObject.tag);
//			print ("other tag is " + coll.gameObject.tag);
//			print ("pan down!!!!!!!!!!!");
			//pan down I guess?
			if (!CameraPan.c.panning_down) {
				Vector3 new_pos = coll.gameObject.transform.position;
				new_pos.y -= 4;
				coll.gameObject.transform.position = new_pos;
				Vector3 temp = CameraPan.c.destination;
				temp.y -= CameraPan.c.height;
				CameraPan.c.destination = temp;
				CameraPan.c.panning_down = true;
			}
		} else if (this.gameObject.tag == "DoorRight" && coll.gameObject.tag == "Player"
			&& coll.gameObject.GetComponent<PlayerController>().current_direction == Direction.EAST) {
//			print ("tag is " + this.gameObject.tag);
//			print ("other tag is " + coll.gameObject.tag);
//			print ("pan right!!!!!!!!!!!");
			//pan down I guess?
			if (!CameraPan.c.panning_right) {
				Vector3 new_pos = coll.gameObject.transform.position;
				new_pos.x += 4;
				coll.gameObject.transform.position = new_pos;
				Vector3 temp = CameraPan.c.destination;
				temp.x += CameraPan.c.width;
				CameraPan.c.destination = temp;
				CameraPan.c.panning_right = true;
			}
		} else if (this.gameObject.tag == "DoorUp" && coll.gameObject.tag == "Player"
			&& coll.gameObject.GetComponent<PlayerController>().current_direction == Direction.NORTH) {
//			print ("tag is " + this.gameObject.tag);
//			print ("other tag is " + coll.gameObject.tag);
//			print ("pan up!!!!!!!!!!!");
			//pan down I guess?
			if (!CameraPan.c.panning_up) {
				//Vector3 new_pos = coll.gameObject.transform.position;
				//new_pos.x += 5;
				//coll.gameObject.transform.position = new_pos;
				Vector3 new_pos = coll.gameObject.transform.position;
				new_pos.y += 4;
				coll.gameObject.transform.position = new_pos;
				Vector3 temp = CameraPan.c.destination;
				temp.y += CameraPan.c.height;
				CameraPan.c.destination = temp;
				CameraPan.c.panning_up = true;
			}
		} else if (this.gameObject.tag == "DoorLeft" && coll.gameObject.tag == "Player"
			&& coll.gameObject.GetComponent<PlayerController>().current_direction == Direction.WEST) {
//			print ("tag is " + this.gameObject.tag);
//			print ("other tag is " + coll.gameObject.tag);
//			print ("pan left!!!!!!!!!!!");
			//pan down I guess?
			if (!CameraPan.c.panning_left) {
				//Vector3 new_pos = coll.gameObject.transform.position;
				//new_pos.x += 5;
				//coll.gameObject.transform.position = new_pos;
				Vector3 new_pos = coll.gameObject.transform.position;
				new_pos.x -= 4;
				coll.gameObject.transform.position = new_pos;
				Vector3 temp = CameraPan.c.destination;
				temp.x -= CameraPan.c.width;
				CameraPan.c.destination = temp;
				CameraPan.c.panning_down = true;
			}
		}
	}

    /* Customize this tile based on the contents of Collision.txt
     * 
     * The function below uses a switch statement to decide whether a given tile
     * requires a box collider. It decides this by looking into the Collision.txt text file
     * for the code that corresponds to this tile. If the code is "S", it stands for solid, and
     * this tile received a collider.
     * 
     * Study this function, and consider adding more cases to allow for more advanced customization
     * of tiles.
     * 
     * - AY
     */
    void Customize() {
        
        bc.enabled = true;
        char c = ShowMapOnCamera.S.collisionS[tileNum];
        switch (c) {
		case 'S': // Solid
			bc.center = Vector3.zero;
			bc.size = Vector3.one;
            break;
		case 'W': // Wall
			bc.center = Vector3.zero;
			bc.size = Vector3.one;
			this.gameObject.tag = "Wall";
			break;
		case 'D': //DoorDown
			bc.center = Vector3.zero;
			bc.size = Vector3.one;
			bc.isTrigger = true;
			this.gameObject.layer = 11;
			this.gameObject.tag = "DoorDown";
			break;
		case 'U': //DoorUp
			bc.center = Vector3.zero;
			bc.size = Vector3.one;
			bc.isTrigger = true;
			this.gameObject.layer = 11;
			this.gameObject.tag = "DoorUp";
			break;
		case 'L': //DoorLeft
			bc.center = Vector3.zero;
			bc.size = Vector3.one;
			bc.isTrigger = true;
			this.gameObject.layer = 11;
			this.gameObject.tag = "DoorLeft";
			break;
		case 'R': //DoorRight
			bc.center = Vector3.zero;
			bc.size = Vector3.one;
			bc.isTrigger = true;
			this.gameObject.layer = 11;
			this.gameObject.tag = "DoorRight";
			break;
		case 'B': //Block (aka one of those Dragon tiles)
			bc.center = Vector3.zero;
			bc.size = Vector3.one;
			this.gameObject.layer = 10;
			break;
        default:
            bc.enabled = false;
            break;
		}
    }	
}