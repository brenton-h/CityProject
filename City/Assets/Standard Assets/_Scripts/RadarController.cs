//Assets/Standard Assets/_RadarAssets/02_11_radar/_Scripts
/*using UnityEngine;
using UnityEngine.UI;

public class RadarController : MonoBehaviour {
	
	public GameObject radar;
	
	private string hotKey;

    private static bool disRad;
	public static bool displayRadar { get { return disRad; } set {
            if (!value) {
                foreach (GameObject blip in GameObject.FindGameObjectsWithTag("Blip"))
                    Destroy(blip);
            }
            disRad = value;
        } }
	
	void Start() {
		hotKey = "r";
		displayRadar = false;
	}
	
	void Update() {
		
		if (Input.GetKeyDown(hotKey)) {
			if (displayRadar) {
				radar.GetComponent<RawImage>().enabled = false;
			} else {
				radar.GetComponent<RawImage>().enabled = true;
			}
			displayRadar = !displayRadar;
		}	
	}
}
*/