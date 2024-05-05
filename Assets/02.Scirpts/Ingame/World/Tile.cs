using _02.Scirpts.Ingame.Entity;
using UnityEngine;

namespace _02.Scirpts.Ingame
{
    public class Tile : MonoBehaviour
    {
        private int i, j;
        private bool isConstructable;
        private float size;

        public bool IsInitialized { private set; get; } = false;

        /// <summary>
        /// 건축가능 여부
        /// </summary>
        public bool IsConstructable => isConstructable || Construct != null;

        public AbstractConstruct Construct;

        /// <summary>
        /// 타일 초기화
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="size"></param>
        /// <param name="isConstructable"></param>
        /// <param name="chaneState">해당 값으로 크기와 위치를 변경할지 여부</param>
        public void Init(int i, int j, float size, bool isConstructable = true, bool chaneState = true)
        {
            this.i = i;
            this.j = j;
            this.size = size;
            this.isConstructable = isConstructable;
            IsInitialized = true;
            if (chaneState)
            {
                transform.localPosition = new Vector3(i * size, 0,j * size);
                transform.localScale = new Vector3(size,transform.localScale.y,size);
            }
        }
        
        
    }
}