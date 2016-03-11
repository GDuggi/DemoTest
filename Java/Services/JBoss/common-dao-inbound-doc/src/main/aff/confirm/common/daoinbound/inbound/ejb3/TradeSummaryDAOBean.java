package aff.confirm.common.daoinbound.inbound.ejb3;

import aff.confirm.common.daoinbound.common.GenericEJB3DAO;
import aff.confirm.common.daoinbound.inbound.model.TradeSummaryEntity;

import javax.ejb.*;

/**
 * User: mthoresen
 * Date: Feb 4, 2010
 * Time: 2:32:24 PM
 */
@Stateless
@TransactionAttribute(TransactionAttributeType.REQUIRED)
@Local(TradeSummaryDAOLocal.class)
@Remote(TradeSummaryDAORemote.class)
public class TradeSummaryDAOBean extends GenericEJB3DAO<TradeSummaryEntity, Long> implements TradeSummaryDAOLocal, TradeSummaryDAORemote {
}
