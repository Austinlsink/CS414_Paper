#pragma checksum "C:\Users\123630\Desktop\Paper\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\Departments.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8ec95b61f2a6ec1b2482078255c92b83ef0a9ed2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_Departments), @"mvc.1.0.view", @"/Views/Admin/Departments.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Admin/Departments.cshtml", typeof(AspNetCore.Views_Admin_Departments))]
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
#line 1 "C:\Users\123630\Desktop\Paper\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\Departments.cshtml"
using BrainNotFound.Paper.WebApp.Models.BusinessModels;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8ec95b61f2a6ec1b2482078255c92b83ef0a9ed2", @"/Views/Admin/Departments.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f48f96d82608096bf852c265cc2bc51ad1ced1e0", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_Departments : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<Department>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "NewDepartment", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Admin", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-dark"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "EditDepartment", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(82, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 4 "C:\Users\123630\Desktop\Paper\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\Departments.cshtml"
  
    ViewData["Title"] = "Department Info - Admin";
    int rowIndex = 0;

#line default
#line hidden
            BeginContext(166, 51, true);
            WriteLiteral("\r\n\r\n\r\n    <h1>Deptartments</h1>\r\n    <hr />\r\n\r\n    ");
            EndContext();
            BeginContext(217, 95, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ace985b5ebb7421794b8a3deff559683", async() => {
                BeginContext(291, 17, true);
                WriteLiteral("Create Department");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(312, 1418, true);
            WriteLiteral(@"

    <div class=""col-md-12 col-sm-12 col-xs-12"">
        <div class=""x_panel dept_table"">
            <div class=""x_content"">

                <p>Click checkbox to select multiple departments.</p>

                <div class=""table-responsive"">
                    <table class=""table table-striped jambo_table bulk_action"">
                        <thead>
                            <tr class=""headings"">
                                <th>
                                    <input type=""checkbox"" id=""check-all"" class=""flat"">
                                </th>
                                <th class=""column-title"">Dept. Name </th>
                                <th class=""column-title"">Dept. Code </th>
                                <th class=""column-title no-link last""><span class=""nobr"">Action</span>
                                <th class=""column-title no-link last"">
                                    <span class=""nobr"">Action</span>
                                </th>
   ");
            WriteLiteral(@"                             <th class=""bulk-actions"" colspan=""7"">
                                    <a class=""antoo"" style=""color:#fff; font-weight:500;"">Bulk Actions ( <span class=""action-cnt""> </span> ) <i class=""fa fa-chevron-down""></i></a>
                                </th>
                            </tr>
                        </thead>

                        <tbody>

");
            EndContext();
#line 43 "C:\Users\123630\Desktop\Paper\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\Departments.cshtml"
                             foreach (Department department in Model)
                            {


#line default
#line hidden
            BeginContext(1834, 310, true);
            WriteLiteral(@"                                <tr class=""odd pointer"">
                                    <td class=""a-center "">
                                        <input type=""checkbox"" class=""flat"" name=""table_records"">
                                    </td>
                                    <td class="" "">");
            EndContext();
            BeginContext(2145, 25, false);
#line 50 "C:\Users\123630\Desktop\Paper\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\Departments.cshtml"
                                             Write(department.DepartmentName);

#line default
#line hidden
            EndContext();
            BeginContext(2170, 58, true);
            WriteLiteral(" </td>\r\n                                    <td class=\" \">");
            EndContext();
            BeginContext(2229, 25, false);
#line 51 "C:\Users\123630\Desktop\Paper\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\Departments.cshtml"
                                             Write(department.DepartmentCode);

#line default
#line hidden
            EndContext();
            BeginContext(2254, 104, true);
            WriteLiteral(" </td>\r\n                                    <td class=\" last\">\r\n                                        ");
            EndContext();
            BeginContext(2358, 62, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "5c26c6905f524547b61811e560fe8343", async() => {
                BeginContext(2412, 4, true);
                WriteLiteral("Edit");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2420, 247, true);
            WriteLiteral("\r\n                                    </td>\r\n                                    <td class=\" last\">\r\n                                        <a href=\"#\">Remove</a>\r\n                                    </td>\r\n                                </tr>\r\n");
            EndContext();
#line 59 "C:\Users\123630\Desktop\Paper\BrainNotFound.Paper\BrainNotFound.Paper.WebApp\Views\Admin\Departments.cshtml"

                                rowIndex++;
                            }

#line default
#line hidden
            BeginContext(2745, 136, true);
            WriteLiteral("\r\n                        </tbody>\r\n                    </table>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<Department>> Html { get; private set; }
    }
}
#pragma warning restore 1591