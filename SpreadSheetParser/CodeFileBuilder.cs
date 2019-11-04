using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetParser
{
    public class FieldData
    {
        public string strFieldName;
        public string strTypeName;
        public string strComment;

        public FieldData(string strFieldName, string strTypeName, string strComment = "")
        {
            this.strFieldName = strFieldName; this.strTypeName = strTypeName; this.strComment = strComment;
        }

        public Type GetFieldType_OrNull()
        {
            System.Type pType = null;
            string strKey = strTypeName.ToLower();
            switch (strKey)
            {
                case "double": pType = typeof(double); break;
                case "float": pType = typeof(float); break;

                case "int": pType = typeof(int); break;
                case "string": pType = typeof(string); break;

                default:

                    break;
            }

            return pType;
        }
    }

    public class EnumFieldData
    {
        public string strFieldName;
        public int iNumber;
        public string strComment;

        public EnumFieldData(string strFieldName)
        {
            this.strFieldName = strFieldName; this.iNumber = int.MaxValue; this.strComment = "";
        }

        public EnumFieldData(string strFieldName, string strComment = "")
        {
            this.strFieldName = strFieldName; this.iNumber = int.MaxValue; this.strComment = strComment;
        }

        public EnumFieldData(string strFieldName, int iNumber = int.MaxValue, string strComment = "")
        {
            this.strFieldName = strFieldName; this.iNumber = iNumber; this.strComment = strComment;
        }
    }


    public class CodeFileBuilder
    {
        CodeNamespace _pNameSpace;
        CodeCompileUnit _pCompileUnit;

        Dictionary<string, System.Type> _mapEnum = new Dictionary<string, Type>();

        public CodeFileBuilder()
        {
            _pCompileUnit = new CodeCompileUnit();

            _pNameSpace = new CodeNamespace();
            _pCompileUnit.Namespaces.Add(_pNameSpace);
        }

        /// <summary>
        /// Generate CSharp source code from the compile unit.
        /// </summary>
        /// <param name="filename">Output file name</param>
        public void GenerateCSharpCode(string fileName)
        {
            if (fileName.Contains(".cs") == false)
                fileName += ".cs";

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (StreamWriter sourceWriter = new StreamWriter(fileName))
            {
                provider.GenerateCodeFromCompileUnit(
                    _pCompileUnit, sourceWriter, options);
            }
        }

        public CodeTypeDeclaration AddClass(string strTypeName, string strComment = "", TypeAttributes eTypeAttributeFlags = TypeAttributes.Public | TypeAttributes.Class)
        {
            CodeTypeDeclaration pClass = new CodeTypeDeclaration(strTypeName);
            _pNameSpace.Types.Add(pClass);

            pClass.IsClass = true;
            pClass.TypeAttributes = eTypeAttributeFlags;

            if (string.IsNullOrEmpty(strComment) == false)
            {
                pClass.Comments.Add(new CodeCommentStatement("<summary>", true));
                pClass.Comments.Add(new CodeCommentStatement(strComment, true));
                pClass.Comments.Add(new CodeCommentStatement("</summary>", true));
            }

            return pClass;
        }

        public CodeTypeDeclaration AddEnum(string strTypeName, string strComment = "")
        {
            CodeTypeDeclaration pClass = new CodeTypeDeclaration(strTypeName);
            _pNameSpace.Types.Add(pClass);
            _mapEnum.Add(strTypeName.ToLower(), pClass.GetType());

            pClass.IsEnum = true;

            if (string.IsNullOrEmpty(strComment) == false)
            {
                pClass.Comments.Add(new CodeCommentStatement("<summary>", true));
                pClass.Comments.Add(new CodeCommentStatement(strComment, true));
                pClass.Comments.Add(new CodeCommentStatement("</summary>", true));
            }

            return pClass;
        }

        #region Setter

        public CodeFileBuilder Set_Namespace(string strNamespace)
        {
            _pNameSpace.Name = strNamespace;

            return this;
        }

        public CodeFileBuilder Set_UsingNameList(List<string> listImportName)
        {
            _pNameSpace.Imports.Clear();
            for (int i = 0; i < listImportName.Count; i++)
                _pNameSpace.Imports.Add(new CodeNamespaceImport(listImportName[i]));

            return this;
        }

        #endregion
    }

    static public class CodeFileHelper
    {
        public static void AddField(this CodeTypeDeclaration pCodeType, FieldData pFieldData)
        {
            CodeMemberField pField = new CodeMemberField();
            pField.Attributes = MemberAttributes.Public;
            pField.Name = pFieldData.strFieldName;

            Type pType = pFieldData.GetFieldType_OrNull();
            if (pType == null)
                pField.Type = new CodeTypeReference(pFieldData.strTypeName);
            else
                pField.Type = new CodeTypeReference(pType);

            if (string.IsNullOrEmpty(pFieldData.strComment) == false)
            {
                pField.Comments.Add(new CodeCommentStatement("<summary>", true));
                pField.Comments.Add(new CodeCommentStatement(pFieldData.strComment, true));
                pField.Comments.Add(new CodeCommentStatement("</summary>", true));
            }

            pCodeType.Members.Add(pField);
        }

        public static void AddEnumField(this CodeTypeDeclaration pCodeType, EnumFieldData pFieldData)
        {
            CodeMemberField pField = new CodeMemberField(pCodeType.Name, pFieldData.strFieldName);

            if(pFieldData.iNumber != int.MaxValue)
                pField.InitExpression = new CodePrimitiveExpression(pFieldData.iNumber);

            if (string.IsNullOrEmpty(pFieldData.strComment) == false)
            {
                pField.Comments.Add(new CodeCommentStatement("<summary>", true));
                pField.Comments.Add(new CodeCommentStatement(pFieldData.strComment, true));
                pField.Comments.Add(new CodeCommentStatement("</summary>", true));
            }

            pCodeType.Members.Add(pField);
        }
    }
}
