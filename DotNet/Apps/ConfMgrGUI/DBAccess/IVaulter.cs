
namespace DBAccess
{
    public interface IVaulter
    {
        void VaultAssociatedDoc(int docId, string metaData);
        void VaultTradeRqmtConfirm(int confirmId, string metaData);
    }
}
