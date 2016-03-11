package aff.confirm.opsmanager.creditmargin;

import aff.confirm.jboss.common.service.ServiceMBean;


/**
 * User: srajaman
 * Date: Dec 3, 2008
 * Time: 10:54:38 AM
 */
public interface CreditMarginServiceMBean extends ServiceMBean {


    String applyCreditMargin(String tradeSystem,long tradeId,int version);
       String getTibcoServerName();
    void setTibcoServerName(String serverName);

    String getUser();
    void setUser(String userId);

 //   String getPwd();
    void   setPwd(String pwd);

    String getQueueName();
    void setQueueName(String queueName);

    String getNotifyAddr();
    void setNotifyAddr(String emailAddr);

    void setSmtpHost(String smtpHost);
	String getSmtpHost();

	void setSmtpPort(String smtpPort);
	String getSmtpPort();

    void setCreditStatusUrl(String statusUrl);
    String getCreditStatusUrl();

    void setCreditMarginUrl(String marginUrl);
    String getCreditMarginUrl();

    void setCreditMarginToken(String marginToken);
    String getCreditMarginToken();

    void setTraderUrl(String traderUrl);
    String getTraderUrl();

    void setVaultEJBName(String ejbName);
    String getVaultEJBName();

    void setDbUserName(String userName);
    String getDbUserName();

    void setDbpwd(String pwd);
 //   String getDbpwd();



}
