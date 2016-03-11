package aff.confirm.common.daoinbound.inbound.ejb3;

import aff.confirm.common.daoinbound.common.GenericEJB3DAO;
import aff.confirm.common.daoinbound.inbound.model.InboundDocsEntity;

import javax.ejb.*;

/**
 * User: mthoresen
 * Date: Jun 22, 2009
 * Time: 1:34:56 PM
 */
@Stateless
@TransactionAttribute(TransactionAttributeType.REQUIRED)
@Local(InboundDocsDAOLocal.class)
@Remote(InboundDocsDAORemote.class)
public class InboundDocsDAOBean extends GenericEJB3DAO<InboundDocsEntity, Long> implements InboundDocsDAOLocal, InboundDocsDAORemote {
}
