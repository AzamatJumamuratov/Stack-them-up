using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float moveSmoothness = 2f;
    public float zoomSmoothness = 2f;
    public float verticalOffset = 2f; // Чтоб камера не была вплотную к краю
    public float minOrthographicSize = 10f;

    public Vector2 outGamePos = new(4, -14);

    public bool focusOnTower;
    Vector3 targetPosition;
    private float targetOrthographicSize;
    private Camera cam;

    private Vector2 StartViewPos;

    void Start()
    {
        targetPosition = transform.localPosition;
        StartViewPos = new Vector2(transform.localPosition.x, transform.localPosition.z);
        cam = GetComponent<Camera>();
        targetOrthographicSize = cam.orthographicSize;
    }
    void Update()
    {
        float towerView = CalculateTowerView(Stack_Manager.instance.GetTowerHeight());

        if (focusOnTower)
        {
            targetOrthographicSize = towerView;
            targetPosition = new Vector3(outGamePos.x, targetPosition.y, outGamePos.y);
        }
        else
        {
            targetOrthographicSize = minOrthographicSize;
            targetPosition = new Vector3(StartViewPos.x, targetPosition.y, StartViewPos.y);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, moveSmoothness * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetOrthographicSize, zoomSmoothness * Time.deltaTime);
    }

    public void GoUpper(float amount)
    {
        targetPosition.y += amount;
    }


    public float CalculateTowerView(float towerHeight)
    {
        Vector3 cameraDir = transform.forward;
        float verticalComponent = Mathf.Abs(Vector3.Dot(cameraDir, Vector3.up));
        float requiredHeight = towerHeight + verticalOffset;
        float requiredSize = requiredHeight / (2f * verticalComponent);
        requiredSize = (requiredSize < minOrthographicSize) ? minOrthographicSize : requiredSize;
        return requiredSize;
    }
}
