using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.IO;

// Resources.Loadのラッパークラス
public static class ResourceLoader
{
    public enum DataFolder
    {
        Root,
        Effect,

		// アイコン関係(キャラ以外)
		IconSprite,

		// キャラ関係
        Character,
		Yaorozu,
		Aragami,
		/// <summary>
		/// キャラSD画像
		/// </summary>
		CharacterSD,
		/// <summary>
		/// キャラ顔アップ画像
		/// </summary>
		ChaIcon,
		/// <summary>
		/// キャラ立ち絵画像
		/// </summary>
		ChaImage,
		/// <summary>
		/// キャラ装備顔アップ画像
		/// </summary>
		ChaEquipIcon,

        // スキル関係
        SkillCommand,
		SpecialCommand,
		BasicCommand,
		AragamiCommand,
		CounterAction,
		CommandTable,
		Ability,
		AbnormalState,  // 状態異常

        Motion,
		Equipment,
        Craft,				// 神具作製.
        ComboTable,			// 敵が落とすマナなと, コンボ数に応じたテーブル

		// バトル関係
		MoveMotion,     // 移動モーション
		BasicMotion,	// 待機など基本モーション
		MotionPattern,	// モーションパターン
		VoiceSet,		// ボイスセット


		// バトルその他
		Battle,
		BattleField,	//バトルの背景画像
		BattleCutin,    //カットイン画像
		BattleStage,	//ステージ情報
		BattleEnemyGroup,	//敵グループ
		BattleAreaEffect,	//エリア特性	

		BattleYaorozu,		//ヤオロズのプレハブ
		BattleAragami,      //アラガミのプレハブ

		/// <summary> バトルで使うScriptableObjectのパラメータ. Battle/BattleParameter/ </summary>
		BattleParameter,

		// マップ
		MapUI,

		MapParameter,
		Quest,		//クエストデータ
		Mission,	// ミッション

		MapField,		// マップのzdc用のプレハブ
		ActionPoint,	// アクションポイント
		ActionPointIcon,		//アクションポイントのアイコン
		MapActionTreasureBox,	// マップアクション：宝箱
		MapActionGathering,		// マップアクション：採取
		MapActionShop,			// マップアクション：お店
		MapActionInn,			// マップアクション：宿屋
		MapActionNpc,			// マップアクション：NPC
		MapActionBattle,		// マップアクション：バトル

		MapItem,	// マップアイテム
		ShopTable,	// 販売物テーブル

		// アイテム関係
		Material,		// 素材
		WarTicket,		// 戦符
		Mallet,			// 小槌
		LotteryCard,	// 福引券

		ItemParameter,
		
		// イベント
		Event,
		EventFogPrefab,	//イベント邪気

        ExpTable,           // 経験値テーブル.
        MaterialExp,        // 素材ヤオロズ基礎経験値テーブル.
        DisposalPrice,      // ヤオロズ売却金額テーブル.
        SpiritBallPrice,    // 神珠金額.
        SpiritBallCost,     // 必要神珠数.

		RewardTable,		// 報酬テーブル

        Gacha,              // ガチャマスタ.
        BannerImage,        // バナー画像フォルダ.

		WebError,			// 通信エラーテーブル
#if DEBUG
        // Debug
        Debug,
#endif
	}

	interface IResourceInfo
	{
		string GetFolderPath();
		string GetFullPath(string subPath);
		string GetFullPathById<T>(T id);
		string GetIdString<T>(T id);
		string GetFilePrefix();
	}

	// リソースのフォルダやパス情報
	// new ResourceInfo( "Hoge", "Foo", 5) => ファイル名はResources/Hoge/{5桁ID}/Foo{5桁ID} となる
	class ResourceInfo : IResourceInfo
	{
		// フォルダ場所
	    private string FolderPath { get; }

		// idのフォーマット
	    private string IdFormat { get; }

		// id指定によるファイル名のプリフィックス
	    private string IdFileFormat { get; }

		//
	    private string FilePrefix { get; }

		// IDの長さ
		// TODO
		// Traderファイルから参照できるように用意してあるが
		// UNITY_EDITORマクロでくくった方が良い??
	    private int IdLength { get; }

		// Rootなど数値のIDを持たない場合はこっちのコンストラクタを呼ぶ
		public ResourceInfo(string folder)
			:this(folder, "", 0)
		{
		}

		// 数値のIDを持つ場合はこっちのコンストラクタを呼ぶ
		// "{folder}/{id}/prefix{id}.asset" の形式のデータ構造の場合は
		public ResourceInfo(string folder, string prefix, int idLength = 0)
		{
			// フォルダパス
			FolderPath = folder;

			// IDの長さ
			IdLength = idLength;

			// ファイルのprefix
			FilePrefix = prefix;

			// IDの文字列フォーマット
			IdFormat   = string.Format("{{0:D{0}}}", IdLength);

			// {id}/{prefix}{id}の形式
			IdFileFormat = string.Format("{0}/{1}{0}", IdFormat, FilePrefix);
		}

		// 文字列(ファイルパスを直接指定しての)フルパスの取得
		public string GetFullPath(string subPath)
		{
			return Path.Combine(FolderPath, subPath);
		}

		// IDからパスの取得
		public string GetFullPathById<T>(T id)
		{
			return GetFullPath(string.Format(IdFileFormat,id));
		}

		// ID→ファイルパス取得
		public string GetIdString<T>(T id)
		{
			return string.Format(IdFormat, id);
		}

		public string GetFilePrefix()
		{
			return FilePrefix;
		}

		public string GetFolderPath()
		{
			return FolderPath;
		}
	}

	class DirectResourceInfo : IResourceInfo
	{
		// フォルダ場所
		public string Path { get; private set; }

		public DirectResourceInfo(string path)
		{
			Path = path;
		}

		public string GetFullPath(string subPath)
		{
			return Path;
		}

		// IDからパスの取得
		public string GetFullPathById<T>(T id)
		{
			return Path;
		}

		// ID→ファイルパス取得
		public string GetIdString<T>(T id)
		{
			return string.Empty;
		}

		public string GetFilePrefix()
		{
			return "";
		}
		public string GetFolderPath()
		{
			return Path;
		}
	}


	static readonly Dictionary<DataFolder, IResourceInfo> ResourceInfos = new Dictionary<DataFolder, IResourceInfo>()
    {
		// ルートフォルダ
		{ DataFolder.Root, new ResourceInfo("") },

		{DataFolder.IconSprite, new ResourceInfo("UI/_Common/Textures/Icon") },

		// キャラのパラメータ関係
		{ DataFolder.Character  , new ResourceInfo("Character/", "Character", 4) },
        { DataFolder.Yaorozu    , new ResourceInfo("Yaorozu/"  , "Yaorozu", 5) },
        { DataFolder.Aragami    , new ResourceInfo("Aragami/") },
        { DataFolder.CharacterSD, new ResourceInfo("Character/", "Textures/ChaSd", 4) },
        { DataFolder.ChaIcon    , new ResourceInfo("Character/", "Textures/ChaIcon", 4) }, // アイコン画像.
        { DataFolder.ChaImage   , new ResourceInfo("Character/", "Textures/Cha", 4) }, // 大画像.
        { DataFolder.ChaEquipIcon, new ResourceInfo("Character/", "Textures/ChaItem", 4) }, // 装備アイコン画像.

		// キャラのスキル
		{ DataFolder.SkillCommand  , new ResourceInfo("Command/SkillCommand/") },
        { DataFolder.BasicCommand  , new ResourceInfo("Command/BasicCommand/") },
        { DataFolder.SpecialCommand, new ResourceInfo("Command/SpecialCommand/") },
        { DataFolder.AragamiCommand, new ResourceInfo("Command/AragamiCommand/") },
        { DataFolder.CounterAction , new ResourceInfo("Command/CounterAction/") },
        { DataFolder.CommandTable  , new ResourceInfo("Command/CommandTable/", "CommandTable", 4) },
		{ DataFolder.AbnormalState , new ResourceInfo("AbnormalState/") },

		// キャラのアビリティ
		{ DataFolder.Ability, new ResourceInfo("Ability/") },

		// キャラのモーション
		{ DataFolder.Motion       , new ResourceInfo("Motion/_Common/") },
        { DataFolder.MoveMotion   , new ResourceInfo("Motion/Move/") },
        { DataFolder.BasicMotion  , new ResourceInfo("Motion/Basic/") },
        { DataFolder.MotionPattern, new ResourceInfo("Motion/Pattern/", "", 4) },

		// 装備
		{ DataFolder.Equipment, new ResourceInfo("Equipment/Equipment/", "Equipment", 5) },
        { DataFolder.Craft, new ResourceInfo("Equipment/Craft/", "Craft", 5) },


		// バトル --------------------------
		// キャラのプレハブ
		{ DataFolder.BattleYaorozu,  new ResourceInfo("Character/", "Prefabs/ChaSd", 4) },
        { DataFolder.BattleAragami,  new ResourceInfo("Character/", "Prefabs/Cha"  , 4) },

        { DataFolder.ComboTable      , new ResourceInfo("Battle/ComboTable")  },
        { DataFolder.Battle          , new ResourceInfo("Battle/")  },
        { DataFolder.BattleField     , new ResourceInfo("Bg/Battle/")  },
        { DataFolder.BattleCutin     , new ResourceInfo("UI/Battle/CutIn/Parts/", "ChaCutIn", 5) },
        { DataFolder.BattleStage     , new ResourceInfo("Battle/BattleStage/", "Stage"       , 4) },
        { DataFolder.BattleEnemyGroup, new ResourceInfo("Battle/EnemyGroup/" , ""            , 5) },
		{ DataFolder.BattleAreaEffect, new ResourceInfo("Battle/AreaEffect"  , "Area"        , 5) },
		{ DataFolder.VoiceSet        , new ResourceInfo("Battle/VoiceSet/"   , "VoiceSet"    , 5) },
		{ DataFolder.BattleParameter, new ResourceInfo("Battle/BattleParameter/") },

		// マップ ---------------------------
		{DataFolder.MapUI, new ResourceInfo("UI/Map") },

		// 共通パラメータ
		{DataFolder.MapParameter, new ResourceInfo("Map/Parameter/") },
		{DataFolder.ItemParameter, new ResourceInfo("Item/Parameter/") },

		// マップの画像用のプレハブ
		{DataFolder.MapField,     new ResourceInfo("Map/Field") },

		// クエスト
		{ DataFolder.Quest, new ResourceInfo("Quest/", "Quest", 4) },
		// ミッション
		{ DataFolder.Mission, new ResourceInfo("Map/Mission/", "", 4) },

		// アクションポイント
		{ DataFolder.ActionPoint    , new ResourceInfo("Map/ActionPoint/", "", 5) },
        { DataFolder.ActionPointIcon, new ResourceInfo("UI/Map/ActionPoint/") },

        { DataFolder.MapActionTreasureBox, new ResourceInfo("Map/MapAction/TreasureBox/", "TreasureBox", 4) },
        { DataFolder.MapActionGathering  , new ResourceInfo("Map/MapAction/Gathering/"  , "Gathering", 4) },
        { DataFolder.MapActionShop       , new ResourceInfo("Map/MapAction/Shop/"       , "Shop", 4) },
        { DataFolder.MapActionInn        , new ResourceInfo("Map/MapAction/Inn/"        , "Inn", 4) },
        { DataFolder.MapActionNpc        , new ResourceInfo("Map/MapAction/Npc/"        , "Npc", 4)},
        { DataFolder.MapActionBattle     , new ResourceInfo("Map/MapAction/Battle/"     , "Battle", 4)},
        { DataFolder.MapItem             , new ResourceInfo("Map/MapItem/"              , "MapItem", 4)},
        { DataFolder.ShopTable           , new ResourceInfo("Map/ShopTable/"            , ""      , 4)},

		// アイテム
		{ DataFolder.Material       , new ResourceInfo("Item/Material/"                 , "Material"      ,4) },
        { DataFolder.WarTicket      , new ResourceInfo("Item/WarTicket/"                , "WarTicket"      ,4) },
        { DataFolder.Mallet         , new ResourceInfo("Item/Mallet/"                   , "Mallet"      ,4) },
        { DataFolder.LotteryCard    , new ResourceInfo("Item/LotteryCard/"              , "LotteryCard"      ,4) },

		// エフェクト
		{ DataFolder.Effect, new ResourceInfo("Effect/") },

		// イベント
		{ DataFolder.Event, new ResourceInfo("Event/", "Event", 5) },
        { DataFolder.EventFogPrefab, new ResourceInfo("Effect/UiEffect/") },

        // 経験値テーブル.
        {DataFolder.ExpTable, new DirectResourceInfo("ExpTable/ExpTable") },
        {DataFolder.MaterialExp, new ResourceInfo("Menu/MaterialExp/","YaorozuMaterialExp",4) }, // 素材経験値.
        {DataFolder.DisposalPrice, new ResourceInfo("Menu/DisposalPrice/","YaorozuDisposalPrice",4) },  // 売却金額.
        {DataFolder.SpiritBallPrice, new ResourceInfo("Menu/SpiritBallPrice/","SpiritBallPrice",1) }, // 神珠金額.
        {DataFolder.SpiritBallCost, new ResourceInfo("Menu/SpiritBallCost/","SpiritBallCost",4) },  // 必要神珠数.

		// 報酬テーブル
		{DataFolder.RewardTable, new ResourceInfo("RewardTable") },
        // ガチャマスタ.
        {DataFolder.Gacha, new ResourceInfo("Gacha/Core/","Gacha",4) },
        // バナー画像.
        {DataFolder.BannerImage, new ResourceInfo("UI/_Common/Banner/Textures") },
        

		// 通信エラーテーブル
		{DataFolder.WebError, new DirectResourceInfo("WebErrorInfo/WebErrorInfo") },

#if DEBUG
		// デバッグ
		{ DataFolder.Debug, new ResourceInfo("Debug") }
#endif

	};

	// Resouce.Loadに対応
	static Object LoadImpl(string path)
    {
        var obj = Resources.Load(path);
        if (obj == null)
        {
            DebugPro.LogWarning("cannot load " + path, DebugPro.Tag.Resource);
        }

        //Assert.IsNotNull(obj);
        return obj;
    }

	// Resouce.Loadに対応
	static T LoadImpl<T>(string path) where T:Object
    {
        var obj = Resources.Load<T>(path);

        if(obj == null)
        {
            DebugPro.LogWarning("cannot load " + path, DebugPro.Tag.Resource);
        }
		
        return obj;
    }

	// Resouce.LoadAllに対応
	static T[] LoadAllImpl<T>(string path) where T :Object
	{
		var obj = Resources.LoadAll<T>(path);


		if(obj == null || obj.Length == 0)
		{
			DebugPro.LogWarning("no file at " + path, DebugPro.Tag.Resource);
		}

		return obj;
	}

	// データの種類を指定してロードを行う

	// 名前を直接指定してロードする------------------------------------
	// Resouce.Loadに対応
	public static Object Load(string path, DataFolder folder = DataFolder.Root)
    {
        return LoadImpl(GetFullPath(path, folder));
    }

	// Resouce.Loadに対応
	public static T Load<T>(string path, DataFolder folder = DataFolder.Root) where T : Object
    {
        return LoadImpl<T>( GetFullPath(path, folder));
    }

	// Resouce.LoadAllに対応
	public static T[] LoadAll<T>(string path, DataFolder folder = DataFolder.Root) where T : Object
	{
		return LoadAllImpl<T>(GetFullPath(path, folder));
	}

	public static T LoadAndInstantiate<T>(string path, DataFolder folder = DataFolder.Root) where T : Object
    {
        var prefab = Load<T>(path, folder);

        return Object.Instantiate(prefab);
    }

    // idから名前を生成してロードする--------------------------------------
    public static Object Load(int id, DataFolder folder = DataFolder.Root)
    {
        return LoadImpl( GetFullPathById(id, folder));
    }

    public static T Load<T>(int id, DataFolder folder = DataFolder.Root) where T : Object
    {
        return LoadImpl<T>(GetFullPathById(id, folder));
    }

    public static T LoadAndInstantiate<T>(int id, DataFolder folder = DataFolder.Root) where T : Object
    {
        var prefab = Load<T>(id, folder);
        return GameObject.Instantiate<T>(prefab);
    }

	public static Object LoadById(string id, DataFolder folder)
	{
		return LoadImpl(GetFullPathById(id, folder));
	}

	public static T LoadById<T>(string id, DataFolder folder) where T : Object
	{
		return LoadImpl<T>(GetFullPathById(id, folder));
	}

	public static T LoadAndInstantiateById<T>(string id, DataFolder folder = DataFolder.Root) where T : Object
	{
		var prefab = LoadById<T>(id, folder);
		return Object.Instantiate(prefab);
	}


	// 桁数を考慮したID→文字列変換
	public static string GetIdString<T>(T id, DataFolder folder)
	{
		return ResourceInfos[folder].GetIdString(id);
	}

	// 
	public static string GetFullPath(string path, DataFolder folder = DataFolder.Root)
	{
		return ResourceInfos[folder].GetFullPath(path);
	}

	public static string GetFolderPath(DataFolder folder)
	{
		return ResourceInfos[folder].GetFolderPath();
	}

	public static string GetFilePrefix(DataFolder folder)
	{
		return ResourceInfos[folder].GetFilePrefix();
	}

	// 
	public static string GetFullPathById<T>(T id, DataFolder folder)
	{
		return ResourceInfos[folder].GetFullPathById(id);
	}
}
