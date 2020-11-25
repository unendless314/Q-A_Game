using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReviewCellHandler : MonoBehaviour
{
    public Button expandButton;
    public Button hideButton;

    public Text questionContentsText;
    public Text[] itemsText_Array;
    public Image[] itemsBGImage_Array;
    public Text categoryText;


    public Image favoriteImage;
    public Image recordsImage;

    public Sprite[] heartsSprites_Array;
    public Sprite[] answerRecordSprites_Array;

    public void SetCellContent(Question3 aData, int questionNumber, int answerNumber, int choiceNumber)
    {
        questionContentsText.text = "題目" + questionNumber + ": " + aData.s_topic;
        categoryText.text = ShowCategoryText(aData.i_category);

        itemsText_Array[0].text = "(A) " + aData.s_answer;
        itemsText_Array[1].text = "(B) " + aData.s_item1;
        itemsText_Array[2].text = "(C) " + aData.s_item2;
        itemsText_Array[3].text = "(D) " + aData.s_item3;

        for (int i = 0; i < itemsBGImage_Array.Length; i++)
        {
            itemsBGImage_Array[i].color = new Color();
        }

        if (answerNumber == choiceNumber)
        {
            recordsImage.sprite = answerRecordSprites_Array[1];
            recordsImage.color = new Color(1, 0.8061391f, 0.03301889f, 1);

            favoriteImage.sprite = heartsSprites_Array[1];
        }
        else
        {
            recordsImage.sprite = answerRecordSprites_Array[0];
            recordsImage.color = new Color(1, 0.03137255f, 0.6769125f, 1);

            favoriteImage.sprite = heartsSprites_Array[0];
        }

        if (choiceNumber < 4)
        {
            itemsBGImage_Array[choiceNumber].color = Color.red;
            itemsBGImage_Array[answerNumber].color = Color.green;
        }
        else
        {
            itemsBGImage_Array[answerNumber].color = Color.green;
        }
    }

    public string ShowCategoryText(int categoryNumber)
    {
        switch (categoryNumber)
        {
            case 1:
            case 2:
            case 3:
                return "單字";
            case 4:
            case 5:
                return "文法";
            case 6:
            case 7:
                return "翻譯";
            case 8:
                return "聽力";
            case 9:
            case 10:
            case 11:
                return "閱讀";
            case 12:
                return "克漏字";
            default:
                return "其他類別";
        }
    }

    public void ChangeFavorites()
    {
        if (favoriteImage.sprite == heartsSprites_Array[0])
        {
            favoriteImage.sprite = heartsSprites_Array[1];
        }
        else
        {
            favoriteImage.sprite = heartsSprites_Array[0];
        }
    }
}
