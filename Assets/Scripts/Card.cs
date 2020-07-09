using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Card : MonoBehaviour
{
    #region Card Init Variables
    CardManager cardManager;

    [SerializeField] CardInfo cardInfo;
    int cardDmg;
    int cardHp;
    int manaCost;
    string cardTitle;
    string cardDescription;
    bool isSpell;
    bool hasCharge;

    [SerializeField] Text cardTitleText;
    [SerializeField] Text cardDescriptionText;
    [SerializeField] Text cardDmgText;
    [SerializeField] Text cardHpText;
    [SerializeField] Text manaCostText;

    [SerializeField] Image cardImage;
    [SerializeField] GameObject cardHpImage;

    #endregion

    [SerializeField] bool isCardBlue = true;
    bool isCardSpawned = false;
    bool isCardReady;
    BoardSpace boardSpace;
    Mana manaManager;
    EndTurnManager endTurnManager;

    //TESTING
    [PunRPC]
    void ChangeParent()
    {
        Debug.Log("CHANGING PARENT");
        this.gameObject.transform.SetParent(GameObject.Find("Card Holder").transform, false);
    }



    void Start()
    {
        //InitializeCard();                     //deck.cs
        cardManager = FindObjectOfType<CardManager>();
        manaManager = FindObjectOfType<Mana>();
        endTurnManager = FindObjectOfType<EndTurnManager>();
        //ChangeCardColorToDeck(isCardBlue);    //deck.cs
    }

    [PunRPC]
    public void InitializeCard()
    {
        cardDmg = cardInfo.GetCardDamage();
        cardHp = cardInfo.GetCardHealth();
        manaCost = cardInfo.GetManaCost();
        cardTitle = cardInfo.GetCardName();
        cardDescription = cardInfo.GetCardText();
        isSpell = cardInfo.IsSpell();
        hasCharge = cardInfo.HasCharge();

        cardTitleText.text = cardTitle;
        cardDescriptionText.text = cardDescription;
        cardDmgText.text = cardDmg.ToString();
        cardHpText.text = cardHp.ToString();
        manaCostText.text = manaCost.ToString();

        if(isSpell)
        {
            cardHpText.gameObject.SetActive(false);
            cardHpImage.gameObject.SetActive(false);
        }

    }

    private void OnMouseDown()
    {
        if(isSpell)
        {
            Debug.Log("Casting Spell");
            cardManager.SelectingCard(this, true, isCardBlue);
            return;
        }

        Debug.Log("Card OnMouseDown, STATUS: isSpawned:" + isCardSpawned + " isReady: " + isCardReady);

        if (isCardReady)
        {
            //ChangeCardColor();
            cardManager.SelectingCard(this, isCardSpawned, isCardBlue);
        }
        else
        {
            cardManager.AttackCard(this, isCardSpawned, isCardBlue);
        }

    }

    public string GetCardName() { return cardTitle; }

    public void SummonCard()
    {
        Debug.Log("Spawning Card " + GetCardName());
        if (isCardBlue)
        {
            foreach(BoardManager boardManager in FindObjectsOfType<BoardManager>())
            {
                if(boardManager.IsBoardManagerBlue())
                {
                    boardSpace = boardManager.CheckForSpace();
                }
            }
            if (boardSpace == null)
            {
                Debug.Log("All boardSpaces are full");
                endTurnManager.ChangeWarningText("FULL BOARD");
                return;
            }
            if(!manaManager.CheckManaValue(manaCost,isCardBlue))
            {
                Debug.Log("Not enough mana");
                endTurnManager.ChangeWarningText("NO MANA");
                boardSpace = null;
                return;
            }
            boardSpace.UseBoardSpace();
            transform.position = boardSpace.transform.position;

            foreach(Deck deck in FindObjectsOfType<Deck>())
            {
                if (deck.IsDeckBlue())
                    deck.DecreaseHandSizeAct(this.gameObject);
            }

        }
        else
        {
            foreach (BoardManager boardManager in FindObjectsOfType<BoardManager>())
            {
                if (!boardManager.IsBoardManagerBlue())
                {
                    boardSpace = boardManager.CheckForSpace();
                }
            }
            if (boardSpace == null)
            {
                Debug.Log("All boardSpaces are full");
                endTurnManager.ChangeWarningText("FULL BOARD");
                return;
            }
            if (!manaManager.CheckManaValue(manaCost, isCardBlue))
            {
                Debug.Log("Not enough mana");
                endTurnManager.ChangeWarningText("NO MANA");
                boardSpace = null;
                return;
            }
            boardSpace.UseBoardSpace();
            transform.position = boardSpace.transform.position;

            foreach (Deck deck in FindObjectsOfType<Deck>())
            {
                if (!deck.IsDeckBlue())
                    deck.DecreaseHandSizeAct(this.gameObject);
            }
        }

        isCardSpawned = true;

        if (hasCharge)
            SetCardReadyToTrue();
        else
            SetCardReadyToFalse();
    }

    public int GetAttack() { return cardDmg; }
    public bool IsCardBlue() { return isCardBlue; }
    public void SetCardReadyToFalse()
    {
        isCardReady = false;
        cardImage.color = new Color32(114, 114, 114, 255);

    }
    public void SetCardReadyToTrue()
    {
        isCardReady = true;
        if (!isSpell)
            cardImage.color = new Color32(255, 109, 0, 255);
        else
            cardImage.color = new Color32(0, 255, 233, 255);
    }

    public void TakeDamage(int damage)
    {
        cardHp -= damage;
        cardHpText.text = cardHp.ToString();
        if (cardHp <= 0)
        {
            Debug.Log("Card is RIP");
            if(boardSpace!=null)
                boardSpace.FreeBoardSpace();
            Destroy(gameObject);
        }
    }

    public void GetCardInfo(CardInfo newCardInfo)
    {
        cardInfo = newCardInfo;
        GetComponent<PhotonView>().RPC("InitializeCard", RpcTarget.All);
    }
    public void ChangeCardColorToDeck(bool isDeckBlue)
    {
        isCardBlue = isDeckBlue;

        bool isBlueTurn = FindObjectOfType<EndTurnManager>().IsBlueTurn();

        if ((isCardBlue && isBlueTurn) || (!isCardBlue && !isBlueTurn))
        {
            SetCardReadyToTrue();
        }
        else
        {
            SetCardReadyToFalse();
        }
    }

    public bool CheckForSpell() { return isSpell; }

    public void UseSpell()
    {
        foreach (Deck deck in FindObjectsOfType<Deck>())
        {
            if (deck.IsDeckBlue()&&isCardBlue || !deck.IsDeckBlue() && !isCardBlue)
                deck.DecreaseHandSizeAct(this.gameObject);
        }

        Destroy(this.gameObject);
    }

    public bool CheckForSpellMana()
    {
        if (!manaManager.CheckManaValue(manaCost, isCardBlue))
        {
            endTurnManager.ChangeWarningText("NO MANA");
            return false;
        }
        else
            return true;
    }

}
