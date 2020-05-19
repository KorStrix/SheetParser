using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static SpreadSheetParser.TypeDataHelper;

#if !UNITY_EDITOR
using System.Windows.Forms;
#endif

namespace SpreadSheetParser
{
#if !UNITY_EDITOR
    public partial class Work_Generate_Unity_ScriptableObjectForm : Form
    {
        Work_Generate_Unity_ScriptableObject _pWork;

        public Work_Generate_Unity_ScriptableObjectForm()
        {
            InitializeComponent();
        }

        public void DoInit(Work_Generate_Unity_ScriptableObject pWork)
        {
            _pWork = null;

            checkBox_OpenFolder_AfterBuild.Checked = pWork.bOpenPath_AfterBuild_CSharp;
            textBox_ExportPath.Text = pWork.strExportPath;
            textBox_CommandLine_ForUnitySOWork.Text = pWork.strCommandLine;

            _pWork = pWork;
        }

        private void checkBox_OpenFolder_AfterBuild_CheckedChanged(object sender, EventArgs e)
        {
            if (_pWork == null)
                return;

            _pWork.bOpenPath_AfterBuild_CSharp = checkBox_OpenFolder_AfterBuild.Checked;
        }

        private void button_SavePath_ExportPath_Click(object sender, EventArgs e)
        {
            if (_pWork == null)
                return;

            if (SpreadSheetParser_MainForm.DoShowFolderBrowser_And_SavePath(false, ref textBox_ExportPath))
                _pWork.strExportPath = textBox_ExportPath.Text;
        }

        private void Button_OpenPath_ExportPath_Click(object sender, EventArgs e)
        {
            _pWork.DoOpenFolder(textBox_ExportPath.Text);
        }

        private void button_SaveAndClose_Click(object sender, EventArgs e)
        {
            _pWork.strCommandLine = textBox_CommandLine_ForUnitySOWork.Text;
            _pWork.DoAutoSaveAsync();
            Close();
        }
    }
#endif

    [Serializable]
    public class Work_Generate_Unity_ScriptableObject : WorkBase
    {
        const string const_strFieldName_ListData = "listData";
        const string const_strFieldName_private_instance = "_instance";
        const string const_strFieldName_instance = "instance";

        public string strExportPath;
        public string strCommandLine;
        public bool bOpenPath_AfterBuild_CSharp;

#if !UNITY_EDITOR
        protected override void OnCreateInstance(out Type pFormType, out Type pType)
        {
            pFormType = typeof(Work_Generate_Unity_ScriptableObjectForm);
            pType = GetType();
        }
#endif

        public override string GetDisplayString()
        {
            return "Generate Unity SO";
        }

        public override void DoWork(CodeFileBuilder pCodeFileBuilder, GoogleSpreadSheetConnector pConnector, IEnumerable<TypeData> listSheetData, Action<string> OnPrintWorkState)
        {
            CodeNamespace pNameSpace = new CodeNamespace();

            List<CodeNamespaceImport> listDefaultUsing = new List<CodeNamespaceImport>();
            listDefaultUsing.Add(new CodeNamespaceImport("UnityEngine"));

            List <CommandLineArg> listCommandLine = Parsing_CommandLine(strCommandLine, null);
            for(int i = 0; i < listCommandLine.Count; i++)
            {
                ECommandLine eCommandLine = (ECommandLine)Enum.Parse(typeof(ECommandLine), listCommandLine[i].strArgName);
                switch (eCommandLine)
                {
                    case ECommandLine.addusing:
                        listDefaultUsing.Add(new CodeNamespaceImport(listCommandLine[i].strArgValue));
                        break;

                    case ECommandLine.useusing:
                        pNameSpace.Name = listCommandLine[i].strArgValue;
                        break;
                }
            }
            CodeNamespaceImport[] arrDefaultUsing = listDefaultUsing.ToArray();
            pNameSpace.Imports.AddRange(arrDefaultUsing);

            CodeTypeDeclarationCollection arrTypes = pCodeFileBuilder.GetCodeTypeDeclarationCollection();
            List<CodeTypeDeclaration> listType = new List<CodeTypeDeclaration>();
            foreach (CodeTypeDeclaration pType in arrTypes)
                listType.Add(pType);


            HashSet<CodeTypeDeclaration> setExecutedType = new HashSet<CodeTypeDeclaration>();
            IEnumerable<CodeTypeDeclaration> listUnitySO = listType.Where(p => string.IsNullOrEmpty(p.Name) == false && p.IsClass);
            foreach (CodeTypeDeclaration pType in listUnitySO)
            {
                TypeData pSaveData = listSheetData.FirstOrDefault((pSaveDataSheet) => pSaveDataSheet.strFileName == pType.Name);
                if (pSaveData == null)
                    continue;

                Create_SO(pCodeFileBuilder, pNameSpace, pType, pSaveData);

                CodeTypeDeclaration[] arrEnumTypes = listType.Where(p => pSaveData.listEnumName.Contains(p.Name)).ToArray();
                foreach(var pEnumType in arrEnumTypes)
                    setExecutedType.Add(pEnumType);

                if (pSaveData.eType == ESheetType.Global)
                {
                    Create_GlobalSOContainer(pCodeFileBuilder, pNameSpace, arrDefaultUsing, pType, arrEnumTypes, pSaveData);
                }
                else
                {
                    Create_SOContainer(pCodeFileBuilder, pNameSpace, arrDefaultUsing, pType, arrEnumTypes, pSaveData);
                }

                OnPrintWorkState?.Invoke($"UnitySO - Working SO {pType.Name}");
                setExecutedType.Add(pType);
            }

            // Others
            pNameSpace.Types.Clear();
            IEnumerable<CodeTypeDeclaration> listOthers = listType.Where(p => string.IsNullOrEmpty(p.Name) == false && setExecutedType.Contains(p) == false);
            foreach (CodeTypeDeclaration pType in listOthers)
            {
                OnPrintWorkState?.Invoke($"UnitySO - Working Others {pType.Name}");
                pNameSpace.Types.Add(pType);
                setExecutedType.Add(pType);
            }

            if (pNameSpace.Types.Count != 0)
                pCodeFileBuilder.Generate_CSharpCode(pNameSpace, $"{GetRelative_To_AbsolutePath(strExportPath)}/Others");
        }

        private void Create_SO(CodeFileBuilder pCodeFileBuilder, CodeNamespace pNameSpace, CodeTypeDeclaration pType, TypeData pSaveData)
        {
            pType.AddBaseClass("UnityEngine.ScriptableObject");
            pNameSpace.Types.Clear();
            pNameSpace.Types.Add(pType);

            var listVirtualFieldOption = pSaveData.listFieldData.Where(pExportOption => pExportOption.bDeleteThisField_InCode == false && pExportOption.bIsVirtualField);
            foreach (var pVirtualField in listVirtualFieldOption)
                pType.AddField(pVirtualField);

            pCodeFileBuilder.Generate_CSharpCode(pNameSpace, $"{GetRelative_To_AbsolutePath(strExportPath)}/{pType.Name}");
        }

        private void Create_GlobalSOContainer(CodeFileBuilder pCodeFileBuilder, CodeNamespace pNameSpace, CodeNamespaceImport[] arrDefaultUsing, CodeTypeDeclaration pType, CodeTypeDeclaration[] arrEnumType, TypeData pSaveData)
        {
            CodeTypeDeclaration pContainerType;
            CodeMemberMethod pInitMethod;
            Create_SOContainer(pNameSpace, arrDefaultUsing, pType, arrEnumType, out pContainerType, out pInitMethod);
            CodeTypeDeclaration pEnumHelperClass = GenerateEnumHelperClass(pNameSpace, pContainerType);

            IEnumerable<FieldTypeData> listKeyField = pSaveData.listFieldData.Where(p => p.bIsKeyField);

            string strValueFieldName = "";
            IEnumerable<FieldTypeData> listRealField = pSaveData.listFieldData.Where(p => p.bIsKeyField == false);
            foreach (var pRealField in listRealField)
            {
                if (pRealField.strFieldName.ToLower().Contains(nameof(EGlobalColumnType.Value).ToLower()))
                {
                    strValueFieldName = pRealField.strFieldName;
                    break;
                }
            }

            HashSet<string> setAlreadyExecute = new HashSet<string>();
            foreach (var pFieldData in listKeyField)
            {
                if (setAlreadyExecute.Contains(pFieldData.strFieldType))
                    continue;
                setAlreadyExecute.Add(pFieldData.strFieldType);

                string strFieldName = $"mapData_Type_Is_{pFieldData.strFieldType}";
                string strMemberType = $"Dictionary<{const_GlobalKey_EnumName + "_" + pFieldData.strFieldType}, {pFieldData.strFieldType}>";

                pContainerType.AddField(new FieldTypeData(strFieldName, strMemberType));
                Generate_CacheMethod_Global(pContainerType, pInitMethod, const_strFieldName_ListData, strFieldName, pFieldData.strFieldName, pFieldData.strFieldType, strValueFieldName);
                Generate_ExtensionMethod_ByEnumKey(pEnumHelperClass, pContainerType, strFieldName, const_GlobalKey_EnumName + "_" + pFieldData.strFieldType, pFieldData.strFieldType, false);
            }

            pCodeFileBuilder.Generate_CSharpCode(pNameSpace, $"{GetRelative_To_AbsolutePath(strExportPath)}/{pContainerType.Name}");
        }


        private void Create_SOContainer(CodeFileBuilder pCodeFileBuilder, CodeNamespace pNameSpace, CodeNamespaceImport[] arrDefaultUsing, CodeTypeDeclaration pType, CodeTypeDeclaration[] arrEnumType, TypeData pSaveData)
        {
            CodeTypeDeclaration pContainerType;
            CodeMemberMethod pInitMethod;
            Create_SOContainer(pNameSpace, arrDefaultUsing, pType, arrEnumType, out pContainerType, out pInitMethod);

            IEnumerable<FieldTypeData> listKeyField = pSaveData.listFieldData.Where(p => p.bIsKeyField);

            CodeTypeDeclaration pEnumHelperClass = listKeyField.Count() > 0 ? GenerateEnumHelperClass(pNameSpace, pContainerType) : null;
            foreach (var pFieldData in listKeyField)
            {
                string strFieldName = "";
                string strMemberType = "";
                if (pFieldData.bIsOverlapKey)
                {
                    strFieldName = $"mapData_Key_Is_{pFieldData.strFieldName}";
                    strMemberType = $"Dictionary<{pFieldData.strFieldType}, List<{pType.Name}>>";
                }
                else
                {
                    strFieldName = $"mapData_Key_Is_{pFieldData.strFieldName}";
                    strMemberType = $"Dictionary<{pFieldData.strFieldType}, {pType.Name}>";
                }

                pContainerType.AddField(new FieldTypeData(strFieldName, strMemberType));
                Generate_CacheMethod(pContainerType, pInitMethod, const_strFieldName_ListData, strFieldName, pFieldData.strFieldName, pFieldData.bIsOverlapKey);

                if (pEnumHelperClass != null)
                    Generate_ExtensionMethod_ByEnumKey(pEnumHelperClass, pContainerType, strFieldName, pFieldData.strFieldType, pType.Name, pFieldData.bIsOverlapKey);
            }

            pCodeFileBuilder.Generate_CSharpCode(pNameSpace, $"{GetRelative_To_AbsolutePath(strExportPath)}/{pContainerType.Name}");
        }

        private static CodeTypeDeclaration GenerateEnumHelperClass(CodeNamespace pNameSpace, CodeTypeDeclaration pContainerType)
        {
            // CodeDom에서 확장메서드를 정식으로 지원하지 않기 때문에 편법으로 생성
            // https://stackoverflow.com/questions/6308310/creating-extension-method-using-codedom

            string strContainerTypeName = pContainerType.Name + "Helper";
            CodeTypeDeclaration pEnumHelperClass = new CodeTypeDeclaration(strContainerTypeName);
            pEnumHelperClass.TypeAttributes = System.Reflection.TypeAttributes.Public;

            pEnumHelperClass.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "\nstatic"));
            pEnumHelperClass.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, String.Empty));

            pNameSpace.Types.Add(pEnumHelperClass);

            return pEnumHelperClass;
        }

        private void Generate_ExtensionMethod_ByEnumKey(CodeTypeDeclaration pEnumHelperClass, CodeTypeDeclaration pContainerType, string strMapFieldName, string strKeyTypeName, string strDataTypeName, bool bIsOverlapKey)
        {
            string strMethodName = $"Get{strDataTypeName}";
            if (bIsOverlapKey)
                strMethodName += "_List";

            var pMethod = pEnumHelperClass.AddMethod(strMethodName);
            pMethod.Attributes = MemberAttributes.Public | MemberAttributes.Static;

            if (bIsOverlapKey)
            {
                pMethod.ReturnType = new CodeTypeReference($"List<{strDataTypeName}>");
            }
            else
            {
                switch (strDataTypeName)
                {
                    case "int": strDataTypeName = "System.Int32"; break;
                    case "float": strDataTypeName = "System.Single"; break;
                }

                pMethod.ReturnType = new CodeTypeReference(strDataTypeName);
            }

            pMethod.Parameters.Add(new CodeParameterDeclarationExpression($"this {strKeyTypeName}", "eKey"));
            pMethod.Parameters.Add(new CodeParameterDeclarationExpression("System.Action<string>", "OnError = null"));

            pMethod.Statements.Add(new CodeSnippetStatement($"          {pMethod.ReturnType.BaseType} pData;"));
            pMethod.Statements.Add(new CodeSnippetStatement($"          if({pContainerType.Name}.{const_strFieldName_instance}.{strMapFieldName}.TryGetValue(eKey, out pData) == false)"));
            pMethod.Statements.Add(new CodeSnippetStatement("          {"));
            pMethod.Statements.Add(new CodeSnippetStatement($"              if(OnError != null)"));
            pMethod.Statements.Add(new CodeSnippetStatement($"                  OnError(nameof({pContainerType.Name}) + \"- Not Found Data // Key : \" + eKey);"));
            pMethod.Statements.Add(new CodeSnippetStatement("          }"));

            if(bIsOverlapKey)
            {

            }

            pMethod.Statements.Add(new CodeSnippetStatement("          return pData;"));
        }

        private void Create_SOContainer(CodeNamespace pNameSpace, CodeNamespaceImport[] arrDefaultUsing, CodeTypeDeclaration pType, CodeTypeDeclaration[] arrEnumType, out CodeTypeDeclaration pContainerType, out CodeMemberMethod pInitMethod)
        {
            string strContainerTypeName = pType.Name + "_Container";
            pContainerType = new CodeTypeDeclaration(strContainerTypeName);
            pContainerType.AddBaseClass("UnityEngine.ScriptableObject");

            pNameSpace.Imports.Clear();
            pNameSpace.Imports.AddRange(arrDefaultUsing);
            pNameSpace.Imports.Add(new CodeNamespaceImport("System.Linq"));
            pNameSpace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            pNameSpace.Types.Clear();
            pNameSpace.Types.Add(pContainerType);
            pNameSpace.Types.AddRange(arrEnumType);

            pContainerType.AddField(new FieldTypeData(const_strFieldName_private_instance, strContainerTypeName), MemberAttributes.Private | MemberAttributes.Static);
            var pPublicInstanceProperty = pContainerType.AddProperty(new FieldTypeData(const_strFieldName_instance, strContainerTypeName), MemberAttributes.Public | MemberAttributes.Static);
            pPublicInstanceProperty.GetStatements.Add(new CodeSnippetStatement($"               return {const_strFieldName_private_instance};"));

            pContainerType.AddField(new FieldTypeData(const_strFieldName_ListData, $"List<{pType.Name}>"));
            pInitMethod = Generate_InitMethod(pContainerType, pType.Name);
        }

        private CodeMemberMethod Generate_InitMethod(CodeTypeDeclaration pContainerType, string strTypeName)
        {
            var pMethod = pContainerType.AddMethod($"DoInit", MemberAttributes.Public | MemberAttributes.Static);

            pMethod.Parameters.Add(new CodeParameterDeclarationExpression(pContainerType.Name, "pSingletonInstance"));
            pMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(bool), "bIsUpdateChildAsset"));

            pMethod.Statements.Add(new CodeSnippetStatement($"          {const_strFieldName_private_instance} = pSingletonInstance;"));

            pMethod.Statements.Add(new CodeSnippetStatement("#if UNITY_EDITOR"));
            pMethod.Statements.Add(new CodeSnippetStatement("           if(bIsUpdateChildAsset)"));
            pMethod.Statements.Add(new CodeSnippetStatement("           {"));
            pMethod.Statements.Add(new CodeSnippetStatement($"              {const_strFieldName_private_instance}.listData.Clear();"));
            pMethod.Statements.Add(new CodeSnippetStatement($"               Object[] arrObject = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(UnityEditor.AssetDatabase.GetAssetPath({const_strFieldName_private_instance}));"));
            pMethod.Statements.Add(new CodeSnippetStatement("               for (int i = 0; i < arrObject.Length; i++)"));
            pMethod.Statements.Add(new CodeSnippetStatement($"                  {const_strFieldName_private_instance}.listData.Add(({strTypeName})arrObject[i]);"));

            pMethod.Statements.Add(new CodeSnippetStatement("               if(Application.isPlaying == false)"));
            pMethod.Statements.Add(new CodeSnippetStatement("               {"));
            pMethod.Statements.Add(new CodeSnippetStatement($"                   UnityEditor.EditorUtility.SetDirty({const_strFieldName_private_instance});"));
            pMethod.Statements.Add(new CodeSnippetStatement("                   UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());"));
            pMethod.Statements.Add(new CodeSnippetStatement("               }"));

            pMethod.Statements.Add(new CodeSnippetStatement("           }"));
            pMethod.Statements.Add(new CodeSnippetStatement("#endif"));

            return pMethod;
        }

        private void Generate_CacheMethod_Global(CodeTypeDeclaration pContainerType, CodeMemberMethod pInitMethod, string strListDataName, string strMapFieldName, string strTypeFieldName, string strTypeName, string strValueFieldName)
        {
            string strMethodName = $"Init_{strMapFieldName}";
            var pMethod = pContainerType.AddMethod(strMethodName);
            pMethod.Attributes = MemberAttributes.Private | MemberAttributes.Final;

            CodeFieldReferenceExpression pCasheMemberReference =
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), strMapFieldName);

            CodeTypeReferenceExpression pField_List = new CodeTypeReferenceExpression($"{strListDataName}");

            // 1. Where로 묶는다.
            CodeMethodInvokeExpression pMethod_CachingLocal = new CodeMethodInvokeExpression(
                pField_List, "Where", new CodeSnippetExpression($"x => x.{strTypeFieldName} == \"{strTypeName}\""));

            CodeVariableDeclarationStatement pGroupbyVariableDeclaration = new CodeVariableDeclarationStatement(
                "var", "arrLocal", pMethod_CachingLocal);

            pMethod.Statements.Add(pGroupbyVariableDeclaration);

            // 3. Gropby로 묶은걸 Dictionary로 변환하며 할당한다.
            // 여기서 기본 형식은 다 형변환해야함
            string strParseString = $"p.{strValueFieldName}";
            if (strTypeName == "float")
            {
                strParseString = $"float.Parse({strParseString})";
            }
            else if (strTypeName == "int")
            {
                strParseString = $"int.Parse({strParseString})";
            }
            else if (strTypeFieldName == "string")
            {

            }
            else
            {
                strParseString = $"({strTypeName})System.Enum.Parse(typeof({strTypeName}), {strParseString})";
                
                // SpreadSheetParser_MainForm.WriteConsole($"Error Parsing Not Define {strTypeName}");
            }


            CodeMethodInvokeExpression pMethod_Caching = new CodeMethodInvokeExpression(
                new CodeVariableReferenceExpression("arrLocal"), "ToDictionary", new CodeSnippetExpression($"p => p.{const_GlobalKey_FieldName}_{strTypeName}, p => {strParseString}"));

            CodeAssignStatement pCachAssign = new CodeAssignStatement(pCasheMemberReference, pMethod_Caching);
            pMethod.Statements.Add(pCachAssign);

            pInitMethod.Statements.Add(new CodeMethodInvokeExpression(
                                            new CodeMethodReferenceExpression(
                                                new CodeSnippetExpression(const_strFieldName_private_instance),
                                                strMethodName)));
        }

        private void Generate_CacheMethod(CodeTypeDeclaration pContainerType, CodeMemberMethod pInitMethod, string strListDataName, string strMapFieldName, string strCacheFieldName, bool bIsOverlapKey)
        {
            string strMethodName = $"Init_{strMapFieldName}";
            var pMethod = pContainerType.AddMethod(strMethodName);
            pMethod.Attributes = MemberAttributes.Private | MemberAttributes.Final;

            CodeFieldReferenceExpression pCasheMemberReference =
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), strMapFieldName);

            CodeTypeReferenceExpression pField_List = new CodeTypeReferenceExpression($"{strListDataName}");
            if (bIsOverlapKey)
            {
                CodeMethodInvokeExpression pMethod_CachingLocal = new CodeMethodInvokeExpression(
                    pField_List, "GroupBy", new CodeSnippetExpression($"x => x.{strCacheFieldName}"));

                CodeVariableDeclarationStatement pGroupbyVariableDeclaration = new CodeVariableDeclarationStatement(
                    "var", "arrLocal", pMethod_CachingLocal);

                pMethod.Statements.Add(pGroupbyVariableDeclaration);

                CodeMethodInvokeExpression pMethod_Caching = new CodeMethodInvokeExpression(
                    new CodeVariableReferenceExpression("arrLocal"), "ToDictionary", new CodeSnippetExpression($"g => g.Key, g => g.ToList()"));

                CodeAssignStatement pCacheAssign = new CodeAssignStatement(pCasheMemberReference, pMethod_Caching);
                pMethod.Statements.Add(pCacheAssign);
            }
            else
            {
                CodeMethodInvokeExpression pMethod_Caching = new CodeMethodInvokeExpression(
                    pField_List, "ToDictionary", new CodeSnippetExpression($"x => x.{strCacheFieldName}"));

                CodeAssignStatement pCacheAssign = new CodeAssignStatement(pCasheMemberReference, pMethod_Caching);
                pMethod.Statements.Add(pCacheAssign);
            }

            pInitMethod.Statements.Add(new CodeMethodInvokeExpression(
                                            new CodeMethodReferenceExpression(
                                                new CodeSnippetExpression(const_strFieldName_private_instance),
                                                strMethodName)));
        }


#if !UNITY_EDITOR
        public override void DoWorkAfter()
        {
            //const string const_BuildMethodeName = "UnitySO_Generator.DoBuild";
            //if (string.IsNullOrEmpty(strUnityEditorPath) == false)
            //    System.Diagnostics.Process.Start(strUnityEditorPath, $"-quit -batchmode -executeMethod {const_BuildMethodeName}");

            //if (bOpenPath_AfterBuild_CSharp)
            //    DoOpenFolder(strExportPath);
        }

        protected override void OnShowForm(Form pFormInstance)
        {
            Work_Generate_Unity_ScriptableObjectForm pForm = (Work_Generate_Unity_ScriptableObjectForm)pFormInstance;
            pForm.DoInit(this);
        }
#endif
    }

}
