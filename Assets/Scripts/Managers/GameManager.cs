using UnityEngine;
using System.Collections;
using PL.GameStates;

namespace PL
{
    public class GameManager : MonoBehaviour
    {
        [System.NonSerialized]
        public PlayerHolder[] Players;
        public PlayerHolder CurrentPlayer;

        public PlayerHolder GetOpponentOf(PlayerHolder currentPlayer)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if(Players[i] != currentPlayer)
                {
                    return Players[i];
                }
            }

            return null;
        }

        public CardHolder UserPlayerCardHolder;
        public CardHolder EnemyPlayerCardHolder;
        public State currentState;
        public GameObject cardPrefab;
        public SO.GameEvent onTurnChange;
        public SO.GameEvent onPhaseChange;
        public SO.StringVariable TurnName;

        public int TurnIndex;
        public Turn[] Turns;
        public PlayerStatsVisual[] PlayerStatVisuals;

        public static GameManager Singleton;

        private void Awake()
        {
            Singleton = this;

            Players = new PlayerHolder[Turns.Length];
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i] = Turns[i].Player;
            }
        }

        private void Start()
        {
            Settings.gameManager = this;

            SetupPlayers();
            CreateStartingCards();
            TurnName.value = Turns[TurnIndex].Player.Username;
            onTurnChange.Raise();
        }

        void SetupPlayers()
        {
            for (int i = 0; i < Players.Length; i++)
            {
                //if (Players[i].IsHuman)
                //{
                //    Players[i].CurrentCardHolder = UserPlayerCardHolder;
                //}
                //else
                //{
                //    Players[i].CurrentCardHolder = EnemyPlayerCardHolder;
                //}

                if(i == 0)
                {
                    Players[i].CurrentCardHolder = UserPlayerCardHolder;
                }
                else
                {
                    Players[i].CurrentCardHolder = EnemyPlayerCardHolder;
                }

                if (i < 2)
                {
                    Players[i].Visual = PlayerStatVisuals[i];
                    PlayerStatVisuals[i].Player.LoadPlayerStatsVisual();
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

                Players[i].CurrentCardHolder.LoadPlayer(Players[i], Players[i].Visual);

                Settings.RegisterEvent("Created Starting Cards for Player: " + Players[i].Username, Players[i].PlayerColor);
            }

        }

        public void LoadPlayerActive(PlayerHolder player)
        {
            if (UserPlayerCardHolder.Player != player)
            {
                PlayerHolder previousPlayer = UserPlayerCardHolder.Player;
                LoadPlayerHolder(previousPlayer, EnemyPlayerCardHolder, PlayerStatVisuals[1]);
                LoadPlayerHolder(player, UserPlayerCardHolder, PlayerStatVisuals[0]);
            }
        }

        public void LoadPlayerHolder(PlayerHolder player, CardHolder cardHolder, PlayerStatsVisual visual)
        {
            cardHolder.LoadPlayer(player, visual);
        }

        public bool SwitchPlayer;

        private void Update()
        {
            if (SwitchPlayer)
            {
                SwitchPlayer = false;
                UserPlayerCardHolder.LoadPlayer(Players[1], PlayerStatVisuals[1]);
                EnemyPlayerCardHolder.LoadPlayer(Players[0], PlayerStatVisuals[0]);
            }

            bool isComplete = Turns[TurnIndex].Execute();

            if (isComplete)
            {
                TurnIndex++;

                if (TurnIndex > Turns.Length -1)
                {
                    TurnIndex = 0;
                }

                //Player change here
                CurrentPlayer = Turns[TurnIndex].Player;
                Turns[TurnIndex].OnTurnStart();
                TurnName.value = Turns[TurnIndex].Player.Username;
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
            Turns[TurnIndex].EndCurrentPhase();

            Settings.RegisterEvent("Phase: " + Turns[TurnIndex].name + " ended.", CurrentPlayer.PlayerColor);
        }
    }
}