/* A component for controlling the visual representation of the player character */

// For an explanation of why separation of aesthetics and behavior is important,
// Read the commentary within EECS494FunBallAesthetics.cs
// - AY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAestheticView : MonoBehaviour
{
    static public PlayerAestheticView S;
    

    /* Inspector Tunables */
    public PlayerController player_controller;
    public int showDamageForFrames = 2;

    public bool __________________________________;
    
    //damage flash
    public Color[] originalColors;
    public Material[] materials;
    public int remainingDamageFrames = 0;

    void Awake()
    {
        S = this;
        //grab original colors to use with damage flashing
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }

        //DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        // The aesthetic object should always represent the location of the player object.
        // Thus, we must move to the location of the player object every frame.
        // - AY
        transform.position = player_controller.transform.position;

        ProcessPlayerDirection();
        ProcessPlayerDamageStatus();

        
    }

    /* TODO: Change player sprite based on the direction the player_controller last moved in */
    void ProcessPlayerDirection()
    {
        
    }

    /* TODO: Check if the player_controller is reporting damage. If so, flash a red color */
    void ProcessPlayerDamageStatus()
    {
        if (player_controller.reportingDamage)
        {
            ShowDamage();
            player_controller.reportingDamage = false;
        }

        //handle damage flashing
        if (remainingDamageFrames > 0)
        {
            remainingDamageFrames--;
            if (remainingDamageFrames == 0)
            {
                UnShowDamage();
            }
        }
    }

    //makes sprite flash as damage indication
    void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        remainingDamageFrames = showDamageForFrames;
    }

    void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
    }
}
