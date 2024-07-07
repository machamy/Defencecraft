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
        Obstacle = 2,  // 1 << 1,         // 장애물 지형(통과 불가)
        Void = 3 // 통과불가, 건축불가(맵밖)
                       // 
        //All = 1 << 10 - 1
    }

    public enum Direction
    {
        All = 0,
        Down,
        Up,
        Left,
        Right,
        None
    }

    /// <summary>
    /// 월드의 타일 하나하나.
    /// 위치와 정보를 저장한다.
    /// </summary>
    public class Tile : MonoBehaviour, IHeapItem<Tile>
    {
        [Header("디버그")]
        [SerializeField] private bool debug = false;
        
        private int i, j;
        private float size;

        [SerializeField] private Direction direction;
        [SerializeField] private TileSpriteSO tileType;

        public TileSpriteSO TileType
        {
            get => tileType;
            set
            {
                tileType = value;
            }
        }

        public Direction Direction
        {
            get => direction;
            set
            {
                direction = value;
                _spriteRenderer.sprite = tileType.GetSprite(value);
            }
        }
        
        public int gCost = 0;
        public int hCost;
        public int fCost{ get{ return gCost + hCost;}}
        public Tile Parent;
        int heapIndex;
        private SpriteRenderer _spriteRenderer;

        Vector3 worldPos;


        public bool IsInitialized { private set; get; } = false;

        [SerializeField] private TileInfo tileInfo = TileInfo.None;
        public TileInfo TileInfo => tileInfo;

        /// <summary>
        /// 건축가능 여부
        /// </summary>
        public bool IsConstructable => tileInfo != TileInfo.Void &&
                                       tileInfo == TileInfo.None &&
                                       Construct == null;

        /// <summary>
        /// 이동가능 여부
        /// </summary>
        public bool IsWalkable => Construct == null &&
                                  tileInfo != TileInfo.Obstacle&&
                                  tileInfo != TileInfo.Void;

        /// <summary>
        /// 건축물 여부
        /// </summary>
        public bool IsConstructed => Construct != null;

        /// <summary>
        /// 장애물 지형인지 체크(THIEF를 위한)
        /// </summary>
        public bool IsObstacle => tileInfo == TileInfo.Obstacle;


        public AbstractConstruct Construct;

        /// <summary>
        /// 타일 초기화
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="size"></param>
        /// <param name="isConstructable"></param>
        /// <param name="chaneState">해당 값으로 크기와 위치를 변경할지 여부</param>
        public void Init(Vector3 _worldPos, int i, int j, float size,TileSpriteSO type, Direction dir, TileInfo tileInfo = TileInfo.None, bool chaneState = true)
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            this.worldPos = _worldPos;
            this.i = i;
            this.j = j;
            this.size = size;
            TileType = type;
            this.tileInfo = tileInfo;
            this.Direction = dir;
            Construct = null;
            IsInitialized = true;
            
            
            if (chaneState)
            {
                transform.localPosition = new Vector3(i * size, 0, j * size);
                transform.localScale = new Vector3(size, transform.localScale.y, size);
            }
            Colorize();
        }

        public void SetConstructable()
        {
            //isConstructable = true;
        }

        public void SetUnConstructable()
        {
            //isConstructable = false;
        }
        public int getTileX()
        {
            return i;
        }
        public int getTileY()
        {
            return j;
        }

        /// <summary>
        /// 하양 : 일반 타일
        /// 노랑 : 건설 불가 타일
        /// 빨강 : 이동 불가 타일
        /// </summary>
        private readonly Color[] _debugColors = new[] { Color.white, Color.yellow, Color.red };
        private void Colorize()
        {
            SpriteRenderer meshRenderer = GetComponentInChildren<SpriteRenderer>();

            if (TileInfo == TileInfo.None)
            {
                meshRenderer.material.color = Color.white;
            }

            else if(TileInfo == TileInfo.NotConstructable)
            {
                meshRenderer.material.color = Color.yellow;
            }

            else if(TileInfo == TileInfo.Obstacle)
            {
                meshRenderer.material.color = Color.red;
            }
            else
            {
                Debug.Log("something failed");
            }
        }
        public void change_color(Color color)
        {
            SpriteRenderer meshRenderer = GetComponentInChildren<SpriteRenderer>();
            meshRenderer.material.color = color;
        }
        
        /// <summary>
        /// 디버그 옵션 확인
        /// </summary>
        /// <param name="debug"></param>
        public void CheckDebug(bool debug)
        {
            // if(debug)
            //     EnableDebug();
            // else
            //     DisableDebug();
        }

        private void EnableDebug()
        {
            this.debug = true;
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
                if (Application.isPlaying)
                {
                    meshRenderer.material.color = _debugColors[(int)TileInfo];
                }
            }

        }

        private void DisableDebug()
        {
            this.debug = false;
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                meshRenderer.enabled = false;
        }

        private void OnValidate()
        {
            CheckDebug(debug);
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _spriteRenderer.sprite = tileType.GetSprite(Direction);
        }

        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }
        public int CompareTo(Tile tileToCompare)
        {
            int compare = fCost.CompareTo(tileToCompare.fCost);
            if( compare == 0)
            {
                compare = hCost.CompareTo(tileToCompare.hCost);
            }
            return -compare;
        }
        
    }
}