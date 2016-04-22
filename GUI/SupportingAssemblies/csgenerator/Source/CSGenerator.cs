//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.CSharp;

namespace NSCSGen {
    public partial class CSGenerator {
        #region fields
        static CodeDomProvider provider = null;
        static CodeGeneratorOptions opts = null;
        static readonly CodeExpression ceZero = new CodePrimitiveExpression(0);
        static readonly CodeExpression ceValue = new CodePropertySetValueReferenceExpression();
        static readonly CodeExpression ceThis = new CodeThisReferenceExpression();
        #endregion

        #region properties
        static CodeGeneratorOptions options {
            get {
                if (opts == null)
                    opts = new CodeGeneratorOptions();
                return opts;
            }
        }
        #endregion

        #region methods

        public static void generateCode(string path,string className,IDataReader reader,string nameSpace) {
            CodeCompileUnit ccu;
            CodeNamespace ns0,ns;
            CodeConstructor cc;
            CodeTypeDeclaration ctd;
            StringBuilder sb = null;
            CodeArgumentReferenceExpression careReader;
            CodeVariableReferenceExpression vri;
            CodeStatementCollection csc;
            CodeIterationStatement cis;
            TextWriter tw;
            bool isFile = false;
            string fname = null;

            if (provider == null)
                provider = new CSharpCodeProvider();
            ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(ns0 = ns = new CodeNamespace());
            if (!string.IsNullOrEmpty(nameSpace))
                ccu.Namespaces.Add(ns = new CodeNamespace(nameSpace));

            careReader = new CodeArgumentReferenceExpression("reader");
            vri = new CodeVariableReferenceExpression("i");
            ccu.Namespaces.Add(ns = new CodeNamespace(string.IsNullOrEmpty(nameSpace) ? string.Empty : nameSpace));

            ns0.Imports.AddRange(
                new CodeNamespaceImport[] {
                    new CodeNamespaceImport("System.Data"),
                    new CodeNamespaceImport("System.Diagnostics"),
                });
            ns.Types.Add(ctd = new CodeTypeDeclaration(className));
            ctd.Comments.AddRange(new CodeCommentStatement[] {
                new CodeCommentStatement("<summary>class "+className+"</summary>",true),
                new CodeCommentStatement("<remarks>Generated by user '"+Environment.UserName+
                    "' on machine '"+System.Net.Dns.GetHostName()+
                    "' on "+DateTime.Now.ToString("dd-MMM-yy hh:mm:ss")+"</remarks>",true)
            });

            ctd.Members.AddRange(findProperties(reader,out csc,careReader,vri));
            ctd.Members.Add(cc = new CodeConstructor());

            cc.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start,"ctor"));
            cc.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End,"ctor"));

            cc.Attributes = MemberAttributes.Public;
            cc.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(IDataReader)),careReader.ParameterName));

            cc.Statements.Add(
                cis = new CodeIterationStatement(
                    new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(int)),vri.VariableName,ceZero),
                    new CodeBinaryOperatorExpression(vri,CodeBinaryOperatorType.LessThan,new CodePropertyReferenceExpression(careReader,"FieldCount")),
                    new CodeSnippetStatement(vri.VariableName + "++")));

            cis.Statements.AddRange(new CodeStatement[] {
                new CodeConditionStatement(new CodeMethodInvokeExpression(careReader,"IsDBNull",vri),
                    new CodeSnippetStatement("\t\t\t\tcontinue;")),
                new CodeSnippetStatement("\t\t\tswitch("+careReader.ParameterName +".GetName("+vri.VariableName +")) {"),
            });
            cis.Statements.AddRange(csc);
            cis.Statements.AddRange(
                new CodeStatement[] {
                    new CodeSnippetStatement("\t\t\t}")
                });

            options.BlankLinesBetweenMembers = false;
            options.ElseOnClosing = true;

            if (string.IsNullOrEmpty(path))
                tw = new StringWriter(sb = new StringBuilder());
            else {
                string aPath = path;

                if (Directory.Exists(Path.Combine(path,"Source")))
                    path = Path.Combine(path,"Source");
                tw = new StreamWriter(fname = Path.Combine(path,className + "." + provider.FileExtension));
                isFile = true;
            }
            provider.GenerateCodeFromNamespace(ns0,tw,options);
            if (ns0 != ns)
                provider.GenerateCodeFromNamespace(ns,tw,options);
            tw.Close();
            tw.Dispose();
            if (isFile) {
                Console.Write("wrote: " + fname);
                Debug.Print("wrote: " + fname);
            } else
                Debug.Print(sb.ToString());
        }

        public static void generateCodeFor(Type type,IDataReader reader) {
            generateCode(null,type.Name,reader,null);
        }

        static CodeTypeMemberCollection findProperties(IDataReader reader,out CodeStatementCollection csc,CodeExpression ce,CodeExpression vri) {
            CodeMemberProperty p;
            CodeFieldReferenceExpression fr;
            CodeMemberField f;
            CodeTypeMemberCollection ret = new CodeTypeMemberCollection();
            string name,fname;
            Type type;
            csc = new CodeStatementCollection();
            CodeMethodReferenceExpression cmreString,cmreInt,cmreShort;
            CodeExpression ceblah;
            CodeAttributeDeclaration cad;

            cad = new CodeAttributeDeclaration("DebuggerBrowsable",
                new CodeAttributeArgument(
                    new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("DebuggerBrowsableState"),"Never")));

            cmreString = new CodeMethodReferenceExpression(ce,"GetString");
            cmreInt = new CodeMethodReferenceExpression(ce,"GetInt32");
            cmreShort = new CodeMethodReferenceExpression(ce,"GetInt16");
            p = null;
            f = null;
            for (int i = 0 ;i < reader.FieldCount ;i++) {
                name = makePropertyName(fname = reader.GetName(i));
                type = findPropertyType(reader.GetFieldType(i));
                fr = new CodeFieldReferenceExpression(ceThis,"_" + name);

                ret.AddRange(new CodeTypeMember[] {
                    f=new CodeMemberField( new CodeTypeReference(type),fr.FieldName),
                    p = new CodeMemberProperty()
                });
                if (i == 0) {
                    f.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start,"fields"));
                    p.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start,"properties"));
                }
                f.Attributes = 0;
                f.CustomAttributes.Add(cad);

                p.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                p.Name = name;
                p.Type = new CodeTypeReference(type);
                p.HasSet = p.HasGet = true;
                p.GetStatements.Add(new CodeMethodReturnStatement(fr));
                p.SetStatements.Add(new CodeAssignStatement(fr,ceValue));

                if (type.Equals(typeof(string))) {
                    ceblah = new CodeMethodInvokeExpression(new CodeMethodInvokeExpression(cmreString,vri),"Trim");
                } else if (type.Equals(typeof(int))) {
                    ceblah = new CodeMethodInvokeExpression(cmreInt,vri);
                } else if (type.Equals(typeof(short))) {
                    ceblah = new CodeMethodInvokeExpression(cmreShort,vri);
                } else if (type.Equals(typeof(double))) {
                    ceblah = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(ce,"GetDouble"),vri);
                } else if (type.Equals(typeof(decimal))) {
                    ceblah = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(ce,"GetDecimal"),vri);
                } else if (type.Equals(typeof(DateTime))) {
                    ceblah = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(ce,"GetDateTime"),vri);

                } else
                    throw new InvalidOperationException("ugh: found " + type.FullName);

                csc.AddRange(new CodeStatement[] {
                    new CodeSnippetStatement ("\t\t\t\tcase \""+fname+"\":"),
                    new CodeAssignStatement(fr,ceblah),
                    new CodeSnippetStatement ("\t\t\t\t\tbreak;")
                });
            }
            if (f != null)
                f.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End,"fields"));

            if (p != null)
                p.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End,"properties"));
            return ret;
        }

        public static string makePropertyName(string p) {
            return makeName(p,false);
        }

        public static string makeClassName(string p) {
            return makeName(p,true);
        }

        static string makeName(string p,bool isClass) {
            StringBuilder sb = new StringBuilder();
            bool doUpper = false;
            int len = string.IsNullOrEmpty(p) ? 0 : p.Length;
            char c;

            for (int i = 0 ;i < len ;i++) {
                c = p[i];
                //            foreach (char c in p) {
                if (doUpper || (isClass && i == 0)) {
                    sb.Append(char.ToUpper(c),1);
                    doUpper = false;
                } else {
                    if (c == '_')
                        doUpper = true;
                    else
                        sb.Append(c,1);
                }
            }
            return sb.ToString();
        }

        static Type findPropertyType(Type type) {
            if (type.Equals(typeof(int))) return typeof(int);
            else if (type.Equals(typeof(string))) return typeof(string);
            else if (type.Equals(typeof(short))) return typeof(short);
            else if (type.Equals(typeof(double))) return typeof(double);
            else if (type.Equals(typeof(decimal))) return typeof(decimal);
            else if (type.Equals(typeof(DateTime))) return typeof(DateTime);
            else {
#if true
                Debug.Print("unhandled: " + type.FullName);
                return typeof(string);
#else
                throw new NotImplementedException();
#endif
            }
        }
        #endregion

    }
}