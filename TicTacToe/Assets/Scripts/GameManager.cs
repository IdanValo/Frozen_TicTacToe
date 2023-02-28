using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //canvas
    public GameObject gameBoardGroupLandscape;
    public GameObject mainMenuGroup;
    public GameObject gameOverGroup;
    public Text victorText;
    private bool botGame = true;
    private bool won = false;


    //Landscape
    public GameState[] board = new GameState[9];
    public int[] availableSquares;
    public bool xTurn = true;
    public Text turnIndicatorLandscape;

    public GameObject[] buttonsLandscape;
    public Image[] squaresLandscape;
    public Text[] squareTextsLandscape;

    public Sprite oImage;
    public Sprite xImage;
    private int botMove;
    private int moveCount = 0;

    public AudioSource winSound;

    public void ButtonClick(int squareIndex)
    {
        buttonsLandscape[squareIndex].SetActive(false);
        squaresLandscape[squareIndex].gameObject.SetActive(true);

        if (botGame)
        {
            if (xTurn)
            {
                squaresLandscape[squareIndex].sprite = xImage;
                squareTextsLandscape[squareIndex].text = "X";


                board[squareIndex] = GameState.Xcontrol;
                xTurn = false;
                turnIndicatorLandscape.text = "O's Turn";
            }
            moveCount++;
            CheckVictory();
            if (won == false)
            {
                StartCoroutine(BotMove());
            }



        }
        else
        {
            if (xTurn)
            {
                squaresLandscape[squareIndex].sprite = xImage;
                squareTextsLandscape[squareIndex].text = "X";


                board[squareIndex] = GameState.Xcontrol;
                xTurn = false;
                turnIndicatorLandscape.text = "O's Turn";
            }

            else
            {
                squaresLandscape[squareIndex].sprite = oImage;
                squareTextsLandscape[squareIndex].text = "O";


                board[squareIndex] = GameState.Ocontrol;
                xTurn = true;
                turnIndicatorLandscape.text = "X's Turn";

            }
        }
        moveCount++;
        CheckVictory();
    }

    public void BotClick(int Index)
    {
        buttonsLandscape[Index].SetActive(false);
        squaresLandscape[Index].gameObject.SetActive(true);

        squaresLandscape[Index].sprite = oImage;
        squareTextsLandscape[Index].text = "O";


        board[Index] = GameState.Ocontrol;
        xTurn = true;
        turnIndicatorLandscape.text = "Your Turn";

        CheckVictory();
    }

    public void NewGame()
    {
        won = false;
        xTurn = true;
        botGame = false;
        board = new GameState[9];
        turnIndicatorLandscape.text = "X's Turn";

        for (int i = 0; i < 9; i++)
        {
            buttonsLandscape[i].SetActive(true);
            squaresLandscape[i].gameObject.SetActive(false);
            board[i] = GameState.Clear;
            squareTextsLandscape[i].color = Color.white;
        }
        gameBoardGroupLandscape.SetActive(true);
        mainMenuGroup.SetActive(false);
    }

    public void BotGame()
    {
        won = false;
        xTurn = true;
        botGame = true;
        board = new GameState[9];
        turnIndicatorLandscape.text = "Your Turn";
        moveCount = 0;

        for (int i = 0; i < 9; i++)
        {
            buttonsLandscape[i].SetActive(true);
            squaresLandscape[i].gameObject.SetActive(false);
            board[i] = GameState.Clear;
            squareTextsLandscape[i].color = Color.white;
        }
        gameBoardGroupLandscape.SetActive(true);
        mainMenuGroup.SetActive(false);

    }

    public void CheckVictory()
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i] != GameState.Clear && board[i] == board[i + 3] && board[i] == board[i + 6])
            {
                StartCoroutine(ShowWin(i, i + 3, i + 6));
                return;
            }
            else if (board[i * 3] != GameState.Clear && board[i * 3] == board[(i * 3) + 1] && board[i * 3] == board[(i * 3) + 2])
            {
                StartCoroutine(ShowWin(i * 3, (i * 3) + 1, (i * 3) + 2));
                return;
            }
        }
        if (board[0] != GameState.Clear && board[0] == board[4] && board[0] == board[8])
        {
            StartCoroutine(ShowWin(0, 4, 8));
            return;
        }
        else if (board[2] != GameState.Clear && board[2] == board[4] && board[2] == board[6])
        {
            StartCoroutine(ShowWin(2, 4, 6));
            return;
        }

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == GameState.Clear)
                return;
        }
        SetWinner(GameState.Clear);
    }

    public IEnumerator ShowWin(int x, int y, int z)
    {
        winSound.PlayOneShot(winSound.clip);
        won = true;
        turnIndicatorLandscape.text = "Game Over!";
        squareTextsLandscape[x].color = Color.yellow;
        squareTextsLandscape[y].color = Color.yellow;
        squareTextsLandscape[z].color = Color.yellow;

        yield return new WaitForSeconds(2.0f);

        SetWinner(board[x]);

    }

    public void SetWinner(GameState toWin)
    {
        gameOverGroup.SetActive(true);
        gameBoardGroupLandscape.SetActive(false);

        if (toWin == GameState.Clear)
        {
            victorText.text = "Tie!";
        }
        else if (toWin == GameState.Xcontrol)
        {
            victorText.text = "X Wins!";
        }
        else
        {
            victorText.text = "O Wins!";
        }
    }

    public void BackToMainMenu()
    {
        gameOverGroup.SetActive(false);
        mainMenuGroup.SetActive(true);
    }

    public void getAvailable()
    {
        availableSquares = new int[9 - moveCount];
        int c = 0;
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == GameState.Clear)
            {
                availableSquares[c] = i;
                c++;
            }

        }
    }

    public IEnumerator BotMove()
    {
        int freeMove = 0;
        getAvailable();
        //Freeze(false);

        if (board[0] ==GameState.Ocontrol && board[1] == GameState.Ocontrol && board[2] == GameState.Clear)
        {
            botMove = 2;
        }
        else if (board[1] == GameState.Ocontrol && board[2] == GameState.Ocontrol && board[0] == GameState.Clear)
        {
            botMove = 0;
        }
        else if (board[0] == GameState.Ocontrol && board[2] == GameState.Ocontrol && board[1] == GameState.Clear)
        {
            botMove = 1;
        }
        else if (board[6] == GameState.Ocontrol && board[7] == GameState.Ocontrol && board[8] == GameState.Clear)
        {
            botMove = 8;
        }
        else if (board[7] == GameState.Ocontrol && board[8] == GameState.Ocontrol && board[6] == GameState.Clear)
        {
            botMove = 6;
        }
        else if (board[6] == GameState.Ocontrol && board[8] == GameState.Ocontrol && board[7] == GameState.Clear)
        {
            botMove = 7;
        }
        else if (board[0] == GameState.Ocontrol && board[4] == GameState.Ocontrol && board[8] == GameState.Clear)
        {
            botMove = 8;
        }
        else if (board[0] == GameState.Ocontrol && board[8] == GameState.Ocontrol && board[4] == GameState.Clear)
        {
            botMove = 4;
        }
        else if (board[4] == GameState.Ocontrol && board[8] == GameState.Ocontrol && board[0] == GameState.Clear)
        {
            botMove = 0;
        }
        else if (board[3] == GameState.Ocontrol && board[4] == GameState.Ocontrol && board[5] == GameState.Clear)
        {
            botMove = 5;
        }
        else if (board[3] == GameState.Ocontrol && board[5] == GameState.Ocontrol && board[4] == GameState.Clear)
        {
            botMove = 4;
        }
        else if (board[4] == GameState.Ocontrol && board[5] == GameState.Ocontrol && board[3] == GameState.Clear)
        {
            botMove = 3;
        }
        else if (board[0] == GameState.Ocontrol && board[3] == GameState.Ocontrol && board[6] == GameState.Clear)
        {
            botMove = 6;
        }
        else if (board[0] == GameState.Ocontrol && board[6] == GameState.Ocontrol && board[3] == GameState.Clear)
        {
            botMove = 3;
        }
        else if (board[3] == GameState.Ocontrol && board[6] == GameState.Ocontrol && board[0] == GameState.Clear)
        {
            botMove = 0;
        }
        else if (board[1] == GameState.Ocontrol && board[4] == GameState.Ocontrol && board[7] == GameState.Clear)
        {
            botMove = 7;
        }
        else if (board[1] == GameState.Ocontrol && board[7] == GameState.Ocontrol && board[4] == GameState.Clear)
        {
            botMove = 4;
        }
        else if (board[4] == GameState.Ocontrol && board[7] == GameState.Ocontrol && board[1] == GameState.Clear)
        {
            botMove = 1;
        }
        else if (board[2] == GameState.Ocontrol && board[5] == GameState.Ocontrol && board[8] == GameState.Clear)
        {
            botMove = 8;
        }
        else if (board[2] == GameState.Ocontrol && board[8] == GameState.Ocontrol && board[5] == GameState.Clear)
        {
            botMove = 5;
        }
        else if (board[5] == GameState.Ocontrol && board[8] == GameState.Ocontrol && board[2] == GameState.Clear)
        {
            botMove = 2;
        }
        else if (board[2] == GameState.Ocontrol && board[4] == GameState.Ocontrol && board[6] == GameState.Clear)
        {
            botMove = 6;
        }
        else if (board[2] == GameState.Ocontrol && board[6] == GameState.Ocontrol && board[4] == GameState.Clear)
        {
            botMove = 4;
        }
        else if (board[4] == GameState.Ocontrol && board[6] == GameState.Ocontrol && board[2] == GameState.Clear)
        {
            botMove = 2;
        }
        else if (board[0] == GameState.Xcontrol && board[1] == GameState.Xcontrol && board[2] == GameState.Clear)
        {
            botMove = 2;
        }
        else if (board[1] == GameState.Xcontrol && board[2] == GameState.Xcontrol && board[0] == GameState.Clear)
        {
            botMove = 0;
        }
        else if (board[0] == GameState.Xcontrol && board[2] == GameState.Xcontrol && board[1] == GameState.Clear)
        {
            botMove = 1;
        }
        else if (board[6] == GameState.Xcontrol && board[7] == GameState.Xcontrol && board[8] == GameState.Clear)
        {
            botMove = 8;
        }
        else if (board[7] == GameState.Xcontrol && board[8] == GameState.Xcontrol && board[6] == GameState.Clear)
        {
            botMove = 6;
        }
        else if (board[6] == GameState.Xcontrol && board[8] == GameState.Xcontrol && board[7] == GameState.Clear)
        {
            botMove = 7;
        }
        else if (board[0] == GameState.Xcontrol && board[4] == GameState.Xcontrol && board[8] == GameState.Clear)
        {
            botMove = 8;
        }
        else if (board[0] == GameState.Xcontrol && board[8] == GameState.Xcontrol && board[4] == GameState.Clear)
        {
            botMove = 4;
        }
        else if (board[4] == GameState.Xcontrol && board[8] == GameState.Xcontrol && board[0] == GameState.Clear)
        {
            botMove = 0;
        }
        else if (board[3] == GameState.Xcontrol && board[4] == GameState.Xcontrol && board[5] == GameState.Clear)
        {
            botMove = 5;
        }
        else if (board[3] == GameState.Xcontrol && board[5] == GameState.Xcontrol && board[4] == GameState.Clear)
        {
            botMove = 4;
        }
        else if (board[4] == GameState.Xcontrol && board[5] == GameState.Xcontrol && board[3] == GameState.Clear)
        {
            botMove = 3;
        }
        else if (board[0] == GameState.Xcontrol && board[3] == GameState.Xcontrol && board[6] == GameState.Clear)
        {
            botMove = 6;
        }
        else if (board[0] == GameState.Xcontrol && board[6] == GameState.Xcontrol && board[3] == GameState.Clear)
        {
            botMove = 3;
        }
        else if (board[3] == GameState.Xcontrol && board[6] == GameState.Xcontrol && board[0] == GameState.Clear)
        {
            botMove = 0;
        }
        else if (board[1] == GameState.Xcontrol && board[4] == GameState.Xcontrol && board[7] == GameState.Clear)
        {
            botMove = 7;
        }
        else if (board[1] == GameState.Xcontrol && board[7] == GameState.Xcontrol && board[4] == GameState.Clear)
        {
            botMove = 4;
        }
        else if (board[4] == GameState.Xcontrol && board[7] == GameState.Xcontrol && board[1] == GameState.Clear)
        {
            botMove = 1;
        }
        else if (board[2] == GameState.Xcontrol && board[5] == GameState.Xcontrol && board[8] == GameState.Clear)
        {
            botMove = 8;
        }
        else if (board[2] == GameState.Xcontrol && board[8] == GameState.Xcontrol && board[5] == GameState.Clear)
        {
            botMove = 5;
        }
        else if (board[5] == GameState.Xcontrol && board[8] == GameState.Xcontrol && board[2] == GameState.Clear)
        {
            botMove = 2;
        }
        else if (board[2] == GameState.Xcontrol && board[4] == GameState.Xcontrol && board[6] == GameState.Clear)
        {
            botMove = 6;
        }
        else if (board[2] == GameState.Xcontrol && board[6] == GameState.Xcontrol && board[4] == GameState.Clear)
        {
            botMove = 4;
        }
        else if (board[4] == GameState.Xcontrol && board[6] == GameState.Xcontrol && board[2] == GameState.Clear)
        {
            botMove = 2;
        }
        else
        {
            botMove = Random.Range(0, availableSquares.Length);
            freeMove = 1;
        }

        yield return new WaitForSeconds(0.3f);

        if (freeMove > 0)
        {
            BotClick(availableSquares[botMove]);
        }
        else
        {
            BotClick(botMove);
        }
        getAvailable();
    }
}
