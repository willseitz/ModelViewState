using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace OpenSource.ModelViewState
{
	public static class HtmlHelperExtension
	{
		public static MvcHtmlString StoreModel(this HtmlHelper htmlHelper, object model)
		{
			return htmlHelper.Hidden(model.GetType().ToString(), ModelCompressor.Compress(model));
		}
	}
}

