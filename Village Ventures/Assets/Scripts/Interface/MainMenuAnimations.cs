using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimations : MonoBehaviour
{
    [SerializeField] RectTransform title;
    [SerializeField] float speed = 0.5f;
    [SerializeField] Vector2 displace = new Vector2Int(15, 15);
    [Space]
    [SerializeField] RectTransform startBtn;
    [SerializeField] float btnSpeed = 0.75f;
    [SerializeField] float resize = 0.15f;

    Vector2 pos;


    private void Start() => pos = title.anchoredPosition;

    private void Update()
    {
        float time = Time.time;

        float titleSpeed = time * speed;
        Vector2 disloc = new (Mathf.Sin(titleSpeed), Mathf.Cos(titleSpeed));
        disloc = disloc.normalized * displace;
        title.anchoredPosition = pos + disloc;

        Vector3 resizeAdd = Mathf.Sin(time * btnSpeed) * resize * Vector3.one;
        startBtn.localScale = Vector3.one + resizeAdd;
    }
}
