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
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MString : WEventPublisher
	{
<<<<<<< HEAD
		[Header("_" + nameof(MString))]

		[Header("_" + nameof(MString) + " - Options")]
		[SerializeField, TextArea(3, 10)] private string defaultString = string.Empty;
		public string DefaultString => defaultString;
=======
		[field: Header("_" + nameof(MString))]
		[field: SerializeField, TextArea(3, 10)] public string DefaultString { get; protected set; } = string.Empty;

		[field: Header("_" + nameof(MString) + " - Options")]
>>>>>>> upstream/main
		[SerializeField] private bool useDefaultWhenEmpty = true;
		[SerializeField] private bool useSync;
		[SerializeField] private bool onlyDigit;
		[SerializeField] private int lengthLimit = 2147483647;
<<<<<<< HEAD
=======

>>>>>>> upstream/main
		[UdonSynced, FieldChangeCallback(nameof(SyncedValue))] private string _syncedValue = string.Empty;
		public string SyncedValue
		{
			get => _syncedValue;
			private set
			{
				_syncedValue = value;

				if (useSync)
					SetValue(_syncedValue, isReceiver: true);
			}
		}

		private string _value = string.Empty;
		public string Value
		{
			get => _value;
			private set
			{
				string origin = _value;
				_value = value;
				OnValueChange(origin, _value);
			}
		}

		protected virtual void OnValueChange(string origin, string cur)
		{
			MDebugLog($"{nameof(OnValueChange)} : {origin} -> {cur}");
			SendEvents();
		}

		private void Start()
		{
			Init();
		}
<<<<<<< HEAD
		
		protected virtual void Init()
		{
			MDebugLog($"{nameof(Init)}");
			
			if (useSync)
			{
				if (Networking.IsMaster)
					SetValue(defaultString);
			}
			else
			{
				SetValue(defaultString);
=======

		protected virtual void Init()
		{
			MDebugLog($"{nameof(Init)}");

			if (useSync)
			{
				if (Networking.IsMaster)
					SetValue(DefaultString);
			}
			else
			{
				SetValue(DefaultString);
>>>>>>> upstream/main
			}

			OnValueChange(string.Empty, Value);
		}

		public void SetValue(string newValue, bool isReceiver = false)
		{
			if (isReceiver == false)
			{
				if (useSync && SyncedValue != newValue)
				{
					SetOwner();
					SyncedValue = newValue;
					RequestSerialization();

					return;
				}
			}

			Value = newValue;
		}

		public void ResetValue()
		{
<<<<<<< HEAD
			SetValue(defaultString);
		}
		
=======
			SetValue(DefaultString);
		}

>>>>>>> upstream/main
		public string GetFormatString()
		{
			string formatString = Value;

			if ((formatString == string.Empty) || (formatString.Length == 0))
				if (useDefaultWhenEmpty)
<<<<<<< HEAD
					formatString = defaultString;
=======
					formatString = DefaultString;
>>>>>>> upstream/main

			return formatString;
		}

		public bool IsValidText(string targetText)
		{
			if (onlyDigit && (IsDigit(targetText) == false))
				return false;

			if (targetText.Length > lengthLimit)
				return false;

			return true;
		}
	}
}