package aff.confirm.common.daoinbound.inbound.ejb3;

import aff.confirm.common.daoinbound.common.GenericEJB3DAO;
import aff.confirm.common.daoinbound.inbound.model.VInboundDocsEntity;

import javax.ejb.*;

/**
 * User: mthoresen
 * Date: Sep 25, 2012
 * Time: 10:14:44 AM
 */
@Stateless
@TransactionAttribute(TransactionAttributeType.REQUIRED)
@Local(VInboundDocsDAOLocal.class)
@Remote(VInboundDocsDAORemote.class)
public class VInboundDocsDAOBean extends GenericEJB3DAO<VInboundDocsEntity, Long> implements VInboundDocsDAOLocal, VInboundDocsDAORemote {
}