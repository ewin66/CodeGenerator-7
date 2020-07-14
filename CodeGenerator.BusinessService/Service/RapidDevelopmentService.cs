using CodeGenerator.Entity.Base_SysManage;
using CodeGenerator.Util;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CodeGenerator.BusinessService.IService;

namespace CodeGenerator.BusinessService.Base_SysManage
{
    public class RapidDevelopmentService : BaseService<Base_DatabaseLink>, IRapidDevelopmentService
    {
        static RapidDevelopmentService()
        {
            _contentRootPath = AutofacHelper.GetService<IHostingEnvironment>().ContentRootPath;
        }

        private DbHelper _dbHelper { get; set; }

        private Dictionary<string, DbTableInfo> _dbTableInfoDic { get; set; } = new Dictionary<string, DbTableInfo>();

        private static string _contentRootPath { get; }


        #region 外部接口

        /// <summary>
        /// 获取所有数据库连接
        /// </summary>
        /// <returns></returns>
        public List<Base_DatabaseLink> GetAllDbLink()
        {
            return GetList();
        }

        /// <summary>
        /// 获取数据库所有表
        /// </summary>
        /// <param name="linkId">数据库连接Id</param>
        /// <returns></returns>
        public List<DbTableInfo> GetDbTableList(string linkId)
        {
            if (linkId.IsNullOrEmpty())
                return new List<DbTableInfo>();
            else
                return GetTheDbHelper(linkId).GetDbAllTables();
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="linkId">连接Id</param>
        /// <param name="areaName">区域名</param>
        /// <param name="tables">表列表</param>
        /// <param name="buildType">需要生成类型</param>
        public void BuildCode(string linkId, string areaName, string tables, string buildType, string project, string path)
        {
            //内部成员初始化
            _dbHelper = GetTheDbHelper(linkId,out string linkName);
            GetDbTableList(linkId).ForEach(aTable =>
            {
                _dbTableInfoDic.Add(aTable.TableName, aTable);
            });

            List<string> tableList = tables.ToList<string>();
            List<string> buildTypeList = buildType.ToList<string>();
            tableList.ForEach(aTable =>
            {
                var tableFieldInfo = _dbHelper.GetDbTableInfo(aTable, linkName);

                if (project == "Small")
                {
                    //实体层
                    if (buildTypeList.Exists(x => x.ToLower() == "entity"))
                    {
                        BuildSmallEntity(tableFieldInfo, areaName, aTable, path);
                    }
                    //业务层
                    if (buildTypeList.Exists(x => x.ToLower() == "business"))
                    {
                        BuildSmallBusiness(tableFieldInfo, areaName, aTable, path);
                    }
                    //控制器
                    if (buildTypeList.Exists(x => x.ToLower() == "controller"))
                    {
                        BuildSmallController(tableFieldInfo, areaName, aTable, path);
                    }
                    ////视图
                    //if (buildTypeList.Exists(x => x.ToLower() == "view"))
                    //{
                    //    BuildView(tableFieldInfo, areaName, aTable);
                    //}
                }
                else if (project == "Wms")
                {
                    //实体层
                    if (buildTypeList.Exists(x => x.ToLower() == "entity"))
                    {
                        BuildWmsEntity(tableFieldInfo, areaName, aTable, path);
                    }
                    //dto层
                    if (buildTypeList.Exists(x => x.ToLower() == "dto"))
                    {
                        BuildWmsDto(tableFieldInfo, areaName, aTable, path);
                    }
                    //业务层
                    if (buildTypeList.Exists(x => x.ToLower() == "business"))
                    {
                        BuildWmsIService(tableFieldInfo, areaName, aTable, path);
                        BuildWmsService(tableFieldInfo, areaName, aTable, path);
                    }
                    //控制器
                    if (buildTypeList.Exists(x => x.ToLower() == "controller"))
                    {
                        BuildWmsController(tableFieldInfo, areaName, aTable, path);
                    }
                    //视图
                    if (buildTypeList.Exists(x => x.ToLower() == "view"))
                    {
                        BuildWmsView(tableFieldInfo, areaName, aTable, path);
                    }
                }
                else if (project == "League")
                {
                    //实体层
                    if (buildTypeList.Exists(x => x.ToLower() == "entity"))
                    {
                        BuildLeagueEntity(tableFieldInfo, areaName, aTable, path);
                    }
                    //dto层
                    if (buildTypeList.Exists(x => x.ToLower() == "dto"))
                    {
                        BuildLeagueDto(tableFieldInfo, areaName, aTable, path);
                    }
                    //业务层
                    if (buildTypeList.Exists(x => x.ToLower() == "business"))
                    {
                        BuildLeagueIService(tableFieldInfo, areaName, aTable, path);
                        BuildLeagueService(tableFieldInfo, areaName, aTable, path);
                    }
                    //控制器
                    if (buildTypeList.Exists(x => x.ToLower() == "controller"))
                    {
                        BuildLeagueController(tableFieldInfo, areaName, aTable, path);
                    }
                    //视图
                    if (buildTypeList.Exists(x => x.ToLower() == "view"))
                    {
                        BuildLeagueView(tableFieldInfo, areaName, aTable, path);
                    }
                }
                else
                {

                    //实体层
                    if (buildTypeList.Exists(x => x.ToLower() == "entity"))
                    {
                        BuildEntity(tableFieldInfo, areaName, aTable);
                    }
                    //业务层
                    if (buildTypeList.Exists(x => x.ToLower() == "business"))
                    {
                        BuildBusiness(tableFieldInfo, areaName, aTable);
                    }
                    //控制器
                    if (buildTypeList.Exists(x => x.ToLower() == "controller"))
                    {
                        BuildController(tableFieldInfo, areaName, aTable);
                    }
                    //视图
                    if (buildTypeList.Exists(x => x.ToLower() == "view"))
                    {
                        BuildView(tableFieldInfo, areaName, aTable);
                    }
                }
            });
        }

        #endregion

        #region Reedy

        /// <summary>
        /// 生成实体
        /// </summary>
        /// <param name="tableInfo">表字段信息</param>
        /// <param name="areaName">区域名</param>
        /// <param name="tableName">表名</param>
        private void BuildEntity(List<TableInfo> tableInfo, string areaName, string tableName)
        {
            string entityPath = _contentRootPath.Replace("CodeGenerator.Web", "CodeGenerator.Entity");
            string filePath = Path.Combine(entityPath, "Entities", areaName, $"{tableName}.cs");
            string nameSpace = $@"CodeGenerator.Entity.{areaName}";

            _dbHelper.SaveEntityToFile(tableInfo, tableName, _dbTableInfoDic[tableName].Description, filePath, nameSpace);


            string dtoPath = _contentRootPath.Replace("CodeGenerator.Web", "CodeGenerator.Entity");
            string dtofilePath = Path.Combine(dtoPath, "Dto", $"{areaName}_Dto", $"{tableName}Dto.cs");

            _dbHelper.SaveDtoToFile(tableInfo, tableName, _dbTableInfoDic[tableName].Description, dtofilePath, nameSpace);

        }

        /// <summary>
        /// 生成业务逻辑代码
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildBusiness(List<TableInfo> tableFieldInfo, string areaName, string entityName)
        {

            string createTimeBusiness = null;
            StringBuilder boolTypeBusiness = new StringBuilder();
            string usingEnumBusiness = tableFieldInfo.Any(x => x.Name.StartsWith("Is")) ? $@"using CodeGenerator.Entity.Enums;" : null;

            tableFieldInfo.Where(x => x.Name.Equals("CreateTime")).ForEach((aField, index) =>
           {
               //创建时间
               createTimeBusiness = $@"newData.CreateTime = DateTime.Now;";
           });

            tableFieldInfo.Where(x => x.Name.StartsWith("Is")).ForEach((aField, index) =>
            {
                //搜索的下拉选项
                Type fieldType = _dbHelper.DbTypeStr_To_CsharpType(aField.Type);
                string newOption = $@"list.ForEach(e => {{ e.{aField.Name.Substring(2, aField.Name.Length - 2)}Value = EnumExtension.GetEnumDescription(((EnumWhether)Enum.ToObject(typeof(EnumWhether), e.{aField.Name}))); }});
";
                boolTypeBusiness.Append(newOption);
            });


            string code =
$@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using CodeGenerator.Util;
using CodeGenerator.Entity.Dto;
using CodeGenerator.Entity.{areaName};
using CodeGenerator.BusinessService.IService;
{usingEnumBusiness}

namespace CodeGenerator.BusinessService.{areaName}
{{
    public class {entityName}Service : BaseService<{entityName}>, I{entityName}Service
    {{
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name=""condition"">查询类型</param>
        /// <param name=""keyword"">关键字</param>
        /// <returns></returns>
        public List<{entityName}Dto> GetDataList(string condition, string keyword, Pagination pagination)
        {{
            var q = GetIQueryable();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@""{{condition}}.Contains(@0)"", keyword);
            var list = q.GetPagination(pagination).ToList().MapTo<{entityName}Dto>();
            {boolTypeBusiness.ToString()}
            return list;
        }}

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name=""id"">主键</param>
        /// <returns></returns>
        public {entityName}Dto GetTheData(string id)
        {{
            var model = GetEntity(id).MapTo<{entityName}Dto>(); ;
            return model;
        }}

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name=""newData"">数据</param>
        public int AddData({entityName}Dto newData)
        {{
            newData.{tableFieldInfo[0].Name} = Guid.NewGuid().ToSequentialGuid();
            {createTimeBusiness}

            var result = Insert(newData);
            if (result == 0)
                throw new Exception(""添加失败！"");

            return result;
        }}

        /// <summary>
        /// 更新数据
        /// </summary>
        public int UpdateData({entityName}Dto theData)
        {{
            var result = Update(theData);
            if (result == 0)
                throw new Exception(""更新失败！"");

            return result;
        }}

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name=""theData"">删除的数据</param>
        public int DeleteData(List<string> ids)
        {{
            var result = Delete(ids);
            if (result == 0)
                throw new Exception(""删除失败！"");

            return result;
        }}

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }}
}}";
            string businessPath = _contentRootPath.Replace("CodeGenerator.Web", "CodeGenerator.BusinessService");
            string filePath = Path.Combine(businessPath, "Service", $"{entityName}Service.cs");

            FileHelper.WriteTxt(code, filePath, FileMode.Create);

            string icode = $@"
using System.Collections.Generic;
using CodeGenerator.Util;
using CodeGenerator.Entity.Dto;

namespace CodeGenerator.BusinessService.IService
{{
    public interface I{entityName}Service
    {{
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name=""condition"" >查询类型</param>
        /// <param name=""keyword"" >关键字</param>
        /// <returns></returns>
        List<{entityName}Dto> GetDataList(string condition, string keyword, Pagination pagination);

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name=""id"" >主键</param>
        /// <returns></returns>
        {entityName}Dto GetTheData(string id);


        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name=""newData"" >数据</param>
        int AddData({entityName}Dto newData);

        /// <summary>
        /// 更新数据
        /// </summary>
        int UpdateData({entityName}Dto theData);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name=""theData"" >删除的数据</param>
        int DeleteData(List<string> ids);
        #endregion
       
    }}     
}}";

            string ibusinessPath = _contentRootPath.Replace("CodeGenerator.Web", "CodeGenerator.BusinessService");
            string ifilePath = Path.Combine(ibusinessPath, "IService", $"I{entityName}Service.cs");

            FileHelper.WriteTxt(icode, ifilePath, FileMode.Create);


        }

        /// <summary>
        /// 生成控制器代码
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildController(List<TableInfo> tableFieldInfo, string areaName, string entityName)
        {
            string varService = $@"_{entityName.ToFirstLowerStr()}Service";
            string varBusiness = $@"{entityName.ToFirstLowerStr()}Service";
            string code =
$@"using System;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.Util;
using CodeGenerator.Entity.Dto;
using CodeGenerator.BusinessService.IService;

namespace CodeGenerator.Web
{{
    //[Area(""{areaName}"")]
    public class {entityName}Controller : BaseMvcController
    {{
        private I{entityName}Service {varService} {{ get; }}
        public {entityName}Controller(I{entityName}Service {varBusiness}) {{
            {varService} = {varBusiness};
        }}
        #region 视图功能

        public ActionResult Index()
        {{
            return View();
        }}

        public ActionResult Form(string id)
        {{
            var theData = id.IsNullOrEmpty() ? new {entityName}Dto() : {varService}.GetTheData(id);

            return View(theData);
        }}

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name=""condition"">查询类型</param>
        /// <param name=""keyword"">关键字</param>
        /// <returns></returns>
        public ActionResult GetDataList(string condition, string keyword, Pagination pagination)
        {{
            var dataList = {varService}.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }}

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name=""theData"">保存的数据</param>
        public ActionResult SaveData({entityName}Dto theData)
        {{
            if(theData.{tableFieldInfo[0].Name}.IsNullOrEmpty())
            {{
                {varService}.AddData(theData);
            }}
            else
            {{
                {varService}.UpdateData(theData);
            }}

            return Success();
        }}

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name=""theData"">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {{
            {varService}.DeleteData(ids.ToList<string>());

            return Success(""删除成功！"");
        }}

        #endregion
    }}
}}";
            string filePath = Path.Combine(_contentRootPath, "Areas", areaName, "Controllers", $"{entityName}Controller.cs");
            FileHelper.WriteTxt(code, filePath, FileMode.Create);



        }

        /// <summary>
        /// 生成视图
        /// </summary>
        /// <param name="tableInfoList">表字段信息</param>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildView(List<TableInfo> tableInfoList, string areaName, string entityName)
        {
            //生成Index页面
            StringBuilder searchConditionSelectHtml = new StringBuilder();
            StringBuilder tableColsBuilder = new StringBuilder();
            StringBuilder formRowBuilder = new StringBuilder();

            tableInfoList.Where(x => !x.Name.EndsWith("Id")).ForEach((aField, index) =>
           {
               //搜索的下拉选项
               Type fieldType = _dbHelper.DbTypeStr_To_CsharpType(aField.Type);
               if (fieldType == typeof(string))
               {
                   string newOption = $@"
                <option value=""{aField.Name}"">{aField.Description}</option>";
                   searchConditionSelectHtml.Append(newOption);
               }

               //数据表格列
               string end = (index == tableInfoList.Count - 2) ? "" : ",";
               string newCol;
               if (!aField.Name.StartsWith("Is"))
               {
                   newCol = $@"
                {{ title: '{aField.Description}', field: '{aField.Name}', width: 200 }}{end}";
               }
               else
               {
                   newCol = $@"
                {{ title: '{aField.Description}', field: '{aField.Name.Substring(2, aField.Name.Length - 2)}Value', width: 200 }}{end}";
               }

               tableColsBuilder.Append(newCol);
           });

            tableInfoList.Where(x => !x.Name.EndsWith("Id") && !x.Name.EndsWith("Time")).ForEach((aField, index) =>
            {
                //Form页面中的Html
                string newFormRow;
                if (aField.Name.StartsWith("Is"))
                {
                    newFormRow = $@"
                        <tr>
                            <th>{aField.Description}</th>
                            <td>
                                <select name=""{aField.Name}"" class=""easyui - combobox"" data-options=""width: 200,value: '@obj.{aField.Name.Substring(2, aField.Name.Length - 2)}Value',required: true"" >
                                    <option value = ""false""> 否 </option>
                                    <option value = ""true""> 是 </option>
                                </ select >
                            </td>
                        </tr>";
                }
                else
                {
                    newFormRow = $@"
                        <tr>
                            <th>{aField.Description}</th>
                            <td>
                                <input name=""{aField.Name}"" value=""@obj.{aField.Name}"" class=""easyui-textbox"" data-options=""width:'200px',required:true"">
                            </td>
                        </tr>";
                }
                formRowBuilder.Append(newFormRow);
            });

            string indexHtml =
$@"@{{
    Layout = ""~/Areas/Admin/Views/Shared/_Layout_List.cshtml"";
}}

@section toolbar{{
    <a id=""add"" class=""easyui-linkbutton"" data-options=""iconCls:'icon-add'"">添加</a>
    <a id=""edit"" class=""easyui-linkbutton"" data-options=""iconCls:'icon-edit'"">修改</a>
    <a id=""delete"" class=""easyui-linkbutton"" data-options=""iconCls:'icon-remove'"">删除</a>
}}

@section search{{
    <div class=""search_wrapper"">
        <div class=""search_item"">
            <label class=""search_label"">查询类别</label>
            <select name=""condition"" class=""easyui-combobox"" data-options=""width:100"">
                <option value="""">请选择</option>
                {searchConditionSelectHtml.ToString()}
            </select>
            <input name=""keyword"" class=""easyui-textbox"" style=""width:150px"" />
        </div>
        <div class=""search_submit"">
            <a href=""javascript:;"" class=""easyui-linkbutton"" data-options=""iconCls:'icon-search'"" onclick=""searchGrid(this,'#dataTable')"">查询</a>
        </div>
    </div>
}}
<div id=""dataTable"">

</div>

<script>
    var rootUrl = '@Url.Content(""~/"")';
    var formWidth = 500;
    var formHeight = {(tableInfoList.Count - 1) * 35 + 200};

    function initTable() {{
        $('#dataTable').datagrid({{
            url: rootUrl + '{areaName}/{entityName}/GetDataList',
            method: 'POST',
            //queryParams: {{ 'id': id }},
            idField: '{tableInfoList[0].Name}',
            fit: true,
            fitColumns: true,
            singleSelect: false,
            selectOnCheck: false,
            checkOnSelect: false,
            sortName: '{tableInfoList[0].Name}',
            sortOrder: 'Desc',
            rownumbers: true,
            pagination: true,
            pageSize: 30,
            //nowrap: false,
            pageList: [10, 30, 50, 100, 200, 500, 1000],
            //showFooter: true,
            columns: [[
                {{ title: 'ck', field: 'ck', checkbox: true }},
                {tableColsBuilder.ToString()}
            ]],
            onBeforeLoad: function (param) {{

            }},
            onBeforeSelect: function () {{
                return false;
            }}
        }});
    }}

    $(function () {{
        initTable();

        //添加数据
        $('#add').click(function () {{
            dialogOpen({{
                id: 'form',
                title: '添加数据',
                width: formWidth,
                height: formHeight,
                url: rootUrl + '{areaName}/{entityName}/Form',
            }});
        }});

        //修改数据
        $('#edit').click(function () {{
            var selected = $(""#dataTable"").datagrid(""getChecked"");
            if (!selected || !selected.length) {{
                dialogError('请选择要修改的记录!');
                return;
            }}
            var id = selected[0].{tableInfoList[0].Name};

            dialogOpen({{
                id: 'form',
                title: '修改数据',
                width: formWidth,
                height: formHeight,
                url: rootUrl + '{areaName}/{entityName}/Form?id=' + id,
            }});
        }});

        //删除数据
        $('#delete').click(function () {{
            var checked = $(""#dataTable"").datagrid(""getChecked"");
            if (!checked || !checked.length) {{
                dialogError('请选择要删除的记录!');
                return;
            }}
            var ids = $.map(checked, function (item) {{
                return item['{tableInfoList[0].Name}'];
            }});

            dialogComfirm('确认删除吗？', function () {{
                $.postJSON(rootUrl + '{areaName}/{entityName}/DeleteData', {{ ids: JSON.stringify(ids) }}, function (resJson) {{
                    if (resJson.Success) {{
                        $('#dataTable').datagrid('clearSelections').datagrid('clearChecked');
                        $('#dataTable').datagrid('reload');
                        dialogMsg('删除成功!');
                    }}
                    else {{
                        dialogError(resJson.Msg);
                    }}
                }});
            }});
        }});
    }});
</script>";
            string indexPath = Path.Combine(_contentRootPath, "Areas", areaName, "Views", entityName, "Index.cshtml");

            FileHelper.WriteTxt(indexHtml, indexPath, FileMode.Create);

            //生成Form页面
            string formHtml =
$@"@using CodeGenerator.Entity.Dto;
@using CodeGenerator.Util;

@{{
    Layout = ""~/Areas/Admin/Views/Shared/_Layout_List.cshtml"";

    var obj = ({entityName}Dto)Model;
    var objStr = Html.Raw(obj.ToJson());
}}

<form id=""dataForm"" enctype=""multipart/form-data"" class=""easyui-form"" method=""post"" data-options=""novalidate:true"">
    <table class=""table_base"">
        <colgroup>
            <col style=""width:80px;"" />
        </colgroup>
        <tbody>
            {formRowBuilder.ToString()}
        </tbody>
    </table>
</form>

@section foottoolbar{{
    <a id=""saveForm"" href=""javascript:;"" class=""easyui-linkbutton"" data-options=""iconCls:'icon-save'"">保存</a>
}}

<script>
    var rootUrl = '@Url.Content(""~/"")';
    var theEntity = @objStr;

    $(function () {{
        $('#saveForm').click(function () {{
            if (!$('#dataForm').form('enableValidation').form('validate'))
                return;

            var formValues = $('#dataForm').getValues();
            $.extend(theEntity, formValues);
            $.postJSON(rootUrl + '{areaName}/{entityName}/SaveData', theEntity, function (resJson) {{
                if (resJson.Success) {{
                    parent.dialogMsg('保存成功!');
                    parent.$('#dataTable').datagrid('clearChecked').datagrid('reload');
                    parent.dialogClose('form');
                }}
                else {{
                    dialogError(resJson.Msg);
                }}
            }});
        }});
    }});
</script>
";
            string formPath = Path.Combine(_contentRootPath, "Areas", areaName, "Views", entityName, "Form.cshtml");

            FileHelper.WriteTxt(formHtml, formPath, FileMode.Create);
        }



        #endregion

        #region Small

        /// <summary>
        /// 生成实体
        /// </summary>
        /// <param name="tableInfo">表字段信息</param>
        /// <param name="areaName">区域名</param>
        /// <param name="tableName">表名</param>
        /// <param name="tableName">路径</param>
        private void BuildSmallEntity(List<TableInfo> tableInfo, string areaName, string tableName, string path)
        {
            string entityPath = _contentRootPath.Replace("OpenAuth.Web", "OpenAuth.Entity");
            string filePath = Path.Combine(path, "OpenAuth.Domain", "Entitys", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(tableName.Replace("small_", "")).Replace("_", "")}.cs");
            string nameSpace = $@"OpenAuth.Domain.Entitys";

            _dbHelper.SaveSmallEntityToFile(tableInfo, tableName, _dbTableInfoDic[tableName].Description, filePath, nameSpace);

            string dtoPath = _contentRootPath.Replace("OpenAuth.Web", "OpenAuth.Entity");
            string dtofilePath = Path.Combine(path, "OpenAuth.Domain", "Dto", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(tableName.Replace("small_", "")).Replace("_", "")}Dto.cs");

            _dbHelper.SaveSmallDtoToFile(tableInfo, tableName, _dbTableInfoDic[tableName].Description, dtofilePath, nameSpace);

        }

        /// <summary>
        /// 生成业务逻辑代码
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildSmallBusiness(List<TableInfo> tableFieldInfo, string areaName, string entityName, string path)
        {

            string createTimeBusiness = null;
            StringBuilder boolTypeBusiness = new StringBuilder();
            string usingEnumBusiness = tableFieldInfo.Any(x => x.Name.StartsWith("Is")) ? $@"using OpenAuth.Domain.Enums;" : null;

            tableFieldInfo.Where(x => x.Name.Equals("CreateTime")).ForEach((aField, index) =>
            {
                //创建时间
                createTimeBusiness = $@"newData.CreateTime = DateTime.Now;";
            });

            tableFieldInfo.Where(x => x.Name.StartsWith("Is")).ForEach((aField, index) =>
            {
                //搜索的下拉选项
                Type fieldType = _dbHelper.DbTypeStr_To_CsharpType(aField.Type);
                string newOption = $@"list.ForEach(e => {{ e.{aField.Name.Substring(2, aField.Name.Length - 2)}Value = EnumExtension.GetEnumDescription(((EnumWhether)Enum.ToObject(typeof(EnumWhether), e.{aField.Name}))); }});
";
                boolTypeBusiness.Append(newOption);
            });

            var tableName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.Replace("small_", "")).Replace("_", "");

            string code =
$@"using System.Linq;
using OpenAuth.Domain.Request;
using OpenAuth.Domain.Response;
using OpenAuth.Domain.Entitys;
using OpenAuth.Repository.Interface;
using System.Collections.Generic;
{usingEnumBusiness}

namespace OpenAuth.App
{{
public class {tableName}App : BaseApp<{tableName}>
    {{
        public {tableName}App(IUnitWork unitWork, IRepository<{tableName}> repository) : base(unitWork, repository, null)
        {{
        }}

        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData<List<{tableName}>> Load(PageExtend request)
        {{
            var result = new TableData<List<{tableName}>>();
            var query = UnitWork.Find<{tableName}>(null);

            query = QueryableExtensions.ExtKeyQuery(query, request.keyQuery);

            result.count = query.Count();
            if (result.count == 0)
                return result;

            result.data = query.OrderByDescending(u => u.CreateTime)
                .Skip((request.page - 1) * request.limit)
                .Take(request.limit).ToList();
            return result;
        }}

        public void Add({tableName} obj)
        {{
            //程序类型取入口应用的名称，可以根据自己需要调整
            Repository.Add(obj);
        }}
        
        public void Update({tableName} obj)
        {{
            UnitWork.Update<{tableName}>(u => u.Id == obj.Id, u => new {tableName}
            {{
               //todo:要修改的字段赋值
            }});
        }}
    }}
}}";
            string filePath = Path.Combine(path, "OpenAuth.App", $"{tableName}App.cs");

            FileHelper.WriteTxt(code, filePath, FileMode.Create);
        }

        /// <summary>
        /// 生成控制器代码
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildSmallController(List<TableInfo> tableFieldInfo, string areaName, string entityName, string path)
        {
            var tableName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.Replace("small_", "")).Replace("_", "");

            string code =
$@"using System;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using OpenAuth.App;
using OpenAuth.Domain.Request;
using OpenAuth.Domain.Response;
using OpenAuth.Domain.Entitys;
using System.Collections.Generic;

namespace OpenAuth.WebApi.Controllers.{areaName}
{{
    /// <summary>
    /// {_dbTableInfoDic[entityName].Description}
    /// </summary>
    [Route(""api/[controller]/[action]"")]
    [ApiExplorerSettings(GroupName = ""v2"")]
    [ApiController]
    public class {tableName}Controller : ControllerBase
    {{
        private readonly {tableName}App _app;

        public {tableName}Controller({tableName}App app)
        {{
            _app = app;
        }}

        //获取详情
        [HttpGet]
        public Response<{tableName}> Get(string id)
        {{
            var result = new Response<{tableName}>();
            try
            {{
                result.Result = _app.Get(id);
            }}
            catch (Exception ex)
            {{
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }}

            return result;
        }}

        /// <summary>
        /// 添加
        /// </summary>
        [HttpPost]
        public Response Add({tableName} obj)
        {{
            var result = new Response();
            try
            {{
                _app.Add(obj);
            }}
            catch (Exception ex)
            {{
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }}

            return result;
        }}

        /// <summary>
        /// 修改
        /// </summary>
        [HttpPost]
        public Response Update({tableName} obj)
        {{
            var result = new Response();
            try
            {{
                _app.Update(obj);
            }}
            catch (Exception ex)
            {{
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }}

            return result;
        }}

        /// <summary>
        /// 加载列表
        /// </summary>
        [HttpGet]
        public TableData<List<{tableName}>> Load([FromQuery]PageExtend request)
        {{
            return _app.Load(request);
        }}

        /// <summary>
        /// 批量删除
        /// </summary>
        [HttpPost]
        public Response Delete([FromBody]string[] ids)
        {{
            var result = new Response();
            try
            {{
                _app.Delete(ids);
            }}
            catch (Exception ex)
            {{
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }}

            return result;
        }}
    }}
}}";
            string filePath = Path.Combine(path, "OpenAuth.WebApi", "Controllers", areaName, $"{tableName}Controller.cs");
            FileHelper.WriteTxt(code, filePath, FileMode.Create);
        }


        #endregion

        #region Wms

        /// <summary>
        /// 生成实体
        /// </summary>
        /// <param name="tableInfo">表字段信息</param>
        /// <param name="areaName">区域名</param>
        /// <param name="tableName">表名</param>
        /// <param name="tableName">路径</param>
        private void BuildWmsEntity(List<TableInfo> tableInfo, string areaName, string tableName, string path)
        {
            string filePath = Path.Combine(path, "YdydWms.Entity", "Ydyd", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(tableName.Split('_')[0])}", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(tableName).Replace("_", "")}.cs");
            string nameSpace = $@"YdydWms.Entity";

            _dbHelper.SaveWmsEntityToFile(tableInfo, tableName, _dbTableInfoDic[tableName].Description, filePath, nameSpace);
        }

        /// <summary>
        /// 生成Dto
        /// </summary>
        /// <param name="tableInfo">表字段信息</param>
        /// <param name="areaName">区域名</param>
        /// <param name="tableName">表名</param>
        /// <param name="tableName">路径</param>
        private void BuildWmsDto(List<TableInfo> tableInfo, string areaName, string tableName, string path)
        {
            string dtofilePath = Path.Combine(path, "YdydWms.Entity", "Dto", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(tableName).Replace("_", "")}Dto", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(tableName).Replace("_", "")}Dto.cs");
            string nameSpace = $@"YdydWms.Entity.Dto";
            _dbHelper.SaveWmsDtoToFile(tableInfo, tableName, _dbTableInfoDic[tableName].Description, dtofilePath, nameSpace);

        }

        /// <summary>
        /// 生成业务逻辑代码
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildWmsIService(List<TableInfo> tableFieldInfo, string areaName, string entityName, string path)
        {



            var tableName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "");

            string code =
$@"using YdydWms.Util;
using YdydWms.Entity.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace YdydWms.IService
{{
    public interface I{tableName}Service
    {{
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name=""param"">查询参数</param>
        /// <returns></returns>
        Task <PageResult<{tableName}Dto>> GetDataListAsync(PageInput param);
        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name=""id"">主键id</param>
        /// <returns></returns>
        Task <{tableName}Dto> GetTheDataAsync(string id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name=""param"">新增参数</param>
        /// <returns></returns>
        Task AddDataAsync({tableName}Dto param);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name=""param"">修改参数</param>
        /// <returns></returns>
        Task UpdateDataAsync({tableName}Dto param);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name=""ids"">id数组,JSON数组</param>
        /// <returns></returns>
        Task DeleteDataAsync(List<string> ids);
    }}
}}";
            string filePath = Path.Combine(path, "YdydWms.IService", "IService", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.Split('_')[0])}", $"I{tableName}Service.cs");

            FileHelper.WriteTxt(code, filePath, FileMode.Create);
        }

        /// <summary>
        /// 生成业务逻辑代码
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildWmsService(List<TableInfo> tableFieldInfo, string areaName, string entityName, string path)
        {

            string createTimeBusiness = null;
            StringBuilder boolTypeBusiness = new StringBuilder();
            string usingEnumBusiness = tableFieldInfo.Any(x => x.Name.StartsWith("Is")) ? $@"using YdydWms.Entity.Enums;" : null;

            tableFieldInfo.Where(x => x.Name.Equals("CreateTime")).ForEach((aField, index) =>
            {
                //创建时间
                createTimeBusiness = $@"newData.CreateTime = DateTime.Now;";
            });

            tableFieldInfo.Where(x => x.Name.StartsWith("Is")).ForEach((aField, index) =>
            {
                //搜索的下拉选项
                Type fieldType = _dbHelper.DbTypeStr_To_CsharpType(aField.Type);
                string newOption = $@"list.ForEach(e => {{ e.{aField.Name.Substring(2, aField.Name.Length - 2)}Value = EnumExtension.GetEnumDescription(((EnumWhether)Enum.ToObject(typeof(EnumWhether), e.{aField.Name}))); }});
";
                boolTypeBusiness.Append(newOption);
            });

            var tableName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "");

            string code =
$@"using System;
using LinqKit;
using System.Linq;
using YdydWms.Util;
using YdydWms.Entity;
using YdydWms.IService;
using YdydWms.Repository;
using YdydWms.Entity.Dto;
using YdydWms.Entity.Enums;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace YdydWms.Service
{{
    public class {tableName}Service : BaseService<{tableName}>, I{tableName}Service, ITransientDependency
    {{
        public {tableName}Service(IRepository repository) : base(repository)
        {{
        }}

        #region 外部接口
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name=""param"" > 查询参数</param>
        /// <returns></returns>
        public async Task<PageResult<{tableName}Dto>> GetDataListAsync(PageInput param)
        {{
            var query = GetIQueryable();

            //搜索
            if(!param.keyQuery.IsNullOrEmpty())
            query = QueryableExtensions.ExtKeyQuery(query, param.keyQuery);

            //获取总条数
            int count = await query.CountAsync();
            if (count == 0)
                return new PageResult<{tableName}Dto> {{ Data = null, Total = 0 }};

            //排序分页
            var list = await query.OrderBy($@""{{ param.SortField}} {{param.SortType}}"")
                .Skip((param.PageIndex - 1) * param.PageRows)
                .Take(param.PageRows)
                .ToListAsync();

            var result = new PageResult<{tableName}Dto> {{ Data = list.MapToList<{tableName}Dto>(), Total = count }};
            return result;
        }}

        /// <summary>
        /// 查询详细
        /// </summary>
        /// <param name=""id"" > 主键Id</param>
        /// <returns></returns>
        public async Task<{tableName}Dto> GetTheDataAsync(string id)
        {{
            var model = await GetEntityAsync(id);
            return model.MapTo<{tableName}Dto>();
        }}

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name=""newData"" > 新增参数</param>
        /// <returns></returns>
        public async Task AddDataAsync({tableName}Dto newData)
        {{
            await InsertAsync(newData.MapTo<{tableName}>());
        }}

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name=""thisData"" >修改参数</param>
        /// <returns></returns>
        public async Task UpdateDataAsync({tableName}Dto thisData)
        {{
            await UpdateAsync(thisData.MapTo<{tableName}>());
        }}

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name=""ids"" > id数组,JSON数组</param>
        /// <returns></returns>
        public async Task DeleteDataAsync(List<string> ids)
        {{
            await DeleteAsync(ids);
        }}

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }}
}}";
            string filePath = Path.Combine(path, "YdydWms.Service", "Service", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.Split('_')[0])}", $"{tableName}Service.cs");

            FileHelper.WriteTxt(code, filePath, FileMode.Create);
        }

        /// <summary>
        /// 生成控制器代码
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildWmsController(List<TableInfo> tableFieldInfo, string areaName, string entityName, string path)
        {
            var tableName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.Replace("small_", "")).Replace("_", "");
            var tableLowerName = tableName.First().ToString().ToLower() + tableName.Substring(1);

            string code =
$@"using YdydWms.Util;
using YdydWms.IService;
using YdydWms.Entity.Dto;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace  YdydWms.Api.Controllers.{areaName}
{{
    /// <summary>
    /// {_dbTableInfoDic[entityName].Description}
    /// </summary>
    [Route(""{areaName}/[controller]/[action]"")]
    [ApiExplorerSettings(GroupName = ""v2"")]
    [ApiController]
    public class {tableName}Controller : BaseApiController
    {{
        #region DI
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name=""{tableLowerName}Service"" ></param>
        public {tableName}Controller(I{tableName}Service {tableLowerName}Service)
        {{ 
            _{tableLowerName}Service = {tableLowerName}Service;
        }}
        I{tableName}Service _{tableLowerName}Service {{ get; }}
        #endregion

        #region 获取

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name=""param"" >查询参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult<List<{tableName}Dto>>> GetDataList(PageInput<{tableName}Dto> param)
        {{
            return await _{tableLowerName}Service.GetDataListAsync(param);
        }}

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name=""param"" >查询参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<{tableName}Dto> GetTheData(IdInputDto param)
        {{
            return await _{tableLowerName}Service.GetTheDataAsync(param.id) ?? new {tableName}Dto();
        }}

        #endregion

        #region 提交

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name=""param"" >新增参数</param>
        [HttpPost]
        public async Task SaveData({tableName}Dto param)
        {{
            await _{tableLowerName}Service.AddDataAsync(param);
        }}

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name=""param"" >修改参数</param>
        [HttpPost]
        public async Task ModifyData({tableName}Dto param)
        {{
            await _{tableLowerName}Service.UpdateDataAsync(param);
        }}

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name=""ids"" >id数组,JSON数组</param>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {{
            await _{tableLowerName}Service.DeleteDataAsync(ids);
        }}

        #endregion
    }}
}}";
            string filePath = Path.Combine(path, "YdydWms.Api", "Controllers", areaName, $"{tableName}Controller.cs");
            FileHelper.WriteTxt(code, filePath, FileMode.Create);
        }


        /// <summary>
        /// 生成视图
        /// </summary>
        /// <param name="tableInfoList">表字段信息</param>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildWmsView(List<TableInfo> tableInfoList, string areaName, string entityName, string path)
        {
            //生成Index页面
            StringBuilder searchConditionSelectHtml = new StringBuilder();
            StringBuilder tableColsBuilder = new StringBuilder();
            StringBuilder formRowBuilder = new StringBuilder();

            tableInfoList.Where(x => !x.Name.EndsWith("Id") && !x.Name.EndsWith("id")).ForEach((aField, index) =>
            {
                //搜索的下拉选项
                Type fieldType = _dbHelper.DbTypeStr_To_CsharpType(aField.Type);
                if (fieldType == typeof(string))
                {
                    string newOption = $@"
                <option value=""{aField.Name}"">{aField.Description}</option>";
                    searchConditionSelectHtml.Append(newOption);
                }

                //数据表格列
                string end = (index == tableInfoList.Count - 2) ? "" : ",";
                string newCol = "";
                if ((aField.Name != "IsDeleted" && aField.Name != "is_deleted") && (aField.Name.StartsWith("Is") || aField.Name.StartsWith("is_")))
                {
                    newCol = $@"
  {{ title: '{aField.Description}', dataIndex: '{aField.Name.Replace("_", "").Substring(2).First().ToString().ToLower() + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(aField.Name).Replace("_", "").Substring(2, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(aField.Name).Replace("_", "").Length - 2).Substring(1)}Value', width: 120 }}{end}";
                }
                else if ((aField.Name != "IsDeleted" && aField.Name != "is_deleted") && (aField.Name == "Type" || aField.Name == "State" || aField.Name == "type" || aField.Name == "state"))
                {
                    newCol = $@"
  {{ title: '{aField.Description}', dataIndex: '{aField.Name.First().ToString().ToLower() + aField.Name.Substring(1)}Value', width: 120 }}{end}";
                }
                else if (aField.Name != "IsDeleted" && aField.Name != "is_deleted")
                {
                    newCol = $@"
  {{ title: '{aField.Description}', dataIndex: '{aField.Name.First().ToString().ToLower() + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(aField.Name).Replace("_", "").Substring(1) }', sorter: true, width: 200 }}{end}";
                }
                if (!newCol.IsNullOrEmpty())
                    tableColsBuilder.Append(newCol);
            });

            tableInfoList.Where(x => !x.Name.EndsWith("Id") && !x.Name.EndsWith("id") && !x.Name.EndsWith("Time") && !x.Name.EndsWith("time")).ForEach((aField, index) =>
            {
                //Form页面中的Html
                string newFormRow = "";
                if (aField.Name != "IsDeleted" && aField.Name != "is_deleted")
                {
                    newFormRow = $@"
        <a-form-model-item label=""{aField.Description}"" prop=""{aField.Name.First().ToString().ToLower() + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(aField.Name).Replace("_", "").Substring(1)}"">
          <a-input v-model=""entity.{aField.Name.First().ToString().ToLower() + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(aField.Name).Replace("_", "").Substring(1)}"" autocomplete=""off"" />
        </a-form-model-item>";
                }
                if (!newFormRow.IsNullOrEmpty())
                    formRowBuilder.Append(newFormRow);
            });

            string indexHtml =
$@"<template>
  <a-card :bordered=""false"" >
    <div class=""table-page-search-wrapper"" >
      <a-form layout= ""inline"" >
        <a-row :gutter=""48"" >
          <select-tool :options=""queryItem"" :queryData=""pagination.keyQuery""></select-tool>
          <a-col :md=""6"" :sm=""24"" style=""float:right"">
            <span class=""table-page-search-submitButtons"">
              <a-button
                type= ""primary""
                icon=""search""
                @click=""() => {{this.pagination.current = 1; this.getDataList()}}""
              >查询</a-button>
              <a-button style= ""margin-left: 8px"" type=""primary"" icon=""plus"" @click=""hanldleAdd()"">新建</a-button>
              <a-button
                style= ""margin-left: 8px""
                type=""primary""
                icon=""plus""
                @click=""handleEdit(selectedRowKeys)""
              >编辑</a-button>
              <a-button
                style= ""margin-left: 8px""
                type=""danger""
                icon=""minus""
                @click=""handleDelete(selectedRowKeys)""
                :disabled=""!hasSelected()""
                :loading=""loading""
              >删除</a-button>
            </span>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table
      ref=""table""
      :columns=""columns""
      :scroll=""{{ x: 1500, y: 300 }}""
      :rowKey=""row => row.id""
      :dataSource=""data""
      :pagination=""pagination""
      :loading=""loading""
      @change=""handleTableChange""
      :customRow=""onSelectRow""
      :rowSelection=""{{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange}}""
      :bordered=""true""
      size=""small""
    >
      <span slot= ""action"" slot-scope=""text, record"">
        <template>
          <a @click = ""handleEdit(record.id)"" > 编辑 </a >
          <a-divider type=""vertical"" />
          <a @click = ""handleDelete([record.id])"" > 删除 </a >
        </template >
      </span >
    </a-table >

    <edit-form ref=""editForm"" :afterSubmit=""getDataList""></edit-form>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import SelectTool from '@/components/SelectTool'

const columns = [{tableColsBuilder}
]

export default {{
  components: {{
    EditForm,
    SelectTool
  }},
  mounted () {{
    this.getDataList()
  }},
  data () {{
    return {{
      // 查询相关
      // 分页
      pagination: {{
        current: 1,
        pageSize: 10,
        keyQuery: [
          // {{
          //   condition: 'Name',
          //   keyword: '',
          //   symbol: '',
          //   type: 'string',
          //   isCandel: false
          // }},
          // {{
          //   condition: 'State',
          //   keyword: '',
          //   symbol: '=',
          //   type: 'enum',
          //   isCandel: false
          // }}
        ],
        showTotal: (total, range) => `总数:${{ total}}  当前:${{ range[0]}} - ${{ range[1]}}`
      }},
      // 查询项
      queryItem: [
        // {{
        //  value: 'Name',
        //  label: '名称',
        //  type: 'string'
        // }},
        // {{
        //   value: 'State',
        //   label: '状态',
        //   type: 'enum',
        //   enum: 'EnumStatus',
        //   selectListItem: []
        // }}
      ],
      data: [],
      filters: {{}},
      sorter: {{ field: 'id', order: 'asc' }},
      loading: false,
      columns,
      visible: false,
      selectedRowKeys: []
    }}
  }},
  methods: {{
    handleTableChange (pagination, filters, sorter) {{
      this.pagination = {{ ...pagination }}
      this.filters = {{ ...filters }}
      this.sorter = {{ ...sorter }}
      this.getDataList()
    }},
    getDataList () {{
      this.selectedRowKeys = []
      this.loading = true
      this.$http.post('/{areaName}/{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "")}/GetDataList', {{
        PageIndex: this.pagination.current,
        PageRows: this.pagination.pageSize,
        keyQuery: this.pagination.keyQuery,
        SortField: this.sorter.field || 'id',
        SortType: this.sorter.order === 'ascend' ? 'asc' : 'desc',
        ...this.filters
      }}).then(resJson => {{
        this.loading = false
        this.data = resJson.data
        const pagination = {{ ...this.pagination }}
        pagination.total = resJson.total
        this.pagination = pagination
      }})
    }},
    onSelectChange (selectedRowKeys) {{
      this.selectedRowKeys = selectedRowKeys
      console.log(selectedRowKeys)
    }},
    onSelectRow(record, index) {{
      return {{
      on:
        {{
          click: () => {{
            this.selectedRowKeys = [record.id]
          }}
        }}
      }}
    }},
    hasSelected () {{
      return this.selectedRowKeys.length > 0
    }},
    hanldleAdd () {{
      this.$refs.editForm.openForm()
    }},
    handleEdit (selectedRowKeys) {{
      if (this.selectedRowKeys.length === 1) this.$refs.editForm.openForm(selectedRowKeys[0])
      else this.$message.error('请选择一个进行编辑!')
    }},
    handleDelete(ids) {{
      var thisObj = this
      this.$confirm({{
        title: '确认删除吗?',
        onOk () {{
          return new Promise((resolve, reject) => {{
            thisObj.submitDelete(ids, resolve, reject)
          }}).catch (() => console.log('Oops errors!'))
        }}
      }})
    }},
    submitDelete (ids, resolve, reject) {{
      this.$http.post('/{areaName}/{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "")}/DeleteData', ids).then(resJson => {{
        resolve()
        if (resJson.success)
        {{
          this.$message.success('操作成功!')
          this.getDataList()
        }}
        else
        {{
          this.$message.error(resJson.msg)
        }}
      }})
    }}
  }}
}}
</script>
";
            string indexPath = Path.Combine(path, "YdydWeb", "src", "views", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.Split('_')[0])}", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.PadLeft(entityName.IndexOf("_")).Remove(0, entityName.IndexOf("_"))).Replace("_", "")}", "List.vue");

            FileHelper.WriteTxt(indexHtml, indexPath, FileMode.Create);

            //生成Form页面
            string formHtml =
$@"<template>
  <a-modal
    title=""编辑表单""
    width=""40%""
    :visible=""visible""
    :confirmLoading=""confirmLoading""
    @ok=""handleSubmit""
    @cancel=""()=>{{this.visible=false}}""
  >
    <a-spin :spinning=""confirmLoading"">
      <a-form-model ref=""form"" :model=""entity"" :rules=""rules"" v-bind=""layout"">{formRowBuilder}
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
export default {{
  props: {{
    afterSubmit: {{
      type: Function,
      default: null
    }}
  }},
  data () {{
    return {{
      layout: {{
        labelCol: {{ span: 5 }},
        wrapperCol: {{ span: 18 }}
      }},
      visible: false,
      confirmLoading: false,
      entity: {{}},
      rules: {{
        // name: [{{ required: true, message: '必填' }}],
      }}
    }}
  }},
  methods: {{
    init () {{
      this.visible = true
      this.entity = {{}}
      this.$nextTick(() => {{
        this.$refs['form'].clearValidate()
      }})
    }},
    openForm (id) {{
      this.init()
      if (id) {{
        this.$http.post('/{areaName}/{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "")}/GetTheData', {{ id: id }}).then(resJson => {{
          this.entity = resJson.data
        }})
      }}
    }},
    handleSubmit () {{
      this.$refs['form'].validate(valid => {{
        if (!valid) {{
          return
        }}
        this.confirmLoading = true
        let url = '/{areaName}/{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "")}/ModifyData'
        if (this.entity.id == null) {{
          url = '/{areaName}/{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "")}/SaveData'
        }}
        this.$http.post(url, this.entity).then(resJson => {{
          this.confirmLoading = false
          if (resJson.success) {{
            this.$message.success('操作成功!')
            this.afterSubmit()
            this.visible = false
          }} else {{
            this.$message.error(resJson.msg)
          }}
        }})
      }})
    }}
  }}
}}
</script>
";
            string formPath = Path.Combine(path, "YdydWeb", "src", "views", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.Split('_')[0])}", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.PadLeft(entityName.IndexOf("_")).Remove(0, entityName.IndexOf("_"))).Replace("_", "")}", "EditForm.vue");

            FileHelper.WriteTxt(formHtml, formPath, FileMode.Create);
        }

        #endregion

        #region League

        /// <summary>
        /// 生成实体
        /// </summary>
        /// <param name="tableInfo">表字段信息</param>
        /// <param name="areaName">区域名</param>
        /// <param name="tableName">表名</param>
        /// <param name="tableName">路径</param>
        private void BuildLeagueEntity(List<TableInfo> tableInfo, string areaName, string tableName, string path)
        {
            string filePath = Path.Combine(path, "League.Entity", "Model", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(tableName.Split('_')[0])}", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(tableName).Replace("_", "")}.cs");
            string nameSpace = $@"League.Entity";

            _dbHelper.SaveLeagueEntityToFile(tableInfo, tableName, _dbTableInfoDic[tableName].Description, filePath, nameSpace);
        }

        /// <summary>
        /// 生成Dto
        /// </summary>
        /// <param name="tableInfo">表字段信息</param>
        /// <param name="areaName">区域名</param>
        /// <param name="tableName">表名</param>
        /// <param name="tableName">路径</param>
        private void BuildLeagueDto(List<TableInfo> tableInfo, string areaName, string tableName, string path)
        {
            string dtofilePath = Path.Combine(path, "League.Entity", "Dto", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(tableName).Replace("_", "")}Dto", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(tableName).Replace("_", "")}Dto.cs");
            string nameSpace = $@"League.Entity.Dto";
            _dbHelper.SaveLeagueDtoToFile(tableInfo, tableName, _dbTableInfoDic[tableName].Description, dtofilePath, nameSpace);

        }

        /// <summary>
        /// 生成业务逻辑代码
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildLeagueIService(List<TableInfo> tableFieldInfo, string areaName, string entityName, string path)
        {



            var tableName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "");

            string code =
$@"using League.Util;
using League.Entity.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace League.IService
{{
    public interface I{tableName}Service
    {{
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name=""param"">查询参数</param>
        /// <returns></returns>
        Task <PageResult<{tableName}Dto>> GetDataListAsync(PageInput param);
        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name=""id"">主键id</param>
        /// <returns></returns>
        Task <{tableName}Dto> GetTheDataAsync(string id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name=""param"">新增参数</param>
        /// <returns></returns>
        Task AddDataAsync({tableName}Dto param);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name=""param"">修改参数</param>
        /// <returns></returns>
        Task UpdateDataAsync({tableName}Dto param);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name=""ids"">id数组,JSON数组</param>
        /// <returns></returns>
        Task DeleteDataAsync(List<string> ids);
    }}
}}";
            string filePath = Path.Combine(path, "League.IService", "IService", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.Split('_')[0])}", $"I{tableName}Service.cs");

            FileHelper.WriteTxt(code, filePath, FileMode.Create);
        }

        /// <summary>
        /// 生成业务逻辑代码
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildLeagueService(List<TableInfo> tableFieldInfo, string areaName, string entityName, string path)
        {

            string createTimeBusiness = null;
            StringBuilder boolTypeBusiness = new StringBuilder();
            string usingEnumBusiness = tableFieldInfo.Any(x => x.Name.StartsWith("Is")) ? $@"using League.Entity.Enums;" : null;

            tableFieldInfo.Where(x => x.Name.Equals("CreateTime")).ForEach((aField, index) =>
            {
                //创建时间
                createTimeBusiness = $@"newData.CreateTime = DateTime.Now;";
            });

            tableFieldInfo.Where(x => x.Name.StartsWith("Is")).ForEach((aField, index) =>
            {
                //搜索的下拉选项
                Type fieldType = _dbHelper.DbTypeStr_To_CsharpType(aField.Type);
                string newOption = $@"list.ForEach(e => {{ e.{aField.Name.Substring(2, aField.Name.Length - 2)}Value = EnumExtension.GetEnumDescription(((EnumWhether)Enum.ToObject(typeof(EnumWhether), e.{aField.Name}))); }});
";
                boolTypeBusiness.Append(newOption);
            });

            var tableName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "");

            string code =
$@"using System;
using LinqKit;
using System.Linq;
using League.Util;
using League.Entity;
using League.IService;
using League.Repository;
using League.Entity.Dto;
using League.Entity.Enums;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace League.Service
{{
    public class {tableName}Service : BaseService<{tableName}>, I{tableName}Service, ITransientDependency
    {{
        public {tableName}Service(IRepository repository) : base(repository)
        {{
        }}

        #region 外部接口
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name=""param"" > 查询参数</param>
        /// <returns></returns>
        public async Task<PageResult<{tableName}Dto>> GetDataListAsync(PageInput param)
        {{
            var query = GetIQueryable();

            //搜索
            if(!param.keyQuery.IsNullOrEmpty())
            query = QueryableExtensions.ExtKeyQuery(query, param.keyQuery);

            //获取总条数
            int count = await query.CountAsync();
            if (count == 0)
                return new PageResult<{tableName}Dto> {{ Data = null, Total = 0 }};

            //排序分页
            var list = await query.OrderBy($@""{{ param.SortField}} {{param.SortType}}"")
                .Skip((param.PageIndex - 1) * param.PageRows)
                .Take(param.PageRows)
                .ToListAsync();

            var result = new PageResult<{tableName}Dto> {{ Data = list.MapToList<{tableName}Dto>(), Total = count }};
            return result;
        }}

        /// <summary>
        /// 查询详细
        /// </summary>
        /// <param name=""id"" > 主键Id</param>
        /// <returns></returns>
        public async Task<{tableName}Dto> GetTheDataAsync(string id)
        {{
            var model = await GetEntityAsync(id);
            return model.MapTo<{tableName}Dto>();
        }}

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name=""newData"" > 新增参数</param>
        /// <returns></returns>
        public async Task AddDataAsync({tableName}Dto newData)
        {{
            await InsertAsync(newData.MapTo<{tableName}>());
        }}

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name=""thisData"" >修改参数</param>
        /// <returns></returns>
        public async Task UpdateDataAsync({tableName}Dto thisData)
        {{
            await UpdateAsync(thisData.MapTo<{tableName}>());
        }}

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name=""ids"" > id数组,JSON数组</param>
        /// <returns></returns>
        public async Task DeleteDataAsync(List<string> ids)
        {{
            await DeleteAsync(ids);
        }}

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }}
}}";
            string filePath = Path.Combine(path, "League.Service", "Service", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.Split('_')[0])}", $"{tableName}Service.cs");

            FileHelper.WriteTxt(code, filePath, FileMode.Create);
        }

        /// <summary>
        /// 生成控制器代码
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildLeagueController(List<TableInfo> tableFieldInfo, string areaName, string entityName, string path)
        {
            var tableName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "");
            var tableLowerName = tableName.First().ToString().ToLower() + tableName.Substring(1);

            string code =
$@"using League.Util;
using League.IService;
using League.Entity.Dto;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace  League.Api.Controllers.{areaName}
{{
    /// <summary>
    /// {_dbTableInfoDic[entityName].Description}
    /// </summary>
    [Route(""{areaName}/[controller]/[action]"")]
    [ApiExplorerSettings(GroupName = ""v2"")]
    [ApiController]
    public class {tableName}Controller : BaseApiController
    {{
        #region DI
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name=""{tableLowerName}Service"" ></param>
        public {tableName}Controller(I{tableName}Service {tableLowerName}Service)
        {{ 
            _{tableLowerName}Service = {tableLowerName}Service;
        }}
        I{tableName}Service _{tableLowerName}Service {{ get; }}
        #endregion

        #region 获取

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name=""param"" >查询参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult<List<{tableName}Dto>>> GetDataList(PageInput<{tableName}Dto> param)
        {{
            return await _{tableLowerName}Service.GetDataListAsync(param);
        }}

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name=""param"" >查询参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<{tableName}Dto> GetTheData(IdInputDto param)
        {{
            return await _{tableLowerName}Service.GetTheDataAsync(param.id) ?? new {tableName}Dto();
        }}

        #endregion

        #region 提交

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name=""param"" >新增参数</param>
        [HttpPost]
        public async Task SaveData({tableName}Dto param)
        {{
            await _{tableLowerName}Service.AddDataAsync(param);
        }}

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name=""param"" >修改参数</param>
        [HttpPost]
        public async Task ModifyData({tableName}Dto param)
        {{
            await _{tableLowerName}Service.UpdateDataAsync(param);
        }}

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name=""ids"" >id数组,JSON数组</param>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {{
            await _{tableLowerName}Service.DeleteDataAsync(ids);
        }}

        #endregion
    }}
}}";
            string filePath = Path.Combine(path, "League.Api", "Controllers", areaName, $"{tableName}Controller.cs");
            FileHelper.WriteTxt(code, filePath, FileMode.Create);
        }


        /// <summary>
        /// 生成视图
        /// </summary>
        /// <param name="tableInfoList">表字段信息</param>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildLeagueView(List<TableInfo> tableInfoList, string areaName, string entityName, string path)
        {
            //生成Index页面
            StringBuilder searchConditionSelectHtml = new StringBuilder();
            StringBuilder tableColsBuilder = new StringBuilder();
            StringBuilder formRowBuilder = new StringBuilder();

            tableInfoList.Where(x => !x.Name.EndsWith("Id") && !x.Name.EndsWith("id")).ForEach((aField, index) =>
            {
                //搜索的下拉选项
                Type fieldType = _dbHelper.DbTypeStr_To_CsharpType(aField.Type);
                if (fieldType == typeof(string))
                {
                    string newOption = $@"
                <option value=""{aField.Name}"">{aField.Description}</option>";
                    searchConditionSelectHtml.Append(newOption);
                }

                //数据表格列
                string end = (index == tableInfoList.Count - 2) ? "" : ",";
                string newCol = "";
                if ((aField.Name != "IsDeleted" && aField.Name != "is_deleted") && (aField.Name.StartsWith("Is") || aField.Name.StartsWith("is_")))
                {
                    newCol = $@"
  {{ title: '{aField.Description}', dataIndex: '{aField.Name.Replace("_", "").Substring(2).First().ToString().ToLower() + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(aField.Name).Replace("_", "").Substring(2, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(aField.Name).Replace("_", "").Length - 2).Substring(1)}Value', width: 120 }}{end}";
                }
                else if ((aField.Name != "IsDeleted" && aField.Name != "is_deleted") && (aField.Name == "Type" || aField.Name == "State" || aField.Name == "type" || aField.Name == "state"))
                {
                    newCol = $@"
  {{ title: '{aField.Description}', dataIndex: '{aField.Name.First().ToString().ToLower() + aField.Name.Substring(1)}Value', width: 120 }}{end}";
                }
                else if (aField.Name != "IsDeleted" && aField.Name != "is_deleted")
                {
                    newCol = $@"
  {{ title: '{aField.Description}', dataIndex: '{aField.Name.First().ToString().ToLower() + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(aField.Name).Replace("_", "").Substring(1) }', sorter: true, width: 200 }}{end}";
                }
                if (!newCol.IsNullOrEmpty())
                    tableColsBuilder.Append(newCol);
            });

            tableInfoList.Where(x => !x.Name.EndsWith("Id") && !x.Name.EndsWith("id") && !x.Name.EndsWith("Time") && !x.Name.EndsWith("time")).ForEach((aField, index) =>
            {
                //Form页面中的Html
                string newFormRow = "";
                if (aField.Name != "IsDeleted" && aField.Name != "is_deleted")
                {
                    newFormRow = $@"
        <a-form-model-item label=""{aField.Description}"" prop=""{aField.Name.First().ToString().ToLower() + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(aField.Name).Replace("_", "").Substring(1)}"">
          <a-input v-model=""entity.{aField.Name.First().ToString().ToLower() + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(aField.Name).Replace("_", "").Substring(1)}"" autocomplete=""off"" />
        </a-form-model-item>";
                }
                if (!newFormRow.IsNullOrEmpty())
                    formRowBuilder.Append(newFormRow);
            });

            string indexHtml =
$@"<template>
  <a-card :bordered=""false"" >
    <div class=""table-page-search-wrapper"" >
      <a-form layout= ""inline"" >
        <a-row :gutter=""48"" >
          <select-tool :options=""queryItem"" :queryData=""pagination.keyQuery""></select-tool>
          <a-col :md=""6"" :sm=""24"" style=""float:right"">
            <span class=""table-page-search-submitButtons"">
              <a-button
                type= ""primary""
                icon=""search""
                @click=""() => {{this.pagination.current = 1; this.getDataList()}}""
              >查询</a-button>
              <a-button style= ""margin-left: 8px"" type=""primary"" icon=""plus"" @click=""hanldleAdd()"">新建</a-button>
              <a-button
                style= ""margin-left: 8px""
                type=""primary""
                icon=""plus""
                @click=""handleEdit(selectedRowKeys)""
              >编辑</a-button>
              <a-button
                style= ""margin-left: 8px""
                type=""danger""
                icon=""minus""
                @click=""handleDelete(selectedRowKeys)""
                :disabled=""!hasSelected()""
                :loading=""loading""
              >删除</a-button>
            </span>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table
      ref=""table""
      :columns=""columns""
      :scroll=""{{ x: 1500, y: 300 }}""
      :rowKey=""row => row.id""
      :dataSource=""data""
      :pagination=""pagination""
      :loading=""loading""
      @change=""handleTableChange""
      :customRow=""onSelectRow""
      :rowSelection=""{{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange}}""
      :bordered=""true""
      size=""small""
    >
      <span slot= ""action"" slot-scope=""text, record"">
        <template>
          <a @click = ""handleEdit(record.id)"" > 编辑 </a >
          <a-divider type=""vertical"" />
          <a @click = ""handleDelete([record.id])"" > 删除 </a >
        </template >
      </span >
    </a-table >

    <edit-form ref=""editForm"" :afterSubmit=""getDataList""></edit-form>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import SelectTool from '@/components/SelectTool'

const columns = [{tableColsBuilder}
]

export default {{
  components: {{
    EditForm,
    SelectTool
  }},
  mounted () {{
    this.getDataList()
  }},
  data () {{
    return {{
      // 查询相关
      // 分页
      pagination: {{
        current: 1,
        pageSize: 10,
        keyQuery: [
          // {{
          //   condition: 'Name',
          //   keyword: '',
          //   symbol: '',
          //   type: 'string',
          //   isCandel: false
          // }},
          // {{
          //   condition: 'State',
          //   keyword: '',
          //   symbol: '=',
          //   type: 'enum',
          //   isCandel: false
          // }}
        ],
        showTotal: (total, range) => `总数:${{ total}}  当前:${{ range[0]}} - ${{ range[1]}}`
      }},
      // 查询项
      queryItem: [
        // {{
        //  value: 'Name',
        //  label: '名称',
        //  type: 'string'
        // }},
        // {{
        //   value: 'State',
        //   label: '状态',
        //   type: 'enum',
        //   enum: 'EnumStatus',
        //   selectListItem: []
        // }}
      ],
      data: [],
      filters: {{}},
      sorter: {{ field: 'id', order: 'asc' }},
      loading: false,
      columns,
      visible: false,
      selectedRowKeys: []
    }}
  }},
  methods: {{
    handleTableChange (pagination, filters, sorter) {{
      this.pagination = {{ ...pagination }}
      this.filters = {{ ...filters }}
      this.sorter = {{ ...sorter }}
      this.getDataList()
    }},
    getDataList () {{
      this.selectedRowKeys = []
      this.loading = true
      this.$http.post('/{areaName}/{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "")}/GetDataList', {{
        PageIndex: this.pagination.current,
        PageRows: this.pagination.pageSize,
        keyQuery: this.pagination.keyQuery,
        SortField: this.sorter.field || 'id',
        SortType: this.sorter.order === 'ascend' ? 'asc' : 'desc',
        ...this.filters
      }}).then(resJson => {{
        this.loading = false
        this.data = resJson.data
        const pagination = {{ ...this.pagination }}
        pagination.total = resJson.total
        this.pagination = pagination
      }})
    }},
    onSelectChange (selectedRowKeys) {{
      this.selectedRowKeys = selectedRowKeys
      console.log(selectedRowKeys)
    }},
    onSelectRow(record, index) {{
      return {{
      on:
        {{
          click: () => {{
            this.selectedRowKeys = [record.id]
          }}
        }}
      }}
    }},
    hasSelected () {{
      return this.selectedRowKeys.length > 0
    }},
    hanldleAdd () {{
      this.$refs.editForm.openForm()
    }},
    handleEdit (selectedRowKeys) {{
      if (this.selectedRowKeys.length === 1) this.$refs.editForm.openForm(selectedRowKeys[0])
      else this.$message.error('请选择一个进行编辑!')
    }},
    handleDelete(ids) {{
      var thisObj = this
      this.$confirm({{
        title: '确认删除吗?',
        onOk () {{
          return new Promise((resolve, reject) => {{
            thisObj.submitDelete(ids, resolve, reject)
          }}).catch (() => console.log('Oops errors!'))
        }}
      }})
    }},
    submitDelete (ids, resolve, reject) {{
      this.$http.post('/{areaName}/{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "")}/DeleteData', ids).then(resJson => {{
        resolve()
        if (resJson.success)
        {{
          this.$message.success('操作成功!')
          this.getDataList()
        }}
        else
        {{
          this.$message.error(resJson.msg)
        }}
      }})
    }}
  }}
}}
</script>
";
            string indexPath = Path.Combine(path, "LeagueWeb", "src", "views", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.Split('_')[0])}", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.PadLeft(entityName.IndexOf("_")).Remove(0, entityName.IndexOf("_"))).Replace("_", "")}", "List.vue");

            FileHelper.WriteTxt(indexHtml, indexPath, FileMode.Create);

            //生成Form页面
            string formHtml =
$@"<template>
  <a-modal
    title=""编辑表单""
    width=""40%""
    :visible=""visible""
    :confirmLoading=""confirmLoading""
    @ok=""handleSubmit""
    @cancel=""()=>{{this.visible=false}}""
  >
    <a-spin :spinning=""confirmLoading"">
      <a-form-model ref=""form"" :model=""entity"" :rules=""rules"" v-bind=""layout"">{formRowBuilder}
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
export default {{
  props: {{
    afterSubmit: {{
      type: Function,
      default: null
    }}
  }},
  data () {{
    return {{
      layout: {{
        labelCol: {{ span: 5 }},
        wrapperCol: {{ span: 18 }}
      }},
      visible: false,
      confirmLoading: false,
      entity: {{}},
      rules: {{
        // name: [{{ required: true, message: '必填' }}],
      }}
    }}
  }},
  methods: {{
    init () {{
      this.visible = true
      this.entity = {{}}
      this.$nextTick(() => {{
        this.$refs['form'].clearValidate()
      }})
    }},
    openForm (id) {{
      this.init()
      if (id) {{
        this.$http.post('/{areaName}/{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "")}/GetTheData', {{ id: id }}).then(resJson => {{
          this.entity = resJson.data
        }})
      }}
    }},
    handleSubmit () {{
      this.$refs['form'].validate(valid => {{
        if (!valid) {{
          return
        }}
        this.confirmLoading = true
        let url = '/{areaName}/{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "")}/ModifyData'
        if (this.entity.id == null) {{
          url = '/{areaName}/{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName).Replace("_", "")}/SaveData'
        }}
        this.$http.post(url, this.entity).then(resJson => {{
          this.confirmLoading = false
          if (resJson.success) {{
            this.$message.success('操作成功!')
            this.afterSubmit()
            this.visible = false
          }} else {{
            this.$message.error(resJson.msg)
          }}
        }})
      }})
    }}
  }}
}}
</script>
";
            string formPath = Path.Combine(path, "LeagueWeb", "src", "views", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.Split('_')[0])}", $"{System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(entityName.PadLeft(entityName.IndexOf("_")).Remove(0, entityName.IndexOf("_"))).Replace("_", "")}", "EditForm.vue");

            FileHelper.WriteTxt(formHtml, formPath, FileMode.Create);
        }

        #endregion



        /// <summary>
        /// 获取对应的数据库帮助类
        /// </summary>
        /// <param name="linkId">数据库连接Id</param>
        /// <returns></returns>
        private DbHelper GetTheDbHelper(string linkId, out string linkName)
        {
            var theLink = GetTheLink(linkId);
            DbHelper dbHelper = DbHelperFactory.GetDbHelper(theLink.DbType, theLink.ConnectionStr);
            linkName = theLink.LinkName;
            return dbHelper;
        }

        /// <summary>
        /// 获取对应的数据库帮助类
        /// </summary>
        /// <param name="linkId">数据库连接Id</param>
        /// <returns></returns>
        private DbHelper GetTheDbHelper(string linkId)
        {
            var theLink = GetTheLink(linkId);
            DbHelper dbHelper = DbHelperFactory.GetDbHelper(theLink.DbType, theLink.ConnectionStr);
            return dbHelper;
        }

        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <param name="linkId">连接Id</param>
        /// <returns></returns>
        private Base_DatabaseLink GetTheLink(string linkId)
        {
            Base_DatabaseLink resObj = new Base_DatabaseLink();
            var theModule = GetIQueryable().Where(x => x.Id == linkId).FirstOrDefault();
            resObj = theModule ?? resObj;

            return resObj;
        }


        #region 数据模型

        #endregion
    }
}
