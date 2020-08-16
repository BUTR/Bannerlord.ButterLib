The last stage. ButterLib will create a permanent ``IServiceProvider`` for usage.    
``SubModule.GetServices`` will not be available anymore, thus disabling any configuration/service registering.
  
There's also a substage. When Creating/Loading a Campaign, the ``IServiceProvider`` will create a scope that will span across the whole Campiagn. Should be userful for per-campaign services.