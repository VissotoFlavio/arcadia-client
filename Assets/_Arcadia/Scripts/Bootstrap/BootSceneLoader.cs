using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arcadia.Bootstrap
{
    /// <summary>
    /// Minimal entry point for the Arcadia client. Boot is always the first enabled build scene.
    /// </summary>
    public sealed class BootSceneLoader : MonoBehaviour
    {
        private const string InitialSceneName = "Prototype_Exploration";

        private void Start()
        {
            SceneManager.LoadScene(InitialSceneName, LoadSceneMode.Single);
        }
    }
}
