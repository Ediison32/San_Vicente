using System.Net;
using System.Net.Mail;
using medicos_c.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace medicos_c.Utils.EmailSend
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<(bool exito, string mensaje)> EnviarConfirmacionCitaAsync(Citas cita)
        {
            try
            {
                var paciente = cita.Pacientes;
                var fromAddress = new MailAddress(_emailSettings.SmtpUser, "Clínica Médica");
                var toAddress = new MailAddress(paciente.Email, $"{paciente.Nombre} {paciente.Apellido}");
                Console.WriteLine("try de enviarndo correo +++++++++++++++" + paciente.Email);
                Console.WriteLine("try de enviarndo correo +++++++++++++++" + paciente.Nombre);
                string subject = "Confirmación de cita médica";
                string body = $@"
                    Estimado/a {paciente.Nombre} {paciente.Apellido},

                    Su cita médica ha sido confirmada con éxito.

                    Fecha de la cita: {cita.FechaCita:dddd, dd MMMM yyyy}
                    Hora: {cita.FechaCita:hh:mm tt}
                    Médico: Dr. {cita.Medicos?.Nombre} {cita.Medicos?.Apellido}

                    Por favor, llegue 10 minutos antes de la hora programada.

                    Gracias por confiar en nosotros.
                    Clínica Médica";

                using var smtp = new SmtpClient
                {
                    Host = _emailSettings.SmtpServer,
                    Port = _emailSettings.SmtpPort,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass)
                };

                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };
                await smtp.SendMailAsync(message);

                Console.WriteLine(" ok  try de enviarndo correo +++++++++++++++" + paciente.Nombre);
                return (true, "Correo enviado correctamente ✅");
            }
            catch (SmtpException ex)
            {
                return (false, $"❌ Error SMTP: {ex.StatusCode} - {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"❌ Error general: {ex.Message}");
            }
        }

    }
}
