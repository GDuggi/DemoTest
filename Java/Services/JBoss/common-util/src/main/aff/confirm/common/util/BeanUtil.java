package aff.confirm.common.util;

import org.apache.commons.beanutils.*;
import org.jboss.logging.Logger;

import java.beans.IndexedPropertyDescriptor;
import java.beans.PropertyDescriptor;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.*;

/**
 * @author rnell
 * @date 2015-07-01
 */
public class BeanUtil {
    private final static Logger log = Logger.getLogger(BeanUtil.class);

    public static void setProperties(Properties p, Object o) throws InvocationTargetException, IllegalAccessException {
        // Make all properties start with lowercase
        try {
            populate(o, p);

        } catch (Exception e) {
            throw new IllegalArgumentException("Failed to set a property " + p.stringPropertyNames() + " for bean of class " + o.getClass().getName(), e);
        }
    }

    public static void populate(Object bean, Map properties) throws IllegalAccessException, InvocationTargetException, NoSuchMethodException {
        if(bean != null && properties != null) {
            if(log.isDebugEnabled()) {
                log.debug("populate(" + bean + ", " + properties + ")");
            }

            Iterator names = properties.keySet().iterator();

            while(true) {
                while(true) {
                    String name;
                    do {
                        if(!names.hasNext()) {
                            return;
                        }

                        name = (String)names.next();
                    } while(name == null);

                    Object value = properties.get(name);
                    if(log.isTraceEnabled()) {
                        log.trace("  name=\'" + name + "\', value.class=\'" + (value == null?"NONE":value.getClass().getName()) + "\', value=\'" + (value == null?"NONE":value) + "\'");
                    }

                    PropertyDescriptor descriptor = null;
                    DynaProperty dynaProperty = null;

                    try {
                        if(bean instanceof DynaBean) {
                            dynaProperty = ((DynaBean)bean).getDynaClass().getDynaProperty(name);
                        } else {
                            descriptor = PropertyUtils.getPropertyDescriptor(bean, name);
                        }
                    } catch (Throwable e) {
                        if(log.isTraceEnabled()) {
                            log.trace("getPropertyDescriptor", e);
                        }

                        descriptor = null;
                        dynaProperty = null;
                    }

                    if(descriptor == null && dynaProperty == null) {
                        StringBuilder sb = new StringBuilder();
                        Set<String> l = new TreeSet<>();
                        for( PropertyDescriptor pd : PropertyUtils.getPropertyDescriptors( bean.getClass() ) ) {
                                l.add( pd.getName() );
                        }

                        throw new IllegalArgumentException("No such property: " + name + " in bean " + bean.getClass( ) + ", valid properties:\n" + l );
                    } else {
                        if(log.isTraceEnabled()) {
                            if(descriptor != null) {
                                log.trace("Property descriptor is \'" + descriptor + "\'");
                            } else {
                                log.trace("DynaProperty descriptor is \'" + descriptor + "\'");
                            }
                        }

                        Method newValue;
                        if(descriptor != null) {
                            newValue = null;
                            if(descriptor instanceof IndexedPropertyDescriptor) {
                                newValue = ((IndexedPropertyDescriptor)descriptor).getIndexedWriteMethod();
                            } else if(descriptor instanceof MappedPropertyDescriptor) {
                                newValue = ((MappedPropertyDescriptor)descriptor).getMappedWriteMethod();
                            }

                            if(newValue == null) {
                                newValue = descriptor.getWriteMethod();
                            }

                            if(newValue == null) {
                                if(log.isTraceEnabled()) {
                                    log.trace("No setter method for property " + name );
                                }
                            } else {
                                Class[] type = newValue.getParameterTypes();
                                if(log.isTraceEnabled()) {
                                    log.trace("Setter method is \'" + newValue.getName() + "(" + type[0].getName() + (type.length > 1?", " + type[1].getName():"") + ")\'");
                                }

                                Class e = type[0];
                                if(type.length > 1) {
                                    e = type[1];
                                }

                                Object[] parameters = new Object[1];
                                if(e.isArray()) {
                                    if(value instanceof String) {
                                        String[] e1 = new String[]{(String)value};
                                        parameters[0] = ConvertUtils.convert((String[])e1, e);
                                    } else if(value instanceof String[]) {
                                        parameters[0] = ConvertUtils.convert((String[])value, e);
                                    } else {
                                        parameters[0] = value;
                                    }
                                } else if(value instanceof String) {
                                    parameters[0] = ConvertUtils.convert((String)value, e);
                                } else if(value instanceof String[]) {
                                    parameters[0] = ConvertUtils.convert(((String[])value)[0], e);
                                } else {
                                    parameters[0] = value;
                                }

                                try {
                                    PropertyUtils.setProperty(bean, name, parameters[0]);
                                } catch (NoSuchMethodException var13) {
                                    log.error("  CANNOT HAPPEN (setProperty()): ", var13);
                                }
                            }
                        } else {
                            newValue = null;
                            Class type1 = dynaProperty.getType();
                            Object newValue1;
                            if(type1.isArray()) {
                                if(value instanceof String) {
                                    String[] e2 = new String[]{(String)value};
                                    newValue1 = ConvertUtils.convert((String[])e2, type1);
                                } else if(value instanceof String[]) {
                                    newValue1 = ConvertUtils.convert((String[])value, type1);
                                } else {
                                    newValue1 = value;
                                }
                            } else if(value instanceof String) {
                                newValue1 = ConvertUtils.convert((String)value, type1);
                            } else if(value instanceof String[]) {
                                newValue1 = ConvertUtils.convert(((String[])value)[0], type1);
                            } else {
                                newValue1 = value;
                            }

                            try {
                                PropertyUtils.setProperty(bean, name, newValue1);
                            } catch (NoSuchMethodException var12) {
                                log.error("    CANNOT HAPPEN (setProperty())", var12);
                            }
                        }
                    }
                }
            }
        }
    }

}
