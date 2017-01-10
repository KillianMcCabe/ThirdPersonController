using UnityEngine;
using System.Collections;

public class Dummy : MonoBehaviour {

    HingeJoint[] joints;
    [SerializeField]
    Transform hitPos;

    public bool isDead = false;
    float reviveTime = 5;
    int hitsTilDeath = 3;
    public float timeSinceDied = 0;
    public int hitCount = 0;

	float invincibilityTime = 0.65f;
    public float timeSinceHit = 1000;

    // Use this for initialization
    void Start () {
        joints = GetComponentsInChildren<HingeJoint>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (hitCount >= hitsTilDeath && !isDead)
        {
            foreach(HingeJoint h in joints)
            {
                h.useLimits = false;
            }

            isDead = true;
            timeSinceDied = 0;
        }

        if (isDead)
        {
            if (timeSinceDied > reviveTime)
            {
                isDead = false;
                hitCount = 0;
                foreach (HingeJoint h in joints)
                {
                    h.useLimits = true;
                }
            }
            else
            {
                timeSinceDied += Time.deltaTime;
            }
        }

        timeSinceHit += Time.deltaTime;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttack" && timeSinceHit > invincibilityTime)
        {
            hitCount++;
            Instantiate(Resources.Load("SlashEffect"), hitPos.transform.position, Quaternion.identity);
            timeSinceHit = 0;
        }
        
    }
}
