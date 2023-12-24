using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI textScore;

    private void Update()
    {
        textScore.text = ((int)(player.position.z / 2)).ToString();
    }
}