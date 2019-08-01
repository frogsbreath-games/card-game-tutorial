using PL.GameStates;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PL
{
    public class GameManager : MonoBehaviour
    {
        public bool IsMultiplayer;

        [System.NonSerialized]
        public PlayerHolder[] Players;
        public PlayerHolder CurrentPlayer;

        //This are unused 
        public PlayerHolder LocalPlayer;
        public PlayerHolder ClientPlayer;

        public CardHolder UserPlayerCardHolder;
        public CardHolder EnemyPlayerCardHolder;
        public State currentState;
        public GameObject cardPrefab;
        public SO.GameEvent onTurnChange;
        public SO.GameEvent onPhaseChange;
        public SO.StringVariable TurnName;

        bool GameIsInit;

        public int TurnIndex;
        public Turn[] Turns;
        public PlayerStatsVisual[] PlayerStatVisuals;

        public static GameManager Singleton;

        public PlayerHolder GetOpponentOf(PlayerHolder currentPlayer)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] != currentPlayer)
                {
                    return Players[i];
                }
            }

            return null;
        }

        public Dictionary<CardInstance, BlockInstance> BlockInstances = new Dictionary<CardInstance, BlockInstance>();

        public void AddBlockInstance(CardInstance attacker, CardInstance blocker, ref int count)
        {
            BlockInstance blockInstance = null;
            blockInstance = GetBlockInstanceForAttacker(attacker);
            if (blockInstance == null) {
                blockInstance = new BlockInstance();
                blockInstance.Attacker = attacker;
                BlockInstances.Add(attacker, blockInstance);
            }

            if (!blockInstance.Blockers.Contains(blocker))
            {
                blockInstance.Blockers.Add(blocker);
            }

            count = blockInstance.Blockers.Count;
        }

        public BlockInstance GetBlockInstanceForAttacker(CardInstance attacker)
        {
            BlockInstance blockInstance = null;
            BlockInstances.TryGetValue(attacker, out blockInstance);
            return blockInstance;
        }

        public Dictionary<CardInstance, BlockInstance> GetBlockInstances()
        {
            return BlockInstances;
        }

        public void ClearBlockInstances()
        {
            BlockInstances.Clear();
        }
       
        private void Awake()
        {
            //Two ways to access game manager need to refactor
            Singleton = this;
            Settings.gameManager = this;

           
        }

        private void Start()
        {
        

           
        }

        public void GameInit(int PlayerPhotonId)
        {
            Players = new PlayerHolder[Turns.Length];

            Turn[] turns = new Turn[2];
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i] = Turns[i].Player;

                if(Players[i].PhotonId == PlayerPhotonId)
                {
                    //CurrentPlayer = Players[i];
                    turns[i] = Turns[0];
                } else
                {
                    turns[i] = Turns[1];
                }
            }

            Turns = turns;
            //CurrentPlayer = Turns[0].Player;

            SetupPlayers();

            Turns[0].OnTurnStart();
            TurnName.value = Turns[TurnIndex].Player.Username;
            onTurnChange.Raise();

            GameIsInit = true;
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

            if (previousPlayer != player)
            {
                LoadPlayerHolder(previousPlayer, EnemyPlayerCardHolder, PlayerStatVisuals[1]);
            }
            LoadPlayerHolder(player, UserPlayerCardHolder, PlayerStatVisuals[0]);
        }

        public void LoadPlayerHolder(PlayerHolder player, CardHolder cardHolder, PlayerStatsVisual visual)
        {
            cardHolder.LoadPlayer(player, visual);
        }

        private void Update()
        {
            if (!GameIsInit)
            {
                return;
            }

            bool isComplete = Turns[TurnIndex].Execute();

            if (!IsMultiplayer)
            {
                if (isComplete)
                {
                    TurnIndex++;

                    if (TurnIndex > Turns.Length - 1)
                    {
                        TurnIndex = 0;
                    }

                    //Player change here
                    CurrentPlayer = Turns[TurnIndex].Player;
                    Turns[TurnIndex].OnTurnStart();
                    TurnName.value = Turns[TurnIndex].Player.Username;
                    onTurnChange.Raise();
                }
            }
            else if(isComplete)
            {
                MultiplayerManager.Singleton.PlayerEndsTurn(CurrentPlayer.PhotonId);
            }

            if (currentState != null)
            {
                currentState.Tick(Time.deltaTime);
            }
        }

        int GetTurnIndexForPlayer(int photonId)
        {
            for (int  i = 0;  i < Turns.Length;  i++)
            {
                if(Turns[i].Player.PhotonId == photonId)
                {
                    return i;
                }
            }

            return -1;
        }

        public int GetPhotonIdForNextPlayer()
        {
            int r = TurnIndex;

            r++;

            if(r >= Turns.Length)
            {
                r = 0;
            }

            return Turns[r].Player.PhotonId;
        }

        public void ChangeCurrentTurn(int photonId)
        {
            TurnIndex = GetTurnIndexForPlayer(photonId);

            CurrentPlayer = Turns[TurnIndex].Player;
            Turns[TurnIndex].OnTurnStart();
            TurnName.value = Turns[TurnIndex].Player.Username;
            onTurnChange.Raise();
        }

        public void SetState(State state)
        {
            currentState = state;
        }

        public void EndPhase()
        {
            if (CurrentPlayer.IsHuman)
            {
                Turns[TurnIndex].EndCurrentPhase();

                Settings.RegisterEvent("Phase: " + Turns[TurnIndex].name + " ended.", CurrentPlayer.PlayerColor);
            }
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
            cardInstance.Owner = player;
            cardInstance.currentLogic = CurrentPlayer.handLogic;
            Settings.SetParentForCard(cardObject.transform, player.CurrentCardHolder.HandGrid.value.transform);
            player.HandCards.Add(cardInstance);
        }


        public void PutCardInDiscard(CardInstance instance)
        {
            PlayerHolder owner = instance.Owner;
            owner.CardToDiscard(instance);

            Settings.SetParentForCard(instance.transform, owner.CurrentCardHolder.DiscardPile.value);
            //instance.transform.parent = owner.CurrentCardHolder.DiscardPile.value;
            Vector3 position = Vector3.zero;

            position.x = owner.DiscardCards.Count * 10;
            position.y = owner.DiscardCards.Count * -10;
            instance.transform.localPosition = position;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;
        }
    }
}