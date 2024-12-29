using UnityEditor;

namespace GPHive.System.Editor.EmptyAtZeroCreator
{
    public class EmptyChildAtLocalZeroCreator
    {
        const string _Space = EmptyCreator.Space;
        const string Slash = EmptyCreator.Slash;

        const string _local = "Local";
        const string featureName = EmptyCreator.CreateEmptyChildAt + _local + _Space + EmptyCreator.Zero;
        const string pathName = EmptyCreator.GameObjectStr + EmptyCreator.Slash + featureName + _Space + shortcutName;
        const string shortcutName = EmptyCreator.ControlSymbol + EmptyCreator.AltSymbol + EmptyCreator.ShiftSymbol + EmptyCreator.ShortcutLetter;

        [MenuItem(pathName, false)]
        public static void CreateEmptyChildAtLocalZero(MenuCommand menuCommand)
        {
            EmptyCreator.CreateEmptyGameObject(featureName, false, true, menuCommand);
        }
    }
}