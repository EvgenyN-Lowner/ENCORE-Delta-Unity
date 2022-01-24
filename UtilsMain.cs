using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvgenyN.Utils
{
    public static class UtilsMain
    {
        public const int sortingOrderDefault = 5000;

        //Создать текст в мире
        public static TextMesh createTextInWorld(string text, Transform parent = null, Vector3 localPos = default(Vector3), int fontSize = 10, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
        {
            if (color == null) color = Color.white;
            return createTextInWorld(parent, text, localPos, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }
        public static TextMesh createTextInWorld(Transform parent, string text, Vector3 localPos, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPos;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        //Расчеты
        //Перевести число в положительное
        public static void GetPositiveFloat(float a, out float b)
        {
            if (a < 0)
            {
                b = a * -1;
            } else
            {
                b = a;
            }
        }
    }
}