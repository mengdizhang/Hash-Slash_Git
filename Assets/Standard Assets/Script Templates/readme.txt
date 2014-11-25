How To Use
==========

1. Create a script template, place it to a "ScriptTemplates" folder anywhere in the "Assets" folder of your project, and open your project in Unity. The script template must have the ".txt" extension.

(The most common usage scenario at first would probably be like this:
i. Create a script the standard Unity way.
ii. Modify it to your liking.
iii. Find it in a system file browser (Total Commander, Windows Explorer, Mac Finder, etc.), not Unity, so you can change the file extension.
iv. *Append* ".txt" to the current file extension, e.g. if your file was named "MyScript.cs", now it should be "MyScript.cs.txt".
v. Go back to Unity. Done. 
Now you can use it from the context menu in the Project view.)

2. Right-click a folder in the Project view where you want to create a new script. Select "Create -> Script from template...".

3. Follow instructions from there. A dialog should appear. If it doesn't, wait a little.

4. Voila! You have created a new script.

Note: You can add custom parameters to your templates, for example, #NAMESPACE#, #AUTHOR#, #BASE_CLASS#... Anything that's enclosed in ## and can be regarded as an upper-case identifier, i.e. matches the pattern #[_A-Z][_A-Z0-9]*#. You will be asked to supply values for your parameters when creating a script, but otherwise they are not treated in a special way. The only special parameter is #SCRIPTNAME#, which translates to the name of the script file without extension.

See included script templates in "Assets/Script Templates/Editor/ScriptTemplates" for examples.


Contacts
========

In case of any issues write to: support@equinoxcreations.com

(c) 2013, Equinox Creations