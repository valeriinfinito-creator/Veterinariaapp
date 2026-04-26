using VeterinariaApp.Models;

namespace VeterinariaApp.Validators
{
    public static class PropietarioValidator
    {
        public static (bool IsValid, string Message) Validar(Propietario propietario)
        {
            if (string.IsNullOrWhiteSpace(propietario.Nombre))
                return (false, "El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(propietario.Documento))
                return (false, "El documento es obligatorio");

            if (string.IsNullOrWhiteSpace(propietario.Email))
                return (false, "El email es obligatorio");

            if (!propietario.Email.Contains("@"))
                return (false, "Email inválido");

            if (string.IsNullOrWhiteSpace(propietario.Telefono))
                return (false, "El teléfono es obligatorio");

            return (true, "OK");
        }
    }
}