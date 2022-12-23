using Backend.Components.MultiSlider.Scripts;
using Games.InCircles.Scripts.Categories.Factories;
using Games.InCircles.Scripts.UI;
using Games.Shapes.Scripts;
using Localization.Sample.Scripts;
using Main.Menu.NewMenu.Scripts.Screens;
using Main.Menu.Scripts.Settings;
using Main.Scripts;
using Main.Scripts.ScensGeneral;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Games.InCircles.Scripts
{
    public class GameController_InCircles : GameControllerBase
    {
        [SerializeField] private SelectPlayersCountView startGamePopup;
        [SerializeField] private StartPopupView startFirstPopup;
        [SerializeField] private WinView winPopup;
        [SerializeField] private UIView blocker;
        [SerializeField] private float waitingTransferTime;
        private FasadForGame _fasadForGame;
        private TransferView _currentTransferView;
        private TaskController _taskController;

        protected override void OnAwake()
        {
            _fasadForGame = GetComponent<FasadForGame>();
            
            _taskController = TaskController.NewInstance(this, "Games_InCircles", "Games_InCircles_Speech");
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _fasadForGame.ExercisesCompleted += ViewWinAndTransferPopups;
            _fasadForGame.CircleExercisesCompleted += ViewWinPopups;
            CreateMultiSlider();
        }

        private void CreateMultiSlider()
        {
            MultiSliderHandle[] sliderHandles = new[] {
                new MultiSliderHandle(MultiSliderHandleType.Etalon,  new Color(71, 172, 160), 0.5f)
            };
            var multiSlider = MultiSlider.Create(sliderHandles);
            
            var settingButtonText = 
                LocalizationSettings
                .StringDatabase
                .GetLocalizedString("Games_InCircles", "ModifyGame");
            
            multiSlider.CreateButtons(new[]
            {
                new BottomSliderButton(settingButtonText, ShowSettingsGamePopup)
            });
        }

        
        protected override void OnDisabled()
        {
            base.OnDisabled();
            _fasadForGame.ExercisesCompleted -= ViewWinAndTransferPopups;
            _fasadForGame.CircleExercisesCompleted -= ViewWinPopups;
        }


        protected override void OnDelayedStart()
        {
            base.OnDelayedStart();
            //CalibrationDataHolder.CalibrationDataIsReady += LoadGame;
            //if (CalibrationDataHolder.CalibrationExists) LoadGame(); 
            ShowStartGamePopup();
        }
        
        
        [Sirenix.OdinInspector.Button]
        private void ShowSettingsGamePopup()
        {
            //todo: тут бы останавливать/скрывать остальные открытые окна (таймер, молодец)
            startGamePopup.CountPlayersSelected += StartGame;
            blocker.IsVisible = true;
            startGamePopup.IsVisible = true;
            startFirstPopup.IsVisible = false;
        }
        
        
        private void ShowStartGamePopup()
        {
            startGamePopup.CountPlayersSelected += StartGame;
            blocker.IsVisible = true;
            startGamePopup.IsVisible = true;
            startFirstPopup.IsVisible = true;
        }

        private void StartGame(int playersCount, GameMode mode)
        {
            startGamePopup.CountPlayersSelected -= StartGame;
            startGamePopup.IsVisible = false;
            blocker.IsVisible = false;

            TryResetPopups();
            TryUnloadGame();
            LoadGame(playersCount, mode);
        }

        private void TryResetPopups()
        {
            //throw new System.NotImplementedException();
            if (winPopup != null) winPopup.IsVisible = false;
            if (_currentTransferView != null) _currentTransferView.IsVisible = false;
        }

        private void TryUnloadGame()
        {
            _fasadForGame.UnloadFieldHolder();
        }

        private void LoadGame(int playersCount,GameMode mode)
        {
            _taskController.NewTask();
            if (playersCount != 1)
            {
                _taskController.AddAudioTask("SolvesExercieces_01");
            }
            _taskController.AddAudioTask("DigWhole");
            _taskController.SetTask();
            
            _fasadForGame.LoadFieldHolder(playersCount,mode);
        }

        
        protected override void CreateTask()
        {
            if (!blocker.IsVisible) ForcedWin();
            else if (MainSettings.TaskChangeMode == TaskChangeMode.Manual &&
                     _currentTransferView != null &&
                     _currentTransferView.IsVisible)
            {
                _currentTransferView.IsVisible = false;
            }
        }

        private void ForcedWin()
        {
            _fasadForGame.ForcedWin();
        }

        
        private void ViewWinAndTransferPopups(TransferView transferView)
        {
            _currentTransferView = transferView;
            blocker.IsVisible = true;
            winPopup.WinViewWasHide += ViewTransferPopup;
            winPopup.ShowWinView();
        }

        private void ViewTransferPopup()
        {
            _taskController.NewTask();
            _taskController.AddAudioTask("GoByArrow");
            _taskController.SetTask();
            
            winPopup.WinViewWasHide -= ViewTransferPopup;
            _currentTransferView.TransferViewWasHide += HideBlockerAndLoadNewExercises;
            _currentTransferView.ShowTransferView(waitingTransferTime);
        }

        private void HideBlockerAndLoadNewExercises()
        {
            _currentTransferView.TransferViewWasHide -= HideBlockerAndLoadNewExercises;
            blocker.IsVisible = false;
            _fasadForGame.LoadNewExercisesOnFields();
        }

        
        private void ViewWinPopups()
        {
            blocker.IsVisible = true;
            winPopup.WinViewWasHide += HideBlockerAndLoadNewCategories;
            winPopup.ShowWinView();
        }

        private void HideBlockerAndLoadNewCategories()
        {
            blocker.IsVisible = false;
            winPopup.WinViewWasHide -= HideBlockerAndLoadNewCategories;
            _fasadForGame.LoadNewCategories();
        }
    }
}