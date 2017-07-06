using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill : MonoBehaviour {
	public string skillName;  //技名
	public int power; //威力や回復量とか(もっと綺麗にしたい)
	public int spendMP; //消費MP

	//スキルを初期化するメソッド
	public void SetSkill(string name, int power, int spendMP){
		this.skillName = name;
		this.power = power;
		this.spendMP = spendMP;

	}

}
