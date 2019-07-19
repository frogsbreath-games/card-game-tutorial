using UnityEngine;
using System.Collections;
using PL.GameStates;

namespace PL
{
    public class GameManager : MonoBehaviour
    {
        public PlayerHolder[] Players;
        public PlayerHolder CurrentPlayer;

        public CardHolder UserPlayerCardHolder;
        public CardHolder EnemyPlayerCardHolder;
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

            SetupPlayers();
            CreateStartingCards();
            TurnName.value = turns[turnIndex].Player.Username;
            onTurnChange.Raise();
        }
        void SetupPlayers()
        {
            foreach (PlayerHolder player in Players)
            {
                if (player.IsHuman)
                {
                    player.CurrentCardHolder = UserPlayerCardHolder;
                }
                else
                {
                    player.CurrentCardHolder = EnemyPlayerCardHolder;
                }

            }
        }

        void CreateStartingCards()
        {
            ResourcesManager manager = Settings.GetResourcesManager();
            for (int i = 0; i < Players.Length; i++)
            {
                for (int j = 0; j < Players[i].StartingCards.Length; j++)
                {
                    GameObject cardObject = Instantiate(cardPrefab) as GameObject;
                    CardVisual visual = cardObject.GetComponent<CardVisual>();
                    visual.LoadCard(manager.GetCardInstance(Players[i].StartingCards[j]));
                    CardInstance cardInstance = cardObject.GetComponent<CardInstance>();
                    cardInstance.currentLogic = CurrentPlayer.handLogic;
                    Settings.SetParentForCard(cardObject.transform, Players[i].CurrentCardHolder.HandGrid.value.transform);
                    Players[i].HandCards.Add(cardInstance);
                }
                Settings.RegisterEvent("Created Starting Cards for Player: " + Players[i].Username, Players[i].PlayerColor);
            }

        }

        public bool SwitchPlayer;

        private void Update()
        {
            if (SwitchPlayer)
            {
                SwitchPlayer = false;
                UserPlayerCardHolder.LoadPlayer(Players[1]);
                EnemyPlayerCardHolder.LoadPlayer(Players[0]);
            }

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

        public void EndPhase()
        {
            turns[turnIndex].EndCurrentPhase();

            Settings.RegisterEvent("Phase: " + turns[turnIndex].name + " ended.", CurrentPlayer.PlayerColor);
        }
    }
}