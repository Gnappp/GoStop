using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoStopManager : MonoBehaviour
{
    private List<int> bottomCard; //바닥 패
    private Player player1;

    private void Start()
    {
        player1 = new Player("jin");
        Debug.Log(Jokbo.GetNames(typeof(Jokbo)).Length);
    }

    void CreateDeck() //Card를 사용하여 화투 덱을만든다.
    {

    }

    void DeckToPlayers() //Deck에서 플레이어들에게 패를 돌린다.
    {

    }

    void DeckToBottom() //Deck에서 바닥에 패를 나둔다.
    {

    }



}
