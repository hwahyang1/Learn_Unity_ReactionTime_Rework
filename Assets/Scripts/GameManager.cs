using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*
 * [class] GameManager
 * 게임의 전반적 실행을 관리합니다.
 */
public class GameManager : MonoBehaviour
{
	/*
	 * [Enum] GameStatus
	 * 게임의 진행 상황을 표기합니다.
	 */
	private enum GameStatus
    {
		Title,
		Prepare,
		Wait,
		Record,
		Result,
		Finish
    }

	public Text messageText;

	private Camera camera;

	private GameStatus gameStatus = GameStatus.Title;

	/* 내용을 GameStatus와 동일한 순서로 대입합니다. */
	private List<string> messages = new List<string>()
    {
		"화면을 눌러 반응속도를 테스트 해 보세요!\n(총 {0}번 시행하여 평균값을 표시합니다.)",
		"게임을 준비하고 있어요.\n잠시만 기다려 주세요.",
		"화면이 빨간색이 되면 화면을 클릭하세요!",
		"지금!",
		"[{0}/{1}회] 측정결과: {2}ms\n{3}초 후 다시 측정을 시작해요.",
		"{}회 측정결과: {}ms\n화면을 클릭하여 측정을 다시 할 수 있어요."
    };
	private List<Color> colors = new List<Color>()
	{
		new Color(),
		new Color(),
		new Color(),
		new Color(),
		new Color(),
		new Color()
	};

	private int currentRound = 1;
	[SerializeField] private int maxRound = 5;

	[SerializeField] private float resumeTime = 3f;
	private float waitTime = 0f;
	private float elapsedTime = 0f;
	private List<float> reactionTime = new List<float>();

	private void Start()
	{
		camera = Camera.main;

		camera.backgroundColor = colors[(int)gameStatus];
		messageText.text = string.Format(messages[(int)gameStatus], maxRound);
	}

	private void Update()
	{

	}
}
