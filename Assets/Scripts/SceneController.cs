using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
  [SerializeField] private AudioSource audioSource;
  [SerializeField] private AudioClip matchSound;
  
  public const int gridRows = 2;
  public const int gridCols = 4;
  public const float offsetX = 1f;
  public const float offsetY = 2f;

  public static int TotalScore = 0;

  [SerializeField] private MainCard originalCard;
  [SerializeField] private Sprite[] images;

  private void Start()
  {
    // Get position of original card
    Vector3 startPos = originalCard.transform.position;

    int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 };

    // Function
    numbers = ShuffleArray(numbers);

    for(int i = 0; i < gridCols; i++)
    {
        for(int j = 0; j < gridRows; j++)
        {
            MainCard card;
            if(i == 0 && j == 0)
            {
                card = originalCard;
            }
            else
            {
                card = Instantiate(originalCard) as MainCard;
            }

            int index = j * gridCols + i;
            int id = numbers[index];
            card.ChangeSprite(id, images[id]);

            float posX = (offsetX * i) + startPos.x;
            float posY = (offsetY * j) + startPos.y;
            card.transform.position = new Vector3(posX, posY, startPos.z);
        }
    }

    scoreLabel.text = "Score: " + TotalScore;

  }

  private int[] ShuffleArray(int[] numbers)
  {
    int[] newArray = numbers.Clone() as int[];
    for(int i = 0; i < newArray.Length; i++)
    {
        int tmp = newArray[i];
        int r = Random.Range(i, newArray.Length);
        newArray[i] = newArray[r];
        newArray[r] = tmp;
    }
    return newArray;
  }

  private MainCard _firstRevealed;
  private MainCard _secondRevealed;
  private int _score = 0;
  [SerializeField] private TextMesh scoreLabel;

  public bool canReveal
  {
    get{return _secondRevealed == null;}
  }

  public void CardRevealed(MainCard card)
  {
    if(_firstRevealed == null)
    {
        _firstRevealed = card;
    }
    else
    {
        _secondRevealed = card;
        StartCoroutine(CheckMatch());
    }

  }

  private void PlayMatchSound()
  {
    if(audioSource != null && matchSound != null)
    {
       audioSource.PlayOneShot(matchSound);
    }
  }

  private IEnumerator CheckMatch()
  {
    if(_firstRevealed.id == _secondRevealed.id)
    {
        _score++;
        TotalScore++;
        scoreLabel.text = "Score: " + TotalScore;
        PlayMatchSound();
    }
    else
    {
        yield return new WaitForSeconds(0.5f);
        _firstRevealed.Unreveal();
        _secondRevealed.Unreveal();
    }
    _firstRevealed = null;
    _secondRevealed = null;
  }

  public void Restart()
  {
    SceneManager.LoadScene("Pairs");
  }
}
