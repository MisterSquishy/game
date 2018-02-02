using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exciting_Object : MonoBehaviour {

    [Tooltip("How exciting this object is to Scout (1-10)")]
    public float ExcitementLevel = 0f;
    public bool CanBePickedUp = false;
    public bool CanBeChewed = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
