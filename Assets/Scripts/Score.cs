using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] public int score;
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI textScore;

    private void Update()
    {
        score = (int)(player.position.z / 2);
        textScore.text = score.ToString();
    }
    
}