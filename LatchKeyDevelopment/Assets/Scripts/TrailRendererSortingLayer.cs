using UnityEngine;
using System.Collections;

public class TrailRendererSortingLayer : MonoBehaviour {

	public TrailRenderer trail;
	// Use this for initialization
	void Start () {
		trail = GetComponent<TrailRenderer> ();
		trail.sortingOrder = 3;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
