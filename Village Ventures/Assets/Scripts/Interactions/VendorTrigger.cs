using UnityEngine;

public class VendorTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print($"Collision {collision} with tag: {collision.tag}");
        if (collision != null && collision.CompareTag("Player"))
        {
            print("Calling");
            GameInterface.Instance.CallInterface();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        print($"Collision OUT {collision} with tag: {collision.tag}");
        if (collision != null && collision.CompareTag("Player"))
        {
            print("Closing");
            GameInterface.Instance.CloseInterface();
        }
    }
}
