using TMPro;
using UnityEngine;

public class Stack_Manager : MonoBehaviour
{
    public static Stack_Manager instance;
    public GameObject StartBlockPrefab;
    public Transform CurrentBlockTransform;

    public Transform FallingPartContainer;

    public float offsetToPerfect = 0.1f;
    public int TotalBlocks = 1;


    [Header("Set Dynamically")]
    public Transform PreviousBlockTransform;


    [Header("Camera")]

    public CameraControl camScript;
    public float GoUpAmount = 1f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PlaceNextBlock(StartBlockPrefab.transform.localScale);
    }

    public void PlaceNextBlock(Vector3 previousBlockScale)
    {
        Vector3 newBlockPosition = CurrentBlockTransform.position;
        newBlockPosition.y += CurrentBlockTransform.localScale.y;
        GameObject newBlockGO = Instantiate(StartBlockPrefab, newBlockPosition, Quaternion.identity, transform);
        newBlockGO.transform.localScale = previousBlockScale;

        Direction previousDirection = CurrentBlockTransform.GetComponent<Block_Mover>().StartDirection;
        Block_Mover newBlockMoverScript = newBlockGO.GetComponent<Block_Mover>();
        newBlockMoverScript.StartDirection = (previousDirection == Direction.X) ? Direction.Z : Direction.X;

        PreviousBlockTransform = CurrentBlockTransform;
        CurrentBlockTransform = newBlockGO.transform;
        camScript.GoUpper(GoUpAmount);
    }

    public void IncScore(bool isPerfectlyPlaced)
    {
        TotalBlocks += 1;
        Game_Manager.instance.CalculateScores(isPerfectlyPlaced);
    }


    public void StopGame()
    {
        Game_Manager.instance.EndGame();
    }

    public float GetTowerHeight()
    {
        return TotalBlocks * StartBlockPrefab.transform.localScale.y;
    }

    public float GetTowerWidth()
    {
        return StartBlockPrefab.transform.localScale.x;
    }
}
