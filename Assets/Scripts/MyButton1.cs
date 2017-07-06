using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton1 : MonoBehaviour {

	/// ボタンをクリックした時の処理
	public void OnClickFight() {

		Debug.Log("Fight click!");

		//通常攻撃と防御のボタンをアクティブにする
		MyCanvas.SetActive("Attack");
		MyCanvas.SetActive("Diffense");
	}
}
