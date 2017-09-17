using UnityEngine;
using System.Collections;

public class MonsterType {

	private string monsterName;
	private int maxHp;
	private int atk;
	private int def;
	private string animationPath;

	public MonsterType(string name, int maxHp, int atk, int def, string path) {
		this.monsterName = name;
		this.maxHp = maxHp;
		this.atk = atk;
		this.def = def;
		this.animationPath = path;
	}

	public string getName() {
		return monsterName;
	}

	public int getMaxHp() {
		return maxHp;
	}

	public int getAtk() {
		return atk;
	}

	public int getDef() {
		return def;
	}

	public string getAnimationPath() {
		return animationPath;
	}
}
