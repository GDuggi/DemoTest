package aff.confirm.opsmanager.creditmargin.common;

import javax.swing.text.BadLocationException;
import java.io.*;
import org.jboss.logging.Logger;


/**
 * User: srajaman
 * Date: Dec 9, 2008
 * Time: 12:24:53 PM
 */
public class RTFInserter {
    private static Logger log = Logger.getLogger(RTFInserter.class.getName());

    public String insertMarginToken(String contractData, String creditTerms, String marginToken, CreditLogRec clr) throws IOException, BadLocationException {

        int pos =contractData.indexOf(marginToken);
        System.out.println("Token word found  " + marginToken + " found at " + pos);
        if (  pos > 0) {
            String creditTermBody = creditTerms;
            if ( creditTerms != null && !"".equalsIgnoreCase(creditTerms)) {
                creditTermBody = getBodyText(creditTerms);
            }
            contractData = removeTokenPrefix(contractData.substring(0,pos)) + creditTermBody + removeTokenSuffix(contractData.substring(pos+marginToken.length()));
            clr.setCmt("Credit Margin Token Found and Replaced");
        }
        else {
            clr.setCmt("Credit Margin Token Not Found");
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


    public static void main(String[] arg){


        try {
            File file = new File("c:\\rtftest\\contract.rtf");
            byte[] contract = new byte[(int) file.length()];

            FileInputStream fis = new FileInputStream("c:\\rtftest\\contract.rtf");
            fis.read(contract,0,contract.length);
            String contractData = new String(contract);
            fis.close();

            File file2 = new File("c:\\rtftest\\token.rtf");
            FileInputStream fis1 = new FileInputStream("c:\\rtftest\\token.rtf");
            byte[] term = new byte[(int) file2.length()];
            fis1.read(term,0,term.length);
            fis1.close();
            String creditTerm = new String(term);
            RTFInserter ri = new RTFInserter();
            String newContract = ri.insertMarginToken(contractData,creditTerm,"[MARGIN TOKEN]",null);
            BufferedWriter bw  = new BufferedWriter(new FileWriter("c:\\rtftest\\result.rtf"));
            bw.write(newContract);
            bw.close();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (BadLocationException e) {
            e.printStackTrace();
        }
    }
}
