using System;
using _02.Scirpts.Ingame.Entity;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02.Scirpts.Ingame
{
    public enum TileInfo
    {
        None = 0,
        NotConstructable = 1, // 1 << 0, // 건축 불가 지형
        Obstacle = 2  // 1 << 1,         // 장애물 지형(통과 불가)
        
        //All = 1 << 10 - 1
    }

    /// <summary>
    /// 월드의 타일 하나하나.
    /// 위치와 정보를 저장한다.
    /// </summary>
    public class Tile : MonoBehaviour
    {
        [Header("디버그")]
        [SerializeField] private bool debug = false;
        
        private int i, j;
        private float size;

        public bool IsInitialized { private set; get; } = false;

        [SerializeField] private TileInfo tileInfo = TileInfo.None;
        public TileInfo TileInfo => tileInfo;

        /// <summary>
        /// 건축가능 여부
        /// </summary>
        public bool IsConstructable => tileInfo == TileInfo.None || Construct != null;

        public AbstractConstruct Construct;

        /// <summary>
        /// 타일 초기화
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="size"></param>
        /// <param name="isConstructable"></param>
        /// <param name="chaneState">해당 값으로 크기와 위치를 변경할지 여부</param>
        public void Init(int i, int j, float size, TileInfo tileInfo = TileInfo.None, bool chaneState = true)
        {
            this.i = i;
            this.j = j;
            this.size = size;
            this.tileInfo = tileInfo;
            IsInitialized = true;
            if (chaneState)
            {
                transform.localPosition = new Vector3(i * size, 0, j * size);
                transform.localScale = new Vector3(size, transform.localScale.y, size);
            }
        }

        public void SetConstructable()
        {
            // isConstructable = true;
        }

        public void SetUnConstructable()
        {
            // isConstructable = false;
        }


        /// <summary>
        /// 하양 : 일반 타일
        /// 노랑 : 건설 불가 타일
        /// 빨강 : 이동 불가 타일
        /// </summary>
        private readonly Color[] _debugColors = new[] { Color.white, Color.yellow, Color.red };
        
        
        /// <summary>
        /// 디버그 옵션 확인
        /// </summary>
        /// <param name="debug"></param>
        public void CheckDebug(bool debug)
        {
            if(debug)
                EnableDebug();
            else
                DisableDebug();
        }

        private void EnableDebug()
        {
            this.debug = true;
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;
            if (Application.isPlaying)
            {
                meshRenderer.material.color = _debugColors[(int)TileInfo];
            }
        }

        private void DisableDebug()
        {
            this.debug = false;
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;
        }

        private void OnValidate()
        {
            CheckDebug(debug);
        }
    }
}