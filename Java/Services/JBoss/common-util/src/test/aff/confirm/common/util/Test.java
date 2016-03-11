package aff.confirm.common.util;

/**
 * @author rnell
 * @date 2015-07-06
 */
public class Test {
    String name;
    String city;
    String postalCode;

    public void setName(String name) {
        this.name = name;
    }

    public void setCity(String city) {
        this.city = city;
    }

    public void setPOCode(String postalCode) {
        this.postalCode = postalCode;
    }

    @Override
    public String toString() {
        return "Test{" +
                "name='" + name + '\'' +
                ", city='" + city + '\'' +
                ", postalCode='" + postalCode + '\'' +
                '}';
    }
}
