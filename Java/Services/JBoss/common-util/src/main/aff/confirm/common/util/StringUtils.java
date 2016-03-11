package aff.confirm.common.util;

/**
 * User: ifrankel
 * Date: Sep 15, 2003
 * Time: 1:40:32 PM
 * To change this template use Options | File Templates.
 */
public class StringUtils {

    /**
     * StringOfChar returns a string containing Count characters with the
     * character value given by pChar. For example,
     * S := StringOfChar('A', 10);
     * sets S to the string 'AAAAAAAAAA'.
     * @param pChar
     * @return
     */
    public static String stringOfChar(String pChar, int pCount){
        String result = "";
        int count;
        for (count = 1;count <= pCount;count++)
            result = result + pChar;
        return result;
    }

    /**
     * Converts a value to string format, prefixing as many leading zeros as
     * necessary to bring it to the specified format length.
     * @param pValue
     * @param pFmtLength
     * @return
     */
    public static String zeroFill(int pValue, int pFmtLength){
        String result = "";
        // string value so length can be determined
        String tempFrom = String.valueOf(pValue);
        //determine length
        int valueLength = tempFrom.length();
        //build concatenated string of specified length, left filling zeros
        result = stringOfChar("0", pFmtLength - valueLength) + tempFrom;

        return result;
    }

    /**
     * Takes an array and searches it for a specific value
     * @param pArray
     * @param pValue
     * @return
     */
    public static boolean isValueInArray(String pValue, String[] pArray){
        boolean foundIt = false;
        int count;
        for (count=0; count < pArray.length; count++){
            if (pValue.equalsIgnoreCase(pArray[count])){
                foundIt = true;
                break;
            }
        }
        return foundIt;
    }

    public  static int getNumOfOccurrences(String pStringToTest, String pFindStr){
    int lastIndex = 0;
    int count = 0;

    while(lastIndex != -1){
        lastIndex = pStringToTest.indexOf(pFindStr,lastIndex);

        if( lastIndex != -1){
            count ++;
            lastIndex+=pFindStr.length();
        }
    }

    return count;
}


}
