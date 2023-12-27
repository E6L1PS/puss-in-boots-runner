using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts
{
    public class LosePanel : MonoBehaviour
    {
        public void RestartLevel()
        {
            SceneManager.LoadScene(1);
        }
    
        public void MenuLevel()
        {
            SceneManager.LoadScene(0);
        }
    }
}
