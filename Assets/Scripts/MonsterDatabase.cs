using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterDatabase{

	static private readonly Dictionary<string, MonsterType> monsterTypes = new Dictionary<string, MonsterType>{
		{"bat", new MonsterType("bat", 5, 3, 2, "Animation/Bat")},
		{"slime", new MonsterType("slime", 20, 5, 3, "Animation/Slime")},
		{"yeti", new MonsterType("yeti", 40, 7, 7, "Animation/Yeti")},
		{"boss", new MonsterType("boss", 70, 10, 10, "Animation/Boss")}
	};

	static public MonsterType getMonsterType(string id) {
		return monsterTypes [id];
	}
}
