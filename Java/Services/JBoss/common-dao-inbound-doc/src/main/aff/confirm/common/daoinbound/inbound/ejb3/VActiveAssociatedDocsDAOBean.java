package aff.confirm.common.daoinbound.inbound.ejb3;

import aff.confirm.common.daoinbound.common.GenericEJB3DAO;
import aff.confirm.common.daoinbound.inbound.model.VActiveAssociatedDocsEntity;

import javax.ejb.*;

/**
 * User: mthoresen
 * Date: Sep 25, 2012
 * Time: 12:15:39 PM
 */
@Stateless
@TransactionAttribute(TransactionAttributeType.REQUIRED)
@Local(VActiveAssociatedDocsDAOLocal.class)
@Remote(VActiveAssociatedDocsDAORemote.class)
public class VActiveAssociatedDocsDAOBean extends GenericEJB3DAO<VActiveAssociatedDocsEntity, Long> implements VActiveAssociatedDocsDAOLocal, VActiveAssociatedDocsDAORemote {
}