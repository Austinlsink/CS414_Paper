#pragma checksum "C:\Users\130532\Desktop\PaperBrainProject\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\NewInstructor.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "91023ff59a198fe7bc4a31fbb665c005baca5b6a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_NewInstructor), @"mvc.1.0.view", @"/Views/Admin/NewInstructor.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Admin/NewInstructor.cshtml", typeof(AspNetCore.Views_Admin_NewInstructor))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\130532\Desktop\PaperBrainProject\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\_ViewImports.cshtml"
using BrainNotFound.Paper.WebApp;

#line default
#line hidden
#line 2 "C:\Users\130532\Desktop\PaperBrainProject\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\_ViewImports.cshtml"
using BrainNotFound.Paper.WebApp.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"91023ff59a198fe7bc4a31fbb665c005baca5b6a", @"/Views/Admin/NewInstructor.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f48f96d82608096bf852c265cc2bc51ad1ced1e0", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_NewInstructor : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Users\130532\Desktop\PaperBrainProject\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\NewInstructor.cshtml"
  
    ViewData["Title"] = "Add Instructor Info- Admin";

#line default
#line hidden
            BeginContext(62, 4, true);
            WriteLiteral("<h2>");
            EndContext();
            BeginContext(67, 17, false);
#line 4 "C:\Users\130532\Desktop\PaperBrainProject\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\NewInstructor.cshtml"
Write(ViewData["Title"]);

#line default
#line hidden
            EndContext();
            BeginContext(84, 11, true);
            WriteLiteral("</h2>\r\n<h3>");
            EndContext();
            BeginContext(96, 19, false);
#line 5 "C:\Users\130532\Desktop\PaperBrainProject\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\NewInstructor.cshtml"
Write(ViewData["Message"]);

#line default
#line hidden
            EndContext();
            BeginContext(115, 13, true);
            WriteLiteral("</h3>\r\n\r\n<h5>");
            EndContext();
            BeginContext(129, 18, false);
#line 7 "C:\Users\130532\Desktop\PaperBrainProject\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\NewInstructor.cshtml"
Write(ViewData["Errors"]);

#line default
#line hidden
            EndContext();
            BeginContext(147, 194, true);
            WriteLiteral("</h5>\r\n\r\n<p>This page allows a new instructor to be created with the following information: ID, \tfirst name, last name, salutation, email, password, phone number, user type, and \tsection.</p>\r\n ");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
