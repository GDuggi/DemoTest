/*
 * User: islepini
 * Date: Oct 11, 2002
 * Time: 11:01:29 AM
 * To change template for new class use 
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.jboss.mail;

public class EmailGroup extends Object {
    String groupName;
    String members;
    public EmailGroup(String groupName, String members) {
        this.groupName = groupName;
        this.members = members;
    }
}
