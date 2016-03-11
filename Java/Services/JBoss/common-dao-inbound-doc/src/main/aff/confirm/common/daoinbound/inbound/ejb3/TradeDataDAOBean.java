package aff.confirm.common.daoinbound.inbound.ejb3;

import aff.confirm.common.daoinbound.common.GenericEJB3DAO;
import aff.confirm.common.daoinbound.inbound.model.TradeDataEntity;

import javax.ejb.*;


/**
 * User: mthoresen
 * Date: Feb 3, 2010
 * Time: 10:03:10 AM
 */

@Stateless
@TransactionAttribute(TransactionAttributeType.REQUIRED)
@Local(TradeDataDAOLocal.class)
@Remote(TradeDataDAORemote.class)
public class TradeDataDAOBean extends GenericEJB3DAO<TradeDataEntity, Long> implements TradeDataDAOLocal, TradeDataDAORemote {
}
