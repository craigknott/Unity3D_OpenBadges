using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class BadgeSearch : MonoBehaviour {

	private string searchString = "";
	private GameObject[] Badges;
	private List<OBBadge> badgeMatches;
	private bool window_active = false;
	private Rect badgeWindow = new Rect (Screen.width / 2 - 300, Screen.height / 2 - 300, 600, 600);

	/** 
	 * private SearchForBadge searches all badges for the string contained in
	 * 			searchString from the GUI textfield input. Stores the matching badge objects
	 * 			in a list for display and manipulation.
	 * Params: Void
	 * Returns: Void
	 */
	private void SearchForBadge() {
		badgeMatches = new List<OBBadge>();
		Debug.Log ("Searching for: " + searchString);
		// Do Search
		Badges = GameObject.FindGameObjectsWithTag("Badge");
		foreach (GameObject badge in Badges) {
			if (badge.GetComponent<OBBadge>().get_badge ().ToString().Contains (searchString)) {
				Debug.Log (badge.GetComponent<OBBadge>().get_badge ().ToString());
				badgeMatches.Add(badge.GetComponent<OBBadge>());
			}
		}
		window_active = true;
	}

	/** 
	 * private OnGUI draw GUI objects if the window is active. If the button in the top corner
	 * 			is pressed search for the text in the text field.
	 * Params: Void
	 * Returns: Void
	 */
	private void OnGUI() {
		searchString = GUI.TextField (new Rect (20, 20, 300, 20), searchString);
		if (GUI.Button (new Rect (340, 20, 60, 20), "Search")) {
			SearchForBadge();
		}
		if (window_active) {
			badgeWindow = GUI.Window(1, badgeWindow, DoSearchResults, "Search Results");
		}
	}

	/** 
	 * private DoSearchResults(int windowID) create a GUILayout with the windowID
	 * 			containing the badge name, description, criteria, issuer name and issuer website.
	 * 			draw the texture onto the GUI window and a close button to deactivate the window.
	 * Params: int windowID
	 * Returns: Void
	 */
	private void DoSearchResults(int windowID) {
		foreach (OBBadge badge in badgeMatches) {
			if (GUILayout.Button ("Name: " + badge.get_badge() ["assertion"] ["badge"] ["name"] + ", Issued By:" + 
			                  badge.get_badge() ["assertion"] ["badge"] ["issuer"] ["name"])) {
				Debug.Log ("Button Clicked");
				GameObject.Find("Main Camera").GetComponent<Camera>().transform.position = new Vector3(badge.transform.position.x - 5, 
				                                                                                             badge.transform.position.y, 
				                                                                                             badge.transform.position.z - 1);
				GameObject.Find("Main Camera").GetComponent<Camera>().transform.LookAt (badge.transform.position);
			}
		}
		if (GUI.Button (new Rect (badgeWindow.width / 2 - 100 , badgeWindow.height / 2 + 200, 200, 80), "Close")) {
			window_active = false;
		}			
	}

}
