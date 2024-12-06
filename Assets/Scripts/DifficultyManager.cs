using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public enum DifficultyLevel { Easy, Medium, Hard, Personalized }
    public static DifficultyManager Instance { get; private set; }

    public DifficultyLevel CurrentDifficulty { get; private set; } = DifficultyLevel.Easy;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste à travers les scènes
        }
    }

    public void SetDifficulty(DifficultyLevel difficulty)
    {
        CurrentDifficulty = difficulty;
    }

    public DifficultyManager.DifficultyLevel GetDifficulty()
    {
        return DifficultyManager.Instance.CurrentDifficulty;
    }

    public float GetDamageMultiplier()
    {
        return CurrentDifficulty switch
        {
            DifficultyLevel.Easy => 0.5f,
            DifficultyLevel.Medium => 1.0f,
            DifficultyLevel.Hard => 1.5f,
            _ => 1.0f,
        };
    }
}
