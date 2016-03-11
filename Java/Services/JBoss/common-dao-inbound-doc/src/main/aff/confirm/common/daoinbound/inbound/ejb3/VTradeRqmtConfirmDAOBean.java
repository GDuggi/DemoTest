package aff.confirm.common.daoinbound.inbound.ejb3;

import aff.confirm.common.daoinbound.common.GenericEJB3DAO;
import aff.confirm.common.daoinbound.inbound.model.VTradeRqmtConfirmEntity;

import javax.ejb.*;

/**
 * User: mthoresen
 * Date: Sep 25, 2012
 * Time: 3:03:53 PM
 */
@Stateless(mappedName = "RogerWasHere" )
@TransactionAttribute(TransactionAttributeType.REQUIRED)
@Local(VTradeRqmtConfirmDAOLocal.class)
@Remote(VTradeRqmtConfirmDAORemote.class)
public class VTradeRqmtConfirmDAOBean extends GenericEJB3DAO<VTradeRqmtConfirmEntity, Long> implements VTradeRqmtConfirmDAOLocal, VTradeRqmtConfirmDAORemote {
}
