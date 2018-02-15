using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanelScript : MonoBehaviour
{

    public Text text;
    public GameObject player;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Current Level : " + player.GetComponent<Player>().GetCurrentLevel() + "\nAvailable Skill points :" + player.GetComponent<Player>().GetSkillPoint() + "\nHealth : " + player.GetComponent<Health>().GetMaxHp() + "\nSpeed : " + player.GetComponent<PlayerMovement>().moveSpeed;
    }

    public void IncreaseHealthClicked()
    {
        if (player.GetComponent<Player>().GetSkillPoint() > 0)
        {
            player.SendMessage("IncreaseMaxHp", 10);
            player.SendMessage("DecreaseSkillPoint");
        }
    }

    public void IncreaseSpeedClicked()
    {
        if (player.GetComponent<Player>().GetSkillPoint() > 0)
        {
            player.GetComponent<PlayerMovement>().moveSpeed += 5.0f;
            player.SendMessage("DecreaseSkillPoint");
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }
}
