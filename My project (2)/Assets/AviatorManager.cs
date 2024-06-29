using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AviatorManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI carpan;
    [SerializeField] GameObject baslat;

    [SerializeField] TextMeshProUGUI balanceText;
    [SerializeField] TextMeshProUGUI BetTmp;
    [SerializeField] GameObject gozukenBet;

    [SerializeField] private GameObject textContainer; // Yeni Text elemanlarýný eklemek için bir container
    [SerializeField] private GameObject textPrefab; // Text prefab
    [SerializeField] private GameObject Plane;

    public TextMeshProUGUI betCarpan;

    public int bakiye = 0;
    private float gameWin;

    public MovePlane MovePlane;

    private bool AviatorStarted;

    int bahis = 20;
    int minBahis = 2;
    int maxBahis = 20000;
    int bahisArtisMiktari = 20;
    private float randomNumber;
    int girilenBet = 2;

    private void Start()
    {
        AviatorStarted = false;
        balanceText.text = bakiye.ToString();

        // TextContainer'a Mask ve RectMask2D komponenti ekle
        if (!textContainer.GetComponent<Mask>())
        {
            textContainer.AddComponent<Mask>().showMaskGraphic = false;
        }

        if (!textContainer.GetComponent<RectMask2D>())
        {
            textContainer.AddComponent<RectMask2D>();
        }
    }



    public void ChangeBetAmount(bool isIncrease)
    {
        if (isIncrease)
        {
            if (bahis == maxBahis)
                return;

            if (bahis == 2)
                bahis = 5;
            else if (bahis == 5)
                bahis = 10;
            else if (bahis == 10)
                bahis = 20;
            else if (bahis >= 5000)
            {
                bahis += 1000;
            }
            else if (bahis >= 2000)
            {
                bahis += 400;
            }
            else if (bahis >= 500)
            {
                bahis += 50;
            }
            else
            {
                bahis += bahisArtisMiktari;
            }

            BetTmp.text = bahis.ToString();
        }
        else
        {
            if (bahis == minBahis)
                return;

            if (bahis == 20)
                bahis = 10;
            else if (bahis == 10)
                bahis = 5;
            else if (bahis == 5)
                bahis = 2;
            else if (bahis > 5000)
            {
                bahis -= 1000;
            }
            else if (bahis > 2000)
            {
                bahis -= 400;
            }
            else if (bahis > 500)
            {
                bahis -= 50;
            }
            else
            {
                bahis -= bahisArtisMiktari;
            }

            BetTmp.text = bahis.ToString();
        }
    }

    public void GameStart()
    {
        if (bakiye < girilenBet)
            return;
        AviatorStarted = true; //oyun basladi
        girilenBet = int.Parse(BetTmp.text);
        balanceText.text = (bakiye - girilenBet).ToString();
        bakiye = bakiye - girilenBet;

        randomNumber = GenerateCrashNumber();
        randomNumber = Mathf.Round(randomNumber * 100f) / 100f;
        Debug.Log("bolunmus: " + (randomNumber + 1));

        if (randomNumber > 0f)
        {
            carpan.color = Color.white;
        }

        StartCoroutine(UpdateTextToRandomNumber(randomNumber));

    }

    public void GameEnd()
    {
        if (AviatorStarted == true)
        {
            AviatorStarted = false;
        }
        else
            AviatorStarted = false;
    }

    public IEnumerator UpdateTextToRandomNumber(float targetNumber)
    {
        float currentNumber = 1f;

        while (currentNumber - 1 < targetNumber)
        {
            if (AviatorStarted == false)
                break;
            currentNumber += 0.01f;
            betCarpan.text = (currentNumber * girilenBet).ToString("F1");
            gameWin = (currentNumber * girilenBet);
            carpan.text = currentNumber.ToString("F2") + "x";
            if (currentNumber < 1.1)
            {
                yield return new WaitForSeconds(0.2f);
            }
            else if (currentNumber < 1.2)
            {
                yield return new WaitForSeconds(0.15f);
            }
            else if (currentNumber < 1.4)
            {
                yield return new WaitForSeconds(0.12f);
            }
            else if (currentNumber < 1.5)
            {
                yield return new WaitForSeconds(0.1f);
            }
            else if (currentNumber < 1.6)
            {
                yield return new WaitForSeconds(0.09f);
            }
            else if (currentNumber < 1.8)
            {
                yield return new WaitForSeconds(0.08f);
            }
            else if (currentNumber < 2)
            {
                yield return new WaitForSeconds(0.07f);
            }
            else if (currentNumber < 2.1)
            {
                yield return new WaitForSeconds(0.06f);
            }
            else if (currentNumber < 2.2)
            {
                yield return new WaitForSeconds(0.05f);
            }
            else if (currentNumber < 2.3)
            {
                yield return new WaitForSeconds(0.04f);
            }
            else if (currentNumber < 2.5)
            {
                yield return new WaitForSeconds(0.03f);
            }
            else if (currentNumber < 2.7)
            {
                yield return new WaitForSeconds(0.02f);
            }
            else if (currentNumber < 2.9)
            {
                yield return new WaitForSeconds(0.015f);
            }
            else if (currentNumber < 3)
            {
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                yield return new WaitForSeconds(0.01f);
            }
        }


        gozukenBet.SetActive(false);
        baslat.SetActive(true);
        MovePlane.StopMoving();
        AviatorStarted = false;
        Debug.Log(AviatorStarted);

        if (targetNumber < 2f)
        {
            carpan.color = Color.red;
        }
        

        if (currentNumber - 1 < targetNumber)
        {
            bakiye = bakiye + (int)gameWin;
            balanceText.text = bakiye.ToString();
            Debug.Log(balanceText.text);
        }
        AddNewTextElement(targetNumber + 1);
        carpan.text = (targetNumber + 1).ToString();

        //Debug.Log("kirmizi; " + kirmiziSayac + "  yesil; " + yesilSayac);
    }

    private float GenerateCrashNumber()
    {
        int ucuyozX = Random.Range(0, 100);

        if (ucuyozX < 3) // %3 ihtimalle büyük kazanç
        {
            float randomValue = Random.Range(1.0f, 2.0f); // Maks 25, min 10
            float scaledValue = Mathf.Pow(randomValue, 3) * 3f;
            return scaledValue;
        }
        else if (ucuyozX >= 3 && ucuyozX < 30) // %27 ihtimalle orta kazanç
        {
            float randomValue = Random.Range(0.8f, 2f); // Maks 6, min 1.96
            float scaledValue = Mathf.Pow(randomValue, 2) * 1.5f;
            return scaledValue;
        }
        else // %70 ihtimalle küçük kazanç veya kayýp
        {
            float randomValue = Random.Range(0.2f, 1f); // Maks 2, min 1.04
            float scaledValue = Mathf.Pow(randomValue, 2) * 1f;
            return scaledValue;
        }
    }


    private void AddNewTextElement(float value)
    {
        // Yeni bir Text elemaný oluþtur
        GameObject newTextObject = Instantiate(textPrefab, textContainer.transform);
        TextMeshProUGUI newText = newTextObject.GetComponent<TextMeshProUGUI>();
        newText.text = value.ToString("F2");

        // Renk belirle
        if (value < 2)
        {
            newText.color = Color.red; // Deðer 2'nin altýndaysa kýrmýzý
        }
        else
        {
            newText.color = Color.green; // Deðer 2'nin üstündeyse yeþil
        }

        // Eski Text elemanlarýný saða kaydýr
        foreach (Transform child in textContainer.transform)
        {
            child.localPosition += new Vector3(100, 0, 0); // Her bir Text elemanýný saða kaydýr
        }

        // Yeni Text elemanýný sol tarafa yerleþtir
        newTextObject.transform.localPosition = new Vector3(0, 0, 0);

        // Ekranda görünen alanýn dýþýna çýkmýþ olanlarý gizle
        newTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, textContainer.GetComponent<RectTransform>().rect.height);
    }


}