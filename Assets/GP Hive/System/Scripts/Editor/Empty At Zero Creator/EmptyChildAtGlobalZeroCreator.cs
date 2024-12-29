using UnityEngine;
using UnityEditor;

namespace GPHive.System.Editor.EmptyAtZeroCreator
{
    public class EmptyChildAtGlobalZeroCreator
    {
        const string _Space = EmptyCreator.Space;
        const string Slash = EmptyCreator.Slash;

        const string _global = "Global";
        const string featureName = EmptyCreator.CreateEmptyChildAt + _global + _Space + EmptyCreator.Zero;
        const string pathName = EmptyCreator.GameObjectStr + EmptyCreator.Slash + featureName + _Space + shortcutName;
        const string shortcutName = EmptyCreator.ControlSymbol + EmptyCreator.AltSymbol + EmptyCreator.ShortcutLetter;

        [MenuItem(pathName, false)]
        public static void CreateEmptyChildAtGlobalZero(MenuCommand menuCommand)
        {
            EmptyCreator.CreateEmptyGameObject(featureName, false, false, menuCommand);
        }
    }
}
