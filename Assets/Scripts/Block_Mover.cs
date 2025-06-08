using UnityEngine;

public enum Direction
{
    Z,
    X,
}


public class Block_Mover : MonoBehaviour
{
    public bool movementAllowed = true;

    public Direction StartDirection = Direction.X;
    public float directionInAxis = 1f;
    [SerializeField] float placementSpeed = 7;
    [SerializeField] Vector2 StartLimit = new(10, 10);

    private Vector3 PivotPos;
    private Vector3 moveDirection;
    private float limit;

    void Start()
    {
        moveDirection = (StartDirection == Direction.Z) ? transform.forward : transform.right;
        limit = (StartDirection == Direction.X) ? StartLimit.x : StartLimit.y;

        PivotPos = transform.localPosition;
        Vector3 startFromLimitPos = transform.localPosition;

        if (StartDirection == Direction.X)
        {
            startFromLimitPos.x = -limit;
        }
        else
        {
            startFromLimitPos.z = limit;
        }

        transform.localPosition = startFromLimitPos;
    }

    void Update()
    {
        if (!movementAllowed) return;
        transform.localPosition += moveDirection * placementSpeed * directionInAxis * Time.deltaTime;

        float offset = Utils.GetNonZeroAxisValue(transform.localPosition - PivotPos);

        if (offset >= limit)
        {
            directionInAxis = -1f;
        }
        else if (offset <= -limit)
        {
            directionInAxis = 1f;
        }
    }


}
