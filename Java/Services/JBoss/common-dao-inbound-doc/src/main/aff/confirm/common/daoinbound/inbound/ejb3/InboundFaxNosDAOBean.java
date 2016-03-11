package aff.confirm.common.daoinbound.inbound.ejb3;

import aff.confirm.common.daoinbound.common.GenericEJB3DAO;
import aff.confirm.common.daoinbound.inbound.model.InboundFaxNosEntity;

import javax.ejb.*;

/**
 * User: mthoresen
 * Date: Mar 18, 2010
 * Time: 9:05:48 AM
 */
@Stateless
@TransactionAttribute(TransactionAttributeType.REQUIRED)
@Local(InboundFaxNosDAOLocal.class)
@Remote(InboundFaxNosDAORemote.class)
public class InboundFaxNosDAOBean extends GenericEJB3DAO<InboundFaxNosEntity, Long> implements InboundFaxNosDAOLocal, InboundFaxNosDAORemote {
}
