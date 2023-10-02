using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _hPCount;
    [SerializeField] private TMP_Text _wavesCount;
    [SerializeField] private TMP_Text _wallsCount;
    [SerializeField] private TMP_Text _levelWinText;
    [SerializeField] private TMP_Text _levelLostText;
    [SerializeField] private Button _nextLevelButton;

    private View _view;


    private void Start()
    {
        _view = Globals.Global.View;

        _view.OnHPChange.Subscribe((health) =>
        {
            HPChange(health);
        });
        
        _view.OnWallsCountChange.Subscribe((walls) =>
        {
            WallsCountChange(walls);
        });

        _view.OnWawesChange.Subscribe((waves, totalWaves) =>
        {
            WavesCountChange(waves, totalWaves);
        });

        _view.OnLevelWin.Subscribe(() =>
        {
            LevelWin();
        });
        
        _view.OnLevelLost.Subscribe(() =>
        {
            LevelLost();
        });
        
        _view.OnLevelStart.Subscribe(() =>
        {
            LevelStart();
        });

        _nextLevelButton.gameObject.SetActive(false);
        _levelWinText.gameObject.SetActive(false);
        _levelLostText.gameObject.SetActive(false);
    }

    private void LevelLost()
    {
        _nextLevelButton.gameObject.SetActive(false);
        _levelWinText.gameObject.SetActive(false);
        _levelLostText.gameObject.SetActive(true);
    }

    private void LevelStart()
    {
        _nextLevelButton.gameObject.SetActive(false);
        _levelWinText.gameObject.SetActive(false);
        _levelLostText.gameObject.SetActive(false);
    }

    private void LevelWin()
    {
        _nextLevelButton.gameObject.SetActive(true);
        _levelWinText.gameObject.SetActive(true);
        _levelLostText.gameObject.SetActive(false);
    }

    private void WavesCountChange(int waves, int totalWaves)
    {
        _wavesCount.text = (waves + 1) + " / " + totalWaves;
    }

    private void WallsCountChange(int walls)
    {
        _wallsCount.text = walls.ToString();
    }

    private void HPChange(int health)
    {
        _hPCount.text = health.ToString();
    }
}
