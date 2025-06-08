using UnityEngine;

public class Utils : MonoBehaviour
{
  public static float GetNonZeroAxisValue(Vector3 input)
  {
    if (input.x != 0f) return input.x;
    if (input.y != 0f) return input.y;
    if (input.z != 0f) return input.z;

    Debug.LogWarning("Все компоненты вектора равны нулю!");
    return 0f; // Или можно выбросить исключение
  }

  public static Vector3 Abs(Vector3 vector)
  {
    return new Vector3(
        Mathf.Abs(vector.x),
        Mathf.Abs(vector.y),
        Mathf.Abs(vector.z)
    );
  }

  public static bool IsValuesOfVectorNegative(Vector3 vector)
  {
    return vector.x < 0 || vector.y < 0 || vector.z < 0;
  }
}