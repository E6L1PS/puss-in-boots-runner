using UnityEngine;

namespace Project.Scripts
{
    public class MilkBottle : MonoBehaviour
    {
        public void Update()
        {
            transform.Rotate(0, 100 * Time.deltaTime, 0);
        }
    }
}