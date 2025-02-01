﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
    // AuctionManager, DrawManager 보다 늦게 실행되어야 함
    [DefaultExecutionOrder(-9000)]
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class AuctionDraw : MBase
    {
        [field: SerializeField] public DrawManager DrawManager { get; private set; }
        [field: SerializeField] public AuctionManager AuctionManager { get; private set; }
        [SerializeField] private UIAuctionDraw[] uis;
        [UdonSynced, FieldChangeCallback(nameof(TargetIndex))] private int _targetIndex = NONE_INT;
        public int TargetIndex
        {
            get => _targetIndex;
            set
            {
                _targetIndex = value;
                OnTargetIndexChanged();
            }
        }

        private void OnTargetIndexChanged()
        {
            MDebugLog($"{nameof(OnTargetIndexChanged)}, TargetIndex : {TargetIndex}");

            if (TargetIndex != NONE_INT)
                MDebugLog($"{nameof(OnTargetIndexChanged)}, DrawElementData : {DrawManager.DrawElementDatas[TargetIndex].Name}");

            UpdateUI();
        }

        private void Start()
        {
            Init();
            UpdateUI();
        }

        private void Init()
        {
            MDebugLog(nameof(Init));

            foreach (UIAuctionDraw ui in uis)
                ui.Init(this);

            DrawManager.RegisterListener(this, nameof(UpdateUI));
            DrawManager.RegisterListener(AuctionManager, nameof(AuctionManager.UpdateContent));

            UpdateUI();
            AuctionManager.UpdateContent();

            if (Networking.IsMaster)
                OnWait();
        }

        public void UpdateUI()
        {
            MDebugLog(nameof(UpdateUI));

            foreach (UIAuctionDraw ui in uis)
                ui.UpdateUI();
        }

        public void UpdateDrawByAuction()
        {
            MDebugLog(nameof(UpdateDrawByAuction));

            switch ((AuctionState)AuctionManager.ContentState)
            {
                case AuctionState.Wait:
                    // 아직 팀이 정해지지 않은 랜덤한 한 명 (미리 설정)
                    OnWait();
                    break;
                case AuctionState.ShowTarget:
                    // 경매 대상 공개
                    OnShowTarget();
                    break;
                case AuctionState.AuctionTime:
                    // 경매 시간
                    OnAuctionTime();
                    break;
                case AuctionState.WaitForResult:
                    // 경매 결과 대기
                    OnWaitForResult();
                    break;
                case AuctionState.CheckResult:
                    // 경매 결과 확인
                    OnCheckResult();
                    break;
                case AuctionState.ApplyResult:
                    // 경매 결과 적용
                    OnApplyResult();
                    break;
            }

            UpdateUI();
        }

        protected virtual void OnWait()
        {
            MDebugLog(nameof(OnWait));

            if (IsOwner() == false)
                return;

            DrawElementData randomNoneTeamElement = DrawManager.GetRandomNoneTeamElement();

            if (randomNoneTeamElement != null)
            {
                SetTargetIndex(randomNoneTeamElement.Index);
            }
            else
            {
                MDebugLog("NoneTeamDrawElementData is null");
            }
        }

        protected virtual void OnShowTarget()
        {
            MDebugLog(nameof(OnShowTarget));

            if (IsOwner() == false)
                return;
        }

        protected virtual void OnAuctionTime()
        {
            MDebugLog(nameof(OnAuctionTime));

            if (IsOwner() == false)
                return;
        }

        protected virtual void OnWaitForResult()
        {
            MDebugLog(nameof(OnWaitForResult));

            if (IsOwner() == false)
                return;
        }

        protected virtual void OnCheckResult()
        {
            MDebugLog(nameof(OnCheckResult));

            if (IsOwner() == false)
                return;
        }

        protected virtual void OnApplyResult()
        {
            MDebugLog(nameof(OnApplyResult));

            if (IsOwner(DrawManager.gameObject) == false)
                return;

            if (AuctionManager.WinnerIndex == NONE_INT)
                return;

            // HACK: AuctionSeat와 DrawElementData의 Index가 같다면, 둘 다 동일한 플레이어를 대상으로 한다고 가정
            TeamType teamType = DrawManager.DrawElementDatas[AuctionManager.WinnerIndex].TeamType;
            int maxTryPoint = AuctionManager.GetMaxTurnData();
            DrawManager.SetElementData(TargetIndex, teamType, DrawRole.Normal, true, maxTryPoint.ToString());
        }

        public void SetTargetIndex(int targetIndex)
        {
            MDebugLog($"{nameof(SetTargetIndex)}, TargetIndex : {targetIndex}");

            SetOwner();
            TargetIndex = targetIndex;
            RequestSerialization();
        }

        public void SetAllRemainRandomAndSync()
        {
            MDebugLog(nameof(SetAllRemainRandomAndSync));

            DrawManager.SetAllRemainRandom(true, "0");
        }
    }
}
