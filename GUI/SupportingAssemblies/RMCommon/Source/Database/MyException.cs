using System;
using System.Reflection;
using NSRMLogging;
namespace NSRMCommon {
    public class MyException : Exception {
        public MyException(MethodBase mb) : base(mb == null ? "UGH" : Util.makeSig(mb)) { }
    }
}