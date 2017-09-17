using UnityEngine;
using System.Collections;

public class ItemObtained : MonoBehaviour {

	private Hero hero;
	private GamePlay gameplay;
	public string itemId;

	private int animCount = 0;
	private int animCountMax = 100;
	private Color heroColor;

	// Use this for initialization
	void Start () {
		gameplay = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GamePlay>(); 
		hero = GameObject.FindGameObjectWithTag ("Hero").GetComponent<Hero> ();
		heroColor = hero.gameObject.GetComponent<SpriteRenderer> ().color;
		hero.gameObject.GetComponent<SpriteRenderer> ().color = new Color(heroColor.r, heroColor.g, heroColor.b, 0f);
		if (GamePlay.playerData.numTreasureChestEncounter == 0) {
			GamePlay.playerData.boredness -= GamePlay.borednessDecreaseOfSeeingTreasureChestFirstTime;
			gameplay.showDialogText ("It's a treasure chest!");
		}
		GamePlay.playerData.numTreasureChestEncounter += 1;
		gameplay.playSFX (GamePlay.treasureAudioClip);
	}
	
	// Update is called once per frame
	void Update () {

		animCount += 1;
		if (animCount >= animCountMax) {
			if (!GamePlay.playerData.numItemEncounter.ContainsKey (itemId)) {
				GamePlay.playerData.numItemEncounter [itemId] = 1;
				gameplay.showDialogText ("I got a " + itemId + "!");
				GamePlay.playerData.boredness -= GamePlay.borednessDecreaseOfSeeingNewItem;
			} else {
				GamePlay.playerData.numItemEncounter [itemId] += 1;
				int count = GamePlay.playerData.numItemEncounter [itemId];
				if (count > 5) {
					GamePlay.playerData.boredness += Mathf.RoundToInt (GamePlay.borednessIncreaseOfSeeingSameItem * (float)count);
				} 
				if (count > 5 && count <= 20) {
					gameplay.showDialogText ("Another " + itemId + "...");
				} else if (count > 10) {
					gameplay.showDialogText ("This game does not have a lot of item types.");
				}
			}
			gameplay.statsIncrease (itemId);
			hero.gameObject.GetComponent<SpriteRenderer> ().color = heroColor;
			hero.turnResume ();
			Destroy (gameObject);
		}

	}
}
