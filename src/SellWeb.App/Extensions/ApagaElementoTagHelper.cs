using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace SellWeb.App.Extensions
{
    [HtmlTargetElement("*", Attributes = "omite-por-login")]
    public class ApagaElementoTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ApagaElementoTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HtmlAttributeName("omite-por-login")]
        public string Elemento { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            var podeVer = VerificaLogin.EstaLogado(_contextAccessor.HttpContext);

            if (podeVer) return;

            output.SuppressOutput();
        }
    }

    public class VerificaLogin
    {
        public static bool EstaLogado(HttpContext context)
        {
            return context.User.Identity.IsAuthenticated;
        }
    }
}
