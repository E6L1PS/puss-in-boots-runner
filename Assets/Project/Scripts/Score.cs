using TMPro;
using UnityEngine;

namespace Project.Scripts
{
    public class Score : MonoBehaviour
    {
        [SerializeField] public int score;
        [SerializeField] public int milkBottles;
        [SerializeField] private TextMeshProUGUI scoreCountText;
        [SerializeField] private TextMeshProUGUI milkBottlesCountText;
        [SerializeField] private Transform player;

        private void Update()
        {
            score = (int)(player.position.z / 2);
            scoreCountText.text = score.ToString();
        }

        public void AddMilkBottle()
        {
            milkBottles++;
            milkBottlesCountText.text = milkBottles.ToString();
        }
    }
}