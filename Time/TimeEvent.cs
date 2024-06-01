﻿using Newtonsoft.Json.Linq;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class TimeEvent : MEventSender
	{
		[Header("_" + nameof(TimeEvent))]
		[SerializeField] private TextMeshProUGUI[] timeTexts;
		[SerializeField] private TimeEventBarUI[] timeEventBarUIs;
		[SerializeField] private Image[] buttonUIImages;
		[SerializeField] private GameObject ownerObject;
		[field: SerializeField] public int TimeByDecisecond { get; set; } = 50;
		[SerializeField] private CustomBool isCounting;

		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(ExpireTime))]
		private int _expireTime = NONE_INT;

		public int ExpireTime
		{
			get => _expireTime;
			set
			{
				_expireTime = value;
				OnExpireTimeChange();
			}
		}

		public bool IsExpired => (ExpireTime == NONE_INT);

		private void Start()
		{
			foreach (TimeEventBarUI timeEventBarUI in timeEventBarUIs)
				timeEventBarUI.Init(this);
		}

		private void Update()
		{
			foreach (var timeText in timeTexts)
				timeText.text = ExpireTime == NONE_INT
					? "0"
					: Networking.GetServerTimeInMilliseconds() >= ExpireTime
						? "0"
						: ((ExpireTime - Networking.GetServerTimeInMilliseconds()) / 1000).ToString();

			foreach (TimeEventBarUI timeEventBarUI in timeEventBarUIs)
				timeEventBarUI.UpdateUI();

			if (!IsOwner())
				return;

			if (ExpireTime == NONE_INT)
				return;

			if (Networking.GetServerTimeInMilliseconds() >= ExpireTime)
			{
				MDebugLog("Expired!");
				SendEvents();
				ResetTime();
			}
		}

		private void OnExpireTimeChange()
		{
			MDebugLog($"{nameof(OnExpireTimeChange)} : ChangeTo = {ExpireTime}");
			if (isCounting)
				isCounting.SetValue(ExpireTime != NONE_INT);

			foreach (Image buttonUIImage in buttonUIImages)
				buttonUIImage.color = MColorUtil.GetGreenOrRed(isCounting);
		}

		public void ResetTimeIfOwner()
		{
			MDebugLog(nameof(ResetTimeIfOwner));

			if (ownerObject != null)
				if (!IsOwner(ownerObject))
					return;

			ResetTime();
		}

		public void ResetTime()
		{
			MDebugLog(nameof(ResetTime));

			SetOwner();
			ExpireTime = NONE_INT;
			RequestSerialization();
		}

		public void SetTimeIfOwner()
		{
			MDebugLog(nameof(SetTimeIfOwner));

			if (ownerObject != null)
				if (!IsOwner(ownerObject))
					return;

			SetTime();
		}

		public void SetTime()
		{
			MDebugLog(nameof(SetTime));

			SetOwner();
			ExpireTime = Networking.GetServerTimeInMilliseconds() + (TimeByDecisecond * 100);
			RequestSerialization();
		}

		public void AddTime()
		{
			MDebugLog(nameof(AddTime));

			SetOwner();
			ExpireTime += TimeByDecisecond * 100;
			RequestSerialization();
		}

		public void ToggleTime()
		{
			MDebugLog(nameof(ToggleTime));

			SetOwner();
			ExpireTime = ExpireTime == NONE_INT ? Networking.GetServerTimeInMilliseconds() + (TimeByDecisecond * 100) : NONE_INT;
			RequestSerialization();
		}
	}
}