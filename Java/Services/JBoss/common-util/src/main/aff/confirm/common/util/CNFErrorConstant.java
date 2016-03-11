package aff.confirm.common.util;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 10/23/12
 * Time: 11:39 AM
 */
public interface CNFErrorConstant {


    String ACTION_FATAL = "FATAL";
    String ACTION_ERROR = "ERROR";
    String ACTION_IGNORE = "IGNORE";
    String ACTION_REDO = "REDO";

    String APP_CODE = "CNF"    ;


    String DATABASE_CONNECTION_ERROR =  "CNF-0001";




    String HTTP_SOCKET_ERROR            =  "CNF-2001";
    String HTTP_IO_EXCEPTION_ERROR      = "CNF-2002";
    String HTTP_AUTHENTICATION_ERROR    = "CNF-2003";
    String ECONFIRM_GENERAL_ERROR       = "CNF-2004";


    String  JMS_CONNECTION_ERROR   = "CNF-30001";

    String ECONF_MONITOR_ERROR = "CNF-4001";
    String ECONF_MONITOR_DB_ERROR = "CNF-4002";







}
