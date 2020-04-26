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
            _dbHelper = GetTheDbHelper(linkId);
            GetDbTableList(linkId).ForEach(aTable =>
            {
                _dbTableInfoDic.Add(aTable.TableName, aTable);
            });

            List<string> tableList = tables.ToList<string>();
            List<string> buildTypeList = buildType.ToList<string>();
            tableList.ForEach(aTable =>
            {
                var tableFieldInfo = _dbHelper.GetDbTableInfo(aTable);

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
    [Route(""api /[controller] /[action]"")]
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
