using System;
using System.Management;

public class AccountChecker
{
    public static void Main()
    {
        Console.WriteLine("Iniciando el verificador de cuentas para Left 4 Dead 2...");
        Console.WriteLine("----------------------------------------------------");

        try
        {
            int userAccountCount = GetLocalUserAccountCount();

            Console.WriteLine($"Se han detectado {userAccountCount} cuentas de usuario en este equipo.");
            Console.WriteLine("----------------------------------------------------");

            // Lógica de validación
            const int maxAllowedAccounts = 3;
            if (userAccountCount > maxAllowedAccounts)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ALERTA: Se ha excedido el límite de {maxAllowedAccounts} cuentas permitidas.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Verificación completada. El número de cuentas está dentro del límite permitido.");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Error al intentar leer las cuentas de usuario: {ex.Message}");
            Console.WriteLine("Asegúrate de ejecutar esta herramienta con permisos de administrador.");
            Console.ResetColor();
        }

        Console.WriteLine("\nPresiona cualquier tecla para cerrar el programa.");
        Console.ReadKey();
    }

    /// <summary>
    /// Obtiene el número de cuentas de usuario locales en el sistema.
    /// </summary>
    /// <returns>El número total de cuentas de usuario.</returns>
    public static int GetLocalUserAccountCount()
    {
        int count = 0;
        try
        {
            // Utiliza WMI (Windows Management Instrumentation) para consultar las cuentas
            SelectQuery query = new SelectQuery("Win32_UserAccount", "LocalAccount = true");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject user in searcher.Get())
            {
                // Opcional: Imprimir el nombre de cada cuenta encontrada para depuración
                // Console.WriteLine($" - Cuenta encontrada: {user["Name"]}");
                count++;
            }
        }
        catch (ManagementException e)
        {
            // Lanza una excepción para que el método Main la maneje
            throw new Exception("No se pudo realizar la consulta WMI. ¿Tienes los permisos necesarios?", e);
        }
        
        return count;
    }
}
