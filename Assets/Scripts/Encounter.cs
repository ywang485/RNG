using UnityEngine;
using System.Collections;

public class Encounter : MonoBehaviour {

	private GamePlay gameplay;

	// Use this for initialization
	void Start () {
		gameplay = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GamePlay>();
	}
	
	// Update is called once per frame
	void OnDestroy () {
		gameplay.showEncounterQuery ();
	}
}
