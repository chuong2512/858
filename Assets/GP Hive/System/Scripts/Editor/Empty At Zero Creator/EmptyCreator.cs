using UnityEngine;
using UnityEditor;

namespace GPHive.System.Editor.EmptyAtZeroCreator
{
    public static class EmptyCreator
    {
        public const string GameObjectStr = "GameObject";

        public const string Space = " ";
        public const string Slash = "/";

        public const string CreateEmpty = Create + Space + Empty + Space;
        public const string CreateEmptyChildAt = CreateEmpty + Child + Space + At + Space;

        private const string Create = "Create";
        private const string Empty = "Empty";
        private const string Child = "Child";

        public const string At = "At";
        public const string Zero = "Zero";

        public const string ShortcutLetter = "N";
        public const string ControlSymbol = "%";
        public const string ShiftSymbol = "#";
        public const string AltSymbol = "&";

        public static void CreateEmptyGameObject(string featureName, bool hasToDeselect, bool hasToResetLocalValues,
            MenuCommand menuCommand)
        {
            if (hasToDeselect)
            {
                //Reset selection
                Selection.activeGameObject = null;
            }

            //Create the new empty gameObject
            var gameObjectName = GameObjectStr;
            var spawnedGameObject = new GameObject(gameObjectName);


            if (hasToDeselect)
            {
                GameObjectUtility.SetParentAndAlign(spawnedGameObject, menuCommand.context as GameObject);
            }

            //Undo
            var undoMethodName = featureName;
            Undo.RegisterCreatedObjectUndo(spawnedGameObject, undoMethodName);

            if (Selection.activeGameObject != null)
            {
                //Set parent
                spawnedGameObject.transform.parent = Selection.activeGameObject.transform;

                if (hasToResetLocalValues)
                {
                    //Reset local values
                    spawnedGameObject.transform.localPosition = Vector3.zero;
                    spawnedGameObject.transform.localRotation = Quaternion.identity;
                    spawnedGameObject.transform.localScale = Vector3.one;
                }
            }

            Selection.activeGameObject = spawnedGameObject;

            //Add a RectTransform if needed
            if (spawnedGameObject.transform.parent == null) return;
            var parentRectTransform = spawnedGameObject.transform.parent.GetComponent<RectTransform>();

            if (parentRectTransform == null) return;
            var rectTransform = spawnedGameObject.gameObject.AddComponent(typeof(RectTransform)) as RectTransform;
            if (rectTransform == null) return;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}