using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneUtil : MonoBehaviour
    {
        public static void Goto(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}