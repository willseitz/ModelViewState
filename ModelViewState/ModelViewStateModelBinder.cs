using System;
using System.Web.Mvc;

namespace OpenSource.ModelViewState
{
	public class ModelViewStateModelBinder : DefaultModelBinder
	{
		protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext,
		                                      Type modelType)
		{
			string modelName = bindingContext.ModelType.FullName;
			ValueProviderResult clientObject = bindingContext.ValueProvider.GetValue(modelName);
			return clientObject == null
			       	? base.CreateModel(controllerContext, bindingContext, modelType)
			       	: ModelCompressor.Decompress(bindingContext.ModelType, clientObject.AttemptedValue);
		}
	}
}
