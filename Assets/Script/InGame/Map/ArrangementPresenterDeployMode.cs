using System;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// ArrangementPresenter での実際に配置するときの見え方を定義
    /// Arrangement が実行されたときに、IPlayerArrangementTargetの状態によって、インスタンスを作成して配置
    /// </summary>
    public interface IArrangementPresenterDeployMode
    {
        void Arrangement(IPlayerArrangementTarget playerArrangementTarget);
        void Dispose();
    }
    public struct ArrangementPresenterDeployModeSetting
    {
        public Camera MainCameta { get; private set;}
        public ArrangementPresenter ArrangementPresenter { get; private set; }
        public GameObject ArrangementRoot { get; private set;}
        public GameObject ArrangementPrefab { get; private set; }
        public GameObject SelectedArrangementPrefab { get; private set; }
        public GameObject ReserveArrangementPrefab { get; private set; }
        public IPlayerArrangementTargetRepository PlayerArrangementTargetRepository { get; private set;} 

        public ArrangementPresenterDeployModeSetting(
            Camera mainCameta,
            ArrangementPresenter arrangementPresenter,
            GameObject arrangementRoot,
            GameObject arrangementPrefab,
            GameObject selectedArrangementPrefab, 
            GameObject reserveArrangementPrefab, 
            IPlayerArrangementTargetRepository playerArrangementTargetRepository)
        {
            this.MainCameta = mainCameta;
            this.ArrangementPresenter = arrangementPresenter;
            this.ArrangementRoot = arrangementRoot;
            this.ArrangementPrefab = arrangementPrefab;
            this.SelectedArrangementPrefab = selectedArrangementPrefab;
            this.ReserveArrangementPrefab = reserveArrangementPrefab;
            this.PlayerArrangementTargetRepository = playerArrangementTargetRepository;
        }
    }

    /// <summary>
    /// ArrangementPresenter での実際に配置するときの見え方を定義
    /// </summary>
    public abstract class ArrangementPresenterDeployModeBase: IArrangementPresenterDeployMode
    {
        protected readonly ArrangementPresenterDeployModeSetting arrangementPresenterDeployModeSetting;     
        protected List<IDisposable> disposables;

        public ArrangementPresenterDeployModeBase(ArrangementPresenterDeployModeSetting arrangementPresenterDeployModeSetting)
        {
            this.disposables = new List<IDisposable>();
            this.arrangementPresenterDeployModeSetting = arrangementPresenterDeployModeSetting;
        }

        public abstract void Arrangement(IPlayerArrangementTarget playerArrangementTarget);
        public void Dispose()
        {
            foreach (var disposable in this.disposables)
            {
                disposable.Dispose();
            }
            this.disposables.Clear();
        }

        protected void appearArrangement (IPlayerArrangementTarget arrangementTarget, GameObject prefab, bool isReserve = false) {
            arrangementTarget.ArrangementPositions.ForEach (arrangementPosition => {
                var instance = Object.AppearToFloor (
                    prefab, 
                    this.arrangementPresenterDeployModeSetting.ArrangementRoot, 
                    new Vector3 (
                        arrangementPosition.x * ArrangementAnnotater.ArrangementWidth,
                        0,
                        arrangementPosition.z * ArrangementAnnotater.ArrangementHeight)
                );

                Debug.Assert (instance != null, "Arrangement が生成されていません");
                Debug.Assert (instance.GetComponent<ArrangementView> () != null, "Arrangement の ArrangementView が設定されておりません。");

                var arrangementView = instance.GetComponent<ArrangementView> ();

                // 選択時の挙動を追加
                if (!isReserve) {
                    arrangementView.OnSelect
                        .Subscribe (_ => {
                            this.arrangementPresenterDeployModeSetting.ArrangementPresenter.OnTouchArrangement.Execute(arrangementTarget);
                        });
                }

                this.arrangementPresenterDeployModeSetting.ArrangementPresenter.AddInstance (instance);
            });
        }        
    }

    /// <summary>
    /// 通常配置や消去時
    /// </summary>
    public class ArrangementPresenterDeployMode : ArrangementPresenterDeployModeBase
    {
        private readonly UnReserveArrangementService unReserveArrangementService;
        private readonly GameObject arrangementCancelAnnotationPrefab;

        public ArrangementPresenterDeployMode(ArrangementPresenterDeployModeSetting arrangementPresenterDeployModeSetting)
        : base(arrangementPresenterDeployModeSetting)
        {
            this.unReserveArrangementService = new UnReserveArrangementService(this.arrangementPresenterDeployModeSetting.PlayerArrangementTargetRepository);
            this.arrangementCancelAnnotationPrefab = ResourceLoader.LoadModel("arrangement_chancel_annotation");
        }

        public override void Arrangement(IPlayerArrangementTarget arrangementTarget)
        {
            if (GameManager.Instance.ArrangementManager.CheckIsSelect (arrangementTarget)) 
            {
                // 選択状態
                this.appearArrangement (arrangementTarget, this.arrangementPresenterDeployModeSetting.SelectedArrangementPrefab);
            } 
            else if (arrangementTarget.ArrangementTargetState == ArrangementTargetState.Reserve) 
            {
                // 予約状態
                this.appearArrangement (arrangementTarget, this.arrangementPresenterDeployModeSetting.ReserveArrangementPrefab);
                this.AddArrangementCancelAnnotation(arrangementTarget);
            } 
            else 
            {
                // 設置状態
                this.appearArrangement (arrangementTarget, this.arrangementPresenterDeployModeSetting.ArrangementPrefab);
            }
        }

        private void AddArrangementCancelAnnotation (IPlayerArrangementTarget arrangementTarget) {
            var instance = Object.AppearToFloor (
                this.arrangementCancelAnnotationPrefab, 
                this.arrangementPresenterDeployModeSetting.ArrangementRoot, 
                arrangementTarget.CenterPosition);
            
            var view = instance.GetComponent<ArrangementReserveCancelView>();
            view.Initialize(this.arrangementPresenterDeployModeSetting.MainCameta);
            view.SetAnnnotationSize(arrangementTarget.MonoInfo.Width, arrangementTarget.MonoInfo.Height);
            this.disposables.Add(view.OnCancelObservable.Subscribe(_ => {
                this.unReserveArrangementService.Execute(arrangementTarget);
            }));
            this.disposables.Add(view.OnClickDirection.Subscribe(direction => {
                var nextPositions = GameManager.Instance.ArrangementManager.GetNearPosition(arrangementTarget, direction);
                if (GameManager.Instance.ArrangementManager.IsSetArrangement(nextPositions, arrangementTarget.ArrangementLayer, new List<IPlayerArrangementTarget>(){arrangementTarget}))
                {
                    GameManager.Instance.ArrangementManager.SetPosition(arrangementTarget, nextPositions);
                    this.arrangementPresenterDeployModeSetting.ArrangementPresenter.ReLoad();
                }
            }));

            this.arrangementPresenterDeployModeSetting.ArrangementPresenter.AddInstance (instance);
        }
    }

    /// <summary>
    /// 移動時のモード
    /// </summary>
    public class MoveArrangementPresenterDeployMode : ArrangementPresenterDeployModeBase
    {
        private readonly GameObject arrangementMovePrefab;
        private readonly GameObject arrangementMoveAnnotationPrefab;

        public MoveArrangementPresenterDeployMode(ArrangementPresenterDeployModeSetting arrangementPresenterDeployModeSetting)
        : base(arrangementPresenterDeployModeSetting)
        {
            this.arrangementMovePrefab = ResourceLoader.LoadModel("arrangement_move");
            this.arrangementMoveAnnotationPrefab = ResourceLoader.LoadModel("arrangement_move_annotation");
        }

        public override void Arrangement(IPlayerArrangementTarget arrangementTarget)
        {
            if (arrangementTarget.ArrangementTargetState == ArrangementTargetState.MoveIndicator) 
            {
                // 移動操作のため
                this.appearArrangement (arrangementTarget, this.arrangementMovePrefab);
                AddArrangementMoveAnnotation(arrangementTarget);
            } 
            else if (arrangementTarget.ArrangementTargetState == ArrangementTargetState.Appear) 
            {
                // 通常の見え方
                this.appearArrangement (arrangementTarget, this.arrangementPresenterDeployModeSetting.ArrangementPrefab);
            } 
            else 
            {
                Debug.Assert(false, "ありえない状態です。" + arrangementTarget.ArrangementTargetState);
            }            
        }

        private void AddArrangementMoveAnnotation (IPlayerArrangementTarget arrangementTarget) {
            var instance = Object.AppearToFloor (
                this.arrangementMoveAnnotationPrefab, 
                this.arrangementPresenterDeployModeSetting.ArrangementRoot, 
                arrangementTarget.CenterPosition);
            
            var view = instance.GetComponent<ArrangementMoveView>();
            view.Initialize(this.arrangementPresenterDeployModeSetting.MainCameta);
            view.SetAnnnotationSize(arrangementTarget.MonoInfo.Width, arrangementTarget.MonoInfo.Height);

            // OK
            this.disposables.Add(view.OnOKObservable.Subscribe(_ => {
                GameManager.Instance.ArrangementMoveIndicatorManager.Decision();
                GameManager.Instance.ArrangementMoveIndicatorManager.End();
                this.arrangementPresenterDeployModeSetting.ArrangementPresenter.ReLoad();
            }));

            // キャンセル
            this.disposables.Add(view.OnCancelObservable.Subscribe(_ => {
                GameManager.Instance.ArrangementMoveIndicatorManager.End();
                this.arrangementPresenterDeployModeSetting.ArrangementPresenter.ReLoad();
            }));

            // 移動
            this.disposables.Add(view.OnClickDirection.Subscribe(direction => {
                if(GameManager.Instance.ArrangementMoveIndicatorManager.MoveIndicatorPosition(direction)) {
                    this.arrangementPresenterDeployModeSetting.ArrangementPresenter.ReLoad();
                }
            }));

            this.arrangementPresenterDeployModeSetting.ArrangementPresenter.AddInstance (instance);
        }        
    }

}