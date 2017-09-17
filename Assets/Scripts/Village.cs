using UnityEngine;
using System.Collections;

public class Village : MonoBehaviour {

	private Hero hero;
	private GamePlay gameplay;
	private GameObject zzz;

	private int sleepCount;
	private int sleepCountMax = 200;
	private Color heroColor;

	// Use this for initialization
	void Start () {
		gameplay = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GamePlay>(); 
		hero = GameObject.FindGameObjectWithTag ("Hero").GetComponent<Hero> ();
		zzz = gameplay.showSleepSymbol (transform.position);
		heroColor = hero.gameObject.GetComponent<SpriteRenderer> ().color;
		hero.gameObject.GetComponent<SpriteRenderer> ().color = new Color(heroColor.r, heroColor.g, heroColor.b, 0f);
		if (GamePlay.playerData.numVillageEncounter == 0) {
			GamePlay.playerData.boredness -= GamePlay.borednessDecreaseOfSeeingVillageFirstTime;
			gameplay.showDialogText ("A village!");
		}
		GamePlay.playerData.numVillageEncounter += 1;
	}
	
	// Update is called once per frame
	void Update () {

		sleepCount += 1;
		if (sleepCount >= sleepCountMax) {
			GamePlay.playerData.hp = GamePlay.playerData.maxHp;
			Destroy (zzz);
			hero.gameObject.GetComponent<SpriteRenderer> ().color = heroColor;
			hero.turnResume ();
			Destroy (gameObject);
		}

	}
}
