using UnityEngine;

namespace _02.Scirpts.Ingame
{
    /// <summary>
    /// 카메라 제어 클래스
    /// </summary>
    public class QuarterviewCamera : MonoBehaviour
    {
        [SerializeField] private Vector2 Angles;
        
        
        void Start()
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,Angles.y,transform.eulerAngles.z);
            transform.GetChild(0).eulerAngles = new Vector3(Angles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        // Update is called once per frame
        void Update()
        {


        }

    }
}

