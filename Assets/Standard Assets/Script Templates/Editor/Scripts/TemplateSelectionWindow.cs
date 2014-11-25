/**
 * @file
 * @brief A dialog window that helps to select a script template.
 * 
 * @author Simon
 * @date November 2013
 * 
 * Copyright (C) 2013, Equinox Creations 
 * 
 */

// .NET includes
using System;
using System.IO;
using System.Collections.Generic;

// Unity includes
using UnityEngine;
using UnityEditor;

// Custom includes

namespace EditorLib {
	
/**
 * @brief A script template selection dialog.
 */
public class TemplateSelectionWindow : EditorWindow
{
	/**
	 * @brief Creates, initialises and shows the dialog. Don't create manually;
	 *	use this method to display the dialog.
	 *
	 * @param _templates a list of template paths to choose from (relative to project folder)
	 *
	 */
	static public void Show(IEnumerable<string> _templates)
	{
		var menu = ScriptableObject.CreateInstance<TemplateSelectionWindow>();
		menu.templatePaths = _templates;
		menu.title = "Choose Template";
		Vector2 menuSize = new Vector2(400, 160);
		var posX = (Screen.currentResolution.width - menuSize.x) / 2;
		var posY = (Screen.currentResolution.height - menuSize.y) / 2;
		menu.position = new Rect(posX, posY, menuSize.x, menuSize.y);
		menu.minSize = new Vector2(menu.position.width, menu.position.height);
		menu.ShowAuxWindow();
	}


	void OnEnable()
	{
		templateMenuScrollPos = new Vector2();
	}
	
	void OnGUI()
	{		
		templateMenuScrollPos = EditorGUILayout.BeginScrollView(templateMenuScrollPos, false, false);		
		int templateCount = 0;
		if (templatePaths != null) {
			foreach (string path in templatePaths) {
				++templateCount;
				if (GUILayout.Button(Path.GetFileNameWithoutExtension(path))) {
					string targetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
					if (! string.IsNullOrEmpty(targetPath)) {
						if ((File.GetAttributes(targetPath) & FileAttributes.Directory) == 0) {
							targetPath = Path.GetDirectoryName(targetPath);
						}
					}
					if (string.IsNullOrEmpty(targetPath)) {
						targetPath = "Assets";
					}
					Close();
					ScriptMenu.ScriptFromTemplate(path, targetPath + "/", templatePaths);
					return;
				}
			}
		}
		if (templateCount == 0) {
			EditorGUILayout.LabelField("No script templates found.");
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.Space();
		
		if (GUILayout.Button("Import...")) {
			Close();
			ScriptMenu.ImportScriptTemplate();
			ScriptMenu.ScriptFromTemplate();
		}
	}	
	
	
	IEnumerable<string> templatePaths;
	
	Vector2 templateMenuScrollPos;		
}
	
}  /* namespace EditorLib */

/* EOF */