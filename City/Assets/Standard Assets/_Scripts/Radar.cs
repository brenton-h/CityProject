using UnityEngine;
using UnityEngine.UI;

public class Radar : MonoBehaviour {
	public float insideRadarDistance = 18;
	public float blipSizePercentage = 5;
    public GameObject rawImageBlipCar;
    public GameObject rawImageBlipNpc;
    public GameObject rawImageBlipSecCam;
    public GameObject rawImageBlipChatter;
    public GameObject rawImageBlipHostile;

	private RawImage rawImageRadarBackground;
	private Transform playerTransform;
	private float radarWidth, radarHeight, blipHeight, blipWidth;


    private bool disRad;
    public bool displayRadar {
        get { return disRad; }
        set { disRad = value;
            if (!disRad) {
                RemoveAllBlips();
            }
            rawImageRadarBackground.enabled = disRad;
        } }

    void Start() {
		rawImageRadarBackground = GetComponent<RawImage>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		radarWidth = rawImageRadarBackground.rectTransform.rect.width;
		radarHeight = rawImageRadarBackground.rectTransform.rect.height;
        
		blipHeight = radarHeight * blipSizePercentage / 100;
		blipWidth = radarWidth * blipSizePercentage / 100;


        displayRadar = false;
	}
    
	void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            displayRadar = !displayRadar;
        }
        if (displayRadar) {
            RemoveAllBlips();

            FindAndDisplayBlipsForTag("Car", rawImageBlipCar);
            FindAndDisplayBlipsForTag("NPC", rawImageBlipNpc);
            FindAndDisplayBlipsForTag("SecCam", rawImageBlipSecCam);
            FindAndDisplayBlipsForTag("chatter", rawImageBlipChatter);
            FindAndDisplayBlipsForTag("Hostile", rawImageBlipHostile);
        }
	}

	private void FindAndDisplayBlipsForTag(string tag, GameObject prefabBlip) {
		Vector3 playerPos = playerTransform.position;
		GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

		foreach (GameObject target in targets) {
			Vector3 targetPos = target.transform.position;
            
			float distanceToTarget = Vector3.Distance(targetPos, playerPos);
            
            if (distanceToTarget <= insideRadarDistance) {
                //if (CheckLayer(target.layer))
				CalculateBlipPositionAndDrawBlip(playerPos, targetPos, prefabBlip);
			}
        }
    }

    private bool CheckLayer(int layer) {
        int cm = LayerController.Self.CullingMask;
        int c = cm, l = 1 << layer;
        return ((c | l) == cm);
    }

    private void CalculateBlipPositionAndDrawBlip(Vector3 playerPos, Vector3 targetPos, GameObject prefabBlip) {
		Vector3 normalisedTargetPosition = NormaisedPosition(playerPos, targetPos);
		Vector2 blipPosition = CalculateBlipPosition(normalisedTargetPosition);
        DrawBlip(blipPosition, prefabBlip);
    }
    
	private void RemoveAllBlips() {
		GameObject[] blips = GameObject.FindGameObjectsWithTag("Blip");
		foreach (GameObject blip in blips)
			Destroy(blip);
	}

	private Vector3 NormaisedPosition(Vector3 playerPos, Vector3 targetPos) {
		float normalisedyTargetX = (targetPos.x - playerPos.x) / insideRadarDistance;
		float normalisedyTargetZ = (targetPos.z - playerPos.z) / insideRadarDistance;
		return new Vector3(normalisedyTargetX, 0, normalisedyTargetZ);
	}

	private Vector2 CalculateBlipPosition(Vector3 targetPos) {
		float angleToTarget = Mathf.Atan2(targetPos.x, targetPos.z) * Mathf.Rad2Deg;
        
		float anglePlayer = playerTransform.eulerAngles.y;
        
		float angleRadarDegrees = angleToTarget - anglePlayer - 90;
        
		float normalisedDistanceToTarget = targetPos.magnitude;
		float angleRadians = angleRadarDegrees * Mathf.Deg2Rad;
		float blipX = normalisedDistanceToTarget * Mathf.Cos(angleRadians),
			  blipY = normalisedDistanceToTarget * Mathf.Sin(angleRadians);
		blipX *= radarWidth / 2;
		blipY *= radarHeight / 2;
		blipX += radarWidth / 2;
		blipY += radarHeight / 2;

		return new Vector2(blipX, blipY);
	}
    
	private void DrawBlip(Vector2 pos, GameObject blipPrefab) {
		GameObject blipGO = (GameObject)Instantiate(blipPrefab);
		blipGO.transform.SetParent(transform);
		RectTransform rt = blipGO.GetComponent<RectTransform>();
		rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, pos.x, blipWidth);
		rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, pos.y, blipHeight);
	}
}