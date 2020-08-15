This is the intermediate stage when SubModules register their services.  
  
Because the IServiceProvider is not yet available, it is not possible to the the full ``ILogger`` implementation at this stage.
You can call ``SubModule.GetTempServiceProvider`` to get the temporary IServiceProvider and resolve ``ILogger`` while logging anything at this stage.  
Do not forget to replace your ``ILogger`` at Stage 3.