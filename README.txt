This implements ViewState for a Model using Microsoft .Net MVC. 

The primary purpose is to eliminate a lot of "HiddenFor" objects and
to allow seamless use of a model between the GET and POST.

Four steps are required to use this.

1) Add references to the dll and but the dll in your bin.

2) Make your model serializable via the [Serializable] attribute.
   I may remove this requirement in the future.
   
3) Set your default model binder. If you are already using custom 
   binders you will want to take a look at the code to make sure there
   are no conflicts.
   So in Global.asax.cs do:
   ModelBinders.Binders.DefaultBinder = new ModelViewStateModelBinder();
   You will need to include "using OpenSource.ModelViewState;"
   
4) In the View do the following:
   @Html.StoreModel(Model)
   You will need to include "@using OpenSource.ModelViewState"
   I have only done this using Razor and if you want to use a different
   View engine you may need to expand this.