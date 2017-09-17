using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogBubble : MonoBehaviour {

	public float duration = 3.0f;
	public string text = "";
	private Text textUI;

	// Use this for initialization
	void Start () {
		textUI = gameObject.GetComponentInChildren<Text> ();
		Destroy (this.gameObject, duration);
	}
	
	// Update is called once per frame
	void Update () {
		textUI.text = text;
	}
}
