using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AimStateManager : MonoBehaviour
{
    #region  Current aim state and available aim states
    AimBaseState currentState;
    public HipFireState Hip = new HipFireState();
    public AimState Aim = new AimState();

    [SerializeField] float mouseSense = 1;
    float xAxis, yAxis;

    #endregion

    [SerializeField] Transform camFollowPos; 
    [HideInInspector] public Animator anim; 
    [HideInInspector] public CinemachineVirtualCamera vCam; 

    public float adsFov = 40; // Field of view when aiming down sights
    [HideInInspector] public float hipFov; // Field of view when not aiming
    [HideInInspector] public float currentFov; // Current field of view
    public float fovSmoothSpeed = 10; // Smooth transition speed for field of view changes

    public Transform aimPos; // Aim position for aiming raycasts
    [SerializeField] float aimSmoothSpeed = 20; // Smooth transition speed for aiming position
    [SerializeField] LayerMask aimMask; // Layer mask for aiming raycasts

    // Start is called before the first frame update
    void Start()
    {
        
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        hipFov = vCam.m_Lens.FieldOfView;
        anim = GetComponentInChildren<Animator>();
        SwitchState(Hip); 
    }

    // Update is called once per frame
    void Update()
    {
        // Handle mouse input for aiming
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSense;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis, -80, 80);

        // Smoothly adjust the field of view
        hipFov = Mathf.Lerp(hipFov, currentFov, fovSmoothSpeed * Time.deltaTime);

        // Cast a ray to determine the aim position based on where the player is looking
        Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCentre);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
        {
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);
        }

        
        currentState.UpdateState(this);
    }

    private void LateUpdate()
    {
        // Adjust camera and player rotation
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state)
    {
        // Switch to the specified aim state
        currentState = state;
        currentState.EnterState(this);
    }
}
