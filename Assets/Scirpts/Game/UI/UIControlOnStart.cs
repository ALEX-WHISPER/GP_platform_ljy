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
        //if (PlayerPrefs.HasKey("StuckSceneIndex")) {
        //    levelLoader.LoadLevel(PlayerPrefs.GetInt("StuckSceneIndex"));
        //} else {
        //    OnGameRestart();
        //}
        OnGameRestart();
    }

    private void OnGameRestart() {
        levelLoader.LoadLevelFromBeginning();
        GameController.GetInstance.ResetGame();
    }

    private void OnEnterGameCenter() {

    }
}
