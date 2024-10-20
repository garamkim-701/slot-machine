using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class User : MonoBehaviour
{
    private int balance = 1000000;
    private int bettingAmount = 9000;
    private int showingBalance = 0;
    private int showingBettingAmount = 0;

    public TMP_Text bettingText;
    public TMP_Text balanceText;

    void Awake()
    {
        bool isInit = !PlayerPrefs.HasKey("balance");
        if(isInit)
        {
            PlayerPrefs.SetInt("balance", balance);
            PlayerPrefs.Save();
        } else
        {
            balance = PlayerPrefs.GetInt("balance");
        }
        
        //betting = GameObject.Find("UI").gameObject.transform.Find("Betting").GetComponent<Transform>.Find("Amount").gameObject.GetComponent<Text>();
        //balance = GameObject.Find("Balance").GetComponent<Transform>.Find("Amount").gameObject.GetComponent<Text>();
    }

    public bool pay()
    {
        if(bettingAmount > balance)
        {
            return false;
        } else
        {
            balance -= bettingAmount;
            PlayerPrefs.SetInt("balance", balance);
            PlayerPrefs.Save();
            return true;
        }
    }

    public void earn(int delta)
    {
        balance += delta;
        PlayerPrefs.SetInt("balance", balance);
        PlayerPrefs.Save();
    }

    public int getBalance()
    {
        return balance;
    }

    public int getBettingAmount()
    {
        return bettingAmount;
    }

    void Update()
    {
        if(showingBalance != balance)
        {
            if(showingBalance < balance)
            {
                showingBalance = (int)Mathf.Min(showingBalance + (balance) * (Time.deltaTime / 2.0f), (float)balance);
            } else
            {
                showingBalance = (int)Mathf.Max(showingBalance - (balance) * (Time.deltaTime / 2.0f), (float)balance);
            }
            balanceText.text = string.Format("{0: #,###; -#,###;0}", showingBalance);
        }

        if (showingBettingAmount != bettingAmount)
        {
            if(showingBettingAmount < bettingAmount)
            {
                showingBettingAmount = (int)Mathf.Min(showingBettingAmount + (bettingAmount) * (Time.deltaTime / 2.0f), (float)bettingAmount);
            } else
            {
                showingBettingAmount = (int)Mathf.Max(showingBettingAmount - (bettingAmount) * (Time.deltaTime / 2.0f), (float)bettingAmount);
            }
           bettingText.text = string.Format("{0: #,###; -#,###;0}", showingBettingAmount);
        }
    }

    public void decreaseBettingAmount()
    {
        if(bettingAmount > 9000)
        {
            bettingAmount /= 2;
        }
    }

    public void incraseBettingAmount()
    {
        if(bettingAmount < 640000)
        {
            bettingAmount *= 2;
        }
    }
}
