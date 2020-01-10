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
    public static class TypeParser
    {
        static public Type GetFieldType_OrNull(string strTypeName)
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

    public class FieldData
    {
        public string strFieldName;
        public string strTypeName;
        public string strComment;

        public FieldData(string strFieldName, string strTypeName, string strComment = "")
        {
            this.strFieldName = strFieldName; this.strTypeName = strTypeName; this.strComment = strComment;
        }
    }

    public class EnumFieldData
    {
        public string strValue;
        public int iNumber;
        public string strComment;

        public EnumFieldData()
        {
            this.strValue = strValue; this.iNumber = int.MaxValue; this.strComment = "";
        }

        public EnumFieldData(string strValue)
        {
            this.strValue = strValue; this.iNumber = int.MaxValue; this.strComment = "";
        }

        public EnumFieldData(string strValue, string strComment = "")
        {
            this.strValue = strValue; this.iNumber = int.MaxValue; this.strComment = strComment;
        }

        public EnumFieldData(string strValue, int iNumber = int.MaxValue, string strComment = "")
        {
            this.strValue = strValue; this.iNumber = iNumber; this.strComment = strComment;
        }
    }


    public class CodeFileBuilder
    {
        public CodeNamespace pNamespaceCurrent { get; private set; }
        public CodeCompileUnit pCompileUnit { get; private set; }

        CodeTypeDeclarationCollection _arrCodeTypeDeclaration = new CodeTypeDeclarationCollection();

        public CodeFileBuilder()
        {
            pNamespaceCurrent = new CodeNamespace();

            pCompileUnit = new CodeCompileUnit();
            pCompileUnit.Namespaces.Add(pNamespaceCurrent);
        }

        public void Generate_CSharpCode(string strFilePath)
        {
            if (strFilePath.Contains(".cs") == false)
                strFilePath += ".cs";

            pNamespaceCurrent.Types.Clear();
            pNamespaceCurrent.Types.AddRange(_arrCodeTypeDeclaration);

            Generate_CSharpCode(pCompileUnit, strFilePath);
        }

        public void Generate_CSharpCode(CodeNamespace pNamespace, string strFilePath)
        {
            if (strFilePath.Contains(".cs") == false)
                strFilePath += ".cs";

            CodeCompileUnit pCompileUnit = new CodeCompileUnit();
            pCompileUnit.Namespaces.Add(pNamespace);

            Generate_CSharpCode(pCompileUnit, strFilePath);
        }

        private void Generate_CSharpCode(CodeCompileUnit pCompileUnit, string strFilePath)
        {
            CodeDomProvider pProvider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions pOptions = new CodeGeneratorOptions();
            pOptions.BracingStyle = "C";
            using (StreamWriter pSourceWriter = new StreamWriter(strFilePath))
            {
                pProvider.GenerateCodeFromCompileUnit(
                    pCompileUnit, pSourceWriter, pOptions);
            }
        }


        public CodeTypeDeclarationCollection GetCodeTypeDeclarationCollection()
        {
            return _arrCodeTypeDeclaration;
        }

        public CodeTypeDeclaration AddCodeType(string strTypeName, string strComment = "", TypeAttributes eTypeAttributeFlags = TypeAttributes.Public)
        {
            CodeTypeDeclaration pCodeType = new CodeTypeDeclaration(strTypeName);
            _arrCodeTypeDeclaration.Add(pCodeType);

            pCodeType.TypeAttributes = eTypeAttributeFlags;
            pCodeType.AddComment(strComment);

            return pCodeType;
        }


        #region Setter

        public CodeFileBuilder Set_Namespace(string strNamespace)
        {
            pNamespaceCurrent.Name = strNamespace;

            return this;
        }

        public CodeFileBuilder Set_UsingList(params string[] arrImportName)
        {
            pNamespaceCurrent.Imports.Clear();
            for (int i = 0; i < arrImportName.Length; i++)
                pNamespaceCurrent.Imports.Add(new CodeNamespaceImport(arrImportName[i]));

            return this;
        }

        #endregion
    }

    static public class CodeFileHelper
    {
        public static void AddComment(this CodeTypeDeclaration pCodeType, string strComment)
        {
            if (string.IsNullOrEmpty(strComment))
                return;

            pCodeType.Comments.Add(new CodeCommentStatement("<summary>", true));
            pCodeType.Comments.Add(new CodeCommentStatement(strComment, true));
            pCodeType.Comments.Add(new CodeCommentStatement("</summary>", true));
        }

        public static void AddField(this CodeTypeDeclaration pCodeType, FieldData pFieldData)
        {
            CodeMemberField pField = new CodeMemberField();
            pField.Attributes = MemberAttributes.Public;
            pField.Name = pFieldData.strFieldName;

            Type pType = TypeParser.GetFieldType_OrNull(pFieldData.strTypeName);
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
            CodeMemberField pField = new CodeMemberField(pCodeType.Name, pFieldData.strValue);

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

        public static void AddBaseClass(this CodeTypeDeclaration pCodeType, Type pBaseType)
        {
            CodeTypeReference pBaseTypeRef = new CodeTypeReference(pBaseType);
            pCodeType.BaseTypes.Add(pBaseTypeRef);
        }
    }
}
