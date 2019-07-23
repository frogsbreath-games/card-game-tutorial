using UnityEngine;
using System.Collections;
using PL.GameStates;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace PL
{
    [CreateAssetMenu(menuName = "Actions/Select Attacking Cards")]
    public class SelectAttackingCards : Action
    {
        public override void Execute(float delta)
        {
            if (Input.GetMouseButtonDown(0))
            {

                List<RaycastResult> results = Settings.GetUIObjectsUnderMouse();

                foreach (RaycastResult result in results)
                {
                    CardInstance instance = result.gameObject.GetComponentInParent<CardInstance>();
                    PlayerHolder player = Settings.gameManager.CurrentPlayer;

                    if (!Settings.gameManager.CurrentPlayer.PlayedCards.Contains(instance))
                    {
                        return;
                    }

                    if (instance.CanAttack())
                    {
                        //Can Attack
                        player.AttackingCards.Add(instance);
                        player.CurrentCardHolder.SetCardOnBattleLine(instance);
                    }
                }
            }
        }
    }
}
