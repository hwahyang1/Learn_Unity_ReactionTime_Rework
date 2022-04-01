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
		"화면을 눌러 반응속도를 테스트 해 보세요!\n(총 {0}번 시행하여 평균값을 표시해요.)",
		"게임을 준비하고 있어요.\n잠시만 기다려 주세요.",
		"화면이 빨간색이 되면 화면을 클릭하세요!",
		"지금!",
		"[{0}/{1}회] 측정결과: <color=#E5879C>{2:F2}ms</color>\n{3}초 후 다시 측정을 시작해요.",
		"{0}회 측정결과: <color=#E5879C>{1:F2}ms</color>ms\n화면을 클릭하여 측정을 다시 할 수 있어요."
	};
	private List<Color> colors = new List<Color>()
	{
		new Color(0.3887505f, 0.6140026f, 0.9056604f),
		new Color(0.8593963f, 0.8962264f, 0.5284354f),
		new Color(0.8593963f, 0.8962264f, 0.5284354f),
		new Color(0.8980392f, 0.5294118f, 0.6105746f),
		new Color(0.5294118f, 0.8980392f, 0.5906148f),
		new Color(0.8643410f, 0.5294118f, 0.8980392f)
	};

	private int currentRound = 0;
	[SerializeField] private int maxRound = 5;

	[SerializeField] private float resumeTime = 3f;
	private float waitTime = 0f;
	private float elapsedTime = 0f; // 측정 시작 후 클릭까지 소요시간
	private List<float> results = new List<float>();

	private void Start()
	{
		camera = Camera.main;

		camera.backgroundColor = colors[(int)gameStatus];
		messageText.text = string.Format(messages[(int)gameStatus], maxRound);
	}

	private void Update()
	{
		switch (gameStatus)
		{
			case GameStatus.Title:
				if (Input.GetMouseButtonDown(0))
				{
					currentRound = 0;
					results.Clear();

					gameStatus = GameStatus.Prepare;

					camera.backgroundColor = colors[(int)gameStatus];
					//messageText.text = messages[(int)gameStatus];
				}
				break;
			case GameStatus.Prepare:
				currentRound++;
				waitTime = Random.Range(1f, 5.5f);
				elapsedTime = 0f;

				gameStatus = GameStatus.Wait;

				camera.backgroundColor = colors[(int)gameStatus];
				messageText.text = messages[(int)gameStatus];
				break;
			case GameStatus.Wait:
				if (0 >= waitTime)
				{
					gameStatus = GameStatus.Record;

					camera.backgroundColor = colors[(int)gameStatus];
					messageText.text = messages[(int)gameStatus];
				}
				else
				{
					waitTime -= Time.deltaTime;
				}
				break;
			case GameStatus.Record:
				if (Input.GetMouseButtonDown(0))
				{
					elapsedTime *= 1000;
					results.Add(elapsedTime);

					waitTime = 0.1f;

					gameStatus = GameStatus.Result;

					camera.backgroundColor = colors[(int)gameStatus];
					messageText.text = string.Format(messages[(int)gameStatus], currentRound, maxRound, elapsedTime, (int)(resumeTime - waitTime) + 1);
				}
				else
				{
					elapsedTime += Time.deltaTime;
				}
				break;
			case GameStatus.Result:
				if (resumeTime <= waitTime)
				{
					if (currentRound == maxRound)
					{
						float averageTime = 0f;

						for (int i = 0; i < results.Count; i++)
						{
							averageTime += results[i];
						}
						averageTime /= results.Count;

						gameStatus = GameStatus.Finish;

						camera.backgroundColor = colors[(int)gameStatus];
						messageText.text = string.Format(messages[(int)gameStatus], maxRound, averageTime);
					}
					else
					{
						gameStatus = GameStatus.Prepare;
					}
				}
				else
				{
					messageText.text = string.Format(messages[(int)gameStatus], currentRound, maxRound, elapsedTime, (int)(resumeTime - waitTime) + 1);
					waitTime += Time.deltaTime;
				}
				break;
			case GameStatus.Finish:
				if (Input.GetMouseButtonDown(0))
				{
					gameStatus = GameStatus.Title;

					camera.backgroundColor = colors[(int)gameStatus];
					messageText.text = string.Format(messages[(int)gameStatus], maxRound);
				}
				break;
		}
	}
}
