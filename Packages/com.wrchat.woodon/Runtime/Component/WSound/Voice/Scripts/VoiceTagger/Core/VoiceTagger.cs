using UnityEngine;
using VRC.SDKBase;
<<<<<<< HEAD
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
{
	public class VoiceTagger : MBase
	{
		[Header("_" + nameof(VoiceTagger))]
		[SerializeField] protected VoiceManager voiceManager;
		[field: SerializeField] public VoiceAreaTag Tag { get; private set; }
		[SerializeField] private float updateTerm = .5f;

		[SerializeField] private MBool localPlayerIn;
		private bool isLocalPlayerIn;
		[SerializeField] private MBool someoneIn;
		private bool isSomeoneIn;
=======
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	public abstract class VoiceTagger : MBase
	{
		[field: Header("_" + nameof(VoiceTagger))]
		[field: SerializeField] public VoiceTag Tag { get; private set; }
		[SerializeField] private float updateTerm = .5f;

		[SerializeField] private MBool localPlayerIn;
		[SerializeField] private MBool someoneIn;
>>>>>>> upstream/main

		protected virtual void Start() => UpdateVoiceLoop();
		public void UpdateVoiceLoop()
		{
			SendCustomEventDelayedSeconds(nameof(UpdateVoiceLoop), updateTerm);
			UpdateAllTag();
		}

		protected virtual void UpdateAllTag()
		{
			if (IsNotOnline())
				return;

<<<<<<< HEAD
			isLocalPlayerIn = false;
			isSomeoneIn = false;

			if (voiceManager.PlayerApis != null &&
				voiceManager.PlayerApis.Length == VRCPlayerApi.GetPlayerCount())
			{
				for (int i = 0; i < voiceManager.PlayerApis.Length; i++)
				{
					bool isIn = IsPlayerIn(voiceManager.PlayerApis[i]);

					UpdatePlayerTag(voiceManager.PlayerApis[i], isIn);

					isSomeoneIn = isSomeoneIn || isIn;
					if (voiceManager.PlayerApis[i].isLocal)
						isLocalPlayerIn = isIn;
				}
			}
			
=======
			bool isLocalPlayerIn = false;
			bool isSomeoneIn = false;

			VRCPlayerApi[] playerApis = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
			VRCPlayerApi.GetPlayers(playerApis);

			for (int i = 0; i < playerApis.Length; i++)
			{
				bool isCondition = IsCondition(playerApis[i]);
				VoiceUtil.SetVoiceTag(playerApis[i], Tag, isCondition);

				isSomeoneIn = isSomeoneIn || isCondition;
				if (playerApis[i].isLocal)
					isLocalPlayerIn = isCondition;
			}

>>>>>>> upstream/main
			if (localPlayerIn)
				localPlayerIn.SetValue(isLocalPlayerIn);
			if (someoneIn)
				someoneIn.SetValue(isSomeoneIn);
		}

<<<<<<< HEAD
		public virtual bool IsPlayerIn(VRCPlayerApi player) { return true; }

		private bool UpdatePlayerTag(VRCPlayerApi player, bool isIn)
		{
			// MDebugLog($"{playerID}{Tag}" + (isin ? TRUE_STRING : FALSE_STRING));
			Networking.LocalPlayer.SetPlayerTag($"{player.playerId}{Tag}", isIn ? TRUE_STRING : FALSE_STRING);
			return isIn;
		}
=======
		public abstract bool IsCondition(VRCPlayerApi player);
>>>>>>> upstream/main
	}
}