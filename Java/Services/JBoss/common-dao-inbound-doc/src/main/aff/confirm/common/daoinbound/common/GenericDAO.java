package aff.confirm.common.daoinbound.common;

import javax.persistence.EntityManager;
import java.io.Serializable;
import java.util.List;

/**
 * User: GenericDAO
 * Date: Mar 17, 2008
 * Time: 2:30:40 PM
 */
public interface GenericDAO<T, ID extends Serializable> {
    T findById(ID id, boolean lock);

    List<T> findAll();

    List<T> findByExample(T exampleInstance, String... excludeProperty);

    T makePersistent(T entity);

    void makeTransient(T entity);

    EntityManager getEntityManager();
}
