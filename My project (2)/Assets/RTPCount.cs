using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RTPCount : MonoBehaviour
{


    public int bakiye = 2000000;
    private float gameWin;

    private float randomNumber;
    int girilenBet = 2;

    private float toplamKazanc = 0f;
    private float toplamBahis = 0f;

    private int kereOynandi = 0;
    private int kereKazandi = 0;
    private int kereKaybetti = 0;
    //private int ikiVeucDegil = 0 ;



    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameStart();
        }
    }


    public void GameStart()
    {
        for (int i = 0; i < 1000000; i++)
        {
            kereOynandi++;
            girilenBet = 2;
            bakiye -= girilenBet;
            toplamBahis += girilenBet;

            randomNumber = GenerateCrashNumber();
            randomNumber = Mathf.Round(randomNumber * 100f) / 100f;

            CarpanControl(randomNumber);
        }

        // RTP Hesaplama
        float RTP = (toplamKazanc/ toplamBahis) * 100f;
        Debug.Log("RTP: " + RTP + "%" + " ToplamBahis: " + toplamBahis + " ToplamKazanc: " + toplamKazanc);

        Debug.Log(kereOynandi + " Game - " + kereKazandi + " Win -" + kereKaybetti + " Lose  ");
    }


    public void CarpanControl(float targetNumber)
    {
        //float currentNumber = 2f;

        if (targetNumber >= 2f)
        {
            gameWin = (targetNumber * girilenBet);
            bakiye += (int)gameWin;
            kereKazandi++;
            toplamKazanc += (int)gameWin;
        }
        else if (targetNumber < 2f)
        {
            kereKaybetti++;
        }
    }


    private float GenerateCrashNumber()
    {
        int ucuyozX = Random.Range(0, 100);

        if (ucuyozX < 3) // %3 ihtimalle büyük kazanç
        {
            float randomValue = Random.Range(1.0f, 2.0f); // Maks 24, min 10
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




}
