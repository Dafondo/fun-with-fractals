﻿using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {

	public int maxDepth;
	public float childScale;
	public Mesh[] meshes;
	public Material material;
	public float spawnProbability;
	public float maxRotationSpeed;
	public float maxTwist;

	private int depth;
	private float rotationSpeed;
	private Material[] materials;
	private static Vector3[] childDirections = {Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back};
	private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler (0f, 0f, -90f),
		Quaternion.Euler (0f, 0f, 90f),
		Quaternion.Euler (90f, 0f, 0f),
		Quaternion.Euler (-90f, 0f, 0f)
	};



	private void InitializeMaterials() {
		materials = new Material[maxDepth + 1];
		for (int i = 0; i <= maxDepth; i++) {
			float t = i / (maxDepth - 1f);
			t *= t;
			materials [i] = new Material (material);
			materials [i].color = Color.Lerp (Color.white, Color.green, t);
		}
		materials [maxDepth].color = Color.blue;
	}

	private void Start() {
		rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
		transform.Rotate (Random.Range (-maxTwist, maxTwist), 0f, 0f);
		if (materials == null) {
			InitializeMaterials ();
		}
		gameObject.AddComponent<MeshFilter> ().mesh = meshes[Random.Range(0, meshes.Length)];
		gameObject.AddComponent<MeshRenderer> ().material = materials[depth];
		if (depth < maxDepth) {
			StartCoroutine (CreateChildren ());
		}
	}

	private void Update() {
		transform.Rotate (0f, rotationSpeed * Time.deltaTime, 0f);
	}

	private void Initialize (Fractal parent, int childIndex) {
		meshes = parent.meshes;
		materials = parent.materials;
		spawnProbability = parent.spawnProbability;
		maxRotationSpeed = parent.maxRotationSpeed;
		maxTwist = parent.maxTwist;
		maxDepth = parent.maxDepth;
		depth = parent.depth + 1;
		childScale = parent.childScale;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
		transform.localRotation = childOrientations[childIndex];
	}

	private IEnumerator CreateChildren() {
		for (int i = 0; i < childDirections.Length; i++) {
			if (Random.value < spawnProbability) {
				yield return new WaitForSeconds (Random.Range (0.1f, 0.5f));
				new GameObject ("Fractal Child").AddComponent<Fractal> ().Initialize (this, i);
			}
		}
	}
}
