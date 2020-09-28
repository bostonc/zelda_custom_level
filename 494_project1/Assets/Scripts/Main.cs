using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    public bool winning = false;
    static public Main S;
    static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;
    public WeaponDefinition[] weaponDefinitions;
    //public WeaponType[] powerUpFrequency = new WeaponType[] {WeaponType.sword,
                                                             //WeaponType.b,
                                                            //WeaponType.spread,
                                                             //WeaponType.shield };

    public GameObject keyPrefab;
    public GameObject heartPrefab;
    public GameObject bombPrefab;
    public GameObject rupeePrefab;
    public Tile tilePrefab;

    public bool _________________________________;

    public GameObject block1;
    public GameObject block2;
    public GameObject pausedText;

    public bool allItems = false;
    public bool paused = false;
    public bool bossAlive = true;
    public int greenDoorsOpened = 0;

    private void Awake(){
        S = this;

        //A generic Dictionary with WeaponType as the key
             W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
             foreach (WeaponDefinition def in weaponDefinitions) {
             W_DEFS[def.type] = def;
        }

        block1 = GameObject.Find("FirstBlock1");
        block2 = GameObject.Find("FirstBlock2");
        if (block1 != null)
        {
            block1.SetActive(false);
            block2.SetActive(false);
        }
    }

   static public WeaponDefinition GetWeaponDefinition(WeaponType wt) {
        //check to make sure key exists in dictionary, non exist would throw error 
        if (W_DEFS.ContainsKey(wt)) {
            return W_DEFS[wt];
        }
        ///this returns none, showing it fialed 
        return new WeaponDefinition();

    }

    // Use this for initialization
    void Start ()
    {       
        //edit classic map (creates background floor underneath push block)
        if (SceneManager.GetActiveScene().name == "Dungeon")
        {
            ShowMapOnCamera.MAP[22, 60] = 029;
            ShowMapOnCamera.MAP[23, 38] = 029;
        }

        pausedText = GameObject.Find("Paused_text");
        pausedText.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        //boss stuff in custom level
        GameObject go = null;
        go = GameObject.FindWithTag("Boss");
        if (go == null && bossAlive)
        {
            open_door_on_boss_death();
            bossAlive = false;
        }

        //restart button
        if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.R))
        {
            foreach (var item in CameraPoint.cameraPoints) {
                //Destroy(item);
            }
            CameraPoint.cameraPoints.Clear(); 
            
            SceneManager.LoadScene("Dungeon");
        }

        //jump to custom
        if (Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("Dungeon_Custom");
            CameraPoint.cameraPoints.Clear();
        }

        //full ammo
        if (Input.GetKeyDown(KeyCode.F4) || Input.GetKeyDown(KeyCode.F))
        {
            PlayerController.S.keys = 9999;
            PlayerController.S.bombs = 9999;
            PlayerController.S.rupees = 9999;
        }

        //all items toggle
        if (Input.GetKeyDown(KeyCode.F5) || Input.GetKeyDown(KeyCode.I))
        {
            if (allItems)
            {
                PlayerController.S.BowActive = false;
                PlayerController.S.PickaxeActive = false;
                PlayerController.S.BoomActive = false;
                allItems = false;
            }
            else
            {
                PlayerController.S.BowActive = true;
                PlayerController.S.PickaxeActive = true;
                PlayerController.S.BoomActive = true;
                allItems = true;
            }
        }

        //pause
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (paused)
            {
                Time.timeScale = 1;
                paused = false;
                pausedText.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                paused = true;
                pausedText.SetActive(true);
            }
        }

    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        
        //SceneManager.LoadScene("Dungeon");
    }

    public void EnemyDestroyed(Enemy e) {

        //potentially generate a powerup
        if (e.dropKey == true) {
             //spawn a key
            GameObject go = Instantiate(keyPrefab) as GameObject;

            //set it to the position of the destoryed ship
            go.transform.position = e.transform.position;
        }
        if (e.dropRupee == true) {
            //spawn a key
            GameObject go = Instantiate(rupeePrefab) as GameObject;

            //set it to the position of the destoryed ship
            go.transform.position = e.transform.position;
        }
        if (e.dropBomb == true) {
            //spawn a key
            GameObject go = Instantiate(bombPrefab) as GameObject;

            //set it to the position of the destoryed ship
            go.transform.position = e.transform.position;
        }
        if (e.dropHeart== true) {
            //spawn a key
            GameObject go = Instantiate(heartPrefab) as GameObject;

            //set it to the position of the destoryed ship
            go.transform.position = e.transform.position;
        }
    }

    //call me when boss in custom level is dead.
    public void open_door_on_boss_death()
    {
        GameObject oldDoor = GameObject.FindWithTag("finalDoor");
        ShowMapOnCamera.MAP[33, 60] = 051;
        Destroy(oldDoor);        
    }

    //call me when a green block runs into a target.
    //MUST BE CALLED IN MAP ORDER OR WEIRD THINGS WILL HAPPEN.
    public void open_green_door()
    {
        if (greenDoorsOpened >= 2) return;

        GameObject[] greenDoors = GameObject.FindGameObjectsWithTag("greenDoor");
        GameObject oldDoor1 = null;
        GameObject oldDoor2 = null; //only used sometimes
        switch(greenDoorsOpened)
        {
            case 0:
                for (int i = 0; i < greenDoors.Length; i++)
                {
                    oldDoor1 = greenDoors[i];
                    if (oldDoor1.transform.position.x == 46 
                        && oldDoor1.transform.position.y == 27)
                    {
                        break;
                    }
                }
                ShowMapOnCamera.MAP[46, 27] = 048;
                Destroy(oldDoor1);
                break;
            case 1:
                for (int i = 0; i < greenDoors.Length; i++)
                {
                    if (greenDoors[i].transform.position.x == 39
                        && greenDoors[i].transform.position.y == 53)
                    {
                        oldDoor1 = greenDoors[i];
                    }
                    if (greenDoors[i].transform.position.x == 40
                        && greenDoors[i].transform.position.y == 53)
                    {
                        oldDoor2 = greenDoors[i];
                    }
                }
                ShowMapOnCamera.MAP[39, 53] = 092;
                Destroy(oldDoor1);
                ShowMapOnCamera.MAP[40, 53] = 093;                
                Destroy(oldDoor2);
                break;
            default:
                break;
        }
        greenDoorsOpened++;
    }



}
