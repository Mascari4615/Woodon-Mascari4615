using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MDataContainer : WEventPublisher
	{
		[field: Header("_" + nameof(MDataContainer))]
		[field: SerializeField] public string Name { get; set; }
		[field: SerializeField, TextArea(3, 10)] public string Value { get; set; } = NONE_STRING;
		[field: SerializeField] public Sprite Sprite { get; set; }
		[field: SerializeField, TextArea(3, 10)] public string[] StringData { get; set; }
		[field: SerializeField] public Sprite[] Sprites { get; set; }

<<<<<<< HEAD
		[SerializeField] protected MData mData;
=======
		[SerializeField] protected WJson wJson;
>>>>>>> upstream/main

		public int RuntimeInt { get; set; } = NONE_INT;
		public bool RuntimeBool { get; set; } = false;
		public string RuntimeString { get; set; } = NONE_STRING;

		public int Index { get; set; } = NONE_INT;

		private void Start()
		{
			Init();
		}

		public virtual void Init()
		{
<<<<<<< HEAD
			if (mData == null)
				return;

			mData.RegisterListener(this, nameof(ParseData), MDataEvent.OnDeserialization);
=======
			if (wJson == null)
				return;

			wJson.RegisterListener(this, nameof(ParseData), WJsonEvent.OnDeserialization);
>>>>>>> upstream/main
		}

		public virtual void SerializeData()
		{
<<<<<<< HEAD
			if (mData == null)
				return;

			mData.SetData("RuntimeInt", RuntimeInt);
			mData.SetData("RuntimeBool", RuntimeBool);
			mData.SetData("RuntimeString", RuntimeString);

			mData.SerializeData();
=======
			if (wJson == null)
				return;

			wJson.SetData("RuntimeInt", RuntimeInt);
			wJson.SetData("RuntimeBool", RuntimeBool);
			wJson.SetData("RuntimeString", RuntimeString);

			wJson.SerializeData();
>>>>>>> upstream/main
		}

		public virtual void ParseData()
		{
<<<<<<< HEAD
			if (mData == null)
				return;

			RuntimeInt = (int)mData.DataDictionary["RuntimeInt"].Double;
			RuntimeBool = mData.DataDictionary["RuntimeBool"].Boolean;
			RuntimeString = mData.DataDictionary["RuntimeString"].String;
=======
			if (wJson == null)
				return;

			RuntimeInt = (int)wJson.GetData("RuntimeInt").Double;
			RuntimeBool = wJson.GetData("RuntimeBool").Boolean;
			RuntimeString = wJson.GetData("RuntimeString").String;
>>>>>>> upstream/main

			SendEvents();
		}

		public void Clear()
		{
			Name = string.Empty;
			Value = string.Empty;
			Sprite = null;
			StringData = null;
			Sprites = null;
		}
	}
}