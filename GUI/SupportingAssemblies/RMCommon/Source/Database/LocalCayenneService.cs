using System;
using com.amphora.cayenne.main;
using com.amphora.cayenne.service.impl;
using NSRMLogging;
using org.apache.cayenne;
using org.apache.cayenne.configuration.server;
using org.apache.cayenne.di;
using org.apache.cayenne.tx;
using REF = System.Reflection;

namespace NSRMCommon {
    public class LocalCayenneService : ICayenneService {

        #region fields
        public static readonly LocalCayenneService sharedInstance;
        Module myModule;
        #endregion

        #region properties
        ServerRuntime runtime { get; set; }
        ObjectContext sharedObjContext { get; set; }
        public bool isOK { get; set; }
        #endregion

        #region cctor
        static LocalCayenneService() {
            try {
                sharedInstance = new LocalCayenneService();
            } catch (Exception ex) {
                Util.show(REF.MethodBase.GetCurrentMethod(),ex);
            }
        }
        #endregion

        #region ctor
        LocalCayenneService() {
            AmphoraDataChannelFilter dataChannelFilter;
            try {
                myModule = new MyPKModel();
                runtime = new ServerRuntime("cayenne-project.xml",myModule);
                sharedObjContext = runtime.newContext();
                dataChannelFilter = new AmphoraDataChannelFilter();
                runtime.getDataDomain().addFilter(dataChannelFilter);
                isOK = true;
            }
            catch (Exception ex) {
                Util.show(REF.MethodBase.GetCurrentMethod(),ex);
            }
        }
        #endregion

        #region ICayenneService Members
        public ObjectContext newObjectContext() { return runtime.newContext(); }
        public object performInTransaction(TransactionalOperation to) {return runtime.performInTransaction(to); }
        public ObjectContext sharedContext() { return this.sharedObjContext; }
        void ICayenneService.listenForCayenneInvalidation(ICayenneInvalidateEvent icie) { Util.show(REF.MethodBase.GetCurrentMethod()); }
        //     void ICayenneService.sendEventInvalidation(string str) { Util.show(REF.MethodBase.GetCurrentMethod()); }
        void ICayenneService.sendEventInvalidation(string str,object obj) { Util.show(REF.MethodBase.GetCurrentMethod()); }
        #endregion

    }

    public class MyPKModel : Module {
        void Module.configure(Binder binder) {
#   if false
            binder.decorate(typeof(DbAdapterDetector)).after(typeof(AmphoraDBAdapterFactory));
#   else
            ModuleHelper.binderHelper(binder);
#   endif
        }
    }
}