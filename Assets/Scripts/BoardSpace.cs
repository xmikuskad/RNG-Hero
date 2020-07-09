using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : MonoBehaviour
{
    bool isUsed = false;

    public bool IsBoardSpaceUsed() { return isUsed; }

    public void UseBoardSpace()
    {
        Debug.Log(this.gameObject.name + " is used");
        isUsed = true;
    }

    public void FreeBoardSpace()
    {
        Debug.Log(this.gameObject.name + " is free");
        isUsed = false;
    }
}
