/**
 * @file
 * @brief Extra functionality for script assets.
 * 
 * Added functions and menu items for script assets:
 * - Create a script from a template (with .stmpl extension)
 * - Create a custom asset from a ScriptableObject
 * 
 * @author Simon
 * @date July 2013
 * 
 * Copyright (C) 2013, Equinox Creations 
 * 
 */

// .NET includes
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

// Unity includes
using UnityEngine;
using UnityEditor;

// Custom includes

namespace EditorLib {
	
/**
 * @brief Encapsulates new menu items for script assets.
 */
public static class ScriptMenu
{
#region nested types
		
  #region nested types: public
		
	
		
  #endregion

  #region nested types: protected
		
	
		
  #endregion

  #region nested types: private
		
	
		
  #endregion
		
#endregion
		
#region static members
		
  #region static public
		
    #region static public: constructors and destructors
		
	
		
    #endregion
		
    #region static public: methods
		
	/**
	 * @brief Creates a new script from a template at the currently selected location.
	 *	This invokes a template selection dialog.
	 *
	 * All files with the #TEMPLATE_EXTENSION placed in a #TEMPLATE_FOLDER subfolder anywhere
	 * in the project are found and listed for user to choose from. After a template is chosen, 
	 * the user is asked to supply a script name and possibly fill in parameters found in 
	 * the template in the #[_A-Z][_A-Z0-9]*# format.
	 *
	 * This is called from the editor menu. Consequently, ScriptFromTemplate() with different
	 * parameters is called in the process, while more information is being obtained.
	 *
	 */
	[MenuItem("Assets/Create/Script from template...", false, 0)]
	static public void ScriptFromTemplate() 
	{
		// Collect available templates.
	
		List<DirectoryInfo> templateFolders = new List<DirectoryInfo>();	
		List<string> templatePaths = new List<string>();
		
		Stack<DirectoryInfo> dirs = new Stack<DirectoryInfo>();
		DirectoryInfo examinedDir;
		
		dirs.Push(new DirectoryInfo("Assets/"));
		do {
			examinedDir = dirs.Pop();
			
			var childDirs = examinedDir.GetDirectories();
			foreach (var dir in childDirs) {
				if (dir.Name == TEMPLATE_FOLDER) {
					templateFolders.Add(dir);
				}
				else {
					dirs.Push(dir);
				}
			}
		} while (dirs.Count > 0);
		
		foreach (var tmplDir in templateFolders) {
			dirs.Push(tmplDir);
			do {
				examinedDir = dirs.Pop();
				
				var files = examinedDir.GetFiles();
				foreach (var file in files) {
					if (file.Name.ToLower().EndsWith(TEMPLATE_EXTENSION)) {
						// A full path with '\' is returned which is not recognized
						// by Unity. So we need to reparse the path and set it relative to project folder.
						var pathTokens = file.FullName.Split('\\', '/');
						StringBuilder unityPath = new StringBuilder(file.FullName.Length);
						int i = 0;
						for ( ; i < pathTokens.Length; ++i) {
							if (pathTokens[i] == "Assets") {
								unityPath.Append(pathTokens[i]);
								++i;
								break;
							}
						}
						for ( ; i < pathTokens.Length; ++i) {
							unityPath.Append("/");
							unityPath.Append(pathTokens[i]);
						}
						templatePaths.Add(unityPath.ToString());
					}
				}
			
				var childDirs = examinedDir.GetDirectories();
				foreach (var dir in childDirs) {
					dirs.Push(dir);
				}
			} while (dirs.Count > 0);
		}
			
		// Show context menu with template selection

		TemplateSelectionWindow.Show(templatePaths);			
	}
	
	/**
	 * @brief Creates a new script at the target path from the given template.
	 *	This is called when the information about the source template and the destination
	 *  folder is already available.
	 *
	 * Template parameters are gathered at this point and presented to user, or the script
	 * is already created if no unfilled parameters are present. Only these parameters have
	 * special meaning:
	 * - #SCRIPTNAME# : The file name of the script without the extension. By convention, 
	 *		it shares the name with the main class it contains.
	 *
	 * Other than that, the parameters act like arbitrary user variables that match the pattern
	 * of an upper-case identifier (#[_A-Z][_A-Z0-9]*#).
	 *
	 * @param _template The path to source template. Cannot be null and must be valid, otherwise expect 
	 *	an exception.
	 * @param _targetPath The target file name with the path relative to project folder, or the destination
	 *	directory ending with "/".
	 * @param _templatePaths Available template selection. If not null, the source template can be changed
	 *	to another one on the list in parameters dialog.
	 *
	 */
	static public void ScriptFromTemplate(string _template, string _targetPath, IEnumerable<string> _templatePaths = null)
	{
		if ((_template == null) || (_targetPath == null)) {
			throw new ArgumentNullException();
		}

		bool targetIsDirectory = _targetPath.EndsWith("/");
		string targetFileName = "";
		if (! targetIsDirectory) {
			_targetPath = ProcessScriptName(_targetPath);
			targetFileName = Path.GetFileNameWithoutExtension(_targetPath);
		}
		
		try {
			var specialSymbols = new Dictionary<string, string>();
			CollectScriptTemplateParams(specialSymbols, _template, targetFileName);
			
			if ((specialSymbols.Keys.Count == 1) && (targetFileName.Length > 0)) {
				// if there are no additional parameters, create the script right away
				DoCreateScriptFromTemplate(_template, _targetPath, specialSymbols);
			}
			else {
				string targetPath = targetIsDirectory ? _targetPath : Path.GetDirectoryName(_targetPath) + "/";
				TemplateParametersWindow.Show(_template, targetPath, specialSymbols, ScriptFromTemplate, _templatePaths);
			}
		}
		catch (Exception _e) {
			EditorUtility.DisplayDialog("Failed To Create Script!", "The script couldn't be created from the given template."
				+ " Please, check log for more details.", "OK");
			throw _e;
		}
	}
	
	/**
	 * @brief Creates a new script at the target path from the given template, replacing the parameters with the supplied 
	 *	values. This is a fully specified request; no further user interaction is needed.
	 *
	 * Arguments are validated and the target file name composed if needed. If the target path specifies only directory,
	 * the file name is defined by the #SCRIPTNAME# parameter. If the parameter is not found, is empty, or invalid, a default
	 * name #NEW_SCRIPT_NAME is used.
	 *
	 * @param _template The path to the source template. If invalid, the user is notified, but no further error is issued.
	 * @param _targetPath The target file name with the path relative to project folder, or the destination
	 *	directory ending with "/".
	 * @param _replaceParams Collection of special tokens and their replacements.
	 *
	 */
	static public void ScriptFromTemplate(string _template, string _targetPath, Dictionary<string, string> _replaceParams)
	{	
		if (! File.Exists(_template)) {
			EditorUtility.DisplayDialog("Script Template Doesn't Exist!", "The script template '" + Path.GetFileName(_template) 
				+ "' doesn't exist anymore. The script cannot be created.", "OK");
			return;
		}
		
		if (! Directory.Exists(Path.GetDirectoryName(_targetPath))) {
			EditorUtility.DisplayDialog("Script Destination Is Invalid!", "The target folder '" + Path.GetDirectoryName(_targetPath)
				+ "' doesn't exist anymore. The script won't be created.", "OK");
			return;
		}
		
		if (_replaceParams == null) {
			throw new ArgumentNullException();
		}
		
		var keys = new List<string>(_replaceParams.Keys);
		foreach (var key in keys) {
			_replaceParams[key] = _replaceParams[key].Trim();
		}
		
		try {
			string targetPath = _targetPath;
			if (targetPath.EndsWith("/")) {
				string scriptName = null;
				const string SCRIPT_NAME_KEY = "#SCRIPTNAME#";
				if (_replaceParams.ContainsKey(SCRIPT_NAME_KEY)) {
					scriptName = _replaceParams[SCRIPT_NAME_KEY];
				}
				if (scriptName != null) {
					scriptName = ProcessScriptName(scriptName);
				}
				if (string.IsNullOrEmpty(scriptName)) {
					scriptName = NEW_SCRIPT_NAME;
					EditorUtility.DisplayDialog("No Valid Script Name!", "No valid script name was specified. A script with the "
						+ "default name '" + NEW_SCRIPT_NAME + "' will be created.", "OK");
				}
				
				targetPath += scriptName + GetScriptExt(_template, true);
				_replaceParams[SCRIPT_NAME_KEY] = scriptName;
			}
			
			DoCreateScriptFromTemplate(_template, targetPath, _replaceParams);
		}
		catch (Exception _e) {
			EditorUtility.DisplayDialog("Failed To Create Script!", "The script couldn't be created from the given template."
				+ " Please, check log for more details.", "OK");
			throw _e;
		}
	}
	
	/**
	 * @brief Validates a selected script template and whether a new script 
	 * 		can be created from it.
	 * 
	 * @return false if there's no selected item or the selected item is not a script template
	 * @return true if no problem detected
	 * 
	 */
	[MenuItem("Assets/Create/Script from selected", true)]
	static public bool ValidateScriptFromSelected()
	{
		// Is an object selected?
		if (Selection.activeObject == null) {
			return false;
		}
			
		// Is the object a script template? Does it have a correct extension?
		var path = AssetDatabase.GetAssetPath(Selection.activeObject);
		string folderPattern = "/" + TEMPLATE_FOLDER + "/";
		if (! path.EndsWith(TEMPLATE_EXTENSION) || (path.IndexOf(folderPattern) < 0)) {
			return false;
		}
			
		return true;
	}
		
	/**
	 * @brief Creates a new script from a selected template.
	 * 
	 * The process is fully interactive. A dialog prompts to select a location for the 
	 * new script, and template parameters are offered for edit if found.
	 * 
	 * The method will fail with an exception if #ValidateScriptFromTemplate() fails.
	 * 
	 */
	[MenuItem("Assets/Create/Script from selected", false, 1)]
	static public void ScriptFromSelected()
	{
		var path = AssetDatabase.GetAssetPath(Selection.activeObject);
		if (! path.EndsWith(TEMPLATE_EXTENSION)) {
			throw new ArgumentException("A script template must end with '" + TEMPLATE_EXTENSION + "' extension!");
		}
		
		string folderPattern = "/" + TEMPLATE_FOLDER + "/";
		if (path.IndexOf(folderPattern) < 0) {
			throw new ArgumentException("A script template must be located in a '" + TEMPLATE_FOLDER + "' folder!");
		}
		
		string targetPath = EditorUtility.SaveFilePanelInProject("Save New Script", NEW_SCRIPT_NAME, GetScriptExt(path, false), 
			"Please, select a name and location for your script.");
		
		if (targetPath.Length > 0) {	
			ScriptFromTemplate(path, targetPath);
		}
	}
	
	/**
	 * @brief Collect tokens to replace for a script template.
	 * 
	 * @param[out] _params Collected tokens. The container is emptied before it is filled and returned.
	 * @param _template The template to scan.
	 * @param _scriptName Optional. A specific value for #SCRIPTNAME# parameter.
	 *
	 * @throws ArgumentNullException If _params or _template is null.
	 *
	 */
	static public void CollectScriptTemplateParams(Dictionary<string, string> _params, string _template, string _scriptName = null)
	{
		if ((_params == null) || (_template == null)) {
			throw new ArgumentNullException();
		}
		
		_params.Clear();
		const string SCRIPT_NAME_SYMBOL = "#SCRIPTNAME#";
		_params.Add(SCRIPT_NAME_SYMBOL, string.IsNullOrEmpty(_scriptName) ? NEW_SCRIPT_NAME : _scriptName);
		ScanTemplateSymbols(_params, _template);
	}
	
	/**
	 * @brief Validates whether a custom asset can be created from the selected script.
	 * 
	 * @return false if there's no selected item or the selected item is not a script
	 * @return false if there's no class in the selected script or it doesn't inherit 
	 *		from ScriptableObject
	 * @return true if no problem detected
	 * 
	 */
	[MenuItem("Assets/Create/Custom asset", true)]
	static public bool ValidateCreateCustomAsset()
	{
		if (! (Selection.activeObject is MonoScript)) {
			return false;
		}
		
		MonoScript scriptFile = (MonoScript) Selection.activeObject;
		var scriptClass = scriptFile.GetClass();
		if ((scriptClass == null) || ! scriptClass.IsSubclassOf(typeof(ScriptableObject))) {
			return false;
		}
		
		return true;
	}
	
	/**
	 * @brief Creates a custom asset from a selected script.
	 * 
	 * The asset is created in the Assets root folder with a default generic name.
	 * 
	 * The method will fail with an exception if #ValidateCreateCustomAsset() fails.
	 * 
	 */
	[MenuItem("Assets/Create/Custom asset", false)]
	static public void CreateCustomAsset()
	{
		// determine script type and validate
		
		MonoScript scriptFile = (MonoScript) Selection.activeObject;
		var scriptClass = scriptFile.GetClass();
		if ((scriptClass == null) || ! scriptClass.IsSubclassOf(typeof(ScriptableObject))) {
			throw new InvalidOperationException("Custom assets can only be created from ScriptableObjects!");
		}
		
    	// create asset
    	
    	var assetFilePath = AssetDatabase.GenerateUniqueAssetPath("Assets/Custom Asset.asset");
    	var asset = ScriptableObject.CreateInstance(scriptClass);
    	AssetDatabase.CreateAsset(asset, assetFilePath);
    	
    	// commit
    	
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
	}
	
	/**
	 * @brief A routine that helps user to conveniently copy a script template to the project
	 *	from an external location.
	 *
	 */
	static public void ImportScriptTemplate()
	{
		string srcFile = EditorUtility.OpenFilePanel("Select Template", "", TEMPLATE_EXTENSION.Substring(1));
		if (srcFile.Length == 0) {
			return;
		}
		
		string destFile = EditorUtility.SaveFilePanelInProject("Import As", Path.GetFileNameWithoutExtension(srcFile),
			TEMPLATE_EXTENSION.Substring(1), "Choose location where the imported template should be placed.");
		if (destFile.Length == 0) {
			return;
		}
		
		try {
			var destFileInfo = new FileInfo(destFile);
			if (destFileInfo.Exists) {
				string backUpName = AssetDatabase.GenerateUniqueAssetPath(destFile);
				string result = AssetDatabase.MoveAsset(destFile, backUpName);
				if (result.Length > 0) {
					throw new InvalidOperationException("The asset at path '" + destFile + "' couldn't be renamed. "
						+ "Overwrite is not possible! Reason: " + result);
				}
			}
			
			FileUtil.CopyFileOrDirectory(srcFile, destFile);
			AssetDatabase.ImportAsset(destFile);
			var asset = AssetDatabase.LoadMainAssetAtPath(destFile);
			AssetDatabase.SetLabels(asset, new string[] {"Script_Template"});
		}
		catch (Exception _e) {
			EditorUtility.DisplayDialog("Failed To Import Template!", "The selected script template couldn't be imported into project."
				+ " Please, check log for details.", "OK");
			throw _e;
		}
	}	
		
	/**
	 * @brief Returns an extension for a new script, parsed from the supplied template name.
	 * 
	 * @param templatePath a path or a file name of the script template
	 * @param includeDot if true, will prepend a '.' to the returned extension if not empty
	 *
	 * @return extension of the new script
	 *
	 */
	static public string GetScriptExt(string _templatePath, bool _includeDot)
	{
		if (! _templatePath.EndsWith(TEMPLATE_EXTENSION)) {
			return Path.GetExtension(_templatePath);
		}
		
		var fileName = Path.GetFileName(_templatePath);
		var indexOfExt = fileName.IndexOf('.');
		var targetExt = fileName.Substring(indexOfExt, fileName.Length - TEMPLATE_EXTENSION.Length - indexOfExt);
		if (! _includeDot && (targetExt.Length > 0)) {
			targetExt = targetExt.Substring(1);
		}
		return targetExt;
	}
	
    #endregion
		
    #region static public: properties
	
	/**
	 * @brief File extension of script templates.
	 */
	static public readonly string TEMPLATE_EXTENSION = ".txt";
		
	/**
	 * @brief The file name of a new script.
	 */
	static public readonly string NEW_SCRIPT_NAME = "NewScript";

	/**
	 * @brief Script templates are processed only when placed to this folder.
	 */
	static public readonly string TEMPLATE_FOLDER = "ScriptTemplates";
	
    #endregion
		
  #endregion

  #region static protected
		
    #region static protected: constructors and destructors
		
	
		
    #endregion
		
    #region static protected: methods
		
	
		
    #endregion
		
    #region static protected: properties
		
	
		
    #endregion
		
  #endregion

  #region static private
		
    #region static private: constructors and destructors
		
		
		
    #endregion
		
    #region static private: methods
	
	/**
	 * @brief Modifies the supplied path, so the file name could be used as a class name.
	 *
	 * That means the raw file name (without extension) must adhere to the formatting rules 
	 * of identifiers. Only letters, underscores '_', and digits, except as a first character,
	 * are allowed. All other characters (including spaces) are left out.
	 * 
	 * @param fileName a path to verify
	 *
	 * @return Full modified path. If the file name is empty after validation, only directory
	 *	with the trailing "/" is returned.
	 *
	 */
	static string ProcessScriptName(string _fileName)
	{	
		_fileName = _fileName.Trim();
			
		StringBuilder validName = new StringBuilder(Path.GetFileName(_fileName).Length);
		string nameWithoutExt = Path.GetFileNameWithoutExtension(_fileName);
		foreach (var letter in nameWithoutExt) {
			if (Char.IsLetter(letter) || (letter == '_')) {
				validName.Append(letter);
			}
			else if ((validName.Length > 0) && Char.IsDigit(letter)) {
				validName.Append(letter);
			}
		}
		
		if (validName.Length > 0) {
			validName.Append(Path.GetExtension(_fileName));
		}
		
		string dir = string.IsNullOrEmpty(_fileName) ? "" : Path.GetDirectoryName(_fileName);
		if (dir.Length == 0) {
			return validName.ToString();
		}
		else {
			return dir + "/" + validName.ToString();
		}
	}
	
	/**
	 * @brief Actually creates the script, replaces parameters and checks for overwrites. If a file exists on the given path,
	 *	it is renamed. All parameters are taken "as is" and no post-processing is performed on them.
	 *
	 * @param _template The path to the source template.
	 * @param _targetPath Target file name for the new script.
	 * @param _replacedSymbols Tokens to replace in the created script.
	 *
	 */
	static void DoCreateScriptFromTemplate(string _template, string _targetPath, Dictionary<string, string> _replacedSymbols)
	{
		// handle overwrite
		if (AssetDatabase.ValidateMoveAsset(_template, _targetPath) != string.Empty) {
			string backUpName = AssetDatabase.GenerateUniqueAssetPath(_targetPath);
			string result = AssetDatabase.MoveAsset(_targetPath, backUpName);
			if (result.Length > 0) {
				throw new InvalidOperationException("The asset at path '" + _targetPath + "' couldn't be renamed. "
					+ "Overwrite is not possible! Reason: " + result);
			}
		}

		// At this point, the target path is available. Do create the script already.
		// And replace special symbols.
										
		using (StreamWriter scriptFile = File.CreateText(_targetPath))
		using (StreamReader templateFile = File.OpenText(_template)) {
			string line = templateFile.ReadLine();
			while (line != null) {
				foreach (string symbol in _replacedSymbols.Keys) {
					line = line.Replace(symbol, _replacedSymbols[symbol]);
				}
				scriptFile.WriteLine(line);
				line = templateFile.ReadLine();
			}
		}
		
		// Import the new script
		
		AssetDatabase.ImportAsset(_targetPath);
		
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(_targetPath);
	}
	
	/**
	 * @brief Scans the given template and returns all unique tokens in format #[_A-Z][_A-Z0-9]*#.
	 *
	 * @param[out] _specialSymbols Collected tokens. If a token is already present, it is not modified.
	 * @param _template the path to the template
	 *
	 */
	static void ScanTemplateSymbols(Dictionary<string, string> _specialSymbols, string _template)
	{
		Regex specSymbolRegEx = new Regex(@"#[_A-Z][_A-Z0-9]*#");
		using (StreamReader templateFile = File.OpenText(_template)) {
			string line = templateFile.ReadLine();
			while (line != null) {
				Match m = specSymbolRegEx.Match(line);
				while (m.Success) {
					if (! _specialSymbols.ContainsKey(m.Value)) {
						_specialSymbols.Add(m.Value, m.Value);
					}
					m = m.NextMatch();
				}

				line = templateFile.ReadLine();
			}
		}
	}
	
    #endregion
		
    #region static private: properties
		

								
    #endregion
		
  #endregion
		
#endregion
		
#region instance members
		
  #region public
		
    #region public constructors and destructors
		
	
		
    #endregion
		
    #region public methods
		
	
		
    #endregion
		
    #region public properties

				
		
    #endregion
		
  #endregion

  #region protected
		
    #region protected constructors and destructors
		
	
		
    #endregion
		
    #region protected methods
		
	
		
    #endregion
		
    #region protected properties
		
	
		
    #endregion
		
  #endregion

  #region private
		
    #region private constructors and destructors
		
	
		
    #endregion
		
    #region private methods
	
	

    #endregion
		
    #region private properties


				
    #endregion
		
  #endregion
		
#endregion
}
	
}  /* namespace EditorLib */

/* EOF */