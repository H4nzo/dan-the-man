using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    int coins;
    [SerializeField] Text coinText;

    void Start()
    {

    }

    void Update()
    {
        coinText.text = coins.ToString();
    }

    public void AddCoin(int coinToAdd)
    {
        coins += coinToAdd;
    }
}
