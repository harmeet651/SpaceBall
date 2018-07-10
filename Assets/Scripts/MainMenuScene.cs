using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene : MonoBehaviour {

	public void PlayGame ()
	{
		SceneManager.LoadScene("Scenes/GameScene");
	}

    public void ChooseAvatar()
    {
        Debug.Log("in avatar");
        SceneManager.LoadScene("Scenes/Selector");
    }
    public void QuitGame ()
	{
		Debug.Log("QUIT!");
		Application.Quit();
	}

	public void TutorialGame ()
	{
		SceneManager.LoadScene("Scenes/TutorialScene");
	}

}
