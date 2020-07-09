using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    bool isCardSelected = false;
    bool isCardSpawned;
    bool isCardBlue;
    Card selectedCard;
    EndTurnManager endTurnManager;

    void Start()
    {
        endTurnManager = FindObjectOfType<EndTurnManager>();
    }

    public void SelectingCard(Card card, bool isSelectedCardSpawned, bool isSelectedCardBlue)
    {
        //Debug.Log("isSel:" + isCardSelected + "| isSp:" + isCardSpawned + "| isSelSp:" + isSelectedCardSpawned + "| isCaBl:" + isCardBlue + "| isSelCaBl:" + isSelectedCardBlue);
        if (isCardSelected && isCardSpawned && isSelectedCardSpawned && isCardBlue != isSelectedCardBlue)
        {
            AttackCard(card, isSelectedCardSpawned, isSelectedCardBlue);
        }
        else
        {
            isCardSelected = true;
            isCardBlue = isSelectedCardBlue;
            selectedCard = card;
            isCardSpawned = isSelectedCardSpawned;
            Debug.Log("CardManager - SelectingCard: " + selectedCard.GetCardName());
        }
    }

    public void AttackCard(Card defendingCard,bool isSelectedCardSpawned, bool isSelectedCardBlue)
    {
        bool isBlueTurn = endTurnManager.IsBlueTurn();
        if(!isCardSelected && ((isSelectedCardBlue && isBlueTurn) || (!isSelectedCardBlue && !isBlueTurn) ))
        {
            endTurnManager.ChangeWarningText("NOT READY");
        }

        if (isCardSelected && isCardSpawned && isSelectedCardSpawned && isCardBlue != isSelectedCardBlue)
        {
            Debug.Log("Giving dmg to card");
            if (!selectedCard.CheckForSpell())
            {
                int selectedCardAtt = selectedCard.GetAttack();
                selectedCard.TakeDamage(defendingCard.GetAttack());
                defendingCard.TakeDamage(selectedCardAtt);
                selectedCard.SetCardReadyToFalse();
            }
            else
            {
                if (!selectedCard.CheckForSpellMana())
                {
                    Debug.Log("Not enough mana");
                    endTurnManager.ChangeWarningText("NO MANA");
                    UnselectCard();
                    return;
                }

                int selectedCardAtt = selectedCard.GetAttack();
                defendingCard.TakeDamage(selectedCardAtt);
                selectedCard.UseSpell();
            }
        }

        UnselectCard();
    }

    public void BoardClicked(bool isBoardBlue)
    {
        //DEBUG
        if (!isBoardBlue && isCardBlue && isCardSelected)
            Debug.Log("WRONG TARGET");
        //-------

        if (isCardSelected)
            PrepareForSummoning(isBoardBlue);

        UnselectCard();
    }

    public void AttackHero(bool isHeroBlue, Hero hero)
    {
        //DEBUG
        if (isHeroBlue && isCardBlue && isCardSpawned && isCardSelected)
            Debug.Log("WRONG TARGET!");
        //--------------

        if(isCardSpawned && isCardSelected && isHeroBlue != isCardBlue )
        {
            if (!selectedCard.CheckForSpell())
            {
                hero.DealDamage(selectedCard.GetAttack());
                selectedCard.SetCardReadyToFalse();
            }
            else
            {
                if (!selectedCard.CheckForSpellMana())
                {
                    Debug.Log("Not enough mana");
                    endTurnManager.ChangeWarningText("NO MANA");
                    UnselectCard();
                    return;
                }
                hero.DealDamage(selectedCard.GetAttack());
                selectedCard.UseSpell();
            }
        }

        UnselectCard();
    }

    private void PrepareForSummoning(bool isBoardBlue)
    {

        if (!isCardSpawned && isBoardBlue == isCardBlue)
        {
            selectedCard.SummonCard();
        }
    }

    public void UnselectCard()
    {
        isCardSelected = false;
        isCardSpawned = false;
    }


}
