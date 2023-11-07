using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [SerializeField] Canvas m_buttonPrompt;
    [SerializeField] Canvas m_minigame;
    [SerializeField] ParticleSystem m_completedParticles;
    List<Player> m_playersInRange = new List<Player>();
    Player m_interactingPlayer;
    bool m_isInteracting;

    public Player m_InteractingPlayer
    {
        get { return m_interactingPlayer; }
        set
        {
            //Set interacting player back to default
            if (value == null)
            {
                if (m_interactingPlayer) m_interactingPlayer.m_state = Player.PlayerState.Default;
                if (m_playersInRange.Count > 0) m_buttonPrompt.gameObject.SetActive(true);
            }
            
            //Set interacting player
            m_interactingPlayer = value;
            
            //Show minigame UI
            m_minigame.gameObject.SetActive(m_interactingPlayer != null);

            //Set player to the interacting state
            if (m_interactingPlayer != null)
            {
                m_buttonPrompt.gameObject.SetActive(false);
                m_interactingPlayer.m_state = Player.PlayerState.TowerRestoring;

                //Change Minigame Colour
                {
                    var minigameImage = m_minigame.GetComponent<Image>();
                    Color newMinigameColour = GameMode.m_current.m_playerColours[m_interactingPlayer.m_playerIndex];
                    newMinigameColour.a = minigameImage.color.a;
                    minigameImage.color = newMinigameColour;
                }
            }

            //Set Is Interacting
            m_isInteracting = (m_interactingPlayer != null);
        }
    }

    void OnTriggerEnter2D(Collider2D _col)
    {
        if (!enabled) return;

        //Check whether the tower does not have a player interacting it
        if (m_InteractingPlayer != null) return;

        //Get player from collider and check its validity
        Player collidingPlayer = _col.GetComponent<Player>();
        if (collidingPlayer == null) return;

        //Add the player to the in range list
        m_playersInRange.Add(collidingPlayer);

        //Show button prompt
        m_buttonPrompt.gameObject.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D _col)
    {
        if (!enabled) return;

        //Get player from collider and check its validity
        Player collidingPlayer = _col.GetComponent<Player>();
        if (collidingPlayer == null) return;

        //Remove the player in range list
        m_playersInRange.Remove(collidingPlayer);

        //Hide button prompt if there are no players near the tower
        if (m_playersInRange.Count <= 0) m_buttonPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        //Failsafe to ensure the UI gets disabled when the interacting player is destroyed (Such as when killed)
        if (m_isInteracting && m_InteractingPlayer == null) m_InteractingPlayer = null;

        //Open the minigame when the player interacts with it
        if (m_InteractingPlayer == null)
        {
            foreach (Player player in m_playersInRange)
            {
                if (player.m_interactAction.triggered == false) continue;
                m_InteractingPlayer = player;

                break;
            }
        }
        //Cancel out of the minigame
        else
        {
            if (m_InteractingPlayer.m_cancelAction.triggered)
            {
                m_interactingPlayer.m_state = Player.PlayerState.Default;
                m_interactingPlayer = null;
                m_minigame.gameObject.SetActive(false);
            }
        }
    }

    public void OnCompleted()
    {
        #region m_completedParticles.Play();
        {
            var particleSettings = m_completedParticles.main;
            particleSettings.startColor = GameMode.m_current.m_playerColours[m_interactingPlayer.m_playerIndex];
            m_completedParticles.Play();
        }
        #endregion
        m_InteractingPlayer = null;
        m_buttonPrompt.gameObject.SetActive(false);
        enabled = false;
        GameMode.m_current.OnTowerCompleted(this);
    }
}
