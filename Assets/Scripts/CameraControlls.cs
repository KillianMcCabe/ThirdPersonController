using UnityEngine;
using System.Collections;

public class CameraControlls : MonoBehaviour {

    public GameObject focus;
    Vector3 focusPole;
    Quaternion cameraRotation;
    //float maxDistanceFromFocus = 5;
    float speed = 120.0f;

    //bool lockedCameraPosition = false;

	// Use this for initialization
	void Start () {
        
        focusPole = new Vector3(-5, 2.5f, 0);

        Vector3 vectorToPlayer = focus.transform.position - transform.position;
        transform.position = transform.position = focus.transform.position + (Quaternion.LookRotation(Vector3.forward) * focusPole);
        transform.rotation = Quaternion.LookRotation(vectorToPlayer.normalized);
    }
	
	// Update is called once per frame
	void Update () {
        //focus = GameObject.Find("Player-FocusTarget");
        
        float horizontal = Input.GetAxis("CameraVertical");
        float vertical = Input.GetAxis("CameraHorizontal");

        
        focusPole = Quaternion.AngleAxis(horizontal * speed * Time.deltaTime, Vector3.up) * focusPole;

        Vector3 newFocusPole = Quaternion.AngleAxis(vertical * speed * Time.deltaTime, transform.right) * focusPole;
        if (Vector3.Dot(Vector3.up, newFocusPole.normalized) <= 0.99f && Vector3.Dot(Vector3.up, newFocusPole.normalized) >= -0.85f)
        {
            focusPole = newFocusPole;
        }
        

        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Terrain");
        if (Physics.Raycast(focus.transform.position, focusPole, out hit, focusPole.magnitude, layerMask))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = focus.transform.position + focusPole;
        }
        transform.LookAt(focus.transform);
    }
}
