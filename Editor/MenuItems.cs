using AM.Unity.Component.System;
using UnityEditor;
using UnityEngine;
public class MenuItems : MonoBehaviour
{
    // Add a menu item to create custom GameObjects.
    // Priority 10 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarchy context menus.
    [MenuItem("GameObject/AM.UCS/Entity Manager", false, 10)]
    static void Create_EntityManager(MenuCommand menuCommand) 
    {
        CreateGameObjectWithComponent<EntityManager>(menuCommand, "EntityManager");
    }

    static void CreateGameObjectWithComponent<T>(MenuCommand menuCommand, string name) where T : Component
    {
        // Create a custom game object
        GameObject go = new GameObject(name);
        go.AddComponent<T>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}