using UnityEngine;
using System.Collections;

public class NewBowScript : MonoBehaviour {
	bool clickedOn;
	public Transform bowButton;
	public float maxStretch;
	public float minArrowSpeed = 1.0f;
	public GameObject arrow;
	public float arwSpeedModifier;
	public Transform arrowSpawn;
	public float bowReturnSpeed;

	//TODO LINK CHARACTER CONTROLER
	bool facingRight = true;


	float stretchStr;
	float weight = 0;	

	Vector2 bowToMouse;
	Vector3 fireDirection;
	Vector3 bowButtonOrigin;
	Ray rayToMouse;
	float maxStretchSqr;

	// Use this for initialization
	void Start () {
		fireDirection = Vector3.zero;
		bowButtonOrigin = bowButton.localPosition;
		//Debug.Log ("Button origin_" + bowButtonOrigin);
		clickedOn = false;
		maxStretchSqr = maxStretch * maxStretch;
	}
	
	// Update is called once per frame
	void Update () {
		if (clickedOn) {
			Dragin ();
			//	LineRendererUpdate ();
		} else {
			if(bowButton.localPosition != bowButtonOrigin){
				ReturnButtonToOrigin();
			}
		}
	}
	void ReturnButtonToOrigin()
    {
		//weight += Time.deltaTime * (stretchStr+minArrowSpeed); //amount
		weight += Time.deltaTime * bowReturnSpeed;
		bowButton.localPosition = Vector3.Lerp (bowButton.localPosition, bowButtonOrigin, weight);
	}
	void Dragin()
    {
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - transform.position;
		bowToMouse = catapultToMouse.normalized;
		if (catapultToMouse.sqrMagnitude > maxStretchSqr) {
			stretchStr = maxStretch;
			rayToMouse = new Ray (transform.position, Vector3.zero);
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint (maxStretch);
		} else
			stretchStr = catapultToMouse.magnitude;

		mouseWorldPoint.z = 0.0f;
		bowButton.position = mouseWorldPoint;
		UpdateBowRotation ();
	}
	void UpdateBowRotation()
    {
		Vector2 measure = facingRight ? Vector2.right : Vector2.left;
		//transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, rot));
		transform.localRotation = Quaternion.FromToRotation (measure, -bowToMouse);
	}
	void OnMouseDown()
    {
		Debug.Log ("clicked");
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		if(hit.collider!=null){
			Debug.Log("clicked collider");
			if(hit.collider.transform.tag == "Bow"){
				Debug.Log("clickedonBow");
				clickedOn = true;
			}
		}
	}
	void OnMouseUp(){
		if (clickedOn) {
			fireDirection = transform.position - bowButton.position;
			fireDirection.z = 0;
			fireDirection = fireDirection.normalized;
			//Debug.Log ("Fire Direction" + fireDirection);
			weight =0; 
			clickedOn = false;
            FireArrow();

        }
	}
    void FireArrow()
    {
        GameObject arw = Instantiate(arrow, arrowSpawn.position, Quaternion.identity) as GameObject;
        Rigidbody2D rbdArw = arw.GetComponent<Rigidbody2D>();
        rbdArw.velocity = new Vector2(fireDirection.x, fireDirection.y) * arwSpeedModifier * ((stretchStr + minArrowSpeed) / 2.0f);
    }
   
}
