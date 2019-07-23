using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PL
{
    public class PlayerStatsVisual : MonoBehaviour
    {
        public PlayerHolder Player;
        public Image Portrait;
        public Text Health;
        public Text Username;

        public void UpdatePlayer()
        {
            Username.text = Player.Username;
            Portrait.sprite = Player.Portrait;
        }

        public void UpdateHealth()
        {
            Health.text = Player.Health.ToString();
        }

        public void UpdateAll()
        {
            UpdatePlayer();
            UpdateHealth();
        }
    }
}
