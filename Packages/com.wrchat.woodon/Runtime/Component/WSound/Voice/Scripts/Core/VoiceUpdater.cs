using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
<<<<<<< HEAD
	// 베이스가 되는 추상 클래스
	// 이 클래스를 상속받아서 UpdateVoice()를 구현해야 함
	public abstract class VoiceUpdater : MBase
	{
		protected VoiceManager voiceManager;

		[SerializeField] protected MBool enable;
		[SerializeField] protected bool usePrevData;

		public virtual void Init(VoiceManager voiceManager)
		{
			this.voiceManager = voiceManager;
		}

		public abstract void UpdateVoice();

		public void SetEnable(bool enable)
		{
			if (this.enable == null)
				return;

			this.enable.SetValue(enable);
		}
=======
	// VoiceManager에서 각 VoiceUpdater.UpdateVoice()를 호출
	// 이 클래스를 상속받아 UpdateVoice()를 구현해야 함
	public abstract class VoiceUpdater : MBase
	{
		[Header("_" + nameof(VoiceUpdater))]
		[SerializeField] private MBool enable;
		[SerializeField] protected bool usePrevData;
	
		public virtual void Init(VoiceManager voiceManager) {}
	
		public bool Enable => (enable == null) || enable.Value;
		public void SetEnable(bool enable)
		{
			if (this.enable != null)
				this.enable.SetValue(enable);
		}

		public abstract void UpdateVoice(VRCPlayerApi[] playerApis, VoiceState[] voiceStates);
>>>>>>> upstream/main
	}
}