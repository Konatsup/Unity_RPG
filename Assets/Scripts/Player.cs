 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public static Player Instance;
	public string playerName;   //プレイヤーの名前
	public int level;  //レベル
	public int exp;  //所持経験値
	public int HP; //HitPoint(HP)
	public int MaxHP; //MaxHitPoint(最大HP)
	public int MP;//MagicPoint(MP)
	public int MaxMP; //MaxMagicPoint(最大MP)
	public int AP;  //AttackPoint(攻撃)
	public int DP;//DiffensePoint(防御力)
	public int speed; //素早さ
	public int SP;    //スキルポイント
	public int skillID;  //どの技を使うのか
	public bool dead; //生きてるかどうか
	public bool poizon; //毒状態かどうか
	public bool mahi; //麻痺状態かどうか
	//public bool tmpCharge; //攻撃溜め状態かどうか
	public bool tmpDiffence; //防御状態かどうか
	public bool actionFlg;  //行動終了しているかどうか
	public Enemy enemy1;   //敵の情報
	public Text playerText;  //プレイヤーの情報を表示するテキスト
    public int poizonTurnCount;
	public int chargeCount;


	// Use this for initialization
	void Start () {
		Instance = this;

		this.dead = false;
		this.actionFlg = false;
		this.InfoTextSet ();
		//this.tmpCharge = false;
		this.chargeCount = 0;
		enemy1 = GameObject.Find ("Enemy1").GetComponent<Enemy>();


	}

	//スキルを初期化するメソッド
	public void SetPlayer(string name, int level, int exp, int HP, int MaxHP, int MP, int MaxMP, int AP, int DP, 
		int speed, int SP, int skillID, bool dead, bool poizon, bool mahi, bool actonFlg){
		this.playerName = name;
		this.exp = exp;
		this.HP = HP;
		this.MaxHP = MaxHP;
		this.MP = MaxMP;
		this.AP = AP;
		this.DP = DP;
		this.speed = speed;
		this.SP = SP;
		this.skillID = skillID;
		this.dead = dead;
		this.poizon = poizon;
		this.mahi = mahi;
		this.actionFlg = actonFlg;
	}



	//プレイヤーの情報を更新するメソッド
	public void InfoTextSet(){
		string s;
		s = this.playerName.ToString() + "\n";

		s = s + "HP：" + this.HP.ToString()+"\n";
		s = s + "MP：" + this.MP.ToString()+"\n";

		playerText.text = s;
	}


	//プレイヤーの攻撃
	public void Action(){
		//選択された技によって条件分岐する
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
		default: //デバッグ用
			Debug.Log ("技が選択されていません!!!");
			break;
		}

		this.actionFlg = false;
	}

	//通常攻撃
	void Attack(){
		int attack; //攻撃を計算
		int damage;
		string s;

		s = (this.playerName + "のこうげき!\n");
		GameManager.Instance.SetText (s);


		if (this.chargeCount >= 1) {
			//this.tmpCharge = false;
			int tmpAP = this.AP;

			for (int i = 0; i < this.chargeCount; i++) {
				tmpAP = (int)(tmpAP * 1.5);
			}

			this.chargeCount = 0;

			attack = tmpAP * this.level; 


			//ダメージ計算
			damage = attack * attack / enemy1.DP; 



		} else {

			attack = this.AP * this.level;
			//ダメージ計算
			damage = attack * attack / enemy1.DP; 


		}
		s = (enemy1.enemyName + "に" + damage + "ダメージ!\n");
		GameManager.Instance.SetText (s);

		//敵のHPに反映
		enemy1.HP = enemy1.HP - damage; 
	}

	//防御
	void Diffense(){
		string s;
		this.DP = this.DP*2;  //防御力を2倍にする
		this.tmpDiffence = true; //状態を防御にする
		s = (this.playerName + "は みをかためている\n");
		GameManager.Instance.SetText (s);
		 

	}	

	 //ファイア
	void Fire(){
		int attack = GameManager.skills[2].power * this.level;   
		int damage;
		string s;

		s = (this.playerName + "は" + GameManager.skills[2].skillName +"をとなえた\n");
		GameManager.Instance.SetText (s);

	

		//ダメージ計算
		damage =  (int)(attack*attack*1.1/enemy1.DP);

		s = (enemy1.enemyName +"に"+ damage +"ダメージ!\n");
		GameManager.Instance.SetText (s);

		//敵HPに反映
		enemy1.HP = enemy1.HP - damage;



		//自分のMPに反映
		this.MP = this.MP - GameManager.skills[2].spendMP;
	}
	 

	//ちからため
	void Charge(){
		
		string s;

		s = (this.playerName + "は" + GameManager.skills[3].skillName+"をつかった\n");
		GameManager.Instance.SetText (s);
		//this.tmpCharge = true;

		this.chargeCount++;

		s = (this.chargeCount + "回ためている\n");
		GameManager.Instance.SetText (s);
		 //this.AP = (int)(this.AP*GameManager.skills[3].power/10);
		//this.AP = this.AP * 2;
		this.MP = this.MP - GameManager.skills[3].spendMP;

	}


	//ヒール
	void Heal(){
		int healPoint = GameManager.skills[4].power;
		int h = this.HP + healPoint;
		string s;
		s = (this.playerName + "は" + GameManager.skills[4].skillName +"をとなえた\n");
		GameManager.Instance.SetText (s);

		s = (this.playerName + "は"+ healPoint + "かいふくした\n");
		GameManager.Instance.SetText (s);

		//もしHPが最大HP以上だったら、最大HPで初期化
		if (h >= this.MaxHP) {
			this.HP = this.MaxHP;
		} else {
			this.HP = h;
		}
			
		this.MP = this.MP - GameManager.skills[4].spendMP;
	}





	//どくどく
	void Poizon(){

		string s;
		s = (this.playerName + "は" + GameManager.skills[5].skillName +"をとなえた\n");
		GameManager.Instance.SetText (s);

		s = (enemy1.enemyName + "は"+"どく状態になった\n");
		GameManager.Instance.SetText (s);

		enemy1.poizon = true;
		enemy1.poizonTurnCount = 5;

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

			s = (this.playerName + "に" + damage +"ダメージ!!\n");
			GameManager.Instance.SetText (s);
		}
			
		this.poizonTurnCount--;

		if (this.poizonTurnCount <= 0) {

			this.poizon = false;
		}

	}



}
