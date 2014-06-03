using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class OBGroup : MonoBehaviour {

	/* public declarations */
	public GameObject OBBadgePrefab;

	/* private declarations */
	private JSONNode my;
	private GameObject[] badges;

	/** 
	 * public initialise_group(JSONNode group) initalises the badges found in the JSON of the group
	 * Params: JSONNode group: The JSON object containing this groups badge information
	 * Returns: void
	 */
	public void initialise_group(JSONNode group) {
		my = group;
		List<GameObject> temp = new List<GameObject> ();
		int i = 0;
		foreach (JSONNode badge in my["badges"].AsArray) {
			//Instantiate new OBBadge based on the badge JSON object
			temp.Add ( (GameObject) Instantiate (OBBadgePrefab, new Vector3(transform.position.x, transform.position.y + i, transform.position.z), transform.rotation));
			temp[i].transform.parent = transform;
			temp[i].GetComponent<OBBadge> ().initialise_badge (badge);
			++i;
		}
		badges = temp.ToArray();
		Debug.Log ("Finished Initialising OBGroup");
		Debug.Log (badges.ToString ());
	}
}
