using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Text rupee_text;
    public Text key_text;
    public Text health_text;
    public Text bomb_text;
    public Text weapon_text;
    public Text god_text;
    public Text win_text;
    private int togglecounter;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int num_player_rupees = PlayerController.S.rupees;
        rupee_text.text = ": " + num_player_rupees.ToString();

        key_text.text = ": " + PlayerController.S.keys.ToString();

        health_text.text = ": " + PlayerController.S.health.ToString();

        bomb_text.text = ": " + PlayerController.S.bombs.ToString();

        weapon_text.text = "Equipped: " + PlayerController.S.equippedWeapon.ToString();

        god_text.text = "Godmode Cheat: " + PlayerController.S.godMode.ToString();

        if (Main.S.winning) {
            win_text.text = "YOU WON! Now please restart the game";
        }
        if (Input.GetKeyDown(KeyCode.RightShift)) {

            togglecounter++;
            print(togglecounter);
            if (PlayerController.S.BowActive && PlayerController.S.BombActive && !PlayerController.S.BoomActive){
                if (PlayerController.S.BowActive && togglecounter == 1) {
                    PlayerController.S.equippedWeapon = WeaponType.bow;

                } else if (PlayerController.S.BombActive && togglecounter == 2) {
                    PlayerController.S.equippedWeapon = WeaponType.bomb;
                    togglecounter = 0;
                }
            }else if (PlayerController.S.BoomActive && PlayerController.S.BombActive && !PlayerController.S.BowActive) {
                if (PlayerController.S.BoomActive && togglecounter == 1) {
                    PlayerController.S.equippedWeapon = WeaponType.boomerang;

                } else if (PlayerController.S.BombActive && togglecounter == 2) {
                    PlayerController.S.equippedWeapon = WeaponType.bomb;
                    togglecounter = 0;
                }
               
                
            }else if(PlayerController.S.BowActive && PlayerController.S.BoomActive && PlayerController.S.BombActive) {
                if (PlayerController.S.BowActive && togglecounter == 1) {
                    PlayerController.S.equippedWeapon = WeaponType.bow;

                } else if (PlayerController.S.BombActive && togglecounter == 2) {
                    PlayerController.S.equippedWeapon = WeaponType.bomb;
                } else if (PlayerController.S.BoomActive && togglecounter == 3) {

                    PlayerController.S.equippedWeapon = WeaponType.boomerang;
                    togglecounter = 0;
                }


            }else { //bomb only

                PlayerController.S.equippedWeapon = WeaponType.bomb;
                togglecounter = 0;
            }


        }
    }
}
