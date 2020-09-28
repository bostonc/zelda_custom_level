/*
 * The HUD component manages the data and display of the user interface.
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Hud : MonoBehaviour {

	public GameObject heartPrefab;
	public static List<GameObject> heartImages = new List<GameObject>();
	public GameObject rupeeCountText;

	private static Hud instance;

	void Awake () {
		instance = this;
	}

	/*
	 * Refresh the HUD display to ensure it reflects the current status of the game.
	 */
	public static void RefreshDisplay()
	{
		int diff = Player.instance.health - heartImages.Count;
		int absVal = Mathf.Abs(diff);

		// Heart display
		for(int i = 0; i < absVal; i++)
		{
			// Add hearts.
			if(diff > 0)
			{
				GameObject newHeart = Instantiate(instance.heartPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				newHeart.transform.SetParent(instance.gameObject.transform);
				newHeart.GetComponent<RectTransform>().anchoredPosition = new Vector3(heartImages.Count * 30, 0, 0);
				heartImages.Add(newHeart);
			}

			// Remove hearts.
			else if (diff < 0)
			{
				GameObject heartToRemove = heartImages[heartImages.Count-1];
				heartImages.Remove(heartToRemove);
				Destroy(heartToRemove);
			}
		}

		// Rupee Display
		instance.rupeeCountText.GetComponent<Text>().text = Player.rupeeCount.ToString();
	}
}
