using PL.GameStates;
using UnityEngine;

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

            CurrentPlayer = Turns[0].Player;
        }

        private void Start()
        {
            Settings.gameManager = this;

            SetupPlayers();
            Turns[0].OnTurnStart();
            TurnName.value = Turns[TurnIndex].Player.Username;
            onTurnChange.Raise();
        }

        void SetupPlayers()
        {
            ResourcesManager manager = Settings.GetResourcesManager();

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].Init();
                if(i == 0)
                {
                    Players[i].CurrentCardHolder = UserPlayerCardHolder;
                }
                else
                {
                    Players[i].CurrentCardHolder = EnemyPlayerCardHolder;
                }

                Players[i].Visual = PlayerStatVisuals[i];
                Players[i].CurrentCardHolder.LoadPlayer(Players[i], Players[i].Visual);
            }
        }


        public void LoadPlayerActive(PlayerHolder player)
        {
                PlayerHolder previousPlayer = UserPlayerCardHolder.Player;
                LoadPlayerHolder(previousPlayer, EnemyPlayerCardHolder, PlayerStatVisuals[1]);
                LoadPlayerHolder(player, UserPlayerCardHolder, PlayerStatVisuals[0]);
        }

        public void LoadPlayerHolder(PlayerHolder player, CardHolder cardHolder, PlayerStatsVisual visual)
        {
            cardHolder.LoadPlayer(player, visual);
        }

        private void Update()
        {
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

        public void DrawCardFromDeck(PlayerHolder player)
        {
            ResourcesManager manager = Settings.GetResourcesManager();

            if (player.AllCards.Count == 0)
            {
                Settings.RegisterEvent($"{player.Username} has run out of cards", player.PlayerColor);
                return;
            }

            string cardId = player.AllCards[0];
            player.AllCards.RemoveAt(0);
            GameObject cardObject = Instantiate(cardPrefab) as GameObject;
            CardVisual visual = cardObject.GetComponent<CardVisual>();
            visual.LoadCard(manager.GetCardInstance(cardId));
            CardInstance cardInstance = cardObject.GetComponent<CardInstance>();
            cardInstance.currentLogic = CurrentPlayer.handLogic;
            Settings.SetParentForCard(cardObject.transform, player.CurrentCardHolder.HandGrid.value.transform);
            player.HandCards.Add(cardInstance);
        }
    }
}