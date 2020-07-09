using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] bool isBoardBlue = true;

    CardManager cardManager;

    private void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
    }

    private void OnMouseDown()
    {
        cardManager.BoardClicked(isBoardBlue);
    }

    public BoardSpace CheckForSpace()
    {
        foreach(BoardSpace boardSpace in this.GetComponentsInChildren<BoardSpace>())
        {
            if(!boardSpace.IsBoardSpaceUsed())
            {
                return boardSpace;
            }
        }

        return null;
    }
    public bool IsBoardManagerBlue() { return isBoardBlue; }
}
