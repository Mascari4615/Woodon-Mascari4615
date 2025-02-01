using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
<<<<<<< HEAD
using static WRC.Woodon.MUtil;
=======
using static WRC.Woodon.WUtil;
>>>>>>> upstream/main

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceReseter : VoiceUpdater
	{
		[Header("_" + nameof(VoiceReseter))]
		[SerializeField] private MTarget[] ignoreTargets;

		[SerializeField] private bool useIngnoreTargetTag;
<<<<<<< HEAD
		[SerializeField] private VoiceAreaTag IgnoreTargetTag;

		[SerializeField] private bool useTargetTag;
		[SerializeField] private VoiceAreaTag targetTag;

		public override void UpdateVoice()
=======
		[SerializeField] private VoiceTag IgnoreTargetTag;

		[SerializeField] private bool useTargetTag;
		[SerializeField] private VoiceTag targetTag;

		public override void UpdateVoice(VRCPlayerApi[] playerApis, VoiceState[] voiceStates)
>>>>>>> upstream/main
		{
			if (IsNotOnline())
				return;

<<<<<<< HEAD
			if (voiceManager.PlayerApis == null)
				return;

			if (enable != null && (enable.Value == false))
=======
			if (playerApis == null)
				return;

			if (Enable == false)
>>>>>>> upstream/main
				return;

			// 무시 MTarget 대상이라면 return
			foreach (MTarget ignoreTarget in ignoreTargets)
			{
				if (ignoreTarget.IsTargetPlayer(Networking.LocalPlayer))
					return;
			}

			// 무시 태그를 가지고 있으면 return
			if (useIngnoreTargetTag)
			{
				string tag =
					Networking.LocalPlayer.GetPlayerTag($"{Networking.LocalPlayer.playerId}{IgnoreTargetTag}");

				if (tag == TRUE_STRING)
					return;
			}

			// 타겟 태그를 가지고 있지 않으면 return
			if (useTargetTag)
			{
				string tag =
					Networking.LocalPlayer.GetPlayerTag($"{Networking.LocalPlayer.playerId}{targetTag}");

				if (tag != TRUE_STRING)
					return;
			}

			// 보이스 상태 초기화
<<<<<<< HEAD
			for (int i = 0; i < voiceManager.PlayerApis.Length; i++)
				voiceManager.VoiceStates[i] = VoiceState.Default;
=======
			for (int i = 0; i < playerApis.Length; i++)
				voiceStates[i] = VoiceState.Default;
>>>>>>> upstream/main
		}
	}
}