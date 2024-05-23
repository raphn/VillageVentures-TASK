using UnityEngine;
using VillageVentures;

public class GoToTrigger : MonoBehaviour
{
    [SerializeField] Location goesTo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            GameInterface.OpenDialog($"Do you want to go to {goesTo}?", () => GameSingleton.GoTo(goesTo));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        print($"Collision OUT {collision} with tag: {collision.tag}");
        if (collision != null && collision.CompareTag("Player"))
        {
            GameInterface.CloseDialog();
        }
    }
}
