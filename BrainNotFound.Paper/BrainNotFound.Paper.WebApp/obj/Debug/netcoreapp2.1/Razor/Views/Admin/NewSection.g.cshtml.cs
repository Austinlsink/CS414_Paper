#pragma checksum "C:\Users\123630\Desktop\Paper\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\NewSection.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f772c150641db5684a7c9b80b0e382ca3f49dbfe"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_NewSection), @"mvc.1.0.view", @"/Views/Admin/NewSection.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Admin/NewSection.cshtml", typeof(AspNetCore.Views_Admin_NewSection))]
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
#line 1 "C:\Users\123630\Desktop\Paper\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\_ViewImports.cshtml"
using BrainNotFound.Paper.WebApp;

#line default
#line hidden
#line 2 "C:\Users\123630\Desktop\Paper\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\_ViewImports.cshtml"
using BrainNotFound.Paper.WebApp.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f772c150641db5684a7c9b80b0e382ca3f49dbfe", @"/Views/Admin/NewSection.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f48f96d82608096bf852c265cc2bc51ad1ced1e0", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_NewSection : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Users\123630\Desktop\Paper\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\NewSection.cshtml"
  
    ViewData["Title"] = "Add Section Info - Admin";

#line default
#line hidden
            BeginContext(60, 4, true);
            WriteLiteral("<h2>");
            EndContext();
            BeginContext(65, 17, false);
#line 4 "C:\Users\123630\Desktop\Paper\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\NewSection.cshtml"
Write(ViewData["Title"]);

#line default
#line hidden
            EndContext();
            BeginContext(82, 11, true);
            WriteLiteral("</h2>\r\n<h3>");
            EndContext();
            BeginContext(94, 19, false);
#line 5 "C:\Users\123630\Desktop\Paper\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\NewSection.cshtml"
Write(ViewData["Message"]);

#line default
#line hidden
            EndContext();
            BeginContext(113, 183, true);
            WriteLiteral("</h3>\r\n\r\n<p>This page allows a new section to be created with the following information: ID, \tcourse ID, user ID, section number, start time and end time, location, and capacity.</p> ");
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
