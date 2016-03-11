package aff.confirm.common.daoinbound.inbound.ejb3;

import aff.confirm.common.daoinbound.common.GenericEJB3DAO;
import aff.confirm.common.daoinbound.inbound.model.InbAttribEntity;

import javax.ejb.*;

/**
 * User: mthoresen
 * Date: Jun 25, 2009
 * Time: 11:21:21 AM
 */
@Stateless
@TransactionAttribute(TransactionAttributeType.REQUIRED)
@Local(InbAttribDAOLocal.class)
@Remote(InbAttribDAORemote.class)
public class InbAttribDAOBean extends GenericEJB3DAO<InbAttribEntity, Long> implements InbAttribDAOLocal, InbAttribDAORemote {
}
