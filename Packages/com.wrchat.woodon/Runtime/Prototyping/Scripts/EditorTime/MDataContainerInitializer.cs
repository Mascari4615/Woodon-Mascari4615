<<<<<<< HEAD
=======
using System;
>>>>>>> upstream/main
using UnityEditor;
using UnityEngine;
using WRC.Woodon;

namespace WRC
{
#if UNITY_EDITOR
	public class MDataContainerInitializer : MonoBehaviour
	{
<<<<<<< HEAD
		[SerializeField] private string prefix;
		[SerializeField] private string[] someStrings;
=======
		[SerializeField] private string stringPrefix;
		[SerializeField] private string[] strings;
		[SerializeField] private Sprite[] sprites;
>>>>>>> upstream/main

		[ContextMenu(nameof(InitName))]
		public void InitName()
		{
<<<<<<< HEAD
			MDataContainer[] mDatas = GetComponentsInChildren<MDataContainer>(true);

			for (int i = 0; i < transform.childCount; i++)
			{
				MDataContainer mData = mDatas[i];
				mData.Name = $"{prefix}{someStrings[0]}{i}";

				EditorUtility.SetDirty(mData);
			}

			AssetDatabase.SaveAssets();
=======
			ForEachMDataContainer((mDataContainer, index) =>
			{
				mDataContainer.Name = $"{stringPrefix}{strings[0]}{index}";
			});
>>>>>>> upstream/main
		}

		[ContextMenu(nameof(InitValue))]
		public void InitValue()
		{
<<<<<<< HEAD
			MDataContainer[] mDatas = GetComponentsInChildren<MDataContainer>(true);

			for (int i = 0; i < transform.childCount; i++)
			{
				MDataContainer mData = mDatas[i];
				mData.Value = $"{prefix}{someStrings[0]}{i}";

				EditorUtility.SetDirty(mData);
			}

			AssetDatabase.SaveAssets();
=======
			ForEachMDataContainer((mDataContainer, index) =>
			{
				mDataContainer.Value = $"{stringPrefix}{strings[0]}{index}";
			});
>>>>>>> upstream/main
		}

		[ContextMenu(nameof(InitStringData))]
		public void InitStringData()
		{
<<<<<<< HEAD
			MDataContainer[] mDatas = GetComponentsInChildren<MDataContainer>(true);

			for (int i = 0; i < transform.childCount; i++)
			{
				MDataContainer mData = mDatas[i];
				mData.StringData = new string[] { $"{prefix}{someStrings[0]}{i}" };

				EditorUtility.SetDirty(mData);
			}

			AssetDatabase.SaveAssets();
		}

		[SerializeField] private Sprite[] sprite;
		[ContextMenu(nameof(InitSprites))]
		public void InitSprites()
		{
			MDataContainer[] mDatas = GetComponentsInChildren<MDataContainer>(true);

			for (int i = 0; i < transform.childCount; i++)
			{
				MDataContainer mData = mDatas[i];
				mData.Sprite = sprite[i];

				EditorUtility.SetDirty(mData);
=======
			ForEachMDataContainer((mDataContainer, index) =>
			{
				mDataContainer.StringData = new string[] { $"{stringPrefix}{strings[0]}{index}" };
			});
		}

		[ContextMenu(nameof(InitMainSprite))]
		public void InitMainSprite()
		{
			if (sprites == null || sprites.Length == 0)
				return;

			ForEachMDataContainer((mDataContainer, index) =>
			{
				if (sprites.Length <= index)
					return;

				mDataContainer.Sprite = sprites[index];
			});
		}

		[ContextMenu(nameof(InitSprites_A))]
		public void InitSprites_A()
		{
			if (sprites == null || sprites.Length == 0)
				return;

			ForEachMDataContainer((mDataContainer, index) =>
			{
				if (sprites.Length <= index)
					return;

				mDataContainer.Sprites = sprites;
			});
		}

		[ContextMenu(nameof(InitSprites_B))]
		public void InitSprites_B()
		{
			if (sprites == null || sprites.Length == 0)
				return;

			ForEachMDataContainer((mDataContainer, index) =>
			{
				if (sprites.Length <= index)
					return;

				mDataContainer.Sprites = new Sprite[] { sprites[index] };
			});
		}

		private void ForEachMDataContainer(Action<MDataContainer, int> action)
		{
			MDataContainer[] mDataContainer = GetComponentsInChildren<MDataContainer>(true);

			for (int i = 0; i < mDataContainer.Length; i++)
			{
				action(mDataContainer[i], i);
				EditorUtility.SetDirty(mDataContainer[i]);
>>>>>>> upstream/main
			}

			AssetDatabase.SaveAssets();
		}
	}
#endif
}