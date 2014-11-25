/**
 * @file
 * @brief A dialog window that assists with replacing of special script
 *	template tokens.
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
using System.Linq;

// Unity includes
using UnityEngine;
using UnityEditor;

// Custom includes

namespace EditorLib {
	
/**
 * @brief The dialog displays passed script template parameters and allows 
 *	to assign values which will replace them.
 */
public class TemplateParametersWindow : EditorWindow
{
	/**
	 * @brief Callback format for invoker to listen to result.
	 */
	public delegate void CreateScriptCallback(string _template, string _targetPath, Dictionary<string, string> _replaceParams);
	
	
	class KeyboardFocusCycle 
	{
		struct ControlDesc
		{
			public string name;
			public bool textField;
		}
		
		public void RegisterName(string _controlName, bool _textField = false)
		{
			ControlDesc control;
			control.name = _controlName;
			control.textField = _textField;
			
			controls.AddLast(control);
			
			GUI.SetNextControlName(_controlName);
		}
		
		public void RegisterNameBefore(string _controlName, string _nextControl, bool _textField = false)
		{
			var node = controls.First;
			while ((node != null) && (node.Value.name != _nextControl)) {
				node = node.Next;
			}
			
			ControlDesc control;
			control.name = _controlName;
			control.textField = _textField;
			
			if (node == null) {
				controls.AddLast(control);
			}
			else {
				controls.AddBefore(node, control);
			}
			
			GUI.SetNextControlName(_controlName);
		}
		
		public void RegisterNameAfter(string _controlName, string _prevControl, bool _textField = false)
		{
			var node = controls.First;
			while ((node != null) && (node.Value.name != _prevControl)) {
				node = node.Next;
			}
			
			ControlDesc control;
			control.name = _controlName;
			control.textField = _textField;
			
			if (node == null) {
				controls.AddLast(control);
			}
			else {
				controls.AddAfter(node, control);
			}
			
			GUI.SetNextControlName(_controlName);
		}
		
		public void Reset()
		{
			controls.Clear();
			focusedName = "";
			focusWaiting = "";
			refocus = true;
		}
		
		public void Update()
		{
			// Synchronise focus
			
			string systemFocus = GUI.GetNameOfFocusedControl();
			if (! refocus && (focusWaiting == "") && (systemFocus != "")) {
				// If the system focus is set, use it.
				focusedName = systemFocus;
			}
			else {
				bool updateFocus = true;
				if ((focusWaiting != "") && (focusWaiting == systemFocus)) {
					focusWaiting = "";
					updateFocus = refocus;
				}
				
				if (updateFocus) {
					// Otherwise, is there a remembered control?
					if (focusedName != "") {
						// Yep. Try to refocus.
						var control = GetCurrentControl();
						if (control != null) {
							Focus((ControlDesc) control);
						}
						else {
							focusedName = "";					
						}
					}
					
					// If no valid focus is set, focus the first control.
					if (focusedName == "") {
						FocusNext();
					}
				}
			}
			
			// Process user input

			if (tabPressed) {
				FocusNext();
			}
			else if (enterPressed) {
				var control = GetCurrentControl();
				if ((control != null) && control.Value.textField) {
					FocusNext();
				}
			}
			
			// Reset registered controls before next iteration
			
			controls.Clear();
			tabPressed = false;
			enterPressed = false;
			refocus = false;
		}
		
		public void ProcessInput(Event _currentEvent)
		{
			if (_currentEvent.type == EventType.KeyDown) {
				if (_currentEvent.keyCode == KeyCode.Tab) {
					tabPressed = true;
					_currentEvent.Use();
				}
				else if ((_currentEvent.keyCode == KeyCode.Return) || (_currentEvent.keyCode == KeyCode.KeypadEnter)) {
					enterPressed = true;
					_currentEvent.Use();
				}
				else if ((_currentEvent.character == '\t')
					|| (_currentEvent.character == '\r')
					|| (_currentEvent.character == '\n')) {
					// disable built-in reaction to special keys
					_currentEvent.Use();
				}
			}
		}

		public void RecaptureFocus()
		{
			refocus = true;
		}
		
		
		void FocusNext()
		{
			if (controls.Count == 0) {
				return;
			}
			
			ControlDesc nextControl;
			if (focusedName == "") {
				nextControl = controls.First.Value;
			}
			else {
				var node = controls.First;
				while ((node != null) && (node.Value.name != focusedName)) {
					node = node.Next;
				}
				
				if (node != null) {
					node = node.Next;
				}
				if (node == null) {
					node = controls.First;
				}
				
				nextControl = node.Value;
			}
			
			Focus(nextControl);
			focusedName = nextControl.name;
		}
		
		void Focus(ControlDesc _control) 
		{
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
			GUI.FocusControl(_control.name);
#else
			if (_control.textField) {
				EditorGUI.FocusTextInControl(_control.name);
			}
			else {
				GUI.FocusControl(_control.name);
			}			
#endif
			focusWaiting = _control.name;
		}
		
		ControlDesc? GetCurrentControl()
		{
			foreach (var control in controls) {
				if (control.name == focusedName) {
					return control;
				}
			}
			
			return null;
		}			
		
		
		LinkedList<ControlDesc> controls = new LinkedList<ControlDesc>();
		string focusedName = "";
		string focusWaiting = "";
		
		bool enterPressed = false;
		bool tabPressed = false;
		bool refocus = true;
	}
	

	/**
	 * @brief Creates, initialises and shows the dialog. Don't create manually;
	 *	use this method to display the dialog.
	 *
	 * After confirmation (only), the _callback is called with supplied parameters, 
	 * so the invoker may proceed. However, this dialog is non-blocking and there's 
	 * no guarantee of when the confirm button will be pressed. Hence, take care 
	 * to check the parameters upon reception; they may be no longer valid.
	 *
	 * @param _template The path to the source template
	 * @param _targetPath The path to new script location, or destination folder ending with "/"
	 * @param _replaceParams Tokens to replace; existing values will be shown as defaults
	 * @param _callback The listener that will be called if this dialog is confirmed; returned _replaceParams 
	 *	will contain the desired token replacements
	 * @param _templateOffer If not null, the source template will be allowed to change to one 
	 *	of the templates on the offer.
	 *
	 * @throws ArgumentNullException If any of the parameters is null.
	 *
	 */
	static public void Show(string _template, string _targetPath, Dictionary<string, string> _replaceParams, 
		CreateScriptCallback _callback, IEnumerable<string> _templateOffer = null)
	{
		if ((_template == null)
			|| (_targetPath == null)
			|| (_replaceParams == null)
			|| (_callback == null)) {
			
			throw new ArgumentNullException();
		}
				
		var menu = GetWindow<TemplateParametersWindow>(true, "Fill In Template Parameters");
		menu.template = _template;
		menu.targetPath = _targetPath;
		menu.replaceParams = _replaceParams;
		menu.callback = _callback;
		menu.localParams = new Dictionary<string, string>(menu.replaceParams);
		
		menu.selectedTemplate = -1;
		menu.templatePaths = _templateOffer;
		if (menu.templatePaths == null) {
			menu.templateOffer = null;
		}
		else {
			List<string> fileNames = new List<string>(menu.templatePaths.Count());
			int i = 0;
			foreach (string file in menu.templatePaths) {
				fileNames.Add(Path.GetFileNameWithoutExtension(file));
				if (file == menu.template) {
					menu.selectedTemplate = i;
				}
				++i;
			}
			menu.templateOffer = fileNames.ToArray();
		}		
		
		foreach (string param in menu.localParams.Keys) {
			menu.ValidateParam(param, menu.localParams[param]);
		}
			
		Vector2 menuSize = new Vector2(500, 240);
		var posX = (Screen.currentResolution.width - menuSize.x) / 2;
		var posY = (Screen.currentResolution.height - menuSize.y) / 2;
		menu.position = new Rect(posX, posY, menuSize.x, menuSize.y);
		menu.minSize = new Vector2(menu.position.width, menu.position.height);		
	}


	void OnEnable()
	{
		paramScrollPos = new Vector2();
		
		errors = new Dictionary<string, string>();
		errorOrder = new List<string>();
		
		resetIcon = Resources.Load("editorlib_reset-icon", typeof(Texture2D)) as Texture2D;
		resetButtonStyle = new GUIStyle();
		resetButtonStyle.normal.background = resetIcon;
		resetButtonStyle.normal.textColor = Color.clear;
		resetButtonStyle.fixedWidth = resetIcon.width;
		resetButtonStyle.fixedHeight = resetIcon.height;
		resetButtonStyle.margin = new RectOffset(6, 6, 4, 4);
		
		focus = new KeyboardFocusCycle();
	}
	
	void OnGUI()
	{	
		// Process user input before it gets consumed by other fields.
	
		if ((Event.current.type == EventType.KeyDown) && (Event.current.keyCode == KeyCode.Escape)) {
			Close();
			Event.current.Use();
			return;
		}
		
		focus.ProcessInput(Event.current);
		
		// Start layout
	
		GUIStyle importantLabel = GUI.skin.label;
		importantLabel.fontStyle = FontStyle.Bold;

		// - source template
		
		EditorGUILayout.LabelField("Source template:");
		if (selectedTemplate >= 0) {
			int newIndex = EditorGUILayout.Popup(selectedTemplate, templateOffer);
			if (newIndex != selectedTemplate) {
				selectedTemplate = newIndex;
				template = templatePaths.ElementAt(selectedTemplate);
				ScriptMenu.CollectScriptTemplateParams(replaceParams, template);
				
				var newLocalParams = new Dictionary<string, string>(replaceParams);
				FlushErrors();
				foreach (var key in localParams.Keys) {
					if (newLocalParams.ContainsKey(key)) {
						newLocalParams[key] = localParams[key];
						ValidateParam(key, newLocalParams[key]);
					}
				}
				localParams = newLocalParams;
				
				focus.Reset();
				
				// Bug workaround: In Unity 4.3.1 on Mac, GUI.GetNameOfFocusedControl() sometimes 
				// returns old names for dynamically changing layouts. That means after setting
				// focus to a control with a name, a different name may be actually returned
				// by the system for the same control. The only way to flush the old control 
				// names is to close and reopen the window when the layout may change.
				Close();
				// Note: Originally passed parameters are lost by this operation.
				Show(template, targetPath, localParams, callback, templatePaths);
				return;
			}
		}
		else {
			EditorGUILayout.LabelField(Path.GetFileNameWithoutExtension(template), importantLabel);
		}
		EditorGUILayout.Space();
		
		// - target path
		
		EditorGUILayout.LabelField("Script target path:");
		EditorGUILayout.BeginHorizontal();
		importantLabel.stretchWidth = false;
		GUILayout.Label(targetPath, importantLabel);
		if (GUILayout.Button("", resetButtonStyle)) {
			targetPath = "Assets/";
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		// - parameters
		
		paramScrollPos = EditorGUILayout.BeginScrollView(paramScrollPos, false, false);		
		
		var oldBgColor = GUI.backgroundColor;
		foreach (string param in replaceParams.Keys) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(TranslateParamName(param) + ":");
			bool valid = ! errors.ContainsKey(param);
			if (! valid) {
				GUI.backgroundColor = Color.red;
			}
			focus.RegisterName(param, true);
			string newValue = EditorGUILayout.TextField(localParams[param], GUILayout.Width(260));
			if (newValue != localParams[param]) {
				localParams[param] = newValue;
				ValidateParam(param, newValue);
			}
			if (! valid) {
				GUI.backgroundColor = oldBgColor;
			}
			EditorGUILayout.EndHorizontal();
		}
		
		EditorGUILayout.EndScrollView();
		
		// - error message box
		
		string message = "Use Space to activate a button when focused. Press Tab to jump to next field.";
		MessageType msgType = MessageType.None;
		if (errorOrder.Count > 0) {
			message = errors[errorOrder[0]];
			msgType = MessageType.Error;
		}
		EditorGUILayout.HelpBox(message, msgType, true);
		EditorGUILayout.Space();

		// - buttons
		
		EditorGUILayout.BeginHorizontal();
						
		focus.RegisterName("Cancel");
		if (GUILayout.Button("Cancel")) {
			Close();
		}

		bool disableOKButton = (errorOrder.Count > 0);
		EditorGUI.BeginDisabledGroup(disableOKButton);
		if (! disableOKButton) {
			focus.RegisterNameBefore("OK", "Cancel");
		}
		if (GUILayout.Button("Create Script!")) {
			Close();
			callback(template, targetPath, localParams);
			return;
		}
		EditorGUI.EndDisabledGroup();
		
		EditorGUILayout.EndHorizontal();
		
		// Resolve focus cycling
		
		focus.Update();
	}
	
	void OnSelectionChange()
	{
		string path = AssetDatabase.GetAssetPath(Selection.activeObject);
		if (! string.IsNullOrEmpty(path)) {
			if ((File.GetAttributes(path) & FileAttributes.Directory) == 0) {
				path = Path.GetDirectoryName(path);
			}
			targetPath = path + "/";
			Repaint();
		}
	}
	
	void OnFocus() 
	{
		focus.RecaptureFocus();
	}
	
	string TranslateParamName(string _paramName)
	{
		switch (_paramName) {
		
		case "#SCRIPTNAME#":
			return "Script file name (" + _paramName + ")";
			
		default:
			return _paramName;
		}
	}
	
	void FlushErrors()
	{
		errorOrder.Clear();
		errors.Clear();
	}
	
	bool ValidateParam(string _key, string _value)
	{
		string errorMsg = null;
		
		switch (_key) {
		
		case "#SCRIPTNAME#":
			if (! System.Text.RegularExpressions.Regex.IsMatch(_value, @"^[a-zA-Z_][a-zA-Z0-9_]*$")) {
				errorMsg = "Use only a-z, A-Z, _, and 0-9. The first character cannot be a digit.";
			}
			else {
				string targetFile = targetPath + _value + ScriptMenu.GetScriptExt(template, true);
				if (File.Exists(targetFile)) {
					errorMsg = "A file with the same name already exists.";
				}
			}
			break;
		}
		
		if (errorMsg != null) {
			errors[_key] = errorMsg;
			if (! errorOrder.Contains(_key)) {
				errorOrder.Add(_key);
			}
			return false;
		}
		else {
			errors.Remove(_key);
			errorOrder.Remove(_key);
			return true;
		}		
	}
	
	
	string template;
	string targetPath;
	Dictionary<string, string> replaceParams;
	IEnumerable<string> templatePaths;
	CreateScriptCallback callback;
	
	Vector2 paramScrollPos;
	Dictionary<string, string> localParams;	
	string[] templateOffer;
	int selectedTemplate;
	
	Dictionary<string, string> errors;
	List<string> errorOrder;
	
	Texture2D resetIcon;
	GUIStyle resetButtonStyle;
	
	KeyboardFocusCycle focus;
}
	
}  /* namespace EditorLib */

/* EOF */