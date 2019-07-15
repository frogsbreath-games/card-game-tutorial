using UnityEngine;
using System.Collections;
using PL.GameStates;

namespace PL
{
    public class GameManager : MonoBehaviour
    {
        public PlayerHolder currentPlayer;
        public State currentState;
        public GameObject cardPrefab;

        private void Start()
        {
            Settings.gameManager = this;
            CreateStartingCards();
        }

        void CreateStartingCards()
        {
            ResourcesManager manager = Settings.GetResourcesManager();
            for (int i = 0; i < currentPlayer.startingCards.Length; i++)
            {
                GameObject cardObject = Instantiate(cardPrefab) as GameObject;
                CardVisual visual = cardObject.GetComponent<CardVisual>();
                visual.LoadCard(manager.GetCardInstance(currentPlayer.startingCards[i]));
                CardInstance cardInstance = cardObject.GetComponent<CardInstance>();
                cardInstance.currentLogic = currentPlayer.handLogic;
                Settings.SetParentForCard(cardObject.transform, currentPlayer.handGrid.value.transform);
            }
        }

        private void Update()
        {
            currentState.Tick(Time.deltaTime);
        }

        public void SetState(State state)
        {
            currentState = state;
        }

    }
}