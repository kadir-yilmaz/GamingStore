using GamingStore.EL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.BLL.Abstract
{
    public interface IAdminMailService
    {
        Task SendMailAsync(MailRequest mailRequest);
    }
}
