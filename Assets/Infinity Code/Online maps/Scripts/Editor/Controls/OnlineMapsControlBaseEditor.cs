/*     INFINITY CODE 2013-2018      */
/*   http://www.infinity-code.com   */

#if !UNITY_5_2
#define UNITY_5_3P
#endif

using UnityEditor;
using UnityEngine;

#if UNITY_5_3P
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

[CustomEditor(typeof(OnlineMapsControlBase), true)]
public abstract class OnlineMapsControlBaseEditor<T> : OnlineMapsFormattedEditor
    where T : OnlineMapsControlBase
{
    protected OnlineMaps map;
    protected T control;

    protected LayoutItem warningLayoutItem;

    protected SerializedProperty pAllowUserControl;
    protected SerializedProperty pAllowZoom;
    protected SerializedProperty pInvertTouchZoom;
    protected SerializedProperty pZoomInOnDoubleClick;
    protected SerializedProperty pZoomMode;

    protected override void CacheSerializedFields()
    {
        pAllowUserControl = serializedObject.FindProperty("allowUserControl");
        pAllowZoom = serializedObject.FindProperty("allowZoom");
        pInvertTouchZoom = serializedObject.FindProperty("invertTouchZoom");
        pZoomInOnDoubleClick = serializedObject.FindProperty("zoomInOnDoubleClick");
        pZoomMode = serializedObject.FindProperty("zoomMode");
    }

    private static void CheckMultipleInstances(OnlineMapsControlBase control, ref bool dirty)
    {
        OnlineMapsControlBase[] controls = control.GetComponents<OnlineMapsControlBase>();
        if (controls.Length > 1)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.HelpBox("Problem detected:\nMultiple instances of controls.\nYou can use only one control.", MessageType.Error);

            int controlIndex = -1;

            for (int i = 0; i < controls.Length; i++)
            {
                if (GUILayout.Button("Use " + controls[i].GetType())) controlIndex = i;
            }

            if (controlIndex != -1)
            {
                OnlineMapsControlBase activeControl = controls[controlIndex];
                foreach (OnlineMapsControlBase c in controls) if (c != activeControl) OnlineMapsUtils.Destroy(c);
                dirty = true;
            }

            EditorGUILayout.EndVertical();
        }
    }

    protected override void GenerateLayoutItems()
    {
        base.GenerateLayoutItems();

        warningLayoutItem = rootLayoutItem.Create("WarningArea");
        rootLayoutItem.Create(pAllowUserControl);
        LayoutItem lZoom = rootLayoutItem.Create(pAllowZoom);
        lZoom.drawGroup = LayoutItem.Group.valueOn;
        lZoom.Create(pZoomMode);
        lZoom.Create(pZoomInOnDoubleClick);
        lZoom.Create(pInvertTouchZoom);
    }

    private static OnlineMaps GetOnlineMaps(OnlineMapsControlBase control)
    {
        OnlineMaps map = control.GetComponent<OnlineMaps>();

        if (map == null)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.HelpBox("Problem detected:\nCan not find OnlineMaps component.", MessageType.Error);

            if (GUILayout.Button("Add OnlineMaps Component"))
            {
                map = control.gameObject.AddComponent<OnlineMaps>();
                UnityEditorInternal.ComponentUtility.MoveComponentUp(map);
            }

            EditorGUILayout.EndVertical();
        }
        return map;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        map = null;
        control = null;
    }

    protected override void OnEnableBefore()
    {
        base.OnEnableLate();

        control = (T)target;
        map = GetOnlineMaps(control);
        if (control.GetComponent<OnlineMapsMarkerManager>() == null) control.gameObject.AddComponent<OnlineMapsMarkerManager>();
    }

    protected override void OnSetDirty()
    {
        base.OnSetDirty();

        EditorUtility.SetDirty(map);
        EditorUtility.SetDirty(control);

        if (Application.isPlaying) map.Redraw();
    }
}