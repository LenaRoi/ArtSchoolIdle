﻿using UnityEngine.UI;

namespace EKTemplate
{
    public class MainPanel : Panel
    {
        public Text levelText;
        public Text moneyText;

        private void Start()
        {
            levelText.text = "LEVEL " + GameManager.instance.level;
            moneyText.text = GameManager.instance.money.ToString();
        }

        public void OnPressStart()
        {
            LevelManager.instance.StartGame();
        }
    }
}