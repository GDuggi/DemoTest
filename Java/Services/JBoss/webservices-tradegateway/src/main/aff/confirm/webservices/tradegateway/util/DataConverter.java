package aff.confirm.webservices.tradegateway.util;

import javax.xml.bind.*;
import javax.xml.stream.XMLOutputFactory;
import javax.xml.stream.XMLStreamException;
import javax.xml.stream.XMLStreamWriter;
import java.io.InputStream;
import java.io.StringReader;
import java.io.StringWriter;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Types;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 1/28/13
 * Time: 1:06 PM
 */
public class DataConverter {

    public static <T> T convertXMLToObject(Class<T> docClass,String xml) throws JAXBException {

        String className = docClass.getName();
        JAXBContext jc=JAXBContext.newInstance(new Class[] {docClass});
        Unmarshaller unmarshaller = jc.createUnmarshaller();
        T object = (T) unmarshaller.unmarshal(new StringReader(xml));
        return  object;
    }

    public  static String convertObjectToXML(Class docClass, Object object) throws JAXBException {

        StringWriter sw= new StringWriter();
        JAXBContext jc = JAXBContext.newInstance(new Class[] {docClass});
        Marshaller marshaller = jc.createMarshaller();
        marshaller.marshal(object,sw);
        return sw.toString();

    }


    public  static String convertResultSetToXML(ResultSet rs, String rootElement) throws XMLStreamException, SQLException {

        StringWriter  sw = new StringWriter();
        XMLStreamWriter writer = null;
        ResultSetMetaData metaData =null;
        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");


        XMLOutputFactory factory = XMLOutputFactory.newInstance();

        writer = factory.createXMLStreamWriter(sw);
        //Israel 9/16/2013 Replaced TradeAlertData class with String as return object
        //writer.writeStartElement("ContractData");
        writer.writeStartElement(rootElement);
        metaData = rs.getMetaData();
        for (int i=1; i<=metaData.getColumnCount(); ++i) {
            String columnName = metaData.getColumnName(i).toLowerCase();
            writer.writeStartElement(columnName);
            switch (metaData.getColumnType(i)) {
                case Types.BIGINT:
                case Types.NUMERIC:
                case Types.INTEGER:
                case Types.FLOAT:
                    double dblValue =rs.getDouble(i);
                    if (!rs.wasNull()) {
                        writer.writeCharacters(new Double(dblValue).toString());
                    }
                    break;
                case Types.DATE:
                case Types.TIME:
                case Types.TIMESTAMP:
                    Date  date = rs.getDate(i);
                    if (!rs.wasNull()) {
                        writer.writeCharacters(sdf.format(date));
                    }
                    break;
                case Types.CHAR:
                case Types.VARCHAR :
                    String data = rs.getString(i);
                    if (!rs.wasNull()) {
                        writer.writeCharacters(data);
                    }

            }
            writer.writeEndElement();

        }
        writer.writeEndElement();
        writer.flush();
        writer.close();
        return  sw.toString();
    }


}
