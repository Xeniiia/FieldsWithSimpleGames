using System;
using System.Collections.Generic;
using Games.InCircles.Scripts.Categories.Factories;
using Main.Menu.NewMenu.Scripts.Items;
using Main.Menu.NewMenu.Scripts.Items.Interfaces;
using Main.Menu.NewMenu.Scripts.Notifications.Common;
using UnityEngine;

namespace Games.InCircles.Scripts.UI
{
    //TODO Вместо List.Find переписать логику на каст к индеку Enum, если это надо
    [DrawProperties(
        nameof(playersCountGroup),
        nameof(toggleToPlayerCountPair),
        nameof(selectAllButton),
        nameof(toggleToGameModePair)
    )]
    public class SelectPlayersCountView : NotificationView
    {
        [SerializeField] private MenuToggleGroup playersCountGroup;
        [SerializeField] private List<ToggleToPlayerCountPair> toggleToPlayerCountPair;
        [SerializeField] private List<ToggleToGameModePair> toggleToGameModePair;

        [SerializeField] private MenuButton startGameButton;
        [SerializeField] private MenuButton selectAllButton;

        public event Action<int, GameMode> CountPlayersSelected;

        private int _playersSelected = 1;
        private GameMode _gameMode = GameMode.None;

        private void Update()
        {
            if (_gameMode == GameMode.None)
            {
                startGameButton.interactable = false;
            }
            else
            {
                startGameButton.interactable = true;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            playersCountGroup.MenuToggleGroupEvent += PlayersCountGroupOnMenuToggleGroupEvent;
            foreach (var pair in toggleToGameModePair)
            {
                pair.ToggleInterface.MenuToggleEvent += GameModeGroupOnMenuToggleGroupEvent;
            }
            
            selectAllButton.OnClickEvent += SelectAllButtonOnOnClickEvent;
            startGameButton.OnClickEvent += StartGame;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            playersCountGroup.MenuToggleGroupEvent -= PlayersCountGroupOnMenuToggleGroupEvent;
            foreach (var pair in toggleToGameModePair)
            {
                pair.ToggleInterface.MenuToggleEvent -= GameModeGroupOnMenuToggleGroupEvent;
            }

            selectAllButton.OnClickEvent -= SelectAllButtonOnOnClickEvent;
            startGameButton.OnClickEvent -= StartGame;
        }
        

        private void SelectAllButtonOnOnClickEvent()
        {
            foreach (var pair in toggleToGameModePair)
            {
                pair.ToggleInterface.IsOn = true;
            }
        }

        private void GameModeGroupOnMenuToggleGroupEvent(IMenuToggle toggle, bool value)
        {
            var indexOf = toggleToGameModePair.Find(t => t.ToggleInterface == toggle);
            if (indexOf == null) return;

            var gameMode = indexOf.GameMode;
            if (!value) _gameMode ^= gameMode;
            else _gameMode |= gameMode;
        }

        private void PlayersCountGroupOnMenuToggleGroupEvent(IMenuToggleGroup group, IMenuToggle toggle)
        {
            _playersSelected = toggleToPlayerCountPair.Find(x => x.Toggle == toggle)?.PlayerCount ?? 0;
        }

        private void StartGame()
        {
            CountPlayersSelected?.Invoke(_playersSelected, _gameMode);
        }
    }

    [Serializable]
    public class ToggleToGameModePair
    {
        [SerializeField] private MenuToggle toggle;
        [SerializeField] private GameMode gameMode;
        
        public GameMode GameMode => gameMode;
        public IMenuToggle ToggleInterface => toggle;
    }

    [Serializable]
    public class ToggleToPlayerCountPair
    {
        [SerializeField] private MenuToggle toggle;
        [SerializeField] private int playerCount;

        public IMenuToggle Toggle => toggle;
        public int PlayerCount => playerCount;
    }
}