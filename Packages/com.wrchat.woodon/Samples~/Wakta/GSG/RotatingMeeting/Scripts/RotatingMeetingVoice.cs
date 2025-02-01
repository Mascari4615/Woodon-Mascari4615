using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615.Project.ISD.GSG.RotatingMeeting
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class RotatingMeetingVoice : VoiceUpdater
	{
		[Header("_" + nameof(RotatingMeetingVoice))]
		[SerializeField] private CupPicker[] cupPickers;
		[SerializeField] private Image[] icons;

		private int curFocusState = NONE_INT;

		public void SetFocusState0() => SetFocusState(0);
		public void SetFocusState1() => SetFocusState(1);
		public void SetFocusState2() => SetFocusState(2);

		private void Start()
		{
			SetFocusState(NONE_INT);
		}

		private void SetFocusState(int newState)
		{
			if (curFocusState == newState)
				curFocusState = NONE_INT;
			else
				curFocusState = newState;

			for (int i = 0; i < 3; i++)
			{
				bool isFocus = i == curFocusState;
				Color color = MColorUtil.GetColorByBool(isFocus, MColorPreset.Green, MColorPreset.Gray);

				icons[i * 2 + 0].color = color;
				icons[i * 2 + 1].color = color;
			}
		}

<<<<<<< HEAD
		public override void UpdateVoice()
		{
			for (int index = 0; index < voiceManager.VoiceStates.Length; index++)
				voiceManager.VoiceStates[index] = VoiceState.Default;
=======
		public override void UpdateVoice(VRCPlayerApi[] playerApis, VoiceState[] voiceStates)
		{
			for (int index = 0; index < voiceStates.Length; index++)
				voiceStates[index] = VoiceState.Default;
>>>>>>> upstream/main

			for (int cupPickerIndex = 0; cupPickerIndex < cupPickers.Length; cupPickerIndex++)
			{
				// 여성 ID
				int pickerOwnerId = cupPickers[cupPickerIndex].OwnerID;

				if (cupPickers[cupPickerIndex].TargetCupIndex == NONE_INT)
					continue;

				// 남성 ID
				int cupOwnerId = cupPickers[cupPickerIndex].TargetCup.OwnerID;

				bool isLocalPlayerTarget = (pickerOwnerId == Networking.LocalPlayer.playerId) ||
										   (cupOwnerId == Networking.LocalPlayer.playerId) ||
										   (curFocusState == cupPickerIndex);

				if (!isLocalPlayerTarget)
					continue;

<<<<<<< HEAD
				for (int vi = 0; vi < voiceManager.VoiceStates.Length; vi++)
				{
					VRCPlayerApi targetPlayer = voiceManager.PlayerApis[vi];
=======
				for (int vi = 0; vi < voiceStates.Length; vi++)
				{
					VRCPlayerApi targetPlayer = playerApis[vi];
>>>>>>> upstream/main

					bool targetPlayerState = (pickerOwnerId == targetPlayer.playerId) ||
											 (cupOwnerId == targetPlayer.playerId);

<<<<<<< HEAD
					voiceManager.VoiceStates[vi] = (targetPlayerState ? VoiceState.Default : VoiceState.Quiet);
=======
					voiceStates[vi] = (targetPlayerState ? VoiceState.Default : VoiceState.Quiet);
>>>>>>> upstream/main
				}

				break;
			}
		}
	}
}