<<<<<<< HEAD
﻿
using System;
using UdonSharp;
using UnityEngine;
=======
﻿using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
>>>>>>> upstream/main
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class ContentManager : WEventPublisher
	{
		public MSeat[] MSeats { get; private set; }

		[field: Header("_" + nameof(ContentManager))]
		[SerializeField] private int stateMax = 1;

<<<<<<< HEAD
		[UdonSynced, FieldChangeCallback(nameof(CurGameState))] private int _curGameState = 0;
		public int CurGameState
		{
			get => _curGameState;
			private set
			{
				// MDebugLog($"{nameof(CurGameState)} Changed, {CurGameState} to {value}");

				int origin = _curGameState;
				_curGameState = value;
				OnGameStateChange(DataChangeStateUtil.GetChangeState(origin, value));
			}
=======
		[SerializeField] protected WJson contentData;

		public int CurGameState
		{
			get => contentData.GetData("CurGameState", 0);
			private set => contentData.SetData("CurGameState", value);
>>>>>>> upstream/main
		}

		protected virtual void OnGameStateChange(DataChangeState changeState)
		{
<<<<<<< HEAD
			// MDebugLog($"{nameof(OnGameStateChange)}, {changeState}");

			UpdateContent();
=======
			MDebugLog($"{nameof(OnGameStateChange)}, {changeState}");

			// UpdateContent();
>>>>>>> upstream/main
			SendEvents();
		}

		public void SetGameState(int newGameState)
		{
<<<<<<< HEAD
			SetOwner();
			CurGameState = (newGameState + stateMax) % stateMax;
			RequestSerialization();
=======
			CurGameState = (newGameState + stateMax) % stateMax;
			contentData.SerializeData();
>>>>>>> upstream/main
		}
		public void SetGameState(Enum newGameState) => SetGameState(Convert.ToInt32(newGameState));

		public bool IsCurGameState(int gameState) => CurGameState == gameState;
		public bool IsCurGameState(Enum gameState) => CurGameState == Convert.ToInt32(gameState);

		public void NextGameState() => SetGameState(CurGameState + 1);
		public void PrevGameState() => SetGameState(CurGameState - 1);

		public bool IsInited { get; protected set; } = false;

		protected virtual void Start()
		{
			Init();
			UpdateContent();
		}

		protected virtual void Init()
		{
			// MDebugLog($"{nameof(Init)}");
<<<<<<< HEAD

			if (IsInited)
				return;
			IsInited = true;

			MSeats = GetComponentsInChildren<MSeat>();

			for (int i = 0; i < MSeats.Length; i++)
				MSeats[i].Init(this, i);
=======
			if (IsInited)
				return;
			
			{
				MSeats = GetComponentsInChildren<MSeat>();
				contentData.RegisterListener(this, nameof(OnContentDataChanged), WJsonEvent.OnDeserialization);
			
				for (int i = 0; i < MSeats.Length; i++)
					MSeats[i].Init(this, i);

				if (Networking.IsMaster)
				{
					CurGameState = 0;
					contentData.SerializeData();
				}
			}
			
			IsInited = true;
		}

		public virtual void OnContentDataChanged()
		{
			if (contentData.HasDataChanged("CurGameState", out int origin, out int cur))
				OnGameStateChange(DataChangeStateUtil.GetChangeState(origin, cur));

			UpdateContent();
>>>>>>> upstream/main
		}

		public virtual void UpdateContent()
		{
			// MDebugLog($"{nameof(UpdateStuff)}");

			if (IsInited == false)
				Init();

			foreach (MSeat seat in MSeats)
				seat.UpdateSeat();
		}

		public virtual void OnSeatTargetChanged(MSeat changedSeat)
		{
			// 중복 제거 (한 플레이어가 한 번에 하나의 자리에만 등록 가능하도록)
			{
				foreach (MSeat seat in MSeats)
				{
					if (seat == changedSeat)
						continue;

					if (seat.IsTargetPlayer() && seat.TargetPlayerID == changedSeat.TargetPlayerID)
						seat.SetTargetNone();
				}
			}
		}
		
		public MSeat GetLocalPlayerSeat()
		{
			foreach (MSeat seat in MSeats)
				if (seat.IsTargetPlayer())
					return seat;
			return null;
		}
		
		// ===================================

		[field: SerializeField] public int DefaultData { get; private set; } = 0;
		[field: SerializeField] public string[] DataToString { get; protected set; }
		[field: SerializeField] public bool ResetDataWhenOwnerChange { get; private set; }
		[field: SerializeField] public bool UseDataSprites { get; private set; }
		[field: SerializeField] public bool IsDataElement { get; private set; }
		[field: SerializeField] public Sprite[] DataSprites { get; protected set; }
		[field: SerializeField] public Sprite DataNoneSprite { get; protected set; }

		[field: Header("_" + nameof(ContentManager) + "_TurnData")]
		[field: SerializeField] public int DefaultTurnData { get; private set; } = 0;
		[field: SerializeField] public string[] TurnDataToString { get; protected set; }
		[field: SerializeField] public bool ResetTurnDataWhenOwnerChange { get; private set; }
		[field: SerializeField] public bool UseTurnDataSprites { get; private set; }
		[field: SerializeField] public bool IsTurnDataElement { get; private set; }
		[field: SerializeField] public Sprite[] TurnDataSprites { get; protected set; }
		[field: SerializeField] public Sprite TurnDataNoneSprite { get; protected set; }

		public int GetMaxTurnData()
		{
			int maxTurnData = 0;

			foreach (MSeat mTurnSeat in MSeats)
				maxTurnData = Mathf.Max(maxTurnData, mTurnSeat.TurnData);

			return maxTurnData;
		}

		public MSeat[] GetMaxTurnDataSeats()
		{
			int maxTurnData = GetMaxTurnData();
			int maxTurnDataCount = 0;
			MSeat[] maxTurnDataSeats = new MSeat[MSeats.Length];

			foreach (MSeat mTurnSeat in MSeats)
			{
				if (mTurnSeat.TurnData == maxTurnData)
				{
					maxTurnDataSeats[maxTurnDataCount] = mTurnSeat;
					maxTurnDataCount++;
				}
			}

<<<<<<< HEAD
			MDataUtil.ResizeArr(ref maxTurnDataSeats, maxTurnDataCount);
=======
			WUtil.Resize(ref maxTurnDataSeats, maxTurnDataCount);
>>>>>>> upstream/main

			return maxTurnDataSeats;
		}
	}
}