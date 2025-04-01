using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerController.Instance.TakeDamage(null);
        }
    }
}
