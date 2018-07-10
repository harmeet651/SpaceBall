using UnityEngine;
using System.Collections;

public class TP_Camera : MonoBehaviour {

    public static TP_Camera Instance;

    public Transform TargetLookAt;
    public float Distance = 5f;
    public float DistanceMin = 1f;
    public float DistanceMax = 15f;
    public float DistanceSmooth = 0.05f;
    public float DistanceResumeSmooth = 0.3f;
    public float MouseSensitivity = 5f;   
    public float MouseWheelSensitivity = 7f;
    public float X_Smooth = 0.01f;
    public float Y_Smooth = 0.03f;
    public float Y_MinLimit = -40f;
    public float Y_MaxLimit = 80f;
    public float OcclusionDistanceStep = 0.1f;
    public int MaxOcclusionChecks = 20;
    public int OcclusionCheckGridSize = 2;
    public float NearCameraFarClipPlane = 50f;
    public float FadeDistance = 2.5f;
    public float FadeOffset = 0.3f;
    public float TargetLookAtOffset = 1.2f;

    private float mouseX = 0f;
    private float mouseY = 0f;
    private float mouseRotate = 0f;
    private float velX = 0f;
    private float velY = 0f;
    private float velZ = 0f;
    private float velDistance = 0f;
    private float velTargetLookAt = 0f;
    private float startDistance = 0f;
    private Vector3 position = Vector3.zero;
    private Vector3 desiredPosition = Vector3.zero;
    private float desiredDistance = 0f;
    private float preOccludedDistance = 0f;
    private float distanceSmooth = 0f;
    private bool isFPS;
    private Transform _transform;
    private float nearClipPlane = 0.2f;
    private float time = 0f;
    private float deltaTimeForCameraChange = 1f;
    private ArrayList materials;
    private ArrayList materialsOrg;
    private Shader transparentShader = Shader.Find("Transparent/Diffuse");
    private float currentTargetLookAtOffset;

    public bool RMBRotate { get; set;}

    void Awake()
    {
        Instance = this;
        _transform = transform;
        materials = new ArrayList();
        materialsOrg = new ArrayList();
    }


	void Start () {
        Distance = Mathf.Clamp(Distance, DistanceMin, DistanceMax);
        startDistance = Distance;
        Reset();
        SaveCharacherMaterials();
	}
	
	void LateUpdate () 
    {
        if (TargetLookAt == null)
            return;


        HandlePlayerInput();
        ResetDesiredDistance();

        int count = 0;
        do
        {
            CalculateDesiredPosition();
            count++;
        } while (CheckIfOccluded(count));

        
        UpdatePosition();
        FadePlayer();
	}


    void HandlePlayerInput()
    {
        var deadZone = 0.01f;

        mouseX += mouseRotate;
        if ((RMBRotate && Input.GetMouseButton(1)) || !RMBRotate)
        {
            mouseX += Input.GetAxis("Mouse X") * MouseSensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * MouseSensitivity;
        }
		
        
        // Limit rotation on mouseY
        mouseY = Helper.ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit);


        if (Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
        {
            desiredDistance = Mathf.Clamp(desiredDistance - Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity, 0, DistanceMax);
            
            // If we scrool the mouse wheel preOccludedDistance becomes the desiredDistance
            preOccludedDistance = desiredDistance;
            distanceSmooth = DistanceSmooth;
        }
    }

    void CalculateDesiredPosition()
    {
        // Evaluate distance  
        Distance = Mathf.SmoothDamp(Distance, desiredDistance, ref velDistance, distanceSmooth);

        // Calculate desired position
        desiredPosition = CalculatePosition(mouseY, mouseX, Distance);
        
    }

    Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
    {       
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        return TargetLookAt.position + rotation * direction;
    }


    bool CheckIfOccluded(int count)
    {
        CheckState();
        if (isFPS)
            return false;

        bool isOccluded = false;

        float nearestDistance = CheckCameraPoints(TargetLookAt.position, desiredPosition);

        if (nearestDistance != -1)
        {
            isOccluded = true;
            if (count < MaxOcclusionChecks)
            {
                desiredDistance -= OcclusionDistanceStep;
            }
            else
            {
                Distance = nearestDistance - nearClipPlane;
                desiredDistance = Distance;
            }

            distanceSmooth = DistanceResumeSmooth;
        }

        return isOccluded;
    }



    float CheckCameraPoints(Vector3 from, Vector3 to)
    {
        float nearestDistance = -1f;
        RaycastHit hitInfo;
        Vector3[,] planePoints = Helper.CalculatePlanePoints(to, OcclusionCheckGridSize);


        // Draw lines in the editor to make it easier to visualise
        Debug.DrawLine(from, to + transform.forward * -GetComponent<Camera>().nearClipPlane, Color.red);

        for (int i = 0; i <= OcclusionCheckGridSize; i++)
        {
            for (int j = 0; j <= OcclusionCheckGridSize; j++)
            {
                Debug.DrawLine(from, planePoints[i,j]);

                if (Physics.Linecast(from, planePoints[i,j], out hitInfo) && hitInfo.collider.tag != "Player")
                    nearestDistance = hitInfo.distance;
            }
        }

        // Draw rectangle
        Debug.DrawLine(planePoints[0, 0], planePoints[0, OcclusionCheckGridSize]);
        Debug.DrawLine(planePoints[OcclusionCheckGridSize, OcclusionCheckGridSize], planePoints[0, OcclusionCheckGridSize]);
        Debug.DrawLine(planePoints[OcclusionCheckGridSize, OcclusionCheckGridSize], planePoints[OcclusionCheckGridSize, 0]);
        Debug.DrawLine(planePoints[0, 0], planePoints[OcclusionCheckGridSize, 0]);


        return nearestDistance;
    }


    
    float CheckOcclusionBetween(Vector3 from, Vector3 to)
    {
        float nearestDistance = -1f;
        RaycastHit hitInfo;
        Vector3[,] planePointsTo = Helper.CalculatePlanePoints(to, OcclusionCheckGridSize);
        Vector3[,] planePointsFrom = Helper.CalculatePlanePoints(from, OcclusionCheckGridSize);



        for (int i = 0; i <= OcclusionCheckGridSize; i++)
        {
            for (int j = 0; j <= OcclusionCheckGridSize; j++)
            {
                Debug.DrawLine(planePointsFrom[i, j], planePointsTo[i, j], Color.blue);

                if (Physics.Linecast(planePointsFrom[i, j], planePointsTo[i, j], out hitInfo) && hitInfo.collider.tag != "Player")
                    if (hitInfo.distance < nearestDistance || nearestDistance == -1)
                        nearestDistance = hitInfo.distance;
            }
        }

        return nearestDistance;
    }
    

    void ResetDesiredDistance()
    {
        if (desiredDistance < preOccludedDistance)
        {
            Vector3 preOccludedpos = CalculatePosition(mouseY, mouseX, preOccludedDistance);

            // Check if camera can go back and how much it can go back
            float distance = CheckOcclusionBetween(desiredPosition, preOccludedpos);

            if (distance == -1)
            {
                float nearestDistance = CheckCameraPoints(TargetLookAt.position, preOccludedpos);
                if (nearestDistance == -1)
                {
                    desiredDistance = preOccludedDistance;
                }
            }
            else
            {
                desiredDistance = distance + Distance - nearClipPlane;
            }
            distanceSmooth = DistanceResumeSmooth;
        }
    }

    void UpdatePosition()
    {
        CheckState();
        if (!isFPS)
        {
            float posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velX, X_Smooth);
            float posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velY, Y_Smooth);
            float posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velZ, X_Smooth);
            position = new Vector3(posX, posY, posZ);

            _transform.position = position;

            UpdateTargetLookAtPosition();
            _transform.LookAt(TargetLookAt);
        }
        else
        {
            Vector3 charachterPosition = TP_Controler.Instance._transform.position;

            float posX = Mathf.SmoothDamp(position.x, charachterPosition.x, ref velX, X_Smooth);
            float posY = Mathf.SmoothDamp(position.y, charachterPosition.y, ref velY, Y_Smooth);
            float posZ = Mathf.SmoothDamp(position.z, charachterPosition.z, ref velZ, X_Smooth);
            position = new Vector3(posX, posY, posZ);

            _transform.position = position;
            _transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        }
    }

    void UpdateTargetLookAtPosition()
    {
        // If Distance < MinDistance, Camera is not looking to TargetLookAt
        float desiredOffset;
        if (Distance < FadeDistance)
        {
            desiredOffset = (Distance / FadeDistance) * TargetLookAtOffset;
        }
        else
        {
            desiredOffset = TargetLookAtOffset;
        }
        currentTargetLookAtOffset = Mathf.SmoothDamp(currentTargetLookAtOffset, desiredOffset, ref velTargetLookAt, DistanceSmooth);
        TargetLookAt.transform.position = new Vector3(TP_Controler.Instance._transform.position.x, TP_Controler.Instance._transform.position.y + currentTargetLookAtOffset, TP_Controler.Instance._transform.position.z);
    }

    void FadePlayer()
    {
        float alpha;

        if (Distance < FadeDistance)
        {
            if (isFPS)
                alpha = 0;
            else
                alpha = (Distance / FadeDistance) - FadeOffset;
            for (int i = 0; i < materials.Count; i++)
            {
                ((Material)materials[i]).shader = transparentShader;
                ((Material)materials[i]).color = new Color(((Material)materials[i]).color.r, ((Material)materials[i]).color.g, ((Material)materials[i]).color.b, alpha);
            }
        
        }
        else
        {
            for (int i = 0; i < materials.Count; i++)
            {
                ((Material)materials[i]).shader = ((Material)materialsOrg[i]).shader;
                ((Material)materials[i]).color = new Color(((Material)materials[i]).color.r, ((Material)materials[i]).color.g, ((Material)materials[i]).color.b);
            }
        }

        
    }

    public void Reset()
    {
        mouseX = 0;
        mouseY = 10;
        Distance = startDistance;
        desiredDistance = startDistance;
        preOccludedDistance = Distance;
        currentTargetLookAtOffset = TargetLookAtOffset;
        RMBRotate = false;
    }

    public static void AttachCamera()
    {
        GameObject tmpCamera;
        GameObject tmpHudCamera;
        GameObject tmpFarCamera;
        GameObject targetLookAt;
        TP_Camera myCamera;


        // Camera
        if (Camera.main != null)
        {
            tmpCamera = Camera.main.gameObject;
        }
        else
        {
            tmpCamera = new GameObject("Main Camera");
            tmpCamera.AddComponent<Camera>();
            tmpCamera.tag = "MainCamera";
        }

        tmpCamera.AddComponent<TP_Camera>();
        tmpCamera.GetComponent<Camera>().depth = 1;
        tmpCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
        tmpCamera.GetComponent<Camera>().farClipPlane = TP_Camera.Instance.NearCameraFarClipPlane;
        tmpCamera.GetComponent<Camera>().nearClipPlane = TP_Camera.Instance.nearClipPlane;
        myCamera = tmpCamera.GetComponent("TP_Camera") as TP_Camera;


        // Create HUDCamera
        tmpHudCamera = new GameObject("HUDCamera");
        tmpHudCamera.AddComponent<Camera>();
        tmpHudCamera.GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("HUDLayer");
        tmpHudCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
        tmpHudCamera.GetComponent<Camera>().depth = 2;
        tmpHudCamera.transform.position = tmpCamera.transform.position;
        tmpHudCamera.transform.rotation = Quaternion.identity;
        tmpHudCamera.transform.parent = tmpCamera.transform;

        // Create FarCamera
        tmpFarCamera = new GameObject("FarCamera");
        tmpFarCamera.AddComponent<Camera>();
        tmpFarCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        tmpFarCamera.GetComponent<Camera>().depth = 0;
        tmpFarCamera.transform.position = tmpCamera.transform.position;
        tmpFarCamera.transform.rotation = Quaternion.identity;
        tmpFarCamera.transform.parent = tmpCamera.transform;
        tmpFarCamera.GetComponent<Camera>().nearClipPlane = TP_Camera.Instance.NearCameraFarClipPlane;
        tmpFarCamera.GetComponent<Camera>().farClipPlane = 1000;

        // TargetLookAt
        targetLookAt = GameObject.Find("targetLookAt") as GameObject;

        if (targetLookAt == null)
        {
            targetLookAt = new GameObject("targetLookAt");
            targetLookAt.transform.position = Vector3.zero;
        }

        myCamera.TargetLookAt = targetLookAt.transform;
    }

    void CheckState()
    {
        if (Distance < DistanceMin)
        {
            if (Time.time - time < deltaTimeForCameraChange)
            {
                isFPS = true;
                time = Time.time;
            }
        }
        else
        {
            if (Time.time - time < deltaTimeForCameraChange)
            {
                isFPS = false;
                time = Time.time;
            }
        }
    }

    void SaveCharacherMaterials()
    {
        materials.Clear();
        materialsOrg.Clear();

        // Get materials from player gameObject
        Material[] mats = TP_Controler.CharacterController.GetComponent<Renderer>().materials;
        for (int i = 0; i < mats.Length; i++)
        {
            materials.Add(mats[i]);
            materialsOrg.Add(new Material(mats[i]));
        }

        // Get materials from all children
        Renderer[] allRenderers = TP_Controler.CharacterController.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in allRenderers)
        {
            materials.Add(rend.material);
            materialsOrg.Add(new Material(rend.material));
        }
                
    }



    public void RotateCamera(float Turn)
    {
        mouseRotate = Turn;
    }
}
