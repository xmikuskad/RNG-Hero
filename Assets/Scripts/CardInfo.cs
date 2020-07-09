using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Data", menuName = "Create New Card", order = 1)]
public class CardInfo : ScriptableObject
{

    [SerializeField] string cardName;
    [TextArea]
    [SerializeField] string cardText;
    [SerializeField] int cardHealth;
    [SerializeField] int cardDamage;
    [SerializeField] int manaCost;

    //SPECIAL THINGS
    [SerializeField] bool isSpell;
    [SerializeField] bool hasCharge;

    public string GetCardName() { return cardName; }
    public string GetCardText() { return cardText; }
    public int GetCardHealth() { return cardHealth; }
    public int GetCardDamage() { return cardDamage; }
    public int GetManaCost() { return manaCost; }
    public bool IsSpell() { return isSpell; }
    public bool HasCharge() { return hasCharge; }
}
