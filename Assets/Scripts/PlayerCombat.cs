using UnityEngine;
using System.Collections;

using Xft;

public class PlayerCombat : MonoBehaviour {


    [SerializeField]
    Collider weaponCollider;

    XWeaponTrail weaponTrail;
    

    // Use this for initialization
    void Start () {
        weaponTrail = GetComponentInChildren<XWeaponTrail>();
        weaponTrail.Init();
        weaponTrail.Deactivate();

        weaponCollider.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SwingStart()
    {
        weaponTrail.Activate();
        weaponCollider.enabled = true;
    }

    public void SwingEnd()
    {
        weaponTrail.StopSmoothly(0.3f);
        weaponCollider.enabled = false;
    }
    
}
