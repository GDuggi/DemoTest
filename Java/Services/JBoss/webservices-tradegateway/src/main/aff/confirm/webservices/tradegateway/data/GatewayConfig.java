package aff.confirm.webservices.tradegateway.data;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 1/23/13
 * Time: 4:20 PM
 */
public class GatewayConfig {

    private String tradingSystemCode;
    private String description;
    private String method;
    private String subMethod;
    private String userId;
    private String className;
    private String webServiceURL;
    private String webHeaderElement;

    public String getWebServiceURL() {
        return webServiceURL;
    }

    public void setWebServiceURL(String webServiceURL) {
        this.webServiceURL = webServiceURL;
    }

    public String getClassName() {
        return className;
    }

    public void setClassName(String className) {
        this.className = className;
    }

    public String getTradingSystemCode() {
        return tradingSystemCode;
    }

    public void setTradingSystemCode(String tradingSystemCode) {
        this.tradingSystemCode = tradingSystemCode;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getMethod() {
        return method;
    }

    public void setMethod(String method) {
        this.method = method;
    }

    public String getSubMethod() {
        return subMethod;
    }

    public void setSubMethod(String subMethod) {
        this.subMethod = subMethod;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String pUserID) {
        this.userId = pUserID;
    }

    public String getWebHeaderElement() {
        return webHeaderElement;
    }

    public void setWebHeaderElement(String pWebHeaderElement) {
        this.webHeaderElement = pWebHeaderElement;
    }}
