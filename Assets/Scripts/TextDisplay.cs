using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplay : MonoBehaviour
{
    public string text;
    public int fontSize = 50;
    public int labelWidth = 300;
    public float duration;
    public float outlineWidth = 2;
    public Font textFont;
    public Color textColor;
    public Color outlineColor;
    
    private GUIStyle style;

    private void Start()
    {
        style = new GUIStyle
        {
            fontSize = fontSize,
            font = textFont,
            wordWrap = true,
            alignment = TextAnchor.MiddleCenter
        };
    }

    private void OnGUI()
    {
        StartCoroutine(DisplayText());
    }

    private IEnumerator DisplayText()
    {
        var screenPosition = Camera.main.WorldToScreenPoint(
            new Vector3(transform.position.x, transform.position.y + 0.75f)
        );
        var height = style.CalcHeight(new GUIContent(text), labelWidth);
        screenPosition.y = Screen.height - screenPosition.y;
        DrawTextWithOutline(
            new Rect(
                screenPosition.x - labelWidth / 2,
                screenPosition.y - height, labelWidth, height), 
            text, 
            outlineColor,
            textColor
        );
        
        yield return new WaitForSeconds(duration);
        Destroy(this);
    }

    private void DrawTextWithOutline(Rect position, string text, Color outColor, Color inColor)
    {
        style.normal.textColor = outColor;
        position.x -= outlineWidth;
        GUI.Label(position, text, style);
        position.x += 2 * outlineWidth;
        GUI.Label(position, text, style);
        position.x -= outlineWidth;
        position.y -= outlineWidth;
        GUI.Label(position, text, style);
        position.y += 2 * outlineWidth;
        GUI.Label(position, text, style);
        position.y -= outlineWidth;
        style.normal.textColor = inColor;
        GUI.Label(position, text, style);
    }

    private int CalculateTextWidth()
    {
        var result = 0;
        var characterInfo = new CharacterInfo();
        foreach (var c in text.ToCharArray())
        {
            textFont.GetCharacterInfo(c, out characterInfo, fontSize);
            result += characterInfo.advance;
        }

        return result;
    }
}
