using UnityEngine;
using System.Collections;

public class Battle : MonoBehaviour {

	private Hero hero;
	public Monster monster;
	private bool isHerosTurn = false;
	private int bumpingCount;
	public int maxBumpingCount = 20;
	public float bumpingSpeed = 2f;
	public bool started = false;

	private int waitCount;
	private int waitCountMax = 30;
	private bool wait = false;

	private Vector2 hero_position;
	private Vector2 monster_position;
	private string monsterName;

	private int originalHp;

	private GamePlay gameplay;

	// Use this for initialization
	void Start () {

		originalHp = GamePlay.playerData.hp;
		monsterName = monster.monsterType.getName ();
		bumpingCount = 0;
		hero = GameObject.FindGameObjectWithTag ("Hero").GetComponent<Hero> ();
		hero_position = hero.gameObject.transform.position;
		monster_position = monster.gameObject.transform.position;
		gameplay = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GamePlay> ();

		if (!GamePlay.playerData.numMonsterEncounter.ContainsKey(monster.monsterType.getName ())) {
			gameplay.showDialogText ("Wow...it's a " + monster.monsterType.getName () + ".");
			GamePlay.playerData.numMonsterEncounter [monster.monsterType.getName ()] = 1;
			GamePlay.playerData.boredness -= GamePlay.borednessDecreaseOfSeeingNewMonster;
		} else {
			GamePlay.playerData.numMonsterEncounter [monster.monsterType.getName ()] += 1;
			int count = GamePlay.playerData.numMonsterEncounter [monster.monsterType.getName ()];
			if (count > 5) {
				GamePlay.playerData.boredness += Mathf.RoundToInt(GamePlay.bordnessIncreaseOfSeeingOldMonster * (float)count);
			} 
			if (count > 5 && count <= 20) {
				gameplay.showDialogText (monster.monsterType.getName () + " again?");
			} else if (count > 10) {
				gameplay.showDialogText ("No more " + monster.monsterType.getName () + " please.");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (!started) {
			return;
		}

		if (wait) {
			waitCount += 1;
			if (waitCount >= waitCountMax) {
				waitCount = 0;
				wait = false;
			}
			return;
		}
		if (monster == null) {
			float HpGone = (1f - (float)GamePlay.playerData.hp / (float)originalHp);
			int borednessDec = Mathf.FloorToInt (GamePlay.bordnessDecreaseOfHavingBattle * HpGone);
			GamePlay.playerData.boredness -= borednessDec;
			if (HpGone > 0.8f) {
				gameplay.showDialogText ("It almost got me!");
			} else if (HpGone > 0.5f) {
				gameplay.showDialogText ("It was close.");
			} else if (HpGone < 0.1f) {
				gameplay.showDialogText ("It was too easy.");
				GamePlay.playerData.boredness += GamePlay.borednessIncreaseOfEasyFight;
			}
			if (monsterName == "bat" && !GamePlay.playerData.batDefeated) {
				GamePlay.playerData.batDefeated = true;
			} else if (monsterName == "slime" && !GamePlay.playerData.slimeDefeated) {
				GamePlay.playerData.slimeDefeated = true;
			} else if (monsterName == "yeti" && !GamePlay.playerData.yetiDefeated) {
				GamePlay.playerData.yetiDefeated = true;
			} else if (monsterName == "boss" && !GamePlay.playerData.bossDefeated) {
				GamePlay.playerData.bossDefeated = true;
			}
			hero.turnResume ();
			Destroy (gameObject);
			return;
		}
		bumpingCount += 1;
		if (bumpingCount >= maxBumpingCount) {
			if (isHerosTurn) {
				if (GamePlay.playerData.hp > 0) {
					monster.beAttacked (GamePlay.playerData.atk - monster.monsterType.getDef ());
					hero.transform.position = hero_position;
					isHerosTurn = false;
					gameplay.playSFX (GamePlay.hit2AudioClip);
				}
			} else {
				if (GamePlay.playerData.hp > 0) {
					hero.beAttacked (monster.monsterType.getAtk () - GamePlay.playerData.def);
					monster.transform.position = monster_position;
					gameplay.screenflash ();
					gameplay.playSFX (GamePlay.hitAudioClip);
				}
				isHerosTurn = true;
			}

			bumpingCount = 0;
			wait = true;
		} else {
			if (isHerosTurn) {
				Vector2 currPos = new Vector2 (hero.transform.position.x, hero.transform.position.y);
				hero.transform.position = Vector2.MoveTowards (currPos, monster.gameObject.transform.position, bumpingSpeed * Time.deltaTime);
			} else {
				Vector2 currPos = new Vector2 (monster.transform.position.x, monster.transform.position.y);
				monster.transform.position = Vector2.MoveTowards (currPos, hero.gameObject.transform.position, bumpingSpeed * Time.deltaTime);
			}
		}
			
	}
}
