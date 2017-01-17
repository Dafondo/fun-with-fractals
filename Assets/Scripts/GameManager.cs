using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Fractal fractalPrefab;

	private Fractal fractalInstance;

	// Use this for initialization
	void Start () {
		BeginGame ();
	}
		
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			RestartGame ();
		}
	}

	private void BeginGame() {
		fractalInstance = Instantiate (fractalPrefab) as Fractal;
	}

	private void RestartGame() {
		StopAllCoroutines ();
		Destroy (fractalInstance.gameObject);
		BeginGame ();
	}
}
