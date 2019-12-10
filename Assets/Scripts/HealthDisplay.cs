using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    TMP_Text healthText;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetHealth()<0)
        {
            healthText.text = "0 HP";
        }
        else
        { 
            healthText.text = player.GetHealth().ToString() + " HP";
        }
    }
}
