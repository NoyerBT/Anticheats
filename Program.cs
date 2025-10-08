using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

public class AccountChecker
{
    public static void Main()
    {
        Console.WriteLine("Iniciando el verificador de cuentas de Steam...");
        Console.WriteLine("----------------------------------------------------");

        try
        {
            int steamAccountCount = GetSteamAccountCount();

            Console.WriteLine($"Se han detectado {steamAccountCount} cuentas de Steam en este equipo.");
            Console.WriteLine("----------------------------------------------------");

            // Lógica de validación
            const int maxAllowedAccounts = 3;
            if (steamAccountCount > maxAllowedAccounts)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ALERTA: Se ha excedido el límite de {maxAllowedAccounts} cuentas de Steam permitidas.");
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
            Console.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("\nPresiona cualquier tecla para cerrar el programa.");
        Console.ReadKey();
    }

    /// <summary>
    /// Obtiene el número de cuentas de Steam que han iniciado sesión en el equipo.
    /// </summary>
    /// <returns>El número total de cuentas de Steam encontradas.</returns>
    public static int GetSteamAccountCount()
    {
        try
        {
            string? steamPath = GetSteamInstallPath();
            if (string.IsNullOrEmpty(steamPath))
            {
                throw new Exception("No se pudo encontrar la instalación de Steam en este equipo.");
            }

            string loginUsersVdfPath = Path.Combine(steamPath, "config", "loginusers.vdf");
            if (!File.Exists(loginUsersVdfPath))
            {
                // Si el archivo no existe, es posible que nadie haya iniciado sesión todavía.
                return 0;
            }

            string vdfContent = File.ReadAllText(loginUsersVdfPath);
            
            // Contamos el número de SteamID64 (números de 17 dígitos entre comillas)
            Regex steamId64Regex = new Regex("\"765[0-9]{14}\"");
            MatchCollection matches = steamId64Regex.Matches(vdfContent);

            return matches.Count;
        }
        catch (Exception e)
        {
            // Lanza una excepción más descriptiva para que el método Main la maneje.
            throw new Exception("Ocurrió un error al leer los datos de las cuentas de Steam.", e);
        }
    }

    /// <summary>
    /// Busca la ruta de instalación de Steam en el registro de Windows.
    /// </summary>
    /// <returns>La ruta de instalación de Steam o null si no se encuentra.</returns>
    private static string? GetSteamInstallPath()
    {
        // Steam guarda su ruta en el registro de Windows.
        // La clave puede estar en HKEY_LOCAL_MACHINE para una instalación para todos los usuarios
        // o en HKEY_CURRENT_USER para una instalación del usuario actual.
        string keyPath = @"SOFTWARE\Valve\Steam";
        string valueName = "InstallPath";

        // Primero intentamos en HKEY_CURRENT_USER
        using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(keyPath))
        {
            if (key != null)
            {
                object? path = key.GetValue(valueName);
                if (path != null)
                {
                    return path.ToString();
                }
            }
        }

        // Si no, intentamos en HKEY_LOCAL_MACHINE (para instalaciones de 64-bit y 32-bit)
        keyPath = @"SOFTWARE\WOW6432Node\Valve\Steam";
        using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(keyPath))
        {
            if (key != null)
            {
                object? path = key.GetValue(valueName);
                if (path != null)
                {
                    return path.ToString();
                }
            }
        }
        
        return null; // No se encontró la ruta
    }
}
