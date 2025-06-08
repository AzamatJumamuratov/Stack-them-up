using UnityEditor;
using UnityEngine;

public class Block_Placer : MonoBehaviour
{
    public KeyCode placeKey = KeyCode.Space;

    public Behaviour[] ComponentsToDisable;

    private Block_Mover blockMoverScript;
    private Stack_Manager stackManager;

    private Animation perfectPlaceAnimation;


    void Start()
    {
        blockMoverScript = GetComponent<Block_Mover>();
        stackManager = Stack_Manager.instance;
        perfectPlaceAnimation = transform.GetComponentInChildren<Animation>();
    }

    void Update()
    {
        if (Game_Manager.instance.state != GameState.continues) return;

        if (Input.GetKeyDown(placeKey))
        {
            PlaceBlock();
        }
    }

    void PlaceBlock()
    {
        blockMoverScript.movementAllowed = false;

        Transform currentBlockT = stackManager.CurrentBlockTransform;
        Transform previousBlockT = stackManager.PreviousBlockTransform;

        Vector3 delta = currentBlockT.localPosition - previousBlockT.localPosition;

        //So We Placed Block Now We should Cut Block and Put it to New Place
        Vector3 _placedBlockNewSize = currentBlockT.localScale - Utils.Abs(delta);
        _placedBlockNewSize.y = currentBlockT.localScale.y;

        Vector3 _placedBlockNewPos = currentBlockT.localPosition - delta / 2;
        _placedBlockNewPos.y = currentBlockT.localPosition.y;


        if (Utils.IsValuesOfVectorNegative(_placedBlockNewSize))
        {
            PlaceFallingPart(gameObject, currentBlockT.localScale, currentBlockT.localPosition);
            Destroy(currentBlockT.gameObject);
            stackManager.StopGame();
            return;
        }

        Vector3 deltaXZ = new Vector3(delta.x, 0f, delta.z);
        float offset = deltaXZ.magnitude;

        if (offset < stackManager.offsetToPerfect)
        {
            perfectPlaceAnimation.Play();
            stackManager.IncScore(isPerfectlyPlaced: true);
            _placedBlockNewSize = previousBlockT.localScale;
            _placedBlockNewPos = previousBlockT.localPosition;
            _placedBlockNewPos.y = currentBlockT.localPosition.y;
        }
        else
        {
            CalculateFallingPart(delta, _placedBlockNewPos, _placedBlockNewSize, currentBlockT.localScale, out Vector3 fallingPartPos, out Vector3 fallingPartSize);
            PlaceFallingPart(gameObject, fallingPartSize, fallingPartPos);
            stackManager.IncScore(isPerfectlyPlaced: false);
        }

        currentBlockT.localScale = _placedBlockNewSize;
        currentBlockT.localPosition = _placedBlockNewPos;

        stackManager.PlaceNextBlock(currentBlockT.localScale);

        foreach (Behaviour component in ComponentsToDisable)
        {
            component.enabled = false;
        }

    }

    void PlaceFallingPart(GameObject originalGO, Vector3 size, Vector3 pos)
    {
        GameObject _fallingPartGO = Instantiate(originalGO, stackManager.FallingPartContainer);
        _fallingPartGO.transform.localScale = size;
        _fallingPartGO.transform.localPosition = pos;
        foreach (var script in _fallingPartGO.GetComponents<MonoBehaviour>())
        {
            Destroy(script);
        }
        _fallingPartGO.GetComponent<Rigidbody>().isKinematic = false;
    }


    private void CalculateFallingPart(
    Vector3 delta,
    Vector3 placedBlockPos,
    Vector3 placedBlockSize,
    Vector3 originalBlockSize,
    out Vector3 fallingPartPos,
    out Vector3 fallingPartSize)
    {
        bool isMovingAlongX = Mathf.Abs(delta.x) > 0f;

        float offset = isMovingAlongX ? delta.x : delta.z;
        float direction = Mathf.Sign(offset);
        float halfCutSize = Mathf.Abs(offset) / 2f;
        float placedHalfSize = isMovingAlongX ? placedBlockSize.x / 2f : placedBlockSize.z / 2f;

        fallingPartSize = new Vector3(
            isMovingAlongX ? Mathf.Abs(delta.x) : originalBlockSize.x,
            originalBlockSize.y,
            isMovingAlongX ? originalBlockSize.z : Mathf.Abs(delta.z)
        );

        fallingPartPos = placedBlockPos + new Vector3(
            isMovingAlongX ? direction * (placedHalfSize + halfCutSize) : 0f,
            0f,
            isMovingAlongX ? 0f : direction * (placedHalfSize + halfCutSize)
        );
    }
}
