using UnityEngine;

[CreateAssetMenu(fileName = "GameSetting", menuName = "ScriptableObjects/Game Config", order = 2)]
public class GameConfigSO : ScriptableObject
{
    [Header("Settings Game")]
    public int LogMessageDurationTime = 3;
}

