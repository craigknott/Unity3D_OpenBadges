using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

/**
 * class OBUser: Handles the fetching of user badge information including userId, groupId's and individual badge information
 * as JSON objects.
 */
public class OBUser : MonoBehaviour {

	/* Public Declarations */
	public GameObject OBGroupPrefab;
	public string userEmail;

	/* Private Declarations */
	private JSONNode emailJSON, groupsJSON, badgesJSON;
	
	/**
	 * Default Start function run when object is first created, initialises this user based on the public email.
	 * Params: None
	 * Returns: None
	 */
	void Start() {
		initialise_user (userEmail);
	}

	/**
	 * private get_uid(string email) Sets the emailJSON private to be the JSON string containing the userId corresponding to the email.
	 * Params: String email
	 * Returns: IEnumerator
	 */
	private IEnumerator get_uid(string email) {
		WWWForm wwf = new WWWForm ();
		wwf.AddField ("email", email);
		byte[] postData = wwf.data;

		WWW emailReturn = new WWW ("http://backpack.openbadges.org/displayer/convert/email", postData);

		yield return emailReturn;

		emailJSON = JSON.Parse (emailReturn.text);
		Debug.Log ("emailJSON = " + emailJSON.ToString());
	}

	/**
	 * private get_groups(string uid) Sets the groupsJSON private to be the JSON string containing the userId public badge groups.
	 * Params: String uid
	 * Returns: IEnumerator
	 */
	private IEnumerator get_groups(string uid) {
		WWW groupsReturn = new WWW ("http://backpack.openbadges.org/displayer/" + uid + "/groups.json");
		
		yield return groupsReturn;

		groupsJSON = JSON.Parse (groupsReturn.text);
		Debug.Log ("groupsJSON = " + groupsJSON.ToString());
	}

	/**
	 * private get_badges(string) sets the badgesJSON private to be the JSON string returned from the userId and groupId specified.
	 * Params: String ui, String gid
	 * Returns: IEnumerator
	 */
	private IEnumerator get_badges(string uid, string gid) {
		WWW badgesReturn = new WWW ("http://backpack.openbadges.org/displayer/" + uid + "/group/" + gid + ".json");

		yield return badgesReturn;

		badgesJSON = JSON.Parse (badgesReturn.text);
		Debug.Log ("badgesJSON = " + badgesJSON.ToString());
	}

	/**
	 * private download_user_backpack(string) Runs the web fetching functions in order as coroutines.
	 * Params: String email
	 * Returns: IEnumerator
	 */ 
	private IEnumerator download_user_backpack(string email) {
		//Run the get_uid coroutine
		yield return StartCoroutine( get_uid (email));
		//Run the get_groups coroutine after the previous one completes
		yield return StartCoroutine( get_groups (emailJSON ["userId"]));
		//Run the rest of get_badges after the other two coroutines finish
		List<GameObject> temp = new List<GameObject> ();
		int i = 0;
		foreach (JSONNode group in groupsJSON["groups"].AsArray) {
			yield return StartCoroutine (get_badges (emailJSON ["userId"], group["groupId"]));
			//Instantiate new OBBadge based on the badge JSON object
			Debug.Log ("Badge Group = " + group.ToString ());
			temp.Add ( (GameObject) Instantiate (OBGroupPrefab, new Vector3(transform.position.x + i, transform.position.y, transform.position.z), transform.rotation));
			temp[i].transform.parent = transform;
			temp[i].GetComponent<OBGroup> ().initialise_group (badgesJSON);
			++i;
		}
	}

	/**
	 * public initialise_user(string) downloads the JSON badge objects for the user specified by the email.
	 * Params: String email
	 * Returns: void
	 */
	public void initialise_user(string email) {
		StartCoroutine (download_user_backpack (email));
	}

	/**
	 * public get_badgesJSON() returns badgesJSON object
	 * Params: void
	 * Returns: JSONNode
	 */
	public JSONNode get_badgesJSON() {
		return badgesJSON;
	}

	/**
	 * public get_emailJSON() returns emailJSON object
	 * Params: void
	 * Returns: JSONNode
	 */
	public JSONNode get_emailJSON() {
		return emailJSON;
	}

	/**
	 * public get_groupsJSON() returns groupsJSON object
	 * Params: void
	 * Returns: JSONNode
	 */
	public JSONNode get_groupsJSON() {
		return groupsJSON;
	}
}
