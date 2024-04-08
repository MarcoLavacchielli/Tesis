using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public void TakeDamage(int amount)
    {
        Debug.Log("Player took " + amount + " damage!");
    }
}