using UnityEngine;
using System.Collections;
using SimpleJSON;

public class OBBadge : MonoBehaviour {

	private bool window_active = false;
	private JSONNode badge;
	private Rect badgeWindow = new Rect (Screen.width / 2 - 300, Screen.height / 2 - 300, 600, 600);
	private Texture2D prvTexture = null;
	/** 
	 * private download_texture(string url) download the texture from the url param.
	 * Params: String url: The URL containing the texture to download
	 * Returns: IEnumerator
	 */
	private IEnumerator download_texture(string url) {
		Debug.Log ("url = " + url);
		WWW texture = new WWW (url);

		yield return texture;
		//Texture2D preview = new Texture2D (128, 128);
		texture.LoadImageIntoTexture(renderer.material.mainTexture as Texture2D);
		prvTexture = (Texture2D) renderer.material.mainTexture;
	}

	/** 
	 * private OnMouseDown() when this object is clicked activate the GUI window.
	 * Params: Void
	 * Returns: Void
	 */
	private void OnMouseDown() {
		window_active = !window_active;
	}

	/** 
	 * private OnGUI draw GUI objects if the window is active.
	 * Params: Void
	 * Returns: Void
	 */
	private void OnGUI() {
		if (window_active) {
			badgeWindow = GUI.Window(0, badgeWindow, DoBadgeInfo, "Badge Details");
		}
	}

	/** 
	 * private DoBadgeInfo(int windowID) create a GUILayout with the windowID
	 * 			containing the badge name, description, criteria, issuer name and issuer website.
	 * 			draw the texture onto the GUI window and a close button to deactivate the window.
	 * Params: int windowID
	 * Returns: Void
	 */
	private void DoBadgeInfo(int windowID) {
		GUILayout.Label (badge ["assertion"] ["badge"] ["name"]);
		GUILayout.Label (badge ["assertion"] ["badge"] ["description"]);
		GUILayout.Label (badge ["assertion"] ["badge"] ["criteria"]);
		GUILayout.Label (badge ["assertion"] ["badge"] ["issuer"] ["name"]);
		GUILayout.Label (badge ["assertion"] ["badge"] ["issuer"] ["origin"]);
		GUILayout.Label (renderer.material.mainTexture);
		if (GUI.Button (new Rect (badgeWindow.width / 2 - 100 , badgeWindow.height / 2 + 200, 200, 80), "Close")) {
			window_active = false;
		}			
	}

	/** 
	 * public initialise_badge(JSONNode badge_info) initialise this badge object from the JSONNode data
	 * 			parsed in the badge_info param.
	 * Params: JSONNode badge_info: The JSON containing all the badge assertion information.
	 * Returns: Void
	 */
	public void initialise_badge(JSONNode badge_info) {
		badge = badge_info;
		renderer.material.mainTexture = new Texture2D (128, 128);
		StartCoroutine (download_texture (badge ["imageUrl"]));
		Debug.Log ("Badge Recieved = " + badge.ToString ());
	}

	/** 
	 * public get_badge() return this badges JSONNode data
	 * Params: Void
	 * Returns: JSONNode: The badge data
	 */
	public JSONNode get_badge() {
		return badge;
	}

	/** 
	 * public get_texture() get the texture image of this badge
	 * Params: Void
	 * Returns: Texture: The badge image
	 */
	public Texture get_texture() {
		return prvTexture;
	}

}
