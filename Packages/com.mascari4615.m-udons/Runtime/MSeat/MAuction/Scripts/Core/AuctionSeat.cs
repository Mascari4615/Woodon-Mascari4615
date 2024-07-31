﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static Mascari4615.MUtil;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class AuctionSeat : MTurnSeat
	{
		[field: Header("_" + nameof(AuctionSeat))]
		[field: UdonSynced()] public int TryTime { get; private set; } = NONE_INT;
		public int RemainPoint => Data;
		public int TryPoint => TurnData;
		
		[SerializeField] private MValue tryPointMScore;
		[SerializeField] private TimeEvent timeEvent;
		[SerializeField] private MSFXManager mSFXManager;

		public void UpdateTryPoint()
		{
			if (seatManager.CurGameState != (int)AuctionState.AuctionTime)
				return;

			AuctionManager auctionManager = (AuctionManager)seatManager;

			if (auctionManager.GetMaxTurnData() >= tryPointMScore.Value)
				return;

			SetTryTime(Networking.GetServerTimeInMilliseconds());
			SetTurnData(tryPointMScore.Value);
		}

		public void SetTryTime(int newTryTime)
		{
			SetOwner();
			TryTime = newTryTime;
			RequestSerialization();
		}

		protected override void OnDataChange()
		{
			base.OnDataChange();

			tryPointMScore.SetMinMaxValue(0, Data, IsOwner());
		}

		protected override void OnOwnerChange()
		{
			base.OnOwnerChange();

			if (IsLocalPlayerID(OwnerID))
				tryPointMScore.SetValue(0);
		}

		protected override void OnTurnDataChange(DataChangeState changeState)
		{
			base.OnTurnDataChange(changeState);

			if (seatManager.CurGameState != (int)AuctionState.AuctionTime)
				return;

			if (changeState != DataChangeState.Greater)
				return;

			if (!IsOwner(seatManager.gameObject))
				return;

			if (timeEvent != null)
			{
				timeEvent.SetTimer();
				mSFXManager.PlaySFX_G(1);
			}
		}
	}
}