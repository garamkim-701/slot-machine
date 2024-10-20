using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotController : MonoBehaviour
{
    public struct Paytable
    {
        public int five;
        public int four;
        public int three;
        public Paytable(int five_, int four_, int three_)
        {
            five = five_;
            four = four_; 
            three = three_;
        }
    }

    private Paytable[] paytables;
    private int[] paylines;
    private SpriteRenderer sr;
    private int row_count = 24;
    private int column_count = 5;
    private float[] y_positions;
    private float[] x_positions;
    private float localScaleX, localScaleY, iconSizeX, iconSizeY, iconLocalScaleX, iconLocalScaleY, paddingX, paddingY;
    private GameObject[,] prefabs;
    public GameObject prefab;
    public GameObject linesPopupPrefab;
    public GameObject framePrefab;

    private Sprite[] images;
    private RuntimeAnimatorController[] animatorControllers;

    private float timer = 0.0f;
    private bool isSpinning = false;
    private bool isInspecting = false;
    private bool[] stopCheck;

    private User user;
    public TMP_Text tabloText;
    private int bettingAmount;
    public AudioSource audio;
    public AudioClip coinSound;
    public AudioClip stopSpinSound;
    public AudioClip startSpinSound;

    void Awake()
    {
        images = new Sprite[8];
        animatorControllers = new RuntimeAnimatorController[8];
        user = GameObject.Find("User").GetComponent<User>();
   
        y_positions = new float[row_count];
        x_positions = new float[column_count];
        prefabs = new GameObject[row_count, column_count];
        stopCheck = new bool[column_count];

        for (int i = 0; i < 8; ++i)
        {
            images[i] = Resources.Load<Sprite>($"main_game/icon_{i + 1}") as Sprite;
            animatorControllers[i] = Resources.Load<RuntimeAnimatorController>($"animations/bluredIcons/IconAnimatorOverrideController{i + 1}");
        }

        sr = GetComponent<SpriteRenderer>();

        localScaleX = sr.transform.localScale.x;
        localScaleY = sr.transform.localScale.y;

        float positionY = GetComponent<Transform>().position.y;

        // icon's pixel per unit: 100, icon size 144*144, background pixel per unit: 256, background size 256
        iconSizeX = (8.0f / 46.0f);
        iconSizeY = (8.0f / 28.0f);
        iconLocalScaleX = iconSizeX * 100.0f / (144.0f);
        iconLocalScaleY = iconSizeY * 100.0f / (144.0f);

        paddingX = (1.0f / 46.0f) * 100.0f / 144.0f;
        paddingY = (1.0f / 28.0f) * 100.0f / 144.0f;

        for (int i = -2; i < column_count - 2; ++i)
        {
            if (i < 0)
            {
                x_positions[i + 2] = localScaleX * (i * (iconSizeX + paddingX));
            }
            else if (i == 0)
            {
                x_positions[i + 2] = 0;
            }
            else
            {
                x_positions[i + 2] = localScaleX * (i * (iconSizeX + paddingX));
            }
        }

        for (int i = -1; i < row_count - 1; ++i)
        {
            if (i < 0)
            {
                y_positions[i + 1] = positionY + localScaleY * (i * (iconSizeY + paddingY));
            }
            else if (i == 0)
            {
                y_positions[i + 1] = positionY;
            }
            else
            {
                y_positions[i + 1] = positionY + localScaleY * (i * (iconSizeY + paddingY));
            }
        }

        for (int i = 0; i < row_count; ++i)
        {
            for (int j = 0; j < column_count; ++j)
            {
                prefabs[i, j] = Instantiate(prefab, new Vector3(x_positions[j], y_positions[i], sr.transform.position.z - 1), sr.transform.rotation, sr.transform);
                prefabs[i, j].GetComponent<Icon>().resize(iconLocalScaleX, iconLocalScaleY);
                prefabs[i, j].GetComponent<Icon>().setIndex(j, i);
            }
        }

        initializePaytable();
        initializePaylines();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            timer += Time.deltaTime;
        }

        if (timer > 5.0f)
        {
            
        }
    }

    private void initializePaytable()
    {
        paytables = new Paytable[8];
        paytables[0] = new Paytable(40, 15, 8);
        paytables[1] = new Paytable(7, 3, 1);
        paytables[2] = new Paytable(60, 20, 10);
        paytables[3] = new Paytable(15, 8, 4);
        paytables[4] = new Paytable(10, 4, 2);
        paytables[5] = new Paytable(7, 3, 1);
        paytables[6] = new Paytable(10, 4, 2);
        paytables[7] = new Paytable(60, 20, 10);
    }

    private void initializePaylines()
    {
        paylines = new int[9];
        paylines[0] = (int)(0 + 0 + Mathf.Pow(3,2) * 1 + 0 + 0);
        paylines[1] = (int)(2 + Mathf.Pow(3, 1) * 2 + Mathf.Pow(3, 2) * 1 + Mathf.Pow(3, 3) * 2 + Mathf.Pow(3, 4) * 2);
        paylines[2] = (int)(1 + Mathf.Pow(3, 1) * 0 + Mathf.Pow(3, 2) * 0 + Mathf.Pow(3, 3) * 0 + Mathf.Pow(3, 4) * 1);
        paylines[3] = (int)(1 + Mathf.Pow(3, 1) * 2 + Mathf.Pow(3, 2) * 2 + Mathf.Pow(3, 3) * 2 + Mathf.Pow(3, 4) * 1);
        paylines[4] = (int)(2 + Mathf.Pow(3, 1) * 2 + Mathf.Pow(3, 2) * 2 + Mathf.Pow(3, 3) * 2 + Mathf.Pow(3, 4) * 2);
        paylines[5] = (int)(1 + Mathf.Pow(3, 1) * 1 + Mathf.Pow(3, 2) * 1 + Mathf.Pow(3, 3) * 1 + Mathf.Pow(3, 4) * 1);
        paylines[6] = (int)(0 + Mathf.Pow(3, 1) * 0 + Mathf.Pow(3, 2) * 0 + Mathf.Pow(3, 3) * 0 + Mathf.Pow(3, 4) * 0);
        paylines[7] = (int)(0 + Mathf.Pow(3, 1) * 1 + Mathf.Pow(3, 2) * 2 + Mathf.Pow(3, 3) * 1 + Mathf.Pow(3, 4) * 0);
        paylines[8] = (int)(2 + Mathf.Pow(3, 1) * 1 + Mathf.Pow(3, 2) * 0 + Mathf.Pow(3, 3) * 1 + Mathf.Pow(3, 4) * 2);
    }

    public Sprite getIconSprite(int imageNumber)
    {
        return images[imageNumber - 1];
    }

    public RuntimeAnimatorController getIconAnimatorController(int imageNumber)
    {
        return animatorControllers[imageNumber - 1];
    }

    public void Spin()
    {
        if (timer == 0.0f)
        {
            if (user.pay())
            {
                audio.PlayOneShot(startSpinSound);
                isSpinning = true;
                bettingAmount = user.getBettingAmount();
                for (int i = 0; i < row_count; ++i)
                {
                    for (int j = 0; j < column_count; ++j)
                    {
                        prefabs[i, j].GetComponent<Icon>().setIsSpinning(true);
                    }
                }
            }
        }
    }

    public void createNewIcon(int x, int y)
    {
        float highest_y_locations;
        if (y == 0)
        {
            if (isStopping(timer, x))
            {
                if(x != 0)
                {
                    stopCheck[x] = true;

                    for( int i =0; i < x; ++i)
                    {
                        if (stopCheck[x - 1])
                        {
                            stopCheck[x] = false;
                        }
                    }
                } else
                {
                    stopCheck[x] = true;
                }
            }
            highest_y_locations = prefabs[row_count - 1, x].GetComponent<Transform>().position.y;
        }
        else
        {
            highest_y_locations = prefabs[y - 1, x].GetComponent<Transform>().position.y;
        }
        prefabs[y, x] = Instantiate(prefab, new Vector3(x_positions[x], highest_y_locations + localScaleY * (paddingY + iconSizeY), sr.transform.position.z - 1), sr.transform.rotation, sr.transform);
        prefabs[y, x].GetComponent<Icon>().setIndex(x, y);
        prefabs[y, x].GetComponent<Icon>().resize(iconLocalScaleX, iconLocalScaleY);
        
        if (stopCheck[x])
        {
            prefabs[y, x].GetComponent<Icon>().setDestination(new Vector3(x_positions[x], y_positions[y], sr.transform.position.z - 1));
            if(y == row_count -1)
            {
                stopCheck[x] = false;
                audio.PlayOneShot(stopSpinSound);
                if (x == column_count - 1)
                {
                    isInspecting = true;
                    inspect();
                }
            }
        } else
        {
            prefabs[y, x].GetComponent<Icon>().setIsSpinning(true);
        }
    }

    private bool isStopping(float timer, int x)
    {
        return timer > (float)x;
    }

    public void showLinesPopup()
    {
        Instantiate(linesPopupPrefab);
    }

    private void inspect()
    {
        int lineCount = 0;

        for(int i=0; i < 9; ++i)
        {
            int[] iconCount = new int[8];
            int[] y_indexes = parsePaylines(i);
            List<int> effectTarget = new List<int>();
            for(int j =0; j<5; ++j)
            {
                iconCount[prefabs[y_indexes[j], j].GetComponent<Icon>().getImageNumber() - 1]++;
            }
            
            for(int iconNumber = 0; iconNumber <8; ++iconNumber)
            {
                if (iconCount[iconNumber] >= 3) {
                    for(int j =0; j <5; ++j)
                    {
                        if(prefabs[y_indexes[j], j].GetComponent<Icon>().getImageNumber() -1 == iconNumber)
                        {
                            effectTarget.Add(y_indexes[j]);
                        } else
                        {
                            effectTarget.Add(-1);
                        }
                    }

                    StartCoroutine(turnOnEffect(lineCount * 2.0f, i+1, effectTarget, getPaytable(iconNumber, iconCount[iconNumber])));
                    lineCount++;
                }
            }
        }

        initializeIcons();
        StartCoroutine(resetTimer(lineCount * 2.0f));
    }

    private int[] parsePaylines(int number)
    {
        int[] y_indexes = new int[5];
        int temp = paylines[number];
        for (int i = column_count - 1; i >= 0; --i)
        {
            y_indexes[i] = temp / (int)Mathf.Pow(3, i);
            temp -= y_indexes[i] * (int)Mathf.Pow(3, i);
        }
       
        return y_indexes;
    }

    private IEnumerator turnOnEffect(float waitTime, int lineNumber, List<int> y_indexes, int multiplier)
    {
        List<GameObject> framePrefabs = new List<GameObject>();
        GameObject frameTemp;
        yield return new WaitForSecondsRealtime(waitTime);
        GameObject.Find($"line{lineNumber}").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"main_game/ui/{lineNumber}") as Sprite;
        for (int i=0; i<5; ++i)
        {
            if (y_indexes[i] >= 0)
            {
                frameTemp = Instantiate(framePrefab, new Vector3(x_positions[i], y_positions[y_indexes[i]], sr.transform.position.z - 1), sr.transform.rotation, sr.transform);
                framePrefabs.Add(frameTemp);
                frameTemp.GetComponent<Frame>().resize(iconLocalScaleX, iconLocalScaleY);
            }   
        }

        int reward = multiplier * bettingAmount / 9;
        user.earn(reward);
        tabloText.text = $"Win {string.Format("{0: #,###; -#,###;0}", reward)}!";
        audio.PlayOneShot(coinSound);
        yield return new WaitForSecondsRealtime(2f);
        GameObject.Find($"line{lineNumber}").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"main_game/ui/{lineNumber}_inactive") as Sprite;
        while(framePrefabs.Count > 0)
        {
            Destroy(framePrefabs[0]);
            framePrefabs.RemoveAt(0);
        }
    }

    private IEnumerator resetTimer(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        isSpinning = false;
        timer = 0.0f;
        tabloText.text = "Good Luck!";
    }

    private int getPaytable(int iconNumber, int iconCount)
    {
        switch(iconCount)
        {
            case 3:
                return paytables[iconNumber].three;
            case 4:
                return paytables[iconNumber].four;
            case 5:
                return paytables[iconNumber].five;
            default:
                return 0;
        }
    }

    private void initializeIcons()
    {
        for (int i = 0; i < row_count; ++i)
        {
            for (int j = 0; j < column_count; ++j)
            {
                prefabs[i, j].GetComponent<Icon>().setIsSpinning(false);
                prefabs[i, j].GetComponent<Icon>().setDestination(Vector3.zero);
            }
        }
    }

    public void quit()
    {
        Application.Quit();
    }
}
