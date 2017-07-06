using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2d : MonoBehaviour {


	// 移動スピード
	public float speed = 1;



	void Update ()
	{
		//  入力をxとzに代入
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");


		Vector2 direction = new Vector2 (x, y).normalized;

		// 移動する向きとスピードを代入する
		GetComponent<Rigidbody2D>().velocity = direction * speed;
	}
		

	private void OnCollisionEnter2D(Collision2D coll) {

		Debug.Log("OnCollisionEnter2D Called!!!");

		if (coll.gameObject.tag == "Enemy") {
			Debug.Log("r2D Called!!!");
			SceneManager.LoadScene ("Main"); 

		}
	}
}
