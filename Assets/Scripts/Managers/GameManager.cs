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
        public SO.GameEvent onTurnChange;
        public SO.GameEvent onPhaseChange;
        public SO.StringVariable TurnName;

        public int turnIndex;
        public Turn[] turns;

        private void Start()
        {
            Settings.gameManager = this;
            CreateStartingCards();
            TurnName.value = turns[turnIndex].Player.Username;
            onTurnChange.Raise();
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
            bool isComplete = turns[turnIndex].Execute();

            if (isComplete)
            {
                turnIndex++;

                if (turnIndex > turns.Length -1)
                {
                    turnIndex = 0;
                }

                TurnName.value = turns[turnIndex].Player.Username;
                onTurnChange.Raise();
            }

            if (currentState != null)
            {
                currentState.Tick(Time.deltaTime);
            }
        }

        public void SetState(State state)
        {
            currentState = state;
        }

    }
}