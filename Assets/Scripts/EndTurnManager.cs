using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnManager : MonoBehaviour
{
    [SerializeField] Deck blueHeroDeck;
    [SerializeField] Deck greenHeroDeck;
    [SerializeField] Text turnText;
    [SerializeField] Text warningBlueText;
    [SerializeField] Text warningGreenText;

    bool isBlueTurn = true;
    Mana manaManager;
    Animator animatorTurnText;
    bool notReady = false;


    void Start()
    {
        //Initialization
        manaManager = FindObjectOfType<Mana>();
        manaManager.IncreaseBlueManaMax();
        animatorTurnText = GetComponentInChildren<Animator>();


        ChangeTurnText("blue turn");
        StartCoroutine(AnimationWaiter());
    }

    public void EndTurn()
    {
        if (notReady)
            return;

        StartCoroutine(AnimationWaiter());

        if (isBlueTurn)
        {
            EndBlueTurn();
            isBlueTurn = false;
            greenHeroDeck.SpawnCard();
        }
        else
        {
            EndGreenTurn();
            isBlueTurn = true;
            blueHeroDeck.SpawnCard();
        }
    }

    private void EndBlueTurn()
    {
        Debug.Log("Ending blue turn");
        foreach(Card card in FindObjectsOfType<Card>())
        {
            if(card.IsCardBlue())
            {
                card.SetCardReadyToFalse();
            }
            else
            {
                card.SetCardReadyToTrue();
            }
        }
        manaManager.IncreaseGreenManaMax();
        ChangeTurnText("Green Turn");
    }

    private void EndGreenTurn()
    {
        Debug.Log("Ending green turn");
        foreach (Card card in FindObjectsOfType<Card>())
        {
            if (!card.IsCardBlue())
            {
                card.SetCardReadyToFalse();
            }
            else
            {
                card.SetCardReadyToTrue();
            }
        }
        manaManager.IncreaseBlueManaMax();
        ChangeTurnText("blue turn");
    }

    public bool IsBlueTurn()
    {
        return isBlueTurn;
    }

    IEnumerator AnimationWaiter()
    {
        notReady = true;
        yield return new WaitForSeconds(1.5f);
        notReady = false;
    }

    private void ChangeTurnText(string text)
    {
        turnText.text = text;
        animatorTurnText.SetTrigger("ChangeTurn");
    }
    public void ChangeWarningText(string text)
    {
        StartCoroutine(TextChangeWaiter(text));
    }
    IEnumerator TextChangeWaiter(string text)
    {
        if(isBlueTurn)
        {
            warningBlueText.text = text;
            warningBlueText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            warningBlueText.gameObject.SetActive(false);
        }
        else
        {
            warningGreenText.text = text;
            warningGreenText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            warningGreenText.gameObject.SetActive(false);
        }
    }
}
