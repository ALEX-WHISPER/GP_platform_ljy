using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour {
    public Vector2 playerInitPos_Level1;
    public Vector2 playerInitPos_Level2;

    public GameObject gameOverPanel;

    private static GameController _instance;
    private LevelLoader levelLoader;
    private PlayerHealth m_PlayerHealth;
    private Transform m_PlayerHolder;
    private bool m_IsGameOver;

    public static GameController GetInstance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<GameController>();
            }
            return _instance;
        }
    }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        levelLoader = GetComponent<LevelLoader>();

        if (GameObject.Find("PlayerHolder") == null) {
            Debug.LogError("Player is null!");
            return;
        }

        m_PlayerHolder = GameObject.Find("PlayerHolder").transform;
        m_PlayerHealth = m_PlayerHolder.GetComponentInChildren<PlayerHealth>();
        if (m_PlayerHealth == null) {
            Debug.LogError("PlayerHealth is null!");
            return;
        }
    }

    private void OnEnable() {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDestroy() {
        //SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void Update() {
        if (!m_IsGameOver) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            levelLoader.LoadLevelWithUISettings(0);
        }
    }

    public void ResetUI() {
        gameOverPanel.SetActive(false);
    }

    public void ResetGame() {
        gameOverPanel.SetActive(false);
        m_IsGameOver = false;
        
        m_PlayerHolder.GetComponentInChildren<Damageable>().SetInitialHealth();
        m_PlayerHealth.GetComponentInChildren<PlayerHealth>().PlayerReborn();
    }

    public void GameOver() {
        //  GameOver UI
        if (!gameOverPanel.activeSelf) {
            gameOverPanel.SetActive(true);
        }

        //  关闭player运动功能
        PlayerController2D.GetInstance.DisableMovement();

        //  保存游戏进度
        PlayerPrefs.SetInt("StuckSceneIndex", SceneManager.GetActiveScene().buildIndex);

        m_IsGameOver = true;
    }

    public void GameWin() {

    }

    public void OnPassedLevel() {
        levelLoader.LoadNextLevelOnDelay(0.5f);
    }

    private void OnSceneChanged(Scene from, Scene to) {
        ResetUI();

        Debug.Log(string.Format("From scene: {0} to scene: {1}", from.name, to.name));
        levelLoader.ResetUI();

        if (to.name == "_Start") {
            //  disable player movement
            PlayerController2D.GetInstance.DisableMovement();
            PlayerController2D.GetInstance.transform.GetComponent<PlatformerMotor2D>().frozen = true;

        } else {
            PlayerController2D.GetInstance.EnableMovement();
            PlayerController2D.GetInstance.transform.GetComponent<PlatformerMotor2D>().frozen = false;

            if (to.name == "level1") {
                m_PlayerHolder.localPosition = this.playerInitPos_Level1;
            } else if (to.name == "level2") {
                m_PlayerHolder.localPosition = this.playerInitPos_Level2;
            }
        }
    }
}
