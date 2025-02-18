﻿using UdonSharp;
using UnityEngine;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
    [DefaultExecutionOrder(-10000)]
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class QuizManager : ContentManager
    {
        [Header("_" + nameof(QuizManager))]
        [SerializeField] protected int playerCount = 10;
        [SerializeField] protected MValue curQuizIndex;
        [SerializeField] private GameObject[] waitTimeObjects;
        [SerializeField] private Transform wrongPos;
        [SerializeField] private Transform[] quizDataParents;
        [SerializeField] protected MValue quizDataParentsIndex;
        [SerializeField] private MString seatIndexInputField;

        [field: Header("_" + nameof(QuizManager) + "_GameRule")]
        [field: SerializeField] public bool GameRule_ADD_SCORE_WHEN_CORRECT_ANSWER { get; private set; } = false;
        [field: SerializeField] public bool GameRule_SUB_SCORE_WHEN_WRONG_ANSWER { get; private set; } = false;
        [field: SerializeField] public bool GameRule_DROP_PLAYER_WHEN_WRONG_ANSWER { get; private set; } = false;
        [field: SerializeField] public bool GameRule_DROP_PLAYER_WHEN_ZERO_SCORE { get; private set; } = false;

        protected int[] answerCount = new int[10];

        public QuizData[] QuizDatas { get; private set; }

        public int CurQuizIndex => curQuizIndex.Value;

        public QuizData CurQuizData => QuizDatas[CurQuizIndex];
        public bool CanSelectAnswer { get; protected set; } = true;

        protected override void Start()
        {
            base.Start();
            UpdateContent();
        }

        protected override void Init()
        {
            QuizDatas = quizDataParents[0].GetComponentsInChildren<QuizData>();

            base.Init();

            curQuizIndex.RegisterListener(this, nameof(OnQuizIndexChange));
            quizDataParentsIndex.RegisterListener(this, nameof(OnQuizDataParentChange));

            curQuizIndex.SetMinMaxValue(0, QuizDatas.Length - 1);
            quizDataParentsIndex.SetMinMaxValue(0, quizDataParents.Length - 1);

            OnQuizIndexChange();
            OnQuizDataParentChange();
        }

        public override void UpdateContent()
        {
            CalcAnswerCount();
            SetWaitObjectActive(IsContentState((int)QuizGameState.Wait));

            base.UpdateContent();
        }

        private void CalcAnswerCount()
        {
            answerCount = new int[(int)QuizAnswerType.None + 1];
            foreach (QuizSeat quizSeat in MSeats)
            {
                if ((int)quizSeat.ExpectedAnswer < 0 || (int)quizSeat.ExpectedAnswer >= answerCount.Length)
                    continue;

                answerCount[(int)quizSeat.ExpectedAnswer]++;
            }
        }

        private void SetWaitObjectActive(bool active)
        {
            foreach (GameObject waitTimeObject in waitTimeObjects)
                waitTimeObject.SetActive(active);
        }

        protected override void OnContentStateChange(DataChangeState changeState)
        {
            if (changeState != DataChangeState.Less)
            {
                if (ContentState == (int)QuizGameState.Wait) OnWait();
                else if (ContentState == (int)QuizGameState.ShowQuiz) OnQuizTime();
                else if (ContentState == (int)QuizGameState.SelectAnswer) OnSelectAnswer();
                else if (ContentState == (int)QuizGameState.ShowPlayerAnswer) OnShowPlayerAnswer();
                else if (ContentState == (int)QuizGameState.CheckAnswer) OnCheckAnswer();
                else if (ContentState == (int)QuizGameState.Explaining) OnExplaining();
                else if (ContentState == (int)QuizGameState.Scoring) OnScoring();
            }

            base.OnContentStateChange(changeState);
        }

        public virtual void OnQuizIndexChange()
        {
            UpdateContent();
            SendEvents();
        }

        public virtual void OnQuizDataParentChange()
        {
            QuizDatas = quizDataParents[quizDataParentsIndex.Value].GetComponentsInChildren<QuizData>();
            curQuizIndex.SetMinMaxValue(0, QuizDatas.Length - 1);
            curQuizIndex.SetValue(0);
        }

        public void TeleportToSeat()
        {
            if (!IsDigit(seatIndexInputField.Value))
                return;

            int seatIndex = int.Parse(seatIndexInputField.Value);

            if (0 < seatIndex && seatIndex <= playerCount)
                TP(MSeats[seatIndex - 1].transform);
        }

        public void TP_WrongPos()
        {
            if (wrongPos)
                TP(wrongPos);
        }

        public virtual void OnWait()
        {
            MDebugLog($"{nameof(OnWait)}");

            if (IsOwner() == false)
                return;

            foreach (MSeat turnSeat in MSeats)
                turnSeat.ResetData();
        }

        public virtual void OnQuizTime()
        {
            MDebugLog($"{nameof(OnQuizTime)}");
        }

        public virtual void OnSelectAnswer()
        {
            MDebugLog($"{nameof(OnSelectAnswer)}");
        }

        public virtual void OnShowPlayerAnswer()
        {
            MDebugLog($"{nameof(OnShowPlayerAnswer)}");
        }

        public virtual void OnCheckAnswer()
        {
            MDebugLog($"{nameof(OnCheckAnswer)}");
        }

        public virtual void OnExplaining()
        {
            MDebugLog($"{nameof(OnExplaining)}");
        }

        public virtual void OnScoring()
        {
            MDebugLog($"{nameof(OnScoring)}");
        }
    }
}