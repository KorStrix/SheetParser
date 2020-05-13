using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SpreadSheetParser
{
    public static class TypeParser
    {
        static public Type GetFieldType_OrNull(string strTypeName)
        {
            Type pType = null;
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
        public string strValue;
        public int iNumber;
        public string strComment;

        public EnumFieldData()
        {
            strValue = ""; iNumber = int.MaxValue; strComment = "";
        }

        public EnumFieldData(string strValue)
        {
            this.strValue = strValue; iNumber = int.MaxValue; strComment = "";
        }

        public EnumFieldData(string strValue, string strComment = "")
        {
            this.strValue = strValue; iNumber = int.MaxValue; this.strComment = strComment;
        }

        public EnumFieldData(string strValue, int iNumber = int.MaxValue, string strComment = "")
        {
            this.strValue = strValue; this.iNumber = iNumber; this.strComment = strComment;
        }
    }


    public class CodeFileBuilder
    {
        public CodeDomProvider pProvider_Csharp { get; private set; } = new Microsoft.CSharp.CSharpCodeProvider();

        public CodeNamespace pNamespaceCurrent { get; private set; }
        public CodeCompileUnit pCompileUnit { get; private set; }

        public List<string> listDefaultUsing = new List<string>();

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
            CodeGeneratorOptions pOptions = new CodeGeneratorOptions();
            pOptions.BracingStyle = "C";
            using (StreamWriter pSourceWriter = new StreamWriter(strFilePath))
            {
                // CodeDom API 에선 상단 주석을 커스터마이징 할 수 없으므로 하단으로 대체
                // 파일을 열고 CodeDom을 통해 작성하기 직전에 글을 작성
                // https://stackoverflow.com/questions/2289889/how-do-i-customize-the-auto-generated-comment-when-using-net-codedom-code-gener?noredirect=1&lq=1
                pSourceWriter.WriteLine("//------------------------------------------------------------------------------");
                pSourceWriter.WriteLine("// Author : Strix");
                pSourceWriter.WriteLine("// Github : https://github.com/KorStrix/SheetParser");

                pProvider_Csharp.GenerateCodeFromCompileUnit(
                    pCompileUnit, pSourceWriter, pOptions);
            }
        }


        public CodeTypeDeclarationCollection GetCodeTypeDeclarationCollection()
        {
            return _arrCodeTypeDeclaration;
        }

        public CodeTypeDeclaration AddCodeType(string strTypeName, ESheetType eType, string strComment = "")
        {
            CodeTypeDeclaration pCodeType = new CodeTypeDeclaration(strTypeName);
            _arrCodeTypeDeclaration.Add(pCodeType);

            switch (eType)
            {
                case ESheetType.Class: pCodeType.IsClass = true; break;
                case ESheetType.Struct: pCodeType.IsStruct = true; break;
                case ESheetType.Enum: pCodeType.IsEnum = true; break;
            }

            pCodeType.TypeAttributes = TypeAttributes.Public;
            pCodeType.AddComment(strComment);

            return pCodeType;
        }


        #region Setter

        public void Set_UsingList(params string[] arrImportName)
        {
            pNamespaceCurrent.Imports.Clear();
            for (int i = 0; i < listDefaultUsing.Count; i++)
                pNamespaceCurrent.Imports.Add(new CodeNamespaceImport(listDefaultUsing[i]));

            for (int i = 0; i < arrImportName.Length; i++)
                pNamespaceCurrent.Imports.Add(new CodeNamespaceImport(arrImportName[i]));
        }

        public void Add_UsingList(params string[] arrImportName)
        {
            for (int i = 0; i < arrImportName.Length; i++)
                pNamespaceCurrent.Imports.Add(new CodeNamespaceImport(arrImportName[i]));
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

        public static void RemoveField(this CodeTypeDeclaration pCodeType, string strFieldName)
        {
            foreach (CodeTypeMember pMember in pCodeType.Members)
            {
                if (pMember.Name.Equals(strFieldName))
                {
                    pCodeType.Members.Remove(pMember);
                    break;
                }
            }
        }

        public static void AddField(this CodeTypeDeclaration pCodeType, FieldTypeData pFieldData)
        {
            CodeMemberField pField = new CodeMemberField();
            pField.Attributes = MemberAttributes.Public;
            pField.Name = pFieldData.strFieldName;

            Type pType = TypeParser.GetFieldType_OrNull(pFieldData.strFieldType);
            if (pType == null)
                pField.Type = new CodeTypeReference(pFieldData.strFieldType);
            else
                pField.Type = new CodeTypeReference(pType);

            if(pFieldData.bIsVirtualField)
            {
                pFieldData.strComment = $"자동으로 할당되는 필드입니다. 의존되는 필드 : <see cref=\"{pFieldData.strDependencyFieldName}\"/>";
            }

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
            if (string.IsNullOrEmpty(pFieldData.strValue.Trim()))
                return;

            foreach (CodeTypeMember pMember in pCodeType.Members)
            {
                if (pMember.Name == pFieldData.strValue)
                    return;
            }

            CodeMemberField pField = new CodeMemberField(pCodeType.Name, pFieldData.strValue);

            if (pFieldData.iNumber != int.MaxValue)
                pField.InitExpression = new CodePrimitiveExpression(pFieldData.iNumber);

            if (string.IsNullOrEmpty(pFieldData.strComment) == false)
            {
                pField.Comments.Add(new CodeCommentStatement("<summary>", true));
                pField.Comments.Add(new CodeCommentStatement(pFieldData.strComment, true));
                pField.Comments.Add(new CodeCommentStatement("</summary>", true));
            }

            pCodeType.Members.Add(pField);
        }

        public static void AddBaseClass(this CodeTypeDeclaration pCodeType, string strBaseTypeName)
        {
            CodeTypeReferenceCollection pCollectionBackup = new CodeTypeReferenceCollection(pCodeType.BaseTypes);
            pCodeType.BaseTypes.Clear();

            CodeTypeReference pBaseTypeRef = new CodeTypeReference(strBaseTypeName);
            pCodeType.BaseTypes.Add(pBaseTypeRef);

            pCodeType.BaseTypes.AddRange(pCollectionBackup);
        }

        public static void AddBaseInterface(this CodeTypeDeclaration pCodeType, string strBaseTypeName)
        {
            CodeTypeReference pBaseTypeRef = new CodeTypeReference(strBaseTypeName);
            pCodeType.BaseTypes.Add(pBaseTypeRef);
        }

        public static CodeMemberMethod AddMethod(this CodeTypeDeclaration pCodeType, string strMethodName, MemberAttributes eAttribute = MemberAttributes.Public | MemberAttributes.Final)
        {
            CodeMemberMethod pMethod = new CodeMemberMethod();
            pMethod.Attributes = eAttribute;
            pMethod.Name = strMethodName;

            pCodeType.Members.Add(pMethod);

            return pMethod;
        }

    }
}
