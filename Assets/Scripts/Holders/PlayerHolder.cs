using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PL
{
    [CreateAssetMenu(menuName ="Holders/Player Holder")]
    public class PlayerHolder : ScriptableObject
    {
        public string Username;
        public Sprite Portrait;
        public int Health = 20;
        public PlayerStatsVisual Visual;

        public string[] StartingCards;

        public bool IsHuman;

        public Color PlayerColor;

        public GameElements.GameElementLogic handLogic;
        public GameElements.GameElementLogic playedLogic;

        public int ResourcesPerTurn = 1;

        [System.NonSerialized]
        public CardHolder CurrentCardHolder;

        [System.NonSerialized]
        public int ResourcesPlayedThisTurn;
    
        [System.NonSerialized]
        public List<CardInstance> HandCards = new List<CardInstance>();

        [System.NonSerialized]
        public List<CardInstance> PlayedCards = new List<CardInstance>();

        [System.NonSerialized]
        public List<ResourceHolder> ResourceHolderList = new List<ResourceHolder>();

        [System.NonSerialized]
        public List<CardInstance> AttackingCards = new List<CardInstance>();

        public int ResourceCount
        {
            get { return CurrentCardHolder.ResourceGrid.value.GetComponentsInChildren<CardVisual>().Length; }
        }

        public int AvailableResource()
        {
            int resourceCount = 0;

            foreach (ResourceHolder resourceHolder in ResourceHolderList)
            {
                if (!resourceHolder.IsUsed)
                {
                    resourceCount++;
                }
            }
            return resourceCount;
        }

        public void DropCard(CardInstance cardInstance)
        {
            if (HandCards.Contains(cardInstance))
            {
                HandCards.Remove(cardInstance);
            }

            PlayedCards.Add(cardInstance);

            Settings.RegisterEvent(Username + " dropped card " + cardInstance.visual.card.name + " resouce cost " + cardInstance.visual.card.ResourceCost, PlayerColor);
        }

        public void AddResourceCard(GameObject card)
        {
            ResourceHolder resourceHolder = new ResourceHolder
            {
                ResourceCard = card
            };

            ResourceHolderList.Add(resourceHolder);
            ResourcesPlayedThisTurn++;

            Settings.RegisterEvent(Username + " dropped resource card." , PlayerColor);
        }

        public bool CanPlayCard(Card c)
        {
            bool result = false;
            if (c.cardType is Creature || c.cardType is Spell)
            {
            int currentResource = AvailableResource();

                if (c.ResourceCost <= currentResource)
                {
                    result = true;
                }
            }
            else if (c.cardType is Resource)
            {
                if (ResourcesPerTurn - ResourcesPlayedThisTurn > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public List<ResourceHolder> GetAvailableResource()
        {
            List<ResourceHolder> availableResourceHolders = new List<ResourceHolder>();

            for (int i = 0; i < ResourceHolderList.Count; i++)
            {
                if (!ResourceHolderList[i].IsUsed)
                {
                    availableResourceHolders.Add(ResourceHolderList[i]);
                }
            }

            return availableResourceHolders;
        }

        public void SpendResource(int resources)
        {
            Vector3 rotation = new Vector3(0,0,90);

            List<ResourceHolder> availableResourceHolders = GetAvailableResource();

            for (int i = 0; i < resources; i++)
            {
                availableResourceHolders[i].IsUsed = true;
                availableResourceHolders[i].ResourceCard.transform.localEulerAngles = rotation;
            }
        }

        public void RefreshPlayerResource()
        {
            for (int i = 0; i < ResourceHolderList.Count; i++)
            {
                ResourceHolderList[i].IsUsed = false;
                ResourceHolderList[i].ResourceCard.transform.localEulerAngles = Vector3.zero;
            }

            ResourcesPlayedThisTurn = 0;
        }

        public void LoadPlayerStatsVisual()
        {
            if(Visual != null)
            {
                Visual.Player = this;
                Visual.UpdateAll();
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Visual != null)
            {
                Visual.UpdateHealth();
            }
        }
    }
}
