using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollector : MonoBehaviour
{
    [SerializeField] int healingValue = 50;
    [SerializeField] int scoreValue = 42;

    private void OnTriggerEnter2D(Collider2D other)
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        other.GetComponent<Player>().healthValue += healingValue;
        Destroy(gameObject);     
    }

}
