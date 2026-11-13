using UnityEngine;
using System.Reflection;

public class AardManager : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float verticalSpeed = 5f;
    [SerializeField] private float playerMassDuringAard = 1000f;

    [Header("Настройки камеры (Cinemachine)")]
    [SerializeField] private GameObject cinemachineCameraObject;
    [SerializeField] private float zoomOffsetZ = -3f;

    [Header("Настройки камеры (Физика и Lens Shift)")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector2 activateLensShift = Vector2.zero;
    [SerializeField] private Vector2 deactivateLensShift = new Vector2(0f, -0.6f);

    [Header("Настройки UI")]
    [SerializeField] private GameObject inventoryUI;

    private GameObject currentObject;
    private Rigidbody currentRb;
    private bool isAardActive = false;
    private float verticalInput = 0f;
    private int originalLayer;

    private Rigidbody playerRb;
    private float originalPlayerMass;

    private Component vcam;
    private Component followComponent;

    private Transform savedFollow;
    private Transform savedLookAt;
    private Vector3 savedFollowOffset;
    private bool originalUsePhysicalProperties;
    private bool wasInventoryActive;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        if (playerRb != null)
            originalPlayerMass = playerRb.mass;

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
            originalUsePhysicalProperties = mainCamera.usePhysicalProperties;

        FindCinemachineCamera();
    }

    private void FindCinemachineCamera()
    {
        if (cinemachineCameraObject == null) return;

        foreach (var comp in cinemachineCameraObject.GetComponentsInChildren<Component>())
        {
            if (comp.GetType().Name.Contains("Cinemachine") && !comp.GetType().Name.Contains("Follow"))
            {
                vcam = comp;
                break;
            }
        }

        foreach (var comp in cinemachineCameraObject.GetComponentsInChildren<Component>())
        {
            if (comp.GetType().Name.Contains("Follow"))
            {
                followComponent = comp;
                break;
            }
        }
    }

    public void ActivateAard(GameObject target)
    {
        if (isAardActive) return;

        currentObject = target;
        currentRb = target.GetComponent<Rigidbody>();

        if (currentRb == null)
        {
            Debug.Log("У объекта нет Rigidbody!");
            return;
        }

        isAardActive = true;
        currentRb.useGravity = false;
        currentRb.linearVelocity = Vector3.zero;
        currentRb.angularVelocity = Vector3.zero;

        originalLayer = target.layer;
        target.layer = LayerMask.NameToLayer("Aard");

        if (playerRb != null)
            playerRb.mass = playerMassDuringAard;

        if (inventoryUI != null)
        {
            wasInventoryActive = inventoryUI.activeSelf;
            inventoryUI.SetActive(false);
        }

        if (vcam != null)
        {
            SaveCameraState();
            SetCameraTarget(target.transform);
        }
        if (followComponent != null)
        {
            SetFollowOffset(new Vector3(0, 0, zoomOffsetZ));
        }

        if (mainCamera != null)
        {
            mainCamera.usePhysicalProperties = false;
            mainCamera.lensShift = activateLensShift;
        }

        Debug.Log($"Аард активирован на: {target.name}");
    }

    public void DeactivateAard()
    {
        if (!isAardActive) return;

        isAardActive = false;

        if (currentRb != null)
            currentRb.useGravity = true;

        if (currentObject != null)
            currentObject.layer = originalLayer;

        if (playerRb != null)
            playerRb.mass = originalPlayerMass;

        if (vcam != null && savedFollow != null)
        {
            SetCameraTarget(savedFollow);
        }
        if (followComponent != null)
        {
            SetFollowOffset(savedFollowOffset);
        }


        if (mainCamera != null)
        {
            mainCamera.usePhysicalProperties = true;
            mainCamera.lensShift = deactivateLensShift;
        }

        if (inventoryUI != null)
        {
            inventoryUI.SetActive(wasInventoryActive); 
        }

        Debug.Log($"Аард деактивирован: {currentObject?.name}");
        currentObject = null;
        currentRb = null;
    }


    private void SaveCameraState()
    {
        PropertyInfo followProp = vcam.GetType().GetProperty("Follow");
        PropertyInfo lookAtProp = vcam.GetType().GetProperty("LookAt");

        if (followProp != null)
            savedFollow = followProp.GetValue(vcam) as Transform;

        if (lookAtProp != null)
            savedLookAt = lookAtProp.GetValue(vcam) as Transform;

        if (followComponent != null)
        {
            PropertyInfo offsetProp = followComponent.GetType().GetProperty("FollowOffset");
            FieldInfo offsetField = followComponent.GetType().GetField("FollowOffset");

            if (offsetProp != null)
                savedFollowOffset = (Vector3)offsetProp.GetValue(followComponent);
            else if (offsetField != null)
                savedFollowOffset = (Vector3)offsetField.GetValue(followComponent);
        }
    }

    private void SetCameraTarget(Transform target)
    {
        PropertyInfo followProp = vcam.GetType().GetProperty("Follow");
        PropertyInfo lookAtProp = vcam.GetType().GetProperty("LookAt");

        if (followProp != null)
            followProp.SetValue(vcam, target);

        if (lookAtProp != null)
            lookAtProp.SetValue(vcam, target);
    }

    private void SetFollowOffset(Vector3 offset)
    {
        PropertyInfo offsetProp = followComponent.GetType().GetProperty("FollowOffset");
        FieldInfo offsetField = followComponent.GetType().GetField("FollowOffset");

        if (offsetProp != null)
            offsetProp.SetValue(followComponent, offset);
        else if (offsetField != null)
            offsetField.SetValue(followComponent, offset);
    }

    public void UpdateAardMovement(Vector2 input)
    {
        if (!isAardActive || currentRb == null) return;

        currentRb.linearVelocity = new Vector3(
            input.x * moveSpeed,
            currentRb.linearVelocity.y,
            input.y * moveSpeed
        );
    }

    public void UpdateAardVertical(float input)
    {
        if (!isAardActive || currentRb == null) return;

        verticalInput = input;

        currentRb.linearVelocity = new Vector3(
            currentRb.linearVelocity.x,
            input * verticalSpeed,
            currentRb.linearVelocity.z
        );
    }

    public bool IsAardActive => isAardActive;
}