package aff.confirm.common.daoinbound.inbound.ejb3;

import aff.confirm.common.daoinbound.common.GenericEJB3DAO;
import aff.confirm.common.daoinbound.inbound.model.InbAttribMapPhraseEntity;

import javax.ejb.*;

/**
 * User: mthoresen
 * Date: Jun 25, 2009
 * Time: 11:30:34 AM
 */
@Stateless
@TransactionAttribute(TransactionAttributeType.REQUIRED)
@Local(InbAttribMapPhraseDAOLocal.class)
@Remote(InbAttribMapPhraseDAORemote.class)
public class InbAttribMapPhraseDAOBean extends GenericEJB3DAO<InbAttribMapPhraseEntity, Long> implements InbAttribMapPhraseDAOLocal, InbAttribMapPhraseDAORemote {
}
