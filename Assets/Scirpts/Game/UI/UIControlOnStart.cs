using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LevelLoader))]
public class UIControlOnStart : MonoBehaviour {

    public Button btn_Start;
    public Button btn_Restart;
    public Button btn_GameCenter;

    private LevelLoader levelLoader;

    private void Awake() {
        levelLoader = GetComponent<LevelLoader>();
    }

    private void OnEnable() {
        btn_Start.onClick.AddListener(OnGameStart);
        btn_Restart.onClick.AddListener(OnGameRestart);
        btn_GameCenter.onClick.AddListener(OnEnterGameCenter);
    }

    private void OnDisable() {
        btn_Start.onClick.RemoveListener(OnGameStart);
        btn_Restart.onClick.RemoveListener(OnGameRestart);
        btn_GameCenter.onClick.RemoveListener(OnEnterGameCenter);
    }

    private void OnGameStart() {
        OnGameRestart();
    }

    private void OnGameRestart() {
        levelLoader.LoadLevelFromBeginning();

        //if (GameController.GetInstance != null) {
        //    GameController.GetInstance.ResetGame();
        //}
    }

    private void OnEnterGameCenter() {

    }
}
