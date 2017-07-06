using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeModeButton : MonoBehaviour {

	//シーンを変える
	public void MoveScene()
	{
		// 引数にシーン名を指定する
		// Build Settings で確認できる sceneBuildIndex を指定しても良い
		SceneManager.LoadScene("Main"); 
	}
}
