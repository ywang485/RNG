using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GamePlay : MonoBehaviour {

	public static readonly int borednessDecreaseOfSeeingNewMonster = 2;
	public static readonly float bordnessIncreaseOfSeeingOldMonster = 0.3f;
	public static readonly int borednessDecreaseOfSeeingVillageFirstTime = 1;
	public static readonly float bordnessDecreaseOfHavingBattle = 4f;
	public static readonly int borednessIncreaseOfEasyFight = 2;
	public static readonly int borednessDecreaseOfSeeingTreasureChestFirstTime = 2;
	public static readonly int borednessDecreaseOfSeeingNewItem = 1;
	public static readonly float borednessIncreaseOfSeeingSameItem = 0.1f;
	public static readonly int borednessIncreaseOfSameEncounterInARow = 3;

	private Text statsDisplay;
	private Text borednessDisplay;
	public static PlayerData playerData;
	private GameObject canvas;
	public GameObject encounter;
	public GameObject dialogBubble;
	public GameObject Battle;
	public GameObject encounterQuery;
	public GameObject gameOverPanel;

	private Hero hero;

	static readonly public string restPrefab = "Prefab/rest";
	static readonly public string sleepPrefab = "Prefab/sleep";
	static readonly public string screenFlashingPrefab = "Prefab/flashing";
	static readonly public string itemObtainedPrefab = "Prefab/ItemObtained";

	static readonly public string monsterPrefab = "Prefab/monster";
	static readonly public string villagePrefab = "Prefab/village";
	static readonly public string treasureChestPrefab = "Prefab/TreasureChest";

	static readonly public string selectAudioClip = "SFX/select";
	static readonly public string hitAudioClip = "SFX/hit";
	static readonly public string hit2AudioClip = "SFX/hit2";
	static readonly public string encounterAudioClip = "SFX/encounter";
	static readonly public string treasureAudioClip = "SFX/treasure";

	private int waitCount;
	private int waitCountMax = 200;
	private bool waiting = false;
	private delegate void waitCallback();
	private waitCallback wcb;

	private AudioSource audioSrc;

	public void playSFX(string SFXPath) {
		audioSrc.PlayOneShot (Resources.Load(SFXPath, typeof(AudioClip)) as AudioClip);
	}

	// Use this for initialization
	void Start () {
		audioSrc = GetComponent<AudioSource> ();
		gameOverPanel.SetActive (false);
		playerData = new PlayerData();

		// Intialize player data
		playerData.score = 0;
		playerData.boredness = 5;
		playerData.hp = 10;
		playerData.maxHp = 10;
		playerData.mp = 5;
		playerData.maxMp = 5;
		playerData.atk = 3;
		playerData.def = 2;
		playerData.numItemEncounter = new Dictionary<string, int>();
		playerData.numMonsterEncounter = new Dictionary<string, int> ();
		playerData.numVillageEncounter = 0;
		playerData.numTreasureChestEncounter = 0;

		statsDisplay = GameObject.Find ("StatsDisplay").GetComponent<Text>();
		borednessDisplay = GameObject.Find ("BorednessDisplay").GetComponent<Text>();
		Random.seed = (int)System.DateTime.Now.Ticks;
		canvas = GameObject.Find ("Canvas");

		hero = GameObject.FindGameObjectWithTag ("Hero").GetComponent<Hero> ();

		showDialogText ("Hope this is not just another grindy RPG.");
		hero.turnInterrupt ();
		wcb = resumeHero;
		statsDisplay.text = "HP: " + playerData.hp + "/" + playerData.maxHp + "\n" +
			"ATK: " + playerData.atk + "\n" +
			"DEF: " + playerData.def;

		//borednessDisplay.text = "Boredness: " + playerData.boredness;
		GameObject BorednessBarContent = GameObject.Find ("BorednessBarContent").gameObject;
		BorednessBarContent.transform.localScale = new Vector2 ((float)playerData.boredness/(float)10.0f, 1f);
		waiting = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (waiting) {
			if (waitCount >= waitCountMax) {
				waiting = false;
				waitCount = 0;
				wcb ();
			}
			waitCount += 1;
			return;
		}

		if (playerData.boredness < 0) {
			playerData.boredness = 0;
		}

		statsDisplay.text = "HP: " + playerData.hp + "/" + playerData.maxHp + "\n" +
		"ATK: " + playerData.atk + "\n" +
		"DEF: " + playerData.def;

		//borednessDisplay.text = "Boredness: " + playerData.boredness;
		GameObject BorednessBarContent = GameObject.Find ("BorednessBarContent").gameObject;
		BorednessBarContent.transform.localScale = new Vector2 ((float)playerData.boredness/(float)10.0f, 1f);

		if (playerData.boredness >= 10) {
			showDialogText ("This game is too boring. Going to watch TV instead...");
			wcb = gameOver;
			waiting = true;
		}
	}

	public void resumeHero() {
		hero.turnResume ();
	}

	public void gameBossKilled() {
		//gameOverPanel.SetActive (true);
		hero.turnInterrupt();
		showDialogText ("I beat the game!");
		wcb = gameBeaten;
		waiting = true;
	}

	public void playerKilled() {
		gameOverPanel.SetActive (true);
		hero.turnInterrupt();
		showDialogText ("This game is too hard...Not ganna try again.");
		wcb = gameOver;
		waiting = true;
	}

	public void gameOver() {
		SceneManager.LoadScene ("GameOver");
	}

	public void gameBeaten() {
		SceneManager.LoadScene ("GameBeaten");
	}

	public void showDialogText(string text) {
		GameObject dialog = Instantiate (dialogBubble, new Vector2(-75, -100), Quaternion.identity) as GameObject;
		dialog.transform.SetParent (canvas.transform,false);
		dialog.GetComponent<DialogBubble> ().text = text;
	}

	public void playEncounterAnimation(Mesh loc) {
		GameObject encounterObj = Instantiate (encounter, loc.gameObject.transform.position, Quaternion.identity) as GameObject;
		GameObject.Destroy (encounterObj, 1f);
	}

	public void screenflash() {
		GameObject flashing = Instantiate (Resources.Load (screenFlashingPrefab, typeof(GameObject)) as GameObject);
		flashing.transform.SetParent (canvas.transform,false);
		Destroy (flashing, 0.3f);
	}

	public GameObject showSleepSymbol(Vector2 loc) {
		GameObject zzz = Instantiate (Resources.Load (sleepPrefab, typeof(GameObject)) as GameObject, loc, Quaternion.identity) as GameObject;
		return zzz;
	}

	public void statsIncrease(string itemId) {
		InventoryItem item = ItemDatabase.items [itemId];
		playerData.atk += item.getAtk ();
		playerData.def += item.getDef ();
		playerData.maxHp += item.getMaxHp ();
	}

	public void showEncounterQuery() {

		encounterQuery.SetActive (true);
		
	}

	public void hideEncounterQuery() {
		encounterQuery.SetActive (false);
	}

	public GameObject placeTile(GameObject tile, Vector2 loc) {
		GameObject placedTile = Instantiate (tile, loc, Quaternion.identity) as GameObject;
		playSFX (selectAudioClip);
		return placedTile;
	}

	public void placeMonster(string monsterid) {
		GameObject monsterObj = placeTile (Resources.Load (monsterPrefab, typeof(GameObject)) as GameObject, hero.getCurrBlock ().transform.position);
		monsterObj.GetComponent<Monster> ().monsterType = MonsterDatabase.getMonsterType (monsterid);
		hideEncounterQuery ();
		hero.turnResume ();
	}

	public void placeVillage() {
		placeTile (Resources.Load (villagePrefab, typeof(GameObject)) as GameObject, hero.getCurrBlock ().transform.position);
		hideEncounterQuery ();
		hero.turnResume ();
	}

	public void placeTreasureChest(string itemId) {
		GameObject tcObj = placeTile (Resources.Load (treasureChestPrefab, typeof(GameObject)) as GameObject, hero.getCurrBlock ().transform.position);
		tcObj.GetComponent<TreasureChest> ().itemId = itemId;
		hideEncounterQuery ();
		hero.turnResume ();
	}
}
