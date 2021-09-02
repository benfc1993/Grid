using UnityEngine;

namespace Utils
{
    public static class Utilities
    {
        public static Vector3 GetMouseWorldPosition(Camera worldCamera)
        {
            return GetMouseWorldPosition(Input.mousePosition, worldCamera);
        }
        
        public static Vector3 GetMouseWorldPosition(Vector3 screenPosition, Camera worldCamera)
        {
            Ray ray = worldCamera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out var hit))
            {
                return hit.point;
            }

            return Vector3.positiveInfinity;
        }
        
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3),
            Vector3 rotation = default(Vector3), int fontSize = 38, Color color = default(Color), TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 1 )
        {
            return CreateWorldText(parent, text, localPosition, rotation, fontSize, (Color) color, textAnchor, textAlignment, sortingOrder);
        }

        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition,
            Vector3 rotation, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("{world_text}", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localRotation = Quaternion.Euler(rotation);
            
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.alignment = textAlignment;
            textMesh.anchor = textAnchor;
            textMesh.color = color;
            textMesh.fontSize = fontSize;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            textMesh.text = text;

            return textMesh;
        }
    }
}