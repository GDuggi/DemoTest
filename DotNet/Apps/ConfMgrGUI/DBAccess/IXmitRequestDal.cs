namespace DBAccess
{   
    public interface IXmitRequestDal
    {
        /// <summary>
        /// Saves the Xmit request for an associated document
        /// </summary>
        /// <param name="id">Associated Doc Id</param>
        /// <param name="destinationType">Fax or email</param>
        /// <param name="destination">The fax number or email address</param>
        /// <param name="userName">The user who performed the transmission</param>
        /// <returns>The ID of the row in the XMIT request for use with callbacks</returns>
        int SaveAssociatedDocumentXmitRequest(
            int id,
            TransmitDestinationType destinationType,
            string destination,
            string userName
            );

        /// <summary>
        /// Saves the Xmit request for a Trade Requirement Confirmation
        /// </summary>
        /// <param name="id">Trade Requirement Confirm Id</param>
        /// <param name="destinationType">Fax or email</param>
        /// <param name="destination">The fax number or email address</param>
        /// <param name="userName">The user who performed the transmission</param>
        /// <returns>The ID of the row in the XMIT request for use with callbacks</returns>
        int SaveTradeRqmtConfirmXmitRequest(
            int id,
            TransmitDestinationType destinationType,
            string destination,
            string userName
            );
    }
}
