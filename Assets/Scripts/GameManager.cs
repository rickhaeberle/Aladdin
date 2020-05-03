using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager {
    public float Volume { get; private set; }

    private int _lives;
    private int _jewels;

    private static GameManager instance;

    private HUDManager _hudManager;

    private bool _gameOver;

    private Vector3? _currentCheckpoint;
    private List<Vector3> _activatedCheckpoints;

    private GameManager() {
        Volume = 1f;

        Reset();
    }

    public static GameManager Instance {
        get {
            if (instance == null) {
                instance = new GameManager();
            }

            return instance;
        }
    }

    private void Reset() {
        _lives = 3;
        _jewels = 0;

        _gameOver = false;

        _currentCheckpoint = null;
        _activatedCheckpoints = new List<Vector3>();
    }

    public void Restart() {
        if (_gameOver) {
            Reset();
            SceneManager.LoadScene("GameOver");

        } else {
            SceneManager.LoadScene("Game");

        }
    }

    public void GoToDeathScene() {
        _lives--;
        _gameOver = _lives < 0;

        SceneManager.LoadScene("Death");
    }

    public void LevelCompleted() {
        Reset();
        SceneManager.LoadScene("LevelCompleted");

    }

    public void RegisterHUDManager(HUDManager hudManager) {
        _hudManager = hudManager;
    }

    public void UpdateHUD(int hp, int apples) {
        _hudManager?.UpdateHUD(hp, _lives, _jewels, apples);
    }

    public void ManageJewels(int value) {
        _jewels += value;
    }

    public Vector3? GetCheckpoint() {
        return _currentCheckpoint;
    }

    public void SetCurrentCheckpoint(Vector3 checkpoint) {
        _currentCheckpoint = checkpoint;
        _activatedCheckpoints.Add(checkpoint);
    }

    public bool IsCheckpointActive(Vector3 checkpoint) {
        return _activatedCheckpoints.Contains(checkpoint);
    }

    public bool Buy(PeddlerItem item) {
        if (_jewels >= item.Price) {
            _jewels -= item.Price;

            if (item.Item == EShopItem.Life) {
                _lives++;
            }

            return true;
        }

        return false;
    }


}
