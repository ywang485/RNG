using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	public MonsterType monsterType;
	public int hp;
	private GamePlay gamePlay;

	// Use this for initialization
	void Start () {
		if (monsterType == null) {
			monsterType = MonsterDatabase.getMonsterType ("yeti");
		}
		hp = monsterType.getMaxHp ();
		GetComponent<Animator> ().runtimeAnimatorController = Resources.Load (monsterType.getAnimationPath (), typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
		gamePlay = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GamePlay>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy() {
		if (monsterType.getName() == "boss") {
			gamePlay.gameBossKilled ();
		}
	}

	public void beAttacked(int damage) {
		if (damage <= 0) {
			damage = 1;
		}
		hp -= damage;
		if (hp <= 0) {
			Destroy (gameObject);
		}

		GameObject HPBarContent = transform.Find ("HPBarContent").gameObject;
		HPBarContent.transform.localScale = new Vector2 ((float)hp/(float)monsterType.getMaxHp(), 1f);
	}
}
