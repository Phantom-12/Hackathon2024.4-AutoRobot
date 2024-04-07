using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SliderController : MonoBehaviour
{

    // [System.Serializable]
    // struct GlobalFillerInfo
    // {
    //     [SerializeField]
    //     public OperationType operation;
    //     [SerializeField]
    //     [Tooltip("填充区域的sprite")]
    //     public Sprite fillerSprite;
    //     [SerializeField]
    //     [Tooltip("指示器的sprite")]
    //     public Sprite indicatorSprite;
    //     [SerializeField]
    //     [Tooltip("填充区域的颜色")]
    //     public Color color;
    // }

    [SerializeField]
    GlobalFillerInfo globalFillerInfoData;

    [System.Serializable]
    struct FillerInfo
    {
        [SerializeField]
        public float length;
        [SerializeField]
        public OperationType operation;
    }

    [SerializeField]
    string rawString;
    [SerializeField]
    FillerInfo[] fillerInfos;
    [SerializeField]
    GameObject fillerPrefab;
    GameObject[] fillers;
    [SerializeField]
    GameObject indicatorPrefab;
    GameObject[] indicators;
    [SerializeField]
    GameObject zeroPrefab, onePrefab;
    GameObject[] zeroOnes;

    Dictionary<float, OperationType> opList;

    RectTransform rawStringAreaRect;
    RectTransform fillingAreaRect;
    RectTransform indicatorAreaRect;
    RectTransform pointerRect;
    float pointerWidth;
    bool pointerMovingRight;

    PlayerInputHandler inputHandler;

    bool inverted = false, replayed = false;
    [SerializeField]
    bool start;

    [SerializeField]
    float pointerMoveSpeed;

    OperationType lastOp = OperationType.Die;
    double lastPressTime;
    // readonly double maxProtectTime=0.2f;

    void Awake()
    {
        rawStringAreaRect = transform.Find("Raw String Area").gameObject.GetComponent<RectTransform>();
        fillingAreaRect = transform.Find("Fill Area").gameObject.GetComponent<RectTransform>();
        indicatorAreaRect = transform.Find("Indicator Area").gameObject.GetComponent<RectTransform>();
        pointerRect = transform.Find("Pointer").gameObject.GetComponent<RectTransform>();
        inputHandler = FindObjectOfType<PlayerInputHandler>();
        pointerWidth = pointerRect.rect.width;
        opList = new Dictionary<float, OperationType>();

        globalFillerInfoData.CreateDic();

        InitZeroOnes();
        InitFillers();
        InitPointer();
        UpdateInstruction("", "", "", "", "");
        UpdateFillerAndIndicator();
    }

    void Start()
    {
    }

    void Update()
    {
        //UpdateFiller();
        //CheckSpaceDoubleTap();
        if (start)
        {
            UpdatePointer();
            CheckAndApplyOperation();
        }
        // CheckSpacePress();
    }

    void InitZeroOnes()
    {
        zeroOnes = new GameObject[rawString.Length];
        for (int i = 0; i < rawString.Length; i++)
        {
            if (rawString[i] == '0')
                zeroOnes[i] = Instantiate(zeroPrefab, rawStringAreaRect);
            else
                zeroOnes[i] = Instantiate(onePrefab, rawStringAreaRect);
            zeroOnes[i].GetComponent<UnityEngine.UI.Image>().preserveAspect = true;
        }
        UpdateZeroOnes();
    }

    void UpdateZeroOnes()
    {
        float width = rawStringAreaRect.rect.width;
        float height = rawStringAreaRect.rect.height;
        float sumLen = rawString.Length;
        float nowX = 0;
        for (int i = 0; i < rawString.Length; i++)
        {
            float zeroOneWidth = width / sumLen;
            float zeroOneHeight = height;
            zeroOnes[i].GetComponent<RectTransform>().anchoredPosition = new Vector2((2 * nowX + zeroOneWidth) / 2, 0);
            nowX += zeroOneWidth;
        }
    }

    void InitFillers()
    {
        for (int i = 0; i < fillers?.Length; i++)
            Destroy(fillers[i]);
        for (int i = 0; i < indicators?.Length; i++)
            Destroy(indicators[i]);
        opList.Clear();
        fillers = new GameObject[fillerInfos.Length];
        indicators = new GameObject[fillerInfos.Length];
        for (int i = 0; i < fillerInfos.Length; i++)
        {
            fillers[i] = Instantiate(fillerPrefab, fillingAreaRect);
            fillers[i].GetComponent<UnityEngine.UI.Image>().sprite = globalFillerInfoData.info[fillerInfos[i].operation].fillerSprite;
            fillers[i].GetComponent<UnityEngine.UI.Image>().color = globalFillerInfoData.info[fillerInfos[i].operation].color;
            indicators[i] = Instantiate(indicatorPrefab, indicatorAreaRect);
            indicators[i].GetComponent<UnityEngine.UI.Image>().sprite = globalFillerInfoData.info[fillerInfos[i].operation].indicatorSprite;
            indicators[i].GetComponent<UnityEngine.UI.Image>().preserveAspect = true;
        }
        UpdateFillerAndIndicator();
    }

    void UpdateFillerAndIndicator()
    {
        float width = fillingAreaRect.rect.width;
        float height = fillingAreaRect.rect.height;
        float sumLen = 0;
        foreach (var i in fillerInfos)
        {
            sumLen += i.length;
        }
        float nowX = 0;
        for (int i = 0; i < fillerInfos.Length; i++)
        {
            float fillerWidth = width * fillerInfos[i].length / sumLen;
            float fillerHeight = height;
            fillers[i].GetComponent<RectTransform>().offsetMin = new Vector2(nowX, 0);
            fillers[i].GetComponent<RectTransform>().offsetMax = new Vector2(nowX + fillerWidth, fillerHeight);
            // fillers[i].GetComponent<RectTransform>().anchoredPosition=new Vector2(nowX,0);
            indicators[i].GetComponent<RectTransform>().anchoredPosition = new Vector2((2 * nowX + fillerWidth) / 2, 0);
            nowX += fillerWidth;
            opList.TryAdd(nowX, fillerInfos[i].operation);
        }
    }

    void InitPointer()
    {
        pointerRect.anchoredPosition = new Vector2(0, 0);
        pointerMovingRight = true;
        pointerRect.SetAsLastSibling();
    }

    void UpdatePointer()
    {
        if (pointerMovingRight)
        {
            float newX = pointerRect.anchoredPosition.x + pointerMoveSpeed * (fillingAreaRect.rect.width - pointerRect.rect.width) / .8f * Time.deltaTime;
            if (newX + pointerWidth > fillingAreaRect.rect.width)
                newX = 0;
            pointerRect.anchoredPosition = new Vector2(newX, pointerRect.anchoredPosition.y);
            // pointerMovingRight=false;
        }
        // else
        // {
        //     float newX=pointerRect.anchoredPosition.x-pointerMoveSpeed*(fillingAreaRect.rect.width-pointerRect.rect.width)/.8f*Time.deltaTime;
        //     pointerRect.anchoredPosition=new Vector2(newX,pointerRect.anchoredPosition.y);
        //     if(newX<0)
        //         pointerMovingRight=true;
        // }
    }

    void CheckAndApplyOperation()
    {
        float midPos = pointerRect.anchoredPosition.x + pointerWidth / 2;
        OperationType midOp = OperationType.None;
        foreach (var i in opList)
        {
            if (midPos < i.Key + 0.5f)
            {
                midOp = i.Value;
                break;
            }
        }
        ApplyOperation(midOp);
    }

    void ApplyOperation(OperationType op)
    {
        if (op == OperationType.Die && lastOp != OperationType.Die && lastOp != OperationType.Replay)
        {
            //死亡相关逻辑
            FindObjectOfType<Player>().Die();
        }
        //松手之后replay和invert都可以再次执行
        //invert出去之后可以再次执行
        //replay仅松手后可以再次执行
        if (op == OperationType.None)
        {
            replayed = inverted = false;
        }
        if (op != OperationType.Invert)
        {
            inverted = false;
        }
        if (op != OperationType.Replay)
        {
            replayed = false;
        }

        // if(op!=OperationType.None && lastOp==OperationType.Invert &&Time.time-lastPressTime<maxProtectTime)
        // {
        //     return;
        // }

        //角色移动相关
        Vector2 moveDir = Vector2.zero;
        bool jump = false;

        if (op == OperationType.MoveLeft)
            moveDir += Vector2.left;
        else if (op == OperationType.MoveRight)
            moveDir += Vector2.right;
        else if (op == OperationType.MoveUp)
            moveDir += Vector2.up;
        else if (op == OperationType.MoveLeftUp)
        {
            moveDir += Vector2.left;
            moveDir += Vector2.up;
        }
        else if (op == OperationType.MoveRightUp)
        {
            moveDir += Vector2.right;
            moveDir += Vector2.up;
        }
        else if (op == OperationType.Jump)
            jump = true;
        else if (op == OperationType.JumpLeft)
        {
            jump = true;
            moveDir += Vector2.left;
        }
        else if (op == OperationType.JumpRight)
        {
            jump = true;
            moveDir += Vector2.right;
        }

        inputHandler.OnMoveInput(moveDir);
        if (jump)
            inputHandler.OnJumpInput();

        //条移动相关
        bool replay = false;
        bool invert = false;
        if (op == OperationType.Replay)
            replay = true;
        else if (op == OperationType.Invert)
            invert = true;

        if (replay && !replayed)
        {
            replayed = true;
            PointerReplay();
        }
        if (invert && !inverted)
        {
            inverted = true;
            PointerInvert();
        }
        //if(!(lastOp==OperationType.Die && op==OperationType.None))
        lastOp = op;
        lastPressTime = Time.time;
    }

    void PointerReplay()
    {
        pointerRect.anchoredPosition = new Vector2(0, pointerRect.anchoredPosition.y);
        pointerMovingRight = true;
    }

    void PointerInvert()
    {
        pointerMovingRight = !pointerMovingRight;
    }

    public void Begin()
    {
        start = true;
    }

    FillerInfo[] MergeInstruction(FillerInfo[] ori)
    {
        List<FillerInfo> res = new();
        res.Add(ori[0]);
        for (int i = 1; i < ori.Length; i++)
        {
            if (res[^1].operation == ori[i].operation)
            {
                res[^1] = new()
                {
                    length = res[^1].length + ori[i].length,
                    operation = res[^1].operation
                };
            }
            else
                res.Add(ori[i]);
        }

        return res.ToArray();
    }

    public void UpdateInstruction(string up, string left, string right, string upleft, string upright)
    {
        if(start)
            return ;
        OperationType[] ops = InstructionInterpreter.Interpreter(rawString, up, left, right, upleft, upright);
        fillerInfos = new FillerInfo[ops.Length];
        for (int i = 0; i < ops.Length; i++)
        {
            switch (ops[i])
            {
                case OperationType.MoveUp:
                    fillerInfos[i].length = up.Length;
                    break;
                case OperationType.MoveLeft:
                    fillerInfos[i].length = left.Length;
                    break;
                case OperationType.MoveRight:
                    fillerInfos[i].length = right.Length;
                    break;
                case OperationType.MoveLeftUp:
                    fillerInfos[i].length = upleft.Length;
                    break;
                case OperationType.MoveRightUp:
                    fillerInfos[i].length = upright.Length;
                    break;
                case OperationType.None:
                    fillerInfos[i].length = 1;
                    break;
            }
            fillerInfos[i].operation = ops[i];
        }
        fillerInfos = MergeInstruction(fillerInfos);
        InitFillers();
    }
}
