using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
    

    public Transform player;
    public Texture2D crosshair;

    public Vector3 pivotOffset = new Vector3(0.0f, 1.0f, 0.0f);
    public Vector3 camOffset = new Vector3(0.0f, 0.7f, -3.0f);

    public float smooth = 10f;

    public Vector3 aimPivotOffset = new Vector3(0.0f, 1.7f, -0.3f);
    public Vector3 aimCamOffset = new Vector3(0.8f, 0.0f, -1.0f);

    public float horizontalAimingSpeed = 20f;
    public float verticalAimingSpeed = 400f;
    public float maxVerticalAngle = 30f;
    public float flyMaxVerticalAngle = 60f;
    public float minVerticalAngle = -60f;

    public float mouseSensitivity = 0.3f;

    public float sprintFOV = 100f;

    private PlayerControl playerControl;
    private float angleH = 0;
    private float angleV = 0;
    private Transform cam;

    private Vector3 relCameraPos;
    private float relCameraPosMag;

    private Vector3 smoothPivotOffset;
    private Vector3 smoothCamOffset;
    private Vector3 targetPivotOffset;
    private Vector3 targetCamOffset;

    private bool boolOff = false;

    private float defaultFOV;
    private float targetFOV;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        //if (!target)
        //    return;
        //else
        //    this.transform.Translate(0, 0, target.position.z - this.transform.position.z - 3);
        //    if(System.Math.Abs(target.rotation.y - this.transform.rotation.y) >= 0.1)
        //        this.transform.Rotate(Vector3.up, target.rotation.y);
        cam = transform;

        relCameraPos = transform.position - player.position;
        relCameraPosMag = relCameraPos.magnitude - 0.5f;

        smoothPivotOffset = pivotOffset;
        smoothCamOffset = camOffset;

        defaultFOV = cam.GetComponent<Camera>().fieldOfView;
 
            if(System.Math.Abs(transform.rotation.y - player.rotation.y) >= 0.1)
                angleH += Mathf.Clamp(player.rotation.y, -1, 1) * horizontalAimingSpeed * Time.deltaTime;
            angleV += Mathf.Clamp(0, -1, 1) * verticalAimingSpeed * Time.deltaTime;

            // fly
            if (boolOff)
            {
                //angleV = Mathf.Clamp(angleV, minVerticalAngle, flyMaxVerticalAngle);
            }
            else
            {
                angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);
            }


            Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
            Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
            cam.rotation = aimRotation;

            if (boolOff)
            {
                targetPivotOffset = aimPivotOffset;
                targetCamOffset = aimCamOffset;
            }
            else
            {
                targetPivotOffset = pivotOffset;
                targetCamOffset = camOffset;
            }

            if (boolOff)
            {
                targetFOV = sprintFOV;
            }
            else
            {
                targetFOV = defaultFOV;
            }
            cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.GetComponent<Camera>().fieldOfView, targetFOV, Time.deltaTime);

            // Test for collision
            Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset;
            Vector3 tempOffset = targetCamOffset;
            //for (float zOffset = targetCamOffset.z; zOffset <= 0; zOffset += 0.5f)
            //{
            //    tempOffset.z = zOffset;
            //    if (DoubleViewingPosCheck(baseTempPosition + aimRotation * tempOffset) || zOffset == 0)
            //    {
            //        targetCamOffset.z = tempOffset.z;
            //        break;
            //    }
            //}

            // fly
            if (boolOff)
            {
                targetCamOffset.y = 0;
            }

            smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
            smoothCamOffset = Vector3.Lerp(smoothCamOffset, targetCamOffset, smooth * Time.deltaTime);

            cam.position = player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;

        }

    bool DoubleViewingPosCheck(Vector3 checkPos)
    {
        float playerFocusHeight = player.GetComponent<CapsuleCollider>().height * 0.5f;
        return ViewingPosCheck(checkPos, playerFocusHeight) && ReverseViewingPosCheck(checkPos, playerFocusHeight);
    }

    bool ViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight)
    {
        RaycastHit hit;

        // If a raycast from the check position to the player hits something...
        if (Physics.Raycast(checkPos, player.position + (Vector3.up * deltaPlayerHeight) - checkPos, out hit, relCameraPosMag))
        {
            // ... if it is not the player...
            if (hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                // This position isn't appropriate.
                return false;
            }
        }
        // If we haven't hit anything or we've hit the player, this is an appropriate position.
        return true;
    }

    bool ReverseViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight)
    {
        RaycastHit hit;

        if (Physics.Raycast(player.position + (Vector3.up * deltaPlayerHeight), checkPos - player.position, out hit, relCameraPosMag))
        {
            if (hit.transform != player && hit.transform != transform && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }
        return true;
    }
}
