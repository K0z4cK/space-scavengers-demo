using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataScriptableObject", menuName = "Scriptable Objects/LevelDataScriptableObject")]
public class LevelDataScriptableObject : ScriptableObject
{
    public int asteroidsCount;
    public float minAsteroidSpeed;
    public float maxAsteroidSpeed;
    public int bigAsteroidScore;
    public int smallAsteroidScore;
}
