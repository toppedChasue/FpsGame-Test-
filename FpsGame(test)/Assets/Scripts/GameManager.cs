using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    //게임 상태 UI오브젝트 변수
    public GameObject gameLabel;

    PlayerMove player;

    //게임 상태 UI텍스트 컴포넌트 변수
    Text gameText;
    private void Awake()
    {
        if(gm ==null)
        {
            gm = this;
        }
    }

    public enum GameState
    {
        Ready,
        Run,
        GameOver
    }

    public GameState gState;

    // Start is called before the first frame update
    void Start()
    {
        gState = GameState.Ready;

        gameText = gameLabel.GetComponent <Text>();

        gameText.text = "Ready. . .";
        gameText.color = new Color32(255, 185, 0, 255);

        StartCoroutine(ReadyToStart());

        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.hp <= 0 )
        {
            player.GetComponent<Animator>().SetFloat("MoveMotion", 0f);

            gameLabel.SetActive(true);

            gameText.text = "Game Over";

            gameText.color = new Color32(255, 0, 0, 255);

            gState = GameState.GameOver;
        }
    }

    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2f);

        gameText.text = "Go!";

        yield return new WaitForSeconds(0.5f);

        gameLabel.SetActive(false);
        gState = GameState.Run;
    }
}
