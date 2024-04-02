This is ImGui.NET with our own native callbacks - we converted them to dynamic function pointers.  
One of the reasons is to to be always able to control where cimgui.dll is loaded from.  
Another is the `calli` optimization from function pointers.  