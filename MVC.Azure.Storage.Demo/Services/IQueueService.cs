using MVC.Azure.Storage.Demo.Models.Data;

namespace MVC.Azure.Storage.Demo.Services
{
    public interface IQueueService
    {
        Task SendMessage(EmailMessage emailMessage);
    }
}