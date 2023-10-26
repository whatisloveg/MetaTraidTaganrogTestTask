using MetaTraidTaganrogTestTask.Models;

namespace MetaTraidTaganrogTestTask.Interfaces
{
    public interface IClientRepository
    {
        List<Client> GetAllClients();
        void AddClient(Client client);
    }
}
