﻿using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class DrawManager : MEventSender
	{
		[SerializeField] private int teamCount = 2; // 팀 수
		[SerializeField] private int teamPlayerCount = 2; // 팀당 인원 수

		[SerializeField] private UIDrawTeamList[] drawUIs;
		[SerializeField] private UIDrawController[] drawControllers;

		[SerializeField] private DrawType drawType = DrawType.AllRandom;

		public DrawElementData[] DrawElementDatas { get; private set; }
		public bool IsInited { get; private set; } = false;

		[UdonSynced, FieldChangeCallback(nameof(DataPacks))] private string dataPacks = string.Empty;
		public string DataPacks
		{
			get => dataPacks;
			set
			{
				dataPacks = value;
				OnDataChanged();
			}
		}

		private void OnDataChanged()
		{
			string[] dataPacks = DataPacks.Split(DATA_PACK_SEPARATOR);
		
			string debugString = string.Empty;
			for (int i = 0; i < DrawElementDatas.Length; i++)
				debugString += $"{i} :: {dataPacks[i]}\n";

			MDebugLog($"{nameof(OnDataChanged)}, DataPack : {debugString}");

			if (string.IsNullOrEmpty(DataPacks))
				return;

			for (int i = 0; i < DrawElementDatas.Length; i++)
				DrawElementDatas[i].Load(dataPacks[i]);

			foreach (UIDrawTeamList drawUI in drawUIs)
				drawUI.UpdateUI(DrawElementDatas);

			SendEvents();
		}

		public void SyncData()
		{
			SetOwner();

			string data = string.Empty;
			foreach (DrawElementData drawElementData in DrawElementDatas)
				data += drawElementData.Save() + DATA_PACK_SEPARATOR;
			DataPacks = data;

			RequestSerialization();
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			MDebugLog(nameof(Init));

			if (IsInited)
				return;
			IsInited = true;

			DrawElementDatas = GetComponentsInChildren<DrawElementData>(true);

			foreach (UIDrawTeamList drawUI in drawUIs)
				drawUI.Init();

			foreach (UIDrawController drawController in drawControllers)
				drawController.Init(this);

			InitDataAndSync();
		}

		private void InitData()
		{
			MDebugLog(nameof(InitData));

			// 팀당 인원 수 * 팀 수 만큼 데이터 생성

			// 1. 사전 정의 데이터 설정
			for (int i = 0; i < DrawElementDatas.Length; i++)
			{
				DrawElementDatas[i].Index = i;
				
				SetElementData(
					index: i,
					teamType: DrawElementDatas[i].InitTeamType,
					role: DrawElementDatas[i].InitRole,
					isShowing: DrawElementDatas[i].InitTeamType != TeamType.None,
					syncData: "");
			}

			switch (drawType)
			{
				case DrawType.AllRandom:
					SetAllRemainRandom();
					break;
				case DrawType.Auction:
					// 하나씩 뽑아서 팀 설정
					break;
			}
		}

		public void InitDataAndSync()
		{
			MDebugLog(nameof(InitDataAndSync));

			InitData();
			SyncData();
		}

		public void SetAllRemainRandom(bool isShowing = false, string syncData = NONE_STRING)
		{
			MDebugLog($"{nameof(SetAllRemainRandom)}, IsShowing : {isShowing}, SyncData : {syncData}");

			// 랜덤으로 데이터 생성해서 남은 자리 채워넣기
			int[] remainTeamPlayerCounts = new int[teamCount];
			for (int i = 0; i < teamCount; i++)
				remainTeamPlayerCounts[i] = teamPlayerCount;

			// 이미 설정된 팀이 있으면 패스
			foreach (DrawElementData drawElementData in DrawElementDatas)
			{
				if (drawElementData.TeamType != TeamType.None)
					remainTeamPlayerCounts[(int)drawElementData.TeamType]--;
			}

			// 각 플레이어에 대해 팀과 역할 설정
			for (int i = 0; i < teamCount * teamPlayerCount; i++)
			{
				if (DrawElementDatas[i].TeamType != TeamType.None)
					continue;

				// 랜덤으로 팀 뽑기
				int randomTeamIndex;
				while (true)
				{
					randomTeamIndex = Random.Range(0, teamCount);
					if (remainTeamPlayerCounts[randomTeamIndex] > 0)
					{
						remainTeamPlayerCounts[randomTeamIndex]--;
						break;
					}
				}

				SetElementData(i, (TeamType)randomTeamIndex, DrawRole.Normal, isShowing, syncData);
			}
		}

		public void SetElementData(int index, TeamType teamType, DrawRole role, bool isShowing, string syncData = NONE_STRING)
		{
			MDebugLog($"{nameof(SetElementData)}, Index : {index}, TeamType : {teamType}, Role : {role}, IsShowing : {isShowing}, SyncData : {syncData}");

			DrawElementDatas[index].TeamType = teamType;
			DrawElementDatas[index].Role = role;
			DrawElementDatas[index].IsShowing = isShowing;
			DrawElementDatas[index].RuntimeData = syncData;
		}

		public void ShowTeam(TeamType teamType)
		{
			MDebugLog($"{nameof(ShowTeam)}, TeamType : {teamType}");

			foreach (DrawElementData drawElementData in DrawElementDatas)
			{
				if (drawElementData.TeamType == teamType)
					drawElementData.IsShowing = true;
			}
			SyncData();
		}

		public DrawElementData GetNoneTeamElement()
		{
			foreach (DrawElementData drawElementData in DrawElementDatas)
			{
				if (drawElementData.TeamType == TeamType.None)
					return drawElementData;
			}

			return null;
		}

		public DrawElementData GetRandomNoneTeamElement()
		{
			DrawElementData[] noneTeamDrawElementDatas = new DrawElementData[DrawElementDatas.Length];
			
			int noneTeamElementCount = 0;
			foreach (DrawElementData drawElementData in DrawElementDatas)
			{
				if (drawElementData.TeamType == TeamType.None)
					noneTeamDrawElementDatas[noneTeamElementCount++] = drawElementData;
			}

			if (noneTeamElementCount == 0)
				return null;

			return noneTeamDrawElementDatas[Random.Range(0, noneTeamElementCount)];
		}

		#region HorribleEvents
		[ContextMenu(nameof(ShowTeamA))]
		public void ShowTeamA() => ShowTeam(TeamType.A);
		public void ShowTeamB() => ShowTeam(TeamType.B);
		public void ShowTeamC() => ShowTeam(TeamType.C);
		public void ShowTeamD() => ShowTeam(TeamType.D);
		public void ShowTeamE() => ShowTeam(TeamType.E);
		public void ShowTeamF() => ShowTeam(TeamType.F);
		public void ShowTeamG() => ShowTeam(TeamType.G);
		public void ShowTeamH() => ShowTeam(TeamType.H);
		public void ShowTeamI() => ShowTeam(TeamType.I);
		public void ShowTeamJ() => ShowTeam(TeamType.J);
		public void ShowTeamK() => ShowTeam(TeamType.K);
		public void ShowTeamL() => ShowTeam(TeamType.L);
		public void ShowTeamM() => ShowTeam(TeamType.M);
		public void ShowTeamN() => ShowTeam(TeamType.N);
		public void ShowTeamO() => ShowTeam(TeamType.O);
		public void ShowTeamP() => ShowTeam(TeamType.P);
		public void ShowTeamQ() => ShowTeam(TeamType.Q);
		public void ShowTeamR() => ShowTeam(TeamType.R);
		public void ShowTeamS() => ShowTeam(TeamType.S);
		public void ShowTeamT() => ShowTeam(TeamType.T);
		public void ShowTeamU() => ShowTeam(TeamType.U);
		public void ShowTeamV() => ShowTeam(TeamType.V);
		public void ShowTeamW() => ShowTeam(TeamType.W);
		public void ShowTeamX() => ShowTeam(TeamType.X);
		public void ShowTeamY() => ShowTeam(TeamType.Y);
		public void ShowTeamZ() => ShowTeam(TeamType.Z);
		#endregion
	}
}
