 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class MyCanvas : MonoBehaviour {
	static Canvas _canvas;

	// Use this for initialization
	void Start () {
		// Canvasコンポーネントを保持
		_canvas = GetComponent<Canvas>();
	}

	/// 表示・非表示を設定する
	public static void SetActive(string name, bool b) {
		foreach(Transform child in _canvas.transform) {
			// 子の要素をたどる
			if(child.name == name) {
				// 指定した名前と一致
				// 表示フラグを設定
				child.gameObject.SetActive(b);

				// おしまい
				return;
			}

			//
			foreach (Transform grandChild in child.transform) {
				// 孫の要素をたどる
				if (grandChild.name == name) {
					// 指定した名前と一致
					// 表示フラグを設定
					Debug.Log("GrandChildCalled");
					grandChild.gameObject.SetActive (b);

					// おしまい
					return;
				}
			}

		}
		// 指定したオブジェクト名が見つからなかった
		Debug.LogWarning("Not found objname:"+name);
	}

	/// 表示・非表示を設定する(引数が1つバージョン)
	public static void SetActive(string name) {
		foreach(Transform child in _canvas.transform) {
			// 子の要素をたどる
			if(child.name == name) {
				// 指定した名前と一致
				// 表示フラグを設定
				//child.gameObject.SetActive(!child.gameObject.activeSelf);
				if (child.gameObject.activeInHierarchy == true) {
					//child.gameObject.SetActive (false);
					child.gameObject.GetComponent<Canvas>().enabled = false;
				} else {
					child.gameObject.SetActive (true);
				}
				// おしまい
				return;
			}

			//
			foreach (Transform grandChild in child.transform) {
				// 子の要素をたどる
				if (grandChild.name == name) {
					// 指定した名前と一致
					// 表示フラグを設定
					Debug.Log("GrandChildCalled" + grandChild.gameObject.activeInHierarchy);
					if (grandChild.gameObject.activeInHierarchy == true) {
						grandChild.gameObject.SetActive (false);

						//grandChild.gameObject.GetComponent<Canvas>().enabled = false;
					} else {
						grandChild.gameObject.SetActive (true);
//						grandChild.gameObject.GetComponent<Canvas>().enabled = true;
					}
					//grandChild.gameObject.SetActive (!(grandChild.gameObject.activeInHierarchy));
					//grandChild.gameObject.SetActive (false);
					//grandChild.gameObject.SetActive (grandChild.gameObject.activeSelf);

					Debug.Log("GrandChildCalled" + grandChild.gameObject.activeSelf);
					// おしまい
					return;
				}
			}

		}
		// 指定したオブジェクト名が見つからなかった
		Debug.LogWarning("Not found objname:"+name);
	}
}
