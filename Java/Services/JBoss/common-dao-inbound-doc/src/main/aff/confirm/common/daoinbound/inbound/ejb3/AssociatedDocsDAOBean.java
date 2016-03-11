package aff.confirm.common.daoinbound.inbound.ejb3;

import aff.confirm.common.daoinbound.common.GenericEJB3DAO;
import aff.confirm.common.daoinbound.inbound.model.AssociatedDocsEntity;

import javax.ejb.*;

/**
 * User: mthoresen
 * Date: Sep 2, 2009
 * Time: 9:22:10 AM
 */

@Stateless
@TransactionAttribute(TransactionAttributeType.REQUIRED)
@Local(AssociatedDocsDAOLocal.class)
@Remote(AssociatedDocsDAORemote.class)
public class AssociatedDocsDAOBean extends GenericEJB3DAO<AssociatedDocsEntity, Long> implements AssociatedDocsDAOLocal, AssociatedDocsDAORemote {
}
