using UnityEngine;

public class ShowHideSprite : MonoBehaviour
{
    public GameObject player;
    public GameObject spriteObject;

    void Start()
    {
        spriteObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            spriteObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            spriteObject.SetActive(false);
        }
    }
}