#pragma checksum "C:\Users\130532\Desktop\v7\BrainNotFound.Paper.WebApp\Views\Instructor\Profile.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "49e05586854279bb2e53d7c78c3334ba884233ba"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Instructor_Profile), @"mvc.1.0.view", @"/Views/Instructor/Profile.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Instructor/Profile.cshtml", typeof(AspNetCore.Views_Instructor_Profile))]
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
#line 1 "C:\Users\130532\Desktop\v7\BrainNotFound.Paper.WebApp\Views\_ViewImports.cshtml"
using BrainNotFound.Paper.WebApp;

#line default
#line hidden
#line 2 "C:\Users\130532\Desktop\v7\BrainNotFound.Paper.WebApp\Views\_ViewImports.cshtml"
using BrainNotFound.Paper.WebApp.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"49e05586854279bb2e53d7c78c3334ba884233ba", @"/Views/Instructor/Profile.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f48f96d82608096bf852c265cc2bc51ad1ced1e0", @"/Views/_ViewImports.cshtml")]
    public class Views_Instructor_Profile : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Users\130532\Desktop\v7\BrainNotFound.Paper.WebApp\Views\Instructor\Profile.cshtml"
  
    ViewData["Title"] = "Profile - Instructor";

#line default
#line hidden
            BeginContext(56, 4, true);
            WriteLiteral("<h2>");
            EndContext();
            BeginContext(61, 17, false);
#line 4 "C:\Users\130532\Desktop\v7\BrainNotFound.Paper.WebApp\Views\Instructor\Profile.cshtml"
Write(ViewData["Title"]);

#line default
#line hidden
            EndContext();
            BeginContext(78, 11, true);
            WriteLiteral("</h2>\r\n<h3>");
            EndContext();
            BeginContext(90, 19, false);
#line 5 "C:\Users\130532\Desktop\v7\BrainNotFound.Paper.WebApp\Views\Instructor\Profile.cshtml"
Write(ViewData["Message"]);

#line default
#line hidden
            EndContext();
            BeginContext(109, 122, true);
            WriteLiteral("</h3>\r\n\r\n<p>This page displays the instructor’s profile information and allows the instructor to reset his password.</p>\r\n");
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
