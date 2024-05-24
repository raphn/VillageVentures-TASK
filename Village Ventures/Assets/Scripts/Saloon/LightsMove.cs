using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace VillageVentures
{
    public class LightsMove : MonoBehaviour
    {
        [SerializeField] Light2D r;
        [SerializeField] Light2D g;
        [SerializeField] Light2D b;
        [Space]
        [SerializeField] float speed = .5f;
        [SerializeField] float moveRange = 10f;
        [Space]
        [SerializeField] bool danceTime;

        public bool IsOn
        {
            set
            {
                r.enabled = value;
                g.enabled = value;
                b.enabled = value;
                danceTime = value;
            }
        }


        private void Update()
        {
            if (danceTime)
            {
                float time = Time.time * speed;
                float shrink = Mathf.Sin(time);
                Vector2 pos = moveRange * new Vector2 (shrink, Mathf.Cos(time));

                transform.position = pos;
                transform.localScale = shrink * Vector3.one;
            }
        }
    }
}