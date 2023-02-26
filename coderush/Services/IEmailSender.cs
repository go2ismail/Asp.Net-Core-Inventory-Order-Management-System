﻿using System.Threading.Tasks;

namespace coderush.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
