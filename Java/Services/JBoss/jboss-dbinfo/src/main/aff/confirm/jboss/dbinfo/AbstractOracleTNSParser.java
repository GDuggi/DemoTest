package aff.confirm.jboss.dbinfo;

import java.io.BufferedInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.PushbackInputStream;
import java.util.HashMap;
import java.util.Map;

public abstract class AbstractOracleTNSParser implements IDBPropertyParser {

    public static final char EOL = '\n';
    public static final char CR = '\r';

    protected AbstractOracleTNSParser() {}

    protected HashMap map = new HashMap();

    public String getNewJDBCURL(String serverName, String oldURL) {
        return getJDBCURL(serverName);
    }

    public String getJDBCURL(String serverName) {
        serverName = serverName.toUpperCase();
        return (String) map.get(serverName);
    }

    public Map getJDBCURLMap() {
        return map;
    }

    /**
     * parse Oracle TNSNAMES.ORA
     */
    protected final void parseInternal(InputStream inputStream) throws IOException {
        PushbackInputStream input = new PushbackInputStream(new BufferedInputStream(inputStream));
        StringBuffer name = new StringBuffer();
        String host = null;
        String port = null;
        String sid = null;
        int c = 0;
        boolean newName = true;
        int bracketCount = 0;
        while ((c = input.read()) != -1) {
            // skip comments
            if (c == '#') {
                readLine(input);
                continue;
            }

            boolean next = true;
            if (c == '(')
                bracketCount++;
            else if (c == ')') {
                bracketCount--;
                if (bracketCount == 0) {
                    map.put(name.toString().trim().toUpperCase(), IpInfo.makeOracleThinClientURL(new IpInfo(host, port, sid)));
                    name = new StringBuffer();
                    newName = true;
                }
            }
            else
                next = false;

            if (next) continue;

            if (c == '=')
                newName = false;

            if (newName) {
                name.append((char)c);
                continue;
            }

            // host(name or ip address) is after HOST =
            if (c == 'H') {
                if ((c=input.read()) == 'O' && (c=input.read()) == 'S' && (c=input.read()) == 'T') {
                    skipWhiteSpaceAnd(input, '=');
                    String iP = readUtil(input, ')');
                    host = iP;
                }
                else
                   input.unread(c);
            }

            // port is after PORT =
            if (c == 'P') {
                if ((c=input.read()) == 'O' && (c=input.read()) == 'R' && (c=input.read()) == 'T') {
                    skipWhiteSpaceAnd(input, '=');
                    port = readUtil(input, ')');
                }
                else
                    input.unread(c);
            }

            // sid is after SID OR SERVICE_NAME
            if (c == 'S') {
                // TRY SID FIRST
                if ((c=input.read()) == 'I' && (c=input.read()) == 'D') {
                    skipWhiteSpaceAnd(input, '=');
                    sid = readUtil(input, ')');
                }
                else
                    input.unread(c);

                // NOW TRY SERVICE_NAME
                if ((c=input.read()) == 'E' && (c=input.read()) == 'R' && (c=input.read()) == 'V' && (c=input.read()) == 'I' &&
                    (c=input.read()) == 'C' && (c=input.read()) == 'E' && (c=input.read()) == '_' && (c=input.read()) == 'N' &&
                    (c=input.read()) == 'A' && (c=input.read()) == 'M' && (c=input.read()) == 'E'
                ) {
                    skipWhiteSpaceAnd(input, '=');
                    sid = readUtil(input, ')');
                }
                else
                    input.unread(c);
            }
        }
    }

    private void skipWhiteSpaceAnd(PushbackInputStream input, char c) throws IOException {
        int cc = -1;
        while ((cc = input.read()) != -1 && (cc == c || Character.isWhitespace((char)cc)));

        if (cc != -1)
            input.unread(cc);

    }

    private String readUtil(PushbackInputStream input, char c) throws IOException {
        StringBuffer buf = new StringBuffer();
        int cc = -1;
        while ((cc = input.read()) != -1 && cc != c) {
            if (cc != -1)
            buf.append((char)cc);
        }

        if (cc != -1) {
            input.unread(cc);
        }

        return buf.toString().trim();

    }

    private void readLine(InputStream input) throws IOException {
        int c = 0;
        while ((c = input.read()) != -1) {
            if (c == EOL)
                return;
            else if (c == CR) {
                if (input.read() != EOL)
                    throw new IOException("'\n' is not after '\r'");

                return;
            }
        }
    }

    private int lastIndexOf(String value, char indexC) {
        int index = value.length() - 1;
        while (index >= 0) {
            if (value.charAt(index) == indexC)
                break;
            else
                index--;
        }
        return index;
    }
}
