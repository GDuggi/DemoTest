package cnf.docflow.util;

/**
 * Created by jvega on 10/14/2015.
 */
public class TradeDoesNotExistException extends Exception {

    public TradeDoesNotExistException(){}

    public TradeDoesNotExistException(String message)
    {
        super(message);
    }

}
