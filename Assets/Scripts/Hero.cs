using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Hero : MonoBehaviour
{
    [SerializeField] GameObject blueBlock;
    [SerializeField] GameObject greenBlock;
    [SerializeField] Text healthText;
    [SerializeField] int health = 10;
    [SerializeField] bool isHeroBlue;
    CardManager cardManager;

    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        healthText.text = "HP:"+health.ToString();
        if(PhotonNetwork.IsMasterClient)
        {
            //greenBlock.SetActive(true);
        }
        else
        {
            //blueBlock.SetActive(true);
        }
    }

    private void OnMouseDown()
    {
        cardManager.AttackHero(isHeroBlue,this);
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        healthText.text = "HP:" + health.ToString();
        if (health <= 0)
            Debug.Log("GAME OVER");
    }

    public bool IsHeroBlue() { return isHeroBlue; }

}
