package aff.confirm.opsmanager.confirm.common;

import org.jboss.logging.Logger;


public class RTFInserter {
    private static Logger log = Logger.getLogger(RTFInserter.class);

	public  String getIfMarginTokenReplaced(String contractData, String creditTerms, String marginToken)  {

        int pos =contractData.indexOf(marginToken);
        log.info("Token word found  " + marginToken + " found at " + pos);
        if (  pos > 0) {
            String creditTermBody = creditTerms;
            if ( creditTerms != null && !"".equalsIgnoreCase(creditTerms)) {
                creditTermBody = getBodyText(creditTerms);
            }
            contractData = removeTokenPrefix(contractData.substring(0,pos)) + creditTermBody + removeTokenSuffix(contractData.substring(pos+marginToken.length()));
        }
        else {
        	// set the contract string is null so the caller can process
        	// that there is no margin token replaced, 
        	contractData = null;
        }
        return contractData;
    }
	

    private String removeTokenPrefix(String data){

        int len = data.lastIndexOf("\\par");
        if (len > 0) {
            return data.substring(0,len+4);
        }
        return data;
    }
    private String removeTokenSuffix(String data){
        int len = data.indexOf("\\par");
        if (len > 0) {
            return data.substring(len+4);
        }
        return data;
    }
    private String getBodyText(String creditTerms) {


        int numOpenBracket = 0;
        int beginIndex = creditTerms.indexOf("{\\footer");
        do {
            if ("{".equalsIgnoreCase(creditTerms.substring(beginIndex,beginIndex+1))) {
                ++numOpenBracket;
            }
            else if ("}".equalsIgnoreCase(creditTerms.substring(beginIndex,beginIndex+1))) {
                --numOpenBracket;
            }
            ++beginIndex;
        }
        while (numOpenBracket != 0 && beginIndex > 0);
        String returnText = creditTerms.substring(beginIndex);
        int index = returnText.lastIndexOf("}");
        returnText = returnText.substring(0,index);
        log.info("Credit Term text = " + returnText);
        return returnText;
     }

    
}
