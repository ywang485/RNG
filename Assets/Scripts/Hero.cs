using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour{

	private GamePlay gameplay;
	public int stepCount;
	public int baseStepCountMax = 5;
	public Mesh targetBlock;
	public Mesh currBlock;
	public float speed = 1f;
	private List<Mesh> visited;
	private bool inEvent = false;
	private string lastEncounter = "";
	private string currEncounter = "";
	private Vector3 prevLoc;

	private AudioSource audioSrc;

	public Mesh getCurrBlock() {
		return currBlock;
	}

	public void turnInterrupt() {
		inEvent = true;
		audioSrc.Stop ();
	}

	public void turnResume() {
		stepCount = 0;
		targetBlock = null;
		inEvent = false;
		audioSrc.Play ();
	}

	void Awake() {
		gameplay = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GamePlay>();
		audioSrc = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		stepCount = 0;
		currBlock = GameObject.Find ("Mesh-3,3").GetComponent<Mesh>();
		transform.position = currBlock.gameObject.transform.position;
		visited = new List<Mesh>();
		visited.Add (currBlock);
		audioSrc.Play ();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Mesh") {
			currBlock = other.gameObject.GetComponent<Mesh> ();
		}
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (inEvent) {
			if (other.tag == "Mesh") {
				currBlock = other.gameObject.GetComponent<Mesh> ();
			}
			return;
		}
		if (other.tag == "Monster") {
			lastEncounter = currEncounter;
			currEncounter = other.GetComponent<Monster> ().monsterType.getName();
			turnInterrupt ();
			GameObject battleObj = Instantiate (gameplay.Battle);
			battleObj.GetComponent<Battle> ().monster = other.GetComponent<Monster> ();
			battleObj.GetComponent<Battle> ().started = true;
		} else if (other.tag == "Treasure") {
			turnInterrupt ();
			lastEncounter = currEncounter;
			currEncounter = other.GetComponent<TreasureChest> ().itemId;
			Destroy (other.gameObject);
			GameObject itemObjtainedObj = Instantiate (Resources.Load (GamePlay.itemObtainedPrefab, typeof(GameObject)) as GameObject, transform.position, Quaternion.identity) as GameObject;
			itemObjtainedObj.GetComponent<ItemObtained> ().itemId = other.GetComponent<TreasureChest> ().itemId;

		} else if (other.tag == "Village") {
			turnInterrupt ();
			GameObject restObj = Instantiate (Resources.Load (GamePlay.restPrefab, typeof(GameObject)) as GameObject, transform.position, Quaternion.identity) as GameObject;
		}

		if ((other.tag == "Monster" || other.tag == "Treasure") && lastEncounter == currEncounter) {
			gameplay.showDialogText ("Not " + currEncounter + " again...");
			GamePlay.playerData.boredness += GamePlay.borednessIncreaseOfSameEncounterInARow;
		}
	}

	// Update is called once per frame
	void Update () {
		if (inEvent || stepCount >= baseStepCountMax) {
			return;
		}
		if (targetBlock == null) {
			int r = Random.Range (0, currBlock.neighbours.Length - 1);
			Mesh tmp = currBlock.neighbours[r];
			int tries = 1;
			while (tries < 5 && visited.Contains(tmp)) {
				r = Random.Range (0, currBlock.neighbours.Length - 1);
				tmp = currBlock.neighbours[r];
				tries += 1;
			}
			targetBlock = tmp;
		}

		Vector2 currPos = new Vector2 (transform.position.x, transform.position.y);
		transform.position = Vector2.MoveTowards(currPos, targetBlock.gameObject.transform.position, speed * Time.deltaTime);

		if (currBlock == targetBlock) {
			visited.Add (currBlock);
			stepCount += 1;
			targetBlock = null;
			if (stepCount >= baseStepCountMax) {
				stepCount = 0;
				visited.Clear ();
				// Event
				gameplay.playEncounterAnimation(currBlock);
				gameplay.playSFX (GamePlay.encounterAudioClip);
				turnInterrupt ();
			}
		}
	}

	public void beAttacked(int damage) {
		if (damage <= 0) {
			damage = 1;
		}
		GamePlay.playerData.hp -= damage;
		if (GamePlay.playerData.hp <= 0) {
			// Game Over
			gameplay.playerKilled();
		}
	}
}
