 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//ゲームの状態
public enum GameState{
	Start,    //戦闘開始フェーズ
	Prepare,  //ここは無視(デバッグ中)
	Select, //行動選択フェーズ
	Fight, //戦闘フェーズ
	TurnEnd, //行動終了後フェーズ
	Win, //勝利フェーズ
	Lose, //敗北フェーズ
	SkillAssign, //スキル割り振りフェーズ
	Menu  //Menuシーンに移る
}

public class GameManager : MonoBehaviour {

	public static GameManager Instance;     //ゲームマネージャー本体
	public CSVReader reader;     //ファイル読み込み本体

	public static Skill[] skills = new Skill[10];   //いろいろな技の情報を持った配列
	public Player player1;    //プレイヤーの情報
	public Enemy enemy1;      //敵の情報
	private GameState currentGameState;	// 現在の状態
	public Text log;   //戦闘ログ

	string filePath;
	int count = 0;

	//最初に呼び出される
	void Start(){

		//ゲームマネージャー本体を作成
		Instance = this;
		reader = new CSVReader ();
		filePath = "SkillSample";
		//Debug.Log ("ajsdlkfjasklfd");
		reader.Load (filePath);

		player1 = GameObject.Find ("Player1").GetComponent<Player>();
		enemy1 = GameObject.Find ("Enemy1").GetComponent<Enemy>();

		skills [0] = gameObject.AddComponent<Skill>();
		skills [0].SetSkill ("こうげき", 1, 0);

		skills [1] = gameObject.AddComponent<Skill>();
		skills [1].SetSkill ("ぼうぎょ", 1, 0);

		skills [2] = gameObject.AddComponent<Skill>();
		skills [2].SetSkill ("ファイア", 10, 3);

		skills [3] = gameObject.AddComponent<Skill>();
		skills [3].SetSkill ("ちからため", 15, 2);

		skills [4] = gameObject.AddComponent<Skill>();
		skills [4].SetSkill ("ヒール", 10, 3);

		skills [5] = gameObject.AddComponent<Skill>();
		skills [5].SetSkill ("どくどく", 0, 2);

				


		//player1 = playerObj.transform.Find("Player1").GetComponent<Player>();
		Debug.Log (reader.GetString(0,2));
		//Debug.Log (player1.playerName);

//		enemy1 = enemyObj.transform.Find("Enemy1").GetComponent<Enemy>();	
		//Debug.Log (enemy1.enemyName);

		//戦闘開始フェーズに移る
		SetCurrentState (GameState.Start);  

	}
		
	// 外からこのメソッドを使ってゲームの状態を変更
	public void SetCurrentState(GameState state){
		currentGameState = state;

		//Invoke("", 1.5f);
		OnGameStateChanged (currentGameState);
	}

	public void SetText(string tx){
		log.text = log.text + tx;
		enemy1.InfoTextSet ();
		player1.InfoTextSet ();

	}

	// 状態が変わったら何をするか
	void OnGameStateChanged(GameState state){
		switch (state) {
		case GameState.Start:
			StartAction ();
			break;
		/*case GameState.Prepare:
			//StartCoroutine (PrepareCoroutine ());
			break;*/
		case GameState.Select:
			Debug.Log ("GameStateSelect");
			SelectAction ();
			break;
		case GameState.Fight:
			Debug.Log ("GameStateFight");
			FightAction ();
			break;
		case GameState.TurnEnd:
			Debug.Log ("GameStateTurnEnd");
			TurnEndAction ();
			break;
		case GameState.Win:
			Debug.Log ("GameStateWin");
			WinAction ();
			break;
		case GameState.Lose:
			Debug.Log ("GameStateLose");
			LoseAction ();
			break;
		case GameState.SkillAssign:
			Debug.Log ("GameStateSkillAssign");
			SkillAssignAction ();
			break;
		case GameState.Menu:
			Debug.Log ("GameStateLose");
			//Menu ();
			break;
		default:
			break;
		}
	}

	// Startになったときの処理
	void StartAction(){
		Instance.SetText (enemy1.enemyName + "が あらわれた\n");

		//行動選択フェーズに移る
		SetCurrentState (GameState.Select);
	}


	// Prepareになったときの処理
/*	IEnumerator PrepareCoroutine(){
		//	label.text = "3";
		for(int i = 0; i<100;i++){
			Debug.Log("acccccc");
			yield return new WaitForSeconds (10);
			//	label.text = "2";
		}
		yield return new WaitForSeconds (1);
		//	label.text = "1";
		yield return new WaitForSeconds (1);
		//	label.text = "";
		//SetCurrentState (GameState.Select);
	}
*/

	// Selectになったときの処理
	void SelectAction(){
		//プレイヤーの行動が選択されていたら
		if (player1.actionFlg == true) {
			//戦闘フェーズに移行
			SetCurrentState (GameState.Fight);
		}

		Instance.SetText ("<color=lime>選択中</color>\n"); 

	}

	// Fightになったときの処理
	void FightAction(){

		//スピードを比べて行動順を決定
		if (player1.speed >= enemy1.speed) {   //プレイヤーの方が早かった場合

			//プレイヤーが行動する
			player1.Action ();

			//敵のHPが0か確認し、生きていれば敵の行動を実行
			if (LifeJudge (enemy1) == true) {
				enemy1.Action ();
				LifeJudge (player1);
			}

		} else {   //敵の方が早かった場合
			
			//プレイヤーが行動する
			enemy1.Action ();

			//プレイヤーのHPが0か確認し、生きていればプレイヤーの行動を実行
			if (LifeJudge (player1) == true) {
				player1.Action ();
				LifeJudge (enemy1);
			}

		}


		SetCurrentState (GameState.TurnEnd);

	}
		






	// TurnEndになったときの処理
	void TurnEndAction(){

		//毒ダメージの処理
		player1.AbnormalStateUpdate();
		enemy1.AbnormalStateUpdate();


		//状態異常の更新(麻痺、眠り)



		//ステータスの更新
		if(player1.tmpDiffence == true){
			
			player1.DP = player1.DP / 2;
			player1.tmpDiffence = false;
		}

		if(enemy1.tmpDiffence == true){
			
			enemy1.DP = enemy1.DP / 2;
			enemy1.tmpDiffence = false;
		}



		//行動選択フェーズに移る
		//SetCurrentState (GameState.Select);
	}








	// Winになったときの処理
	void WinAction(){

		int APUP = 1; //AttackPointUP
		int SPUP = 3; //スキルポイントアップ

		Instance.SetText (enemy1.enemyName + "を 倒した\n");
		Instance.SetText ("経験値を" + enemy1.exp + "獲得!!\n");
		player1.exp = player1.exp + enemy1.exp;


		//レベルアップ
		if(player1.exp >= 5){

			Instance.SetText (player1.playerName + "は レベルアップした\n");

			//テキストの表示
			Instance.SetText ("こうげき力が1上がった\n");
			Instance.SetText ("こうげき力が1上がった\n");
			Instance.SetText ("こうげき力が1上がった\n");
			Instance.SetText ("こうげき力が1上がった\n");
			Instance.SetText ("こうげき力が1上がった\n");


			//プレイヤーのステータス更新
			player1.AP = player1.AP + APUP;
			player1.AP = player1.AP + APUP;
			player1.AP = player1.AP + APUP;
			player1.AP = player1.AP + APUP;
			player1.AP = player1.AP + APUP;



			//スキルポイント
			Instance.SetText ("スキルポイントを" +  SPUP  + "取得!!\n");
			player1.SP = player1.SP + SPUP;


		



		}


		//yield return new WaitForSeconds (1);
		//デバッグ中(5/6(土)現在)
		Debug.Log("aaaaaaaa");
		//StartCoroutine(PrepareCoroutine());
		Debug.Log("aaaabbbbb");


		Invoke("LoadScene", 2.5f);

		//メニューシーンに移る
		//SceneManager.LoadScene("2DMain"); 
	}


	void LoadScene(){
		//メニューシーンに移る
		SceneManager.LoadScene("2DMain"); 
	}


	// Loseになったときの処理
	void LoseAction(){
		
		//敗北テキストの表示
		Instance.SetText (player1.playerName + "は 敗北した\n");

	}





	void SkillAssignAction (){




	}





	/*以降のメソッドは別のファイルに記述してインポートしてもいいかも*/


	//敵が生きているかどうか判定するメソッド
	bool LifeJudge(Enemy enemy){
		
		if (enemy.HP <= 0) {
			enemy.dead = true;
			Instance.SetText (enemy.enemyName + "は たおれた\n");

			//勝利フェーズに移行
			SetCurrentState (GameState.Win);

			return false; //敵が死んでいる
		}
		return true;   //敵が生きている
	}


	//プレイヤーが生きているかどうか判定するメソッド
	bool LifeJudge(Player player){
		
		if (player.HP <= 0) {
			player.dead = true;
			Instance.SetText (player.playerName + "は たおれた\n");

			//敗北フェーズに移行
			SetCurrentState (GameState.Lose);

			return false;  //プレイヤーが死んでいる
		}
		return true;    //プレイヤーが生きている
	}


	//技のIDをセットする
	public void SetID(int ID){

		//所持MPが選択された技の消費MPを超えているか判定
		if (player1.MP >= skills [ID].spendMP) {

			player1.skillID = ID;

			//プレイヤーの行動を選択した状態に
			player1.actionFlg = true;

			//行動選択フェーズに移行
			SetCurrentState (GameState.Select);
		
		} else {

			Instance.SetText ("MPが足りません\n");
		}


	


	}


}
