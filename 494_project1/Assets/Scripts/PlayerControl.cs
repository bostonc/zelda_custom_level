using UnityEngine;
using System.Collections;

public enum carDirection {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING, STUNNED, INVINCIBLE, KNOCKBACK };

public class PlayerControl : MonoBehaviour {
    static public PlayerControl S;
    public Sprite[] link_run_down;
	public Sprite[] link_run_up;
	public Sprite[] link_run_right;
	public Sprite[] link_run_left;

    public Sprite[] link_weapon_down;
    public Sprite[] link_weapon_up;
    public Sprite[] link_weapon_right;
    public Sprite[] link_weapon_left;

    StateMachine animation_state_machine;
	StateMachine control_state_machine;
    public float swordRate = .5f;
    private float nextAttack = 0f;
	public EntityState current_state = EntityState.NORMAL;
	public carDirection current_direction = carDirection.SOUTH;

	public GameObject selected_weapon_prefab; //Need to Propogate this to reference of the current weapon

    private void Awake() {
        current_state = EntityState.NORMAL; 
    }
    // Use this for initialization
    void Start () {
        animation_state_machine = new StateMachine();
        animation_state_machine.ChangeState(new StateIdleWithSprite(this, GetComponent<SpriteRenderer>(), link_run_down[0]));
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.A) && Time.time > nextAttack){
            //print("poop");
            current_state = EntityState.ATTACKING;
            nextAttack = Time.time + swordRate;
            if (current_direction == carDirection.SOUTH) {
                SwordAnimation(current_direction,GetComponent<SpriteRenderer>(), link_weapon_down, 6);
            }else if (current_direction == carDirection.NORTH) {
                SwordAnimation(current_direction, GetComponent<SpriteRenderer>(), link_weapon_up, 6);
            } else if (current_direction == carDirection.EAST){
                SwordAnimation(current_direction, GetComponent<SpriteRenderer>(), link_weapon_right, 6);
            } else if (current_direction == carDirection.WEST){
                SwordAnimation(current_direction, GetComponent<SpriteRenderer>(), link_weapon_left, 6);
            }


        }

        if (Time.time > nextAttack && current_state == EntityState.ATTACKING) {
            current_state = EntityState.NORMAL;
            if (current_direction == carDirection.SOUTH) {
                animation_state_machine.ChangeState(new StateIdleWithSprite(this, GetComponent<SpriteRenderer>(), link_run_down[0]));
            } else if (current_direction == carDirection.NORTH) {
                animation_state_machine.ChangeState(new StateIdleWithSprite(this, GetComponent<SpriteRenderer>(), link_run_up[0]));
            } else if (current_direction == carDirection.EAST) {
                animation_state_machine.ChangeState(new StateIdleWithSprite(this, GetComponent<SpriteRenderer>(), link_run_right[0]));
            } else if (current_direction == carDirection.WEST) {
               animation_state_machine.ChangeState(new StateIdleWithSprite(this, GetComponent<SpriteRenderer>(), link_run_left[0]));
            }
            //updates back to idle 
            //current_state = EntityState.NORMAL;
           // Debug.Log("wow");
            
        }
        animation_state_machine.Update();
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            //D//ebug.Log("down");
            current_direction = carDirection.SOUTH;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) { 
           // Debug.Log("up");
            current_direction = carDirection.NORTH;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
           // Debug.Log("right");
            current_direction = carDirection.EAST;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            //Debug.Log("left");
            current_direction = carDirection.WEST;
        }
         

    }


    void SwordAnimation(carDirection dir, SpriteRenderer renderer, Sprite[] animation, int fps ) {
        // float animation_progression;

        //Debug.Log("sword animate");
        float animation_start_time;

        animation_start_time = Time.time;

        int animation_length = animation.Length;
        int current_frame_index = ((int)((Time.time - animation_start_time) / (1.0 / fps)) % animation_length);
        renderer.sprite = animation[1];

    }

}
