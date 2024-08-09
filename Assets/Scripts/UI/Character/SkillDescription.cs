using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Component")]
    public SkillDetailsSO skillDetails;
    public GameObject skillDescriptionPanel;
    public TextMeshProUGUI skillNameCNText;
    public TextMeshProUGUI skillNameENText;
    public TextMeshProUGUI skillRangeXText;
    public TextMeshProUGUI skillRangeYText;
    public TextMeshProUGUI skillPowerText;
    public TextMeshProUGUI skillCountText;
    public TextMeshProUGUI skillDescriptionText;
    
    [Space(10)]
    public GameObject PowerField;
    public GameObject CountField;
    

    private void Awake()
    {
        PowerField.SetActive(false);
        CountField.SetActive(false);
        skillDescriptionPanel.SetActive(false);
    }
    
    public void InitialUpdate(SkillDetailsSO skillDetails)
    {
        this.skillDetails = skillDetails;
        skillNameCNText.text = skillDetails.skillName;
        skillNameENText.text = skillDetails.skillENName;
        
        // Set skill range
        if (skillDetails.skillType == SkillButtonType.Attack)
        {
            if (skillDetails.SkillAimDataList.Count == 0)
            {
                HintPanelManager.Instance.CallError("SkillAimDataList is empty");
                return;                
            }
            skillRangeXText.text = skillDetails.SkillAimDataList[0].skillAttackRange.x.ToString();
            skillRangeYText.text = skillDetails.SkillAimDataList[0].skillAttackRange.y.ToString();
        }
        
        // Set skill power
        switch (skillDetails.skillUseCondition)
        {
            case SkillUseCondition.Power:
                PowerField.SetActive(true);
                skillPowerText.text = skillDetails.skillNeedPower.ToString();
                break;
            case SkillUseCondition.Count:
                CountField.SetActive(true);
                skillCountText.text = skillDetails.needCount.ToString();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        skillDescriptionText.text = DescriptionTextToRichText(skillDetails.skillDescription);

    }

    private string DescriptionTextToRichText(string text)
    {
        // 設定規則來匹配[Power]和[Cast](value)模式
        const string pattern = @"\[(\w+)\](?:\((.*?)\))?";
        
        // 定義不同標籤的顏色
        const string powerColor = "#4CA6FF";
        const string castColor = "#ADC8FF";
        const string damageColor = "#FF4F4F";
        const string hitCountColor = "#FF9B1A";
        const string healthColor = "#3FDB4A";
        

        // 使用正則表達式替換文本
        var result = Regex.Replace(text, pattern, match =>
        {
            var tags = match.Groups[1].Value;  // 取得標籤名稱
            var value = match.Groups[2].Value; // 取得括號內的值（如果有）

            return tags switch
            {
                "Power" => $" <color={powerColor}>{skillDetails.skillNeedPower}</color> ",
                "Count" => $" <color={hitCountColor}>{skillDetails.needCount}</color> ",
                "Cast" => $" <color={castColor}>{value}</color> ",
                "Damage" => $" <color={damageColor}>{skillDetails.damage}</color> ",
                "HitCount" => $" <color={hitCountColor}>{value}</color> ",
                "Health" => $" <color={healthColor}>TODO</color> ",
                _ => match.Value
            };
        });

        return result;
    }
    // private string DescriptionTextToRichText(string text)
    // {
    //     var result = "";
    //     var isInScope = false;
    //     var isInCastScope = false;
    //     var scopeText = "";
    //     var castText = "";
    //     foreach (var c in text)
    //     {
    //         if (isInCastScope)
    //         {
    //             if(c == ')') isInCastScope = false;
    //             else
    //                 castText += c;
    //         }
    //         
    //         if (!isInScope)
    //         {
    //             if (scopeText == string.Empty)
    //             {
    //                 if(c == '[') isInScope = true;
    //             }
    //             else if (c == '(') isInCastScope = true;
    //             else
    //             {
    //                 switch (scopeText)
    //                 {
    //                     case "Damage":
    //                         result += $"<color=#FF4F4F>{skillDetails.damage}</color>";
    //                         break;
    //                     case "HitCount":
    //                         result += $"<color=#FF9B1A>{skillDetails.SkillAimDataList.Count}</color>";
    //                         break;
    //                     case "Power":
    //                         result += $"<color=#4CA6FF>{skillDetails.skillNeedPower}</color>";
    //                         break;
    //                     case "Cast":
    //                         result += $"<color=#ADC8FF>{castText}</color>";
    //                         castText = "";
    //                         break;
    //                     case "Health":
    //                         result += $"<color=#3FDB4A>TODO</color>";
    //                         break;
    //                 }
    //             }
    //         }
    //         if (isInScope)
    //         {
    //             if(c == ']') isInScope = false;
    //             else
    //                 scopeText += c;
    //         }
    //         else
    //         {
    //             result += c;
    //
    //         }
    //     }
    //
    //     return result;
    // }

    public void OnPointerEnter(PointerEventData eventData)
    {
        skillDescriptionPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillDescriptionPanel.SetActive(false);
    }
}
