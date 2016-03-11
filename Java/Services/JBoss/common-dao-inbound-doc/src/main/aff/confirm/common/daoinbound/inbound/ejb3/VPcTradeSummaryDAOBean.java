package aff.confirm.common.daoinbound.inbound.ejb3;

import aff.confirm.common.daoinbound.common.GenericEJB3DAO;
import aff.confirm.common.daoinbound.inbound.model.VPcTradeSummaryEntity;

import javax.ejb.*;

/**
 * User: mthoresen
 * Date: Sep 19, 2012
 * Time: 12:13:15 PM
 */
@Stateless
@TransactionAttribute(TransactionAttributeType.REQUIRED)
@Local(VPcTradeSummaryDAOLocal.class)
@Remote(VPcTradeSummaryDAORemote.class)
public class VPcTradeSummaryDAOBean extends GenericEJB3DAO<VPcTradeSummaryEntity, Long> implements VPcTradeSummaryDAOLocal, VPcTradeSummaryDAORemote {
}
