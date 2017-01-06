using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour {

    public Collider weaponCollider;

	// Use this for initialization
	void Start () {
        weaponCollider.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SwingStart()
    {
        print("SwingStart");
        weaponCollider.enabled = true;
    }

    public void SwingEnd()
    {
        print("SwingEnd");
        weaponCollider.enabled = false;
    }
}
