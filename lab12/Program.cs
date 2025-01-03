using System;
using System.IO;
using System.IO.Compression;

public static class AVFLog
{
    // Используем папку "Документы" для гарантированного доступа
    private static readonly string LogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "avflogfile.txt");

    public static void WriteLog(string action, string details)
    {
        try
        {
            Console.WriteLine($"Попытка записи лога в: {LogFilePath}"); // Диагностика пути
            using (var writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {action}: {details}");
            }
            Console.WriteLine("Лог успешно записан."); // Подтверждение успешной записи
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка записи в лог: {ex.Message}");
        }
    }

    public static void ReadLog()
    {
        try
        {
            if (File.Exists(LogFilePath))
            {
                Console.WriteLine("Содержимое лог-файла:");
                using (var reader = new StreamReader(LogFilePath))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
            else
            {
                Console.WriteLine("Лог файл отсутствует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения лога: {ex.Message}");
        }
    }

    public static void SearchLog(string keyword)
    {
        try
        {
            if (File.Exists(LogFilePath))
            {
                Console.WriteLine($"Результаты поиска по ключевому слову \"{keyword}\":");
                using (var reader = new StreamReader(LogFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains(keyword))
                        {
                            Console.WriteLine(line);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Лог файл отсутствует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка поиска в логе: {ex.Message}");
        }
    }
}

public static class AVFDiskInfo
{
    public static void ShowAllDriveInfo()
    {
        try
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    Console.WriteLine($"Диск {drive.Name}, объем: {drive.TotalSize}, доступный объем: {drive.AvailableFreeSpace}, метка тома: {drive.VolumeLabel}");
                    AVFLog.WriteLog("ShowAllDriveInfo", $"Диск {drive.Name}, объем: {drive.TotalSize}, доступный объем: {drive.AvailableFreeSpace}, метка тома: {drive.VolumeLabel}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка получения информации о дисках: {ex.Message}");
            AVFLog.WriteLog("ShowAllDriveInfo", $"Ошибка: {ex.Message}");
        }
    }
}

public static class AVFFileInfo
{
    public static void ShowFileInfo(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                var fileInfo = new FileInfo(filePath);
                Console.WriteLine($"Путь: {fileInfo.FullName}, Размер: {fileInfo.Length}, Расширение: {fileInfo.Extension}");
                Console.WriteLine($"Дата создания: {fileInfo.CreationTime}, Дата изменения: {fileInfo.LastWriteTime}");
                AVFLog.WriteLog("ShowFileInfo", $"Файл {fileInfo.FullName}, Размер: {fileInfo.Length}, Расширение: {fileInfo.Extension}");
            }
            else
            {
                Console.WriteLine("Файл не найден.");
                AVFLog.WriteLog("ShowFileInfo", "Ошибка: Файл не найден.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка получения информации о файле: {ex.Message}");
            AVFLog.WriteLog("ShowFileInfo", $"Ошибка: {ex.Message}");
        }
    }
}

public static class AVFDirInfo
{
    public static void ShowDirInfo(string dirPath)
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                var dirInfo = new DirectoryInfo(dirPath);
                Console.WriteLine($"Директория: {dirInfo.FullName}");
                Console.WriteLine($"Количество файлов: {dirInfo.GetFiles().Length}");
                Console.WriteLine($"Количество поддиректорий: {dirInfo.GetDirectories().Length}");
                Console.WriteLine($"Дата создания: {dirInfo.CreationTime}");
                Console.WriteLine("Родительские директории:");
                var parent = dirInfo.Parent;
                while (parent != null)
                {
                    Console.WriteLine($"  {parent.FullName}");
                    parent = parent.Parent;
                }
                AVFLog.WriteLog("ShowDirInfo", $"Директория {dirInfo.FullName}, файлов: {dirInfo.GetFiles().Length}, поддиректорий: {dirInfo.GetDirectories().Length}");
            }
            else
            {
                Console.WriteLine("Директория не найдена.");
                AVFLog.WriteLog("ShowDirInfo", "Ошибка: Директория не найдена.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка получения информации о директории: {ex.Message}");
            AVFLog.WriteLog("ShowDirInfo", $"Ошибка: {ex.Message}");
        }
    }
}

public static class AVFFileManager
{
    public static void InspectDrive(string driveName)
    {
        try
        {
            var inspectDir = Path.Combine(driveName, "AVFInspect");
            Directory.CreateDirectory(inspectDir);

            var dirInfoFile = Path.Combine(inspectDir, "avfdirinfo.txt");
            using (var writer = new StreamWriter(dirInfoFile))
            {
                foreach (var dir in Directory.GetDirectories(driveName))
                {
                    writer.WriteLine($"Директория: {dir}");
                }

                foreach (var file in Directory.GetFiles(driveName))
                {
                    writer.WriteLine($"Файл: {file}");
                }
            }

            var copiedFile = Path.Combine(inspectDir, "avfdirinfo_copy.txt");

            if (File.Exists(copiedFile))
            {
                File.Delete(copiedFile);
            }

            File.Copy(dirInfoFile, copiedFile);
            File.Delete(dirInfoFile);
            AVFLog.WriteLog("InspectDrive", $"Информация сохранена в {copiedFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при инспектировании диска: {ex.Message}");
            AVFLog.WriteLog("InspectDrive", $"Ошибка: {ex.Message}");
        }
    }

    public static void ArchiveFiles(string sourceDir, string archivePath)
    {
        try
        {
            ZipFile.CreateFromDirectory(sourceDir, archivePath);
            Console.WriteLine($"Директория {sourceDir} заархивирована в {archivePath}");
            AVFLog.WriteLog("ArchiveFiles", $"Архив создан: {archivePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при архивировании: {ex.Message}");
            AVFLog.WriteLog("ArchiveFiles", $"Ошибка: {ex.Message}");
        }
    }

    public static void ExtractFiles(string archivePath, string extractDir)
    {
        try
        {
            ZipFile.ExtractToDirectory(archivePath, extractDir);
            Console.WriteLine($"Архив {archivePath} разархивирован в {extractDir}");
            AVFLog.WriteLog("ExtractFiles", $"Архив разархивирован: {extractDir}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при разархивировании: {ex.Message}");
            AVFLog.WriteLog("ExtractFiles", $"Ошибка: {ex.Message}");
        }
    }
}

class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Программа запущена.");
            AVFLog.WriteLog("Main", "Начало работы программы");

            AVFDiskInfo.ShowAllDriveInfo();
            AVFDirInfo.ShowDirInfo("C:\\");
            AVFFileManager.InspectDrive("C:\\");
            AVFFileInfo.ShowFileInfo("C:\\path_to_some_file.txt");

            Console.WriteLine("Введите ключевое слово для поиска в логах:");
            string keyword = Console.ReadLine();
            AVFLog.SearchLog(keyword);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Общая ошибка: {ex.Message}");
            AVFLog.WriteLog("Main", $"Ошибка: {ex.Message}");
        }
    }
}
