using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Toggles liés à la difficulté")]
    public Toggle easyToggle; 
    public Toggle mediumToggle; 
    public Toggle hardToggle;
    public ToggleGroup toggleGroup; // Nouveau : groupe de toggles
    /*public Toggle personalizedToggle;*/

    [Header("Dégâts")]
    public float baseDamage = 1f; // Dégâts de base pour les calculs
    private float finalDamage;

    void Start()
    {
        // Associer les toggles au groupe
        easyToggle.group = toggleGroup;
        mediumToggle.group = toggleGroup;
        hardToggle.group = toggleGroup;

        // Initialiser les toggles en fonction de la difficulté actuelle
        InitializeToggles();

        // Associer les événements
        easyToggle.onValueChanged.AddListener(isOn => OnDifficultyChanged(isOn, DifficultyManager.DifficultyLevel.Easy));
        mediumToggle.onValueChanged.AddListener(isOn => OnDifficultyChanged(isOn, DifficultyManager.DifficultyLevel.Medium));
        hardToggle.onValueChanged.AddListener(isOn => OnDifficultyChanged(isOn, DifficultyManager.DifficultyLevel.Hard));

    }

    // Update is called once per frame
    void Update()
    {
        // Calculer les dégâts ajustés en fonction de la difficulté
        float difficultyMultiplier = DifficultyManager.Instance.GetDamageMultiplier();
        finalDamage = baseDamage * difficultyMultiplier;

        // Afficher les informations sur pression de la touche E
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"Attack Damage: {finalDamage}");
            Debug.Log($"Current Difficulty: {DifficultyManager.Instance.CurrentDifficulty}");
        }
    }

    private void InitializeToggles()
    {
        var difficulty = DifficultyManager.Instance.CurrentDifficulty;
        easyToggle.isOn = (difficulty == DifficultyManager.DifficultyLevel.Easy);
        mediumToggle.isOn = (difficulty == DifficultyManager.DifficultyLevel.Medium);
        hardToggle.isOn = (difficulty == DifficultyManager.DifficultyLevel.Hard);
    }

    private void OnDifficultyChanged(bool isOn, DifficultyManager.DifficultyLevel difficulty)
    {
        if (isOn)
        {
            DifficultyManager.Instance.SetDifficulty(difficulty);
            Debug.Log($"Difficulté changée : {difficulty}");
        }
    }

    private void OnDestroy()
    {
        // Nettoyer les événements pour éviter les erreurs
        easyToggle.onValueChanged.RemoveAllListeners();
        mediumToggle.onValueChanged.RemoveAllListeners();
        hardToggle.onValueChanged.RemoveAllListeners();
    }
}
