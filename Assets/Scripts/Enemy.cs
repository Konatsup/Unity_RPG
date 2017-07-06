 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour {
	public string enemyName;   //敵の名前
	public int level;  //レベル
	public int exp;  //所持経験値
	public int AP;  //AttackPoint(攻撃)
	public int DP;//DiffensePoint(防御力)
	public int HP; //HitPoint(HP)
	public int MaxHP; //MaxHitPoint(最大HP)
	public int MP;//MagicPoint(MP)
	public int MaxMP; //MaxMagicPoint(最大MP)
	public int speed; //素早さ
	public int skillID;  //どの技を使うのか
	public bool dead; //生きてるかどうか
	public bool poizon; //毒状態かどうか
	public bool mahi; //麻痺状態かどうか
	public bool tmpDiffence;



	public Player player1;
	public Text enemyText;
	public int poizonTurnCount; //毒状態の残りターン
	// Use this for initialization
	void Start () {
		//this.name = name;
		this.dead = false;
		this.InfoTextSet ();

		player1 = GameObject.Find ("Player1").GetComponent<Player>();



	}
		
	public void InfoTextSet(){
		string s;
		s = this.enemyName.ToString() + "\n";

		s = s + "HP：" + this.HP.ToString()+"\n";
		s = s + "MP：" + this.MP.ToString()+"\n";


		enemyText.text = s;
	}


	public void Action(){

		//
		while(true){
			this.skillID = Random.Range (0,5);
			if(this.MP >= GameManager.skills [this.skillID].spendMP) {
				break; //whileループを抜ける
			} else {
				Debug.Log ("MPが足りません"+this.skillID);
			}
		}


		switch (this.skillID) {
		case 0:
			Attack ();
			break;
		case 1:
			Diffense ();
			break;
		case 2:
			Fire ();
			break;
		case 3:
			Charge ();
			break;
		case 4:
			Heal ();
			break;
		case 5:
			Poizon ();
			break;
		default:
			Debug.Log ("技が選択されていません!!!");
			break;
		}


	}

	void Attack(){

		int attack = this.AP*this.level;
		int damage;
		string s;

		s = (this.enemyName + "のこうげき!\n");
		GameManager.Instance.SetText (s);

		damage = attack*attack/player1.DP;

		s = (player1.playerName +"に"+ damage +"ダメージ!\n");
		GameManager.Instance.SetText (s);

		player1.HP = player1.HP - damage;
	}

	void Diffense(){
		string s;
		this.DP = this.DP*2;
		this.tmpDiffence = true;
		s = (this.enemyName + "は みをかためている\n");
		GameManager.Instance.SetText (s);


	}	

	void Fire(){
		int attack = GameManager.skills[2].power * this.level;
		int damage;
		string s;

		s = (this.enemyName + "は" + GameManager.skills[2].skillName + "をとなえた\n");
		GameManager.Instance.SetText (s);
		damage =  (int)(attack*attack*1.1/player1.DP);

		s = (player1.playerName +"に"+ damage +"ダメージ!\n");
		GameManager.Instance.SetText (s);
		player1.HP = player1.HP - damage;
		this.MP = this.MP - GameManager.skills[2].spendMP;
	}

	void Charge(){
		string s;

		s = (this.enemyName + "は" + GameManager.skills[3].skillName+"をつかった\n");
		GameManager.Instance.SetText (s);
		this.AP = (int)(this.AP*GameManager.skills[3].power/10);


		this.MP = this.MP - GameManager.skills[3].spendMP;
	}

	void Heal(){
		int healPoint = GameManager.skills[4].power;
		int h = this.HP + healPoint;
		string s;
		s = (this.enemyName + "は" + GameManager.skills[4].skillName +"をとなえた\n");
		GameManager.Instance.SetText (s);

		s = (this.enemyName + "は"+ healPoint + "かいふくした\n");
		GameManager.Instance.SetText (s);

		if (h >= this.MaxHP) {
			this.HP = this.MaxHP;
		} else {
			this.HP = h;
		}

		this.MP = this.MP - GameManager.skills[4].spendMP;
	}


	void Poizon(){

		string s;
		s = (this.enemyName + "は" + GameManager.skills[4].skillName +"をとなえた\n");
		GameManager.Instance.SetText (s);

		s = (player1.playerName + "は"+"どく状態になった\n");
		GameManager.Instance.SetText (s);

		player1.poizon = true;
		player1.poizonTurnCount = 5;

		this.MP = this.MP - GameManager.skills[5].spendMP;
	}
		


	//ターン終了後の状態の更新
	public void AbnormalStateUpdate(){

		this.PoizonDamage ();
	}


	//毒ダメージ処理
	void PoizonDamage(){
		string s;
		int damage = this.MaxHP/10; //ダメージ量


		//毒だったらHPを減らす
		if (this.poizon == true) {
			this.HP = this.HP - damage;  //HPを減らす

			s = (this.enemyName + "に" + damage +"ダメージ!!\n");
			GameManager.Instance.SetText (s);
		}

		this.poizonTurnCount--;
			if (this.poizon == true && this.poizonTurnCount <= 0) {

				this.poizon = false;
				s = (this.enemyName + "は毒が治った！！\n");
				GameManager.Instance.SetText (s);
			}

	}






}
