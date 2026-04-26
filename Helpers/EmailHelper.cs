using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using VeterinariaApp.Helpers;

namespace VeterinariaApp.Helpers
{
    public class EmailHelper
    {
        private readonly EmailSettings _settings;

        public EmailHelper(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task EnviarCitaAsignada(string destino, string nombreMascota, string fecha, string veterinario)
        {
            var asunto = "📅 Cita asignada correctamente";

            var mensaje = $@"
            <html>
            <body style='font-family: Arial; padding:20px;'>
                <h2 style='color:#2c3e50;'>🐾 Cita Confirmada</h2>

                <p>Hola 👋,</p>

                <p>Tu cita ha sido asignada exitosamente en <b>Veterinaria App</b>.</p>

                <hr>

                <p><b>🐶 Mascota:</b> {nombreMascota}</p>
                <p><b>📅 Fecha:</b> {fecha}</p>
                <p><b>👨‍⚕️ Veterinario:</b> {veterinario}</p>

                <hr>

                <p style='color:gray;font-size:12px;'>
                    Gracias por confiar en nosotros 💙
                </p>
            </body>
            </html>";

            using var client = new SmtpClient(_settings.SmtpServer)
            {
                Port = _settings.Port,
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, "Veterinaria App"),
                Subject = asunto,
                Body = mensaje,
                IsBodyHtml = true
            };

            mail.To.Add(destino);

            await client.SendMailAsync(mail);
        }
    }
}