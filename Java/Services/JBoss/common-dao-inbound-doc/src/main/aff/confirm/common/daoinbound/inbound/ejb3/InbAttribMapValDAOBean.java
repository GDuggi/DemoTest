package aff.confirm.common.daoinbound.inbound.ejb3;

import aff.confirm.common.daoinbound.common.GenericEJB3DAO;
import aff.confirm.common.daoinbound.inbound.model.InbAttribMapValEntity;

import javax.ejb.*;

/**
 * User: mthoresen
 * Date: Jun 25, 2009
 * Time: 11:42:29 AM
 */
@Stateless
@TransactionAttribute(TransactionAttributeType.REQUIRED)
@Local(InbAttribMapValDAOLocal.class)
@Remote(InbAttribMapValDAORemote.class)
public class InbAttribMapValDAOBean extends GenericEJB3DAO<InbAttribMapValEntity, Long> implements InbAttribMapValDAOLocal, InbAttribMapValDAORemote {
}
