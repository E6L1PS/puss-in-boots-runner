using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI maxScoreCountText;
        [SerializeField] private TextMeshProUGUI milkBottlesCountText;

        private void Start()
        {
            maxScoreCountText.text = PlayerPrefs.GetInt("score", 0).ToString();
            milkBottlesCountText.text = PlayerPrefs.GetInt("milk", 0).ToString();
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}