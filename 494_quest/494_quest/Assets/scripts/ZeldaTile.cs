/*
 * A unit of map!
 * 
 * Tiles are atomic pieces of an environment that may be laid out in different patterns to create new and different maps!
 * Zelda uses a number of different tiles, many with different properties (Physical, water, door, etc), to create its overworld and dungeons.
 * 
 * Tiles for the overworld are contained in the Resources/OverworldTileSprites.png file.
 * A Tile-map, useful for recreating the overworld, may be found in the Resources/OverworldTileMap.txt file. 
 * (courtesy of InventWithPython.com) (http://inventwithpython.com/blog/2012/12/10/8-bit-nes-legend-of-zelda-map-data/)
 */

using UnityEngine;
using System;
using System.Collections;

public class ZeldaTile : MonoBehaviour {

	public string tileCode = "00";

	Sprite[] textures;
	string[] names;

	int tileSwitchTimer = 120;

	void InitTileMap()
	{
		textures = Resources.LoadAll<Sprite>("OverworldTileSprites");
		names = new string[textures.Length];
		
		for(int ii=0; ii< names.Length; ii++) {
			names[ii] = textures[ii].name;
		}
	}

	/*
	 * Note: Some tile-names, such as 'overworldSprites_77', are invalid, hence the try-catch block.
	 */
	void RefreshTileSprite () {
		try{
			Sprite sprite = textures[Array.IndexOf(names, "overworldSprites_" + tileCode)];
			GetComponent<SpriteRenderer>().sprite = sprite;
		}
		catch(Exception e)
		{
			RandomizeTileCode();
			RefreshTileSprite();
		}
	}

	void RandomizeTileCode()
	{
		string resultingTileCode = "";

		// Determine first char.
		resultingTileCode += UnityEngine.Random.Range(0, 9).ToString();

		// Determine second char.
		char[] availableChars = {'1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};
		int availableCharsLength = availableChars.Length;
		char secondChar = availableChars[UnityEngine.Random.Range(0, availableCharsLength)];
		resultingTileCode += secondChar;

		print(resultingTileCode);
		tileCode = resultingTileCode;
	}

	// Use this for initialization
	void Start () {
		InitTileMap();
		RandomizeTileCode();
		RefreshTileSprite();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		tileSwitchTimer --;

		if(tileSwitchTimer <= 0)
		{
			tileSwitchTimer = 120;
			RandomizeTileCode();
			RefreshTileSprite();
		}
	}
}
