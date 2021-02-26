# Exchange PowerShell Automation Sample

This sample application shows how to connect to a remote Exchange Powershell session, run cmdlets, and receive data.  It also shows how variables can be passed into the PowerShell session from the C# code (currently only PSCredential is supported for the variable type).

The code shows how to connect to the PowerShell session directly (which means that only limited calls can be made) or to import the remote session into a local PowerShell session (in which case full scripting should be possible).
