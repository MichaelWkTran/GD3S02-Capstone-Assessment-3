using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class SceneTransition : MonoBehaviour
{
    static SceneTransition m_singleton;

    public string m_newSceneName;
    public AnimationClip m_inTransition;
    public AnimationClip m_outTransition;
    Animator m_animator;
    AnimatorOverrideController m_animatorOverrideController;

    public SceneTransition(string _newSceneName, GameObject _inTransition, GameObject _outTransition)
    {
        m_newSceneName = _newSceneName;
        _inTransition.transform.parent = transform;
        _outTransition.transform.parent = transform;
    }

    void Start()
    {
        //Check singleton
        if (m_singleton) { if (m_singleton != this) { Destroy(gameObject); return; } }
        else { m_singleton = this; }

        //
        m_animator = GetComponent<Animator>();
        m_animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        m_animator.runtimeAnimatorController = m_animatorOverrideController = new AnimatorOverrideController(m_animator.runtimeAnimatorController);
        m_animatorOverrideController.ApplyOverrides(new List<KeyValuePair<AnimationClip, AnimationClip>>()
        {
            new KeyValuePair<AnimationClip, AnimationClip>(m_animatorOverrideController.animationClips[0], m_inTransition),
            new KeyValuePair<AnimationClip, AnimationClip>(m_animatorOverrideController.animationClips[1], m_outTransition)
        });

        //Activate Screen Transition
        DontDestroyOnLoad(gameObject);
        if (m_inTransition == null && m_outTransition == null)
        {
            Debug.LogError("The SceneTransition have transitions");
            SceneManager.LoadScene(m_newSceneName);
        }
        else StartCoroutine(LoadLevel());
        
        //
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    IEnumerator LoadLevel()
    {
        if (m_inTransition != null) yield return new WaitForSecondsRealtime(m_inTransition.length);
        SceneManager.LoadScene(m_newSceneName);
    }

    IEnumerator FinishTransition()
    {
        if (m_outTransition != null) yield return new WaitForSecondsRealtime(m_outTransition.length);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        m_animator.SetTrigger("Load Scene");
        StartCoroutine(FinishTransition());
    }
}