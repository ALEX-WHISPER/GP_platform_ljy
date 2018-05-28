using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour {
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;

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
        } else {
            Destroy(gameObject);
        }

        levelLoader = GetComponent<LevelLoader>();
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
        if (gameOverPanel != null && gameOverPanel.activeSelf) {
            gameOverPanel.SetActive(false);
        }

        levelLoader.ResetUI();
    }

    public void ResetGame() {
        gameOverPanel.SetActive(false);
        m_IsGameOver = false;
        
        if (m_PlayerHolder == null) {
            return;
        }

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
        m_IsGameOver = true;
    }

    public void GameWin() {
        if (!gameWinPanel.activeSelf) {
            gameWinPanel.SetActive(true);
        }

        PlayerController2D.GetInstance.DisableMovement();
        m_IsGameOver = true;
    }

    public void OnPassedLevel() {
        levelLoader.LoadNextLevelOnDelay(0.5f);
    }
}
