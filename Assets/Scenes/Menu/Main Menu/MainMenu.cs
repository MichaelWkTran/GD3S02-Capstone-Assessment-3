using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button m_exitButton;

    void Start()
    {
#if UNITY_PS4
        m_exitButton.gameObject.SetActive(false);
#endif

    }

    public void ExitButtonOnClick()
    {
        Application.Quit();
    }
}
