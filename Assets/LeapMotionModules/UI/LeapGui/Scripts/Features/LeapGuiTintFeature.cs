﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[LeapGuiFeatureName("Tint")]
public class LeapGuiTintFeature : LeapGuiFeature<LeapGuiTintData> {

#if UNITY_EDITOR
  public override void DrawFeatureEditor(Rect rect, bool isActive, bool isFocused) {
    EditorGUI.LabelField(rect, "Tint");
  }

  public override float GetEditorHeight() {
    return EditorGUIUtility.singleLineHeight;
  }
#endif
}

