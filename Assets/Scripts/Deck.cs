using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Deck : MonoBehaviour
{
    [SerializeField] List<CardInfo> deckArray;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject[] placeHolders;
    [SerializeField] GameObject parentPos;

    Vector3[] cardPositionsVector;

    [SerializeField] List<GameObject> cardArray; //serialized for debugging
    [SerializeField] bool isBlue;
    int handSizeAct = 0;
    [SerializeField] int handSizeMax = 7;
    GameObject cardObject;

    PhotonView myPhotonView;
    int randomCardNumber;

    private void Awake()
    {
        isBlue = GetComponent<Hero>().IsHeroBlue();
        SetUpCardPos();
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {/*
            SpawnCard();
            SpawnCard();
            SpawnCard();*/
            GetComponent<PhotonView>().RPC("SpawnCard", RpcTarget.All);
            GetComponent<PhotonView>().RPC("SpawnCard", RpcTarget.All);
            GetComponent<PhotonView>().RPC("SpawnCard", RpcTarget.All);
        }
    }

    [PunRPC]
    public void SpawnCard()
    {

        if(deckArray.Count <=0)
        {
            Debug.Log("Deck is empty");
            return;
        }

        randomCardNumber = Random.Range(0, deckArray.Count);

        if(handSizeAct>=handSizeMax)
        {
            Debug.Log("FULL HAND - Discarding card " + deckArray[randomCardNumber]);
            deckArray.Remove(deckArray[randomCardNumber]);
            return;
        }

       
        cardObject = PhotonNetwork.InstantiateSceneObject("Card Prefab", new Vector3(0, 0, 0), Quaternion.identity,0) as GameObject;
        //cardObject = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //cardArray.Add(cardObject);
        //cardObject.transform.parent = parentPos.transform;
        //cardObject.transform.parent = FindObjectOfType<Canvas>().transform;
       

        myPhotonView = cardObject.GetComponent<PhotonView>();
        myPhotonView.RPC("ChangeParent", RpcTarget.All);
        cardArray.Add(cardObject);


        ChangeCardPos();


        
        Card cardObjectScript = cardObject.GetComponent<Card>();
        if (cardObjectScript == null)
        {
            Debug.LogError("Wrong prefab m8");
            return;
        }

        //myPhotonView.RPC("GetCardInfo", RpcTarget.All);
        cardObjectScript.GetCardInfo(deckArray[randomCardNumber]);
        //GetComponent<PhotonView>().RPC("CallCard", RpcTarget.All);
        cardObjectScript.ChangeCardColorToDeck(isBlue);
        deckArray.Remove(deckArray[randomCardNumber]);

        handSizeAct++;
    }

    public void DecreaseHandSizeAct(GameObject cardObj)
    {
        handSizeAct--;
        cardArray.Remove(cardObj);
        //ChangeCardPos();
    }
    public bool IsDeckBlue() { return isBlue; }

    private void ChangeCardPos()
    {
        int arrayIndex = 0;
        foreach (GameObject cardObject in cardArray)
        {
            cardObject.transform.localScale = new Vector3(107.5269f, 107.5269f, 107.5269f);
            cardObject.transform.position = cardPositionsVector[arrayIndex++];
        }

    }

    private void SetUpCardPos()
    {
        if (isBlue)
        {
            cardPositionsVector = new Vector3[7]
                {
            new Vector3(-3f, -2.45f, 0),
            new Vector3(-1.3f, -2.45f, 0),
            new Vector3(-4.7f, -2.45f, 0),
            new Vector3(0.4f, -2.45f, 0),
            new Vector3(-6.4f, -2.45f, 0),
            new Vector3(2.1f, -2.45f, 0),
            new Vector3(-8.1f, -2.45f, 0)
            };
        }
        else
        {
            cardPositionsVector = new Vector3[7]
                {
            new Vector3(-3f, 5.1f, 0),
            new Vector3(-1.3f, 5.1f, 0),
            new Vector3(-4.7f, 5.1f, 0),
            new Vector3(0.4f, 5.1f, 0),
            new Vector3(-6.4f, 5.1f, 0),
            new Vector3(2.1f, 5.1f, 0),
            new Vector3(-8.1f, 5.1f, 0)
            };
        }

    }

    [PunRPC] void CallCard()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            cardObject.GetComponent<Card>().GetCardInfo(deckArray[randomCardNumber]);
        }
    }


}
