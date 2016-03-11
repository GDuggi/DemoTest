package aff.confirm.common.util;

import java.util.Properties;

import static org.junit.Assert.assertNotNull;

/**
 * @author rnell
 * @date 2015-07-06
 */
public class BeanUtilTester {
    @org.junit.Test
    public void testPopulate() throws Exception {
        Properties p = new Properties();
       // p.setProperty( "CITY", "MyCity");
        p.setProperty( "city", "MyCity");
        p.setProperty("name", "MyName");
        p.setProperty("pOCode", "MyPostalCode");

        Test test = new Test();
        BeanUtil.populate( test, p );

        assertNotNull(test.city);
        assertNotNull(test.name);
        assertNotNull( test.postalCode );

        System.out.println( test );
    }
}
