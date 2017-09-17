using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData {

	public int score;

	public int boredness;
	public int hp;
	public int maxHp;
	public int mp;
	public int maxMp;
	public int atk;
	public int def;

	public Dictionary<string, int> numMonsterEncounter;
	public Dictionary<string, int> numItemEncounter;
	public int numVillageEncounter;
	public int numTreasureChestEncounter;

	public bool batDefeated;
	public bool slimeDefeated;
	public bool yetiDefeated;
	public bool bossDefeated;
}
