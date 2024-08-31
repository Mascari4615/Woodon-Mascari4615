using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MData : MBase
	{
		[field: Header("_" + nameof(MData))]
		[field: SerializeField] public string Name { get; protected set; }
		[field: SerializeField, TextArea(3, 10)] public string Value { get; set; } = NONE_STRING;
		[field: SerializeField] public Sprite Sprite { get; protected set; }
		[field: SerializeField, TextArea(3, 10)] public string[] StringData { get; protected set; }
		[field: SerializeField] public Sprite[] Sprites { get; protected set; }

		public int Index { get; set; } = NONE_INT;
		public string SyncData { get; set; } = string.Empty;

		public virtual string Save()
		{
			string data = string.Empty;

			data += $"{SyncData}{DATA_SEPARATOR}";

			return data;
		}

		public virtual void Load(string data)
		{
			string[] datas = data.Split(DATA_SEPARATOR);

			SyncData = datas[0];
		}
	}
}