﻿using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class VoteSeat : MTurnSeat
	{
		[Header("_" + nameof(VoteSeat))]
		private int _;

		public int VoteIndex => TurnData;

		protected void TryVote(int newTurnData)
		{
			if (turnBaseManager.CurGameState != (int)VoteState.VoteTime)
				return;

			SetTurnData(newTurnData);
		}

		#region HorribleEvents
		[ContextMenu(nameof(Vote0))]
		public void Vote0() => TryVote(0);
		public void Vote1() => TryVote(1);
		public void Vote2() => TryVote(2);
		public void Vote3() => TryVote(3);
		public void Vote4() => TryVote(4);
		public void Vote5() => TryVote(5);
		public void Vote6() => TryVote(6);
		public void Vote7() => TryVote(7);
		public void Vote8() => TryVote(8);
		public void Vote9() => TryVote(9);
		#endregion
	}
}