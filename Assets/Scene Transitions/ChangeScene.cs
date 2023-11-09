using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour 
{
    [SerializeField] SceneTransition m_sceneTransitionPrefab;

    public void LoadScene (string _SceneName) 
    {
        if (m_sceneTransitionPrefab == null) SceneManager.LoadScene(_SceneName);
        else Instantiate(m_sceneTransitionPrefab).m_newSceneName = _SceneName;
    }
}