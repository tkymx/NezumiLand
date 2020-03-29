using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL 
{
    public class ArrangementManager 
    {
        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        };

        /// <summary>
        /// 配置ターゲット
        /// </summary>
        private List<IPlayerArrangementTarget> arrangementTargetStore;
        public List<IPlayerArrangementTarget> ArrangementTargetStore => arrangementTargetStore;

        /// <summary>
        /// 選択されている 配置ターゲット
        /// </summary>
        private List<IPlayerArrangementTarget> selectedArrangementTargets;
        public List<IPlayerArrangementTarget> SelectedArrangementTargets => selectedArrangementTargets;

        /// <summary>
        /// 選択状況
        /// </summary>
        private ArrangementAnnotater arrangementAnnotater;
        public ArrangementAnnotater ArrangementAnnotater => arrangementAnnotater;

        /// <summary>
        /// 隣接状況を保管している
        /// </summary>
        private Dictionary<IPlayerArrangementTarget, List<IPlayerArrangementTarget>> nearMap;

        /// <summary>
        /// 配置の予約されているものがあるか？
        /// </summary>
        /// <returns></returns>
        public bool HasReserveArrangementTarget () {
            return this.arrangementTargetStore.Find(target => target.ArrangementTargetState == ArrangementTargetState.Reserve) != null;
        }

        /// <summary>
        /// モノViewのセットのサービス
        /// </summary>
        private SetMonoViewModelToArrangementService setMonoViewModelToArrangementService = null;

        /// <summary>
        /// 配置物が消去できない
        /// </summary>
        private ArrangementTargetRemoveService arrangementTargetRemoveService = null;

        /// <summary>
        /// 予約状態から可視化する際に使用する
        /// </summary>
        private AppearArrangementService appearArrangementService = null;

        /// <summary>
        /// 予約状況をキャンセルする
        /// </summary>
        private UnReserveArrangementService unReserveArrangementService = null;

        /// <summary>
        /// 座標を変更するサービス
        /// </summary>
        private SetArrangementPositionsService setArrangementPositionsService = null;

        private IPlayerOnegaiRepository playerOnegaiRepository = null;

        private OnegaiMediater onegaiMediater = null;

        public ArrangementManager (GameObject root, IPlayerOnegaiRepository playerOnegaiRepository, IPlayerArrangementTargetRepository playerArrangementTargetRepository) {
            this.arrangementTargetStore = new List<IPlayerArrangementTarget> ();
            this.selectedArrangementTargets = new List<IPlayerArrangementTarget>();
            this.arrangementAnnotater = new ArrangementAnnotater (root);
            this.nearMap = new Dictionary<IPlayerArrangementTarget, List<IPlayerArrangementTarget>>();
            
            this.setMonoViewModelToArrangementService = new SetMonoViewModelToArrangementService(playerArrangementTargetRepository);
            this.arrangementTargetRemoveService = new ArrangementTargetRemoveService(playerArrangementTargetRepository);
            this.appearArrangementService = new AppearArrangementService(playerArrangementTargetRepository);
            this.unReserveArrangementService = new UnReserveArrangementService(playerArrangementTargetRepository);
            this.setArrangementPositionsService = new SetArrangementPositionsService(playerArrangementTargetRepository);

            this.onegaiMediater = new OnegaiMediater(playerOnegaiRepository);
            this.playerOnegaiRepository = playerOnegaiRepository;
        }

        public void UpdateByFrame () {
            if (!GameManager.Instance.TimeManager.IsPause) {
                if (HasReserveArrangementTarget ()) {
                    this.appearArrangementService.Execute ();
                }
            }
        }

        /// <summary>
        /// id 指定して今どのくらい出現しているのかを調べる
        /// ※都度計算しており低速なので後でテーブル化はしたい。
        /// </summary>
        /// <param name="id"></param>
        /// <returns>出現数</returns>
        public int GetAppearMonoCountById (uint id, bool withReserve = true) {
            var result = this.arrangementTargetStore;
            if (!withReserve) {
                result = this.arrangementTargetStore
                    .Where (target => target.ArrangementTargetState == ArrangementTargetState.Appear)
                    .ToList();
            }
            return result
                .Where (target => target.HasMonoInfo)
                .Where (target => target.MonoInfo.Id == id)
                .Count ();
        }

        /// <summary>
        /// 近くの配置物を取得する
        /// </summary>
        /// <param name="arrangementTarget">モデル</param>
        /// <returns></returns>
        public List<IPlayerArrangementTarget> GetNearArrangement (IPlayerArrangementTarget arrangementTarget) {
            if (!this.nearMap.ContainsKey(arrangementTarget)) {
                return new List<IPlayerArrangementTarget>();
            }
            return this.nearMap[arrangementTarget];
        }

        /// <summary>
        /// 近くの配置物を探索する
        /// </summary>
        /// <param name="arrangementTarget">モデル</param>
        /// <returns></returns>
        private List<IPlayerArrangementTarget> SearchNearArrangement (IPlayerArrangementTarget arrangementTarget) {
            var nearArrangementTargets = new List<IPlayerArrangementTarget> ();
            foreach (var arrangementPosition in arrangementTarget.GetEdgePositions ()) {
                var findArrangementTarget = this.Find (arrangementPosition, arrangementTarget.ArrangementLayer);
                if (findArrangementTarget == null) {
                    continue;
                }
                if (nearArrangementTargets.IndexOf (findArrangementTarget) >= 0) {
                    continue;
                }
                if (!findArrangementTarget.HasMonoInfo) {
                    continue;
                }
                nearArrangementTargets.Add (findArrangementTarget);
            }
            return nearArrangementTargets;
        }

        /// <summary>
        /// 配置位置を追加する
        /// </summary>
        public void AddArrangement (IPlayerArrangementTarget arrangementTarget) {
            Debug.Assert (IsSetArrangement (arrangementTarget.ArrangementPositions, arrangementTarget.ArrangementLayer), "セットできない arrangementPosition が選択されています。");
            this.arrangementTargetStore.Add (arrangementTarget);
            GameManager.Instance.ArrangementPresenter.ReLoad ();
        }

        public void CreateAndSetMono (PlayerArrangementTargetModel playerArrangementTargetModel) {
            var monoViewModel = GameManager.Instance.MonoManager.CreateMono (playerArrangementTargetModel.MonoInfo, playerArrangementTargetModel.CenterPosition);
            var foundArrangementTarget = this.arrangementTargetStore.Find(arrangementTarget => arrangementTarget.PlayerArrangementTargetModel.Equals(playerArrangementTargetModel));
            Debug.Assert(foundArrangementTarget != null, "配置ターゲットがありません");

            // 生成した viewModel をセット
            this.setMonoViewModelToArrangementService.Execute(foundArrangementTarget, monoViewModel);

            // 隣接お願いの確認
            this.AppendNearArrangement(foundArrangementTarget);
            GameManager.Instance.OnegaiMediaterManager.NearOnegaiMediater.MediateByArrangement(foundArrangementTarget);

            // 設置数のお願いの確認
            var arrangemntMonoId = playerArrangementTargetModel.MonoInfo.Id;
            this.onegaiMediater.Mediate (
                new NL.OnegaiConditions.ArrangementCount(),
                playerOnegaiRepository.GetAll().ToList()
            ); 

            // 配置時のイベント
            GameManager.Instance.EventManager.PushEventParameter(new NL.EventCondition.AfterArrangement());
        }

        public List<ArrangementPosition> GetNearPosition (IPlayerArrangementTarget arrangementTarget, ArrangementManager.Direction direction) {
            Debug.Assert(this.arrangementTargetStore.Contains(arrangementTarget), "動かしたい、ターゲットがありません");

            // 次の位置を作成
            var nextArrangementPositions = new List<ArrangementPosition>();
            foreach (var arrangementPosition in arrangementTarget.ArrangementPositions)
            {
                switch(direction)
                {
                    case Direction.Left: 
                    {
                        nextArrangementPositions.Add( new ArrangementPosition(){
                            x = arrangementPosition.x - 1,
                            z = arrangementPosition.z
                        });
                        break;
                    }
                    case Direction.Right: 
                    {
                        nextArrangementPositions.Add( new ArrangementPosition(){
                            x = arrangementPosition.x + 1,
                            z = arrangementPosition.z
                        });
                        break;
                    }
                    case Direction.Up: 
                    {
                        nextArrangementPositions.Add( new ArrangementPosition(){
                            x = arrangementPosition.x,
                            z = arrangementPosition.z + 1
                        });
                        break;
                    }
                    case Direction.Down: 
                    {
                        nextArrangementPositions.Add( new ArrangementPosition(){
                            x = arrangementPosition.x,
                            z = arrangementPosition.z - 1
                        });
                        break;
                    }
                    default:
                    {
                        Debug.Assert(false, "未確認の状態です。");
                        break;
                    }
                }
            }
            return nextArrangementPositions;
        }
        public void SetPosition(IPlayerArrangementTarget arrangementTarget, List<ArrangementPosition> positions) 
        {
            Debug.Assert(this.arrangementTargetStore.Contains(arrangementTarget), "動かしたい、ターゲットがありません");

            if (arrangementTarget.HasMonoViewModel) 
            {
                // 近接状況の解除時のお願いの判断
                GameManager.Instance.OnegaiMediaterManager.NearOnegaiMediater.MediateByRBeforeRemoval(arrangementTarget);
                // 近接状況の解除
                this.RemoveNearArrangement(arrangementTarget);
            }

            this.setArrangementPositionsService.Execute(arrangementTarget, positions);

            // 隣接お願いの確認
            if (arrangementTarget.HasMonoViewModel) 
            {
                // 近接状況に追加
                this.AppendNearArrangement(arrangementTarget);
                // 近接状況のお願いの判断
                GameManager.Instance.OnegaiMediaterManager.NearOnegaiMediater.MediateByArrangement(arrangementTarget);
            }
        }

        /// <summary>
        /// 選択を外す
        /// </summary>
        public void RemoveSelection () {
            this.selectedArrangementTargets.Clear();
            GameManager.Instance.ArrangementPresenter.ReLoad ();
            GameManager.Instance.GameUIManager.ArrangementMenuUIPresenter.Close ();
        }

        /// <summary>
        /// 選択を外す
        /// </summary>
        public void RemoveSelection (IPlayerArrangementTarget playerArrangementTarget) {
            Debug.Assert(CheckIsSelect(playerArrangementTarget), "選択されていません");
            this.selectedArrangementTargets.Remove(playerArrangementTarget);
            GameManager.Instance.ArrangementPresenter.ReLoad ();
        }

        /// <summary>
        /// 選択されている配置ターゲットを消す
        /// </summary>
        public void RemoveSelectArrangement () {
            // オブジェクトを消す
            foreach (var selectedArrangementTarget in this.selectedArrangementTargets)
            {
                this.RemoveArranement (selectedArrangementTarget);                
            }
            this.RemoveSelection();
        }

        /// <summary>
        /// 配置位置を消去する
        /// </summary>
        public void RemoveArranement (IPlayerArrangementTarget arrangementTarget) {

            var isContain = arrangementTargetStore.Contains (arrangementTarget);
            Debug.Assert(isContain, "含まれていない配置が消されようとしました。");
            if (isContain) 
            {
                // 設置済みの場合は設置情報を消す
                if (arrangementTarget.HasMonoViewModel) 
                {
                    GameManager.Instance.MonoManager.RemoveMono (arrangementTarget.MonoViewModel);
                    GameManager.Instance.OnegaiMediaterManager.NearOnegaiMediater.MediateByRBeforeRemoval(arrangementTarget);

                    // 近隣配置のお願い（自分の近隣を見るために消える前に必要）
                    this.RemoveNearArrangement(arrangementTarget);
                }

                // 一覧から消す
                this.arrangementTargetStore.Remove (arrangementTarget);

                // プレイヤーデータが存在する場合は消去
                if (arrangementTarget.PlayerArrangementTargetModel != null)
                {
                    this.arrangementTargetRemoveService.Execute(arrangementTarget.PlayerArrangementTargetModel);
                }

                // 設置済みの場合は設置情報を消す
                if (arrangementTarget.HasMonoViewModel) 
                {
                    // 減ったかどうかの判断（減った状態を渡すため、減ったあとに事項する必要あり）
                    var arrangemntMonoId = arrangementTarget.MonoInfo.Id;
                    this.onegaiMediater.ClearResetAndMediate (
                        new NL.OnegaiConditions.ArrangementCount(),
                        playerOnegaiRepository.GetAll().ToList()
                    );
                }
            }
            GameManager.Instance.ArrangementPresenter.ReLoad ();
        }

        /// <summary>
        /// すべての予約状況をキャンセルする
        /// </summary>
        public void UnReserveArrangementAll() {
            var reserveArrangementTarget = this.ArrangementTargetStore.Where(arrangementTarget => arrangementTarget.ArrangementTargetState == ArrangementTargetState.Reserve).ToList();
            foreach (var arrangementTarget in reserveArrangementTarget)
            {
                this.unReserveArrangementService.Execute(arrangementTarget);
            }
        }

        /// <summary>
        /// 近接情報を取得
        /// </summary>
        /// <param name="arrangementTarget"></param>
        private void AppendNearArrangement(IPlayerArrangementTarget arrangementTarget) {
            var nearArrangementTargets = SearchNearArrangement(arrangementTarget);
            if (!this.nearMap.ContainsKey(arrangementTarget)) {
                this.nearMap[arrangementTarget] = new List<IPlayerArrangementTarget>();
            }
            this.nearMap[arrangementTarget].AddRange(nearArrangementTargets);
            foreach (var nearArrangementTarget in nearArrangementTargets)
            {
                if (!this.nearMap.ContainsKey(nearArrangementTarget)) {
                    this.nearMap[nearArrangementTarget] = new List<IPlayerArrangementTarget>();
                }
                if (!this.nearMap[nearArrangementTarget].Contains(arrangementTarget))
                {
                    this.nearMap[nearArrangementTarget].Add(arrangementTarget);
                }
            }
        }

        private void RemoveNearArrangement(IPlayerArrangementTarget arrangementTarget) {
            if (this.nearMap.ContainsKey(arrangementTarget)) {
                foreach (var nearArrangementTarget in this.nearMap[arrangementTarget])
                {
                    this.nearMap[nearArrangementTarget].RemoveAll(target => target == arrangementTarget);
                }
                this.nearMap.Remove(arrangementTarget);
            }
        }

        /// <summary>
        /// 引数の配置位置がすでに存在しているかどうか？
        /// </summary>
        public bool IsSetArrangement (IPlayerArrangementTarget arrangementTarget) {
            if (arrangementTarget == null) {
                return false;
            }
            return IsSetArrangement (arrangementTarget.ArrangementPositions, arrangementTarget.ArrangementLayer);
        }
        public bool IsSetArrangement (List<ArrangementPosition> arrangementPositions, ArrangementLayer arrangementLayer, List<IPlayerArrangementTarget> extractList = null) {
            foreach (var arrangementPosition in arrangementPositions) {
                var found = Find (arrangementPosition, arrangementLayer);
                if (found != null) {
                    if (extractList != null) {
                        if (!extractList.Contains(found)) {
                            return false;
                        }
                    }else{
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 選択する
        /// </summary>
        /// <param name="arrangementTarget"></param>
        public void Select (IPlayerArrangementTarget arrangementTarget) {
            Debug.Assert (arrangementTargetStore.Contains (arrangementTarget), "管理されていないターゲットです。");

            if (this.selectedArrangementTargets.Contains(arrangementTarget)) {
                return;
            }

            this.selectedArrangementTargets.Add(arrangementTarget);
            GameManager.Instance.ArrangementPresenter.ReLoad ();
        }

        /// <summary>
        /// 一つだけ選択する
        /// </summary>
        public void SelectOnly (IPlayerArrangementTarget arrangementTarget) {
            this.RemoveSelection();
            this.Select(arrangementTarget);
        }

        /// <summary>
        /// 選択されているかどうかの確認
        /// </summary>
        /// <param name="arrangementTarget"></param>
        /// <returns></returns>
        public bool CheckIsSelect (IPlayerArrangementTarget arrangementTarget) {
            return selectedArrangementTargets.Contains(arrangementTarget);
        }

        private IPlayerArrangementTarget Find (ArrangementPosition arrangementPosition, ArrangementLayer layer) {
            // layer で filter
            var filterdArrangementTargetStore = this.arrangementTargetStore
                .Where(arrangementTarget => arrangementTarget.ArrangementLayer == layer)
                .ToList();

            // 探索
            foreach (var arrangemetTarget in filterdArrangementTargetStore) {
                var findIndex = arrangemetTarget.ArrangementPositions.FindIndex (targetArrangementPosition => {
                    if (targetArrangementPosition.x != arrangementPosition.x) {
                        return false;
                    }
                    if (targetArrangementPosition.z != arrangementPosition.z) {
                        return false;
                    }
                    return true;
                });

                // 見つかったら返却
                if (findIndex >= 0) {
                    return arrangemetTarget;
                }
            }
            return null;
        }
    }
}