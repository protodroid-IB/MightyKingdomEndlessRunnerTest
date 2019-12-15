using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUpDefinition
{
    Coin,
    Count
}

public class PickUp : InteractableObject
{
    [SerializeField]
    private PickUpDefinition pickUpType;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            switch (pickUpType)
            {
                case PickUpDefinition.Coin:
                    ScoringManager.instance.IncrementCoins();
                    Kill();
                    break;
            }
            
        }
    }

    private void Kill()
    {
        Destroy(gameObject);
    }

}
