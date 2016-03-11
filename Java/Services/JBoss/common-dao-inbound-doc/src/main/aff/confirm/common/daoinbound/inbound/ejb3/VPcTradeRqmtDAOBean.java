package aff.confirm.common.daoinbound.inbound.ejb3;

import aff.confirm.common.daoinbound.common.GenericEJB3DAO;
import aff.confirm.common.daoinbound.inbound.model.VPcTradeRqmtEntity;

import javax.ejb.*;

/**
 * User: mthoresen
 * Date: Sep 24, 2012
 * Time: 7:49:14 AM
 */
@Stateless
@TransactionAttribute(TransactionAttributeType.REQUIRED)
@Local(VPcTradeRqmtDAOLocal.class)
@Remote(VPcTradeRqmtDAORemote.class)
public class VPcTradeRqmtDAOBean extends GenericEJB3DAO<VPcTradeRqmtEntity, Long> implements VPcTradeRqmtDAOLocal, VPcTradeRqmtDAORemote {
}
