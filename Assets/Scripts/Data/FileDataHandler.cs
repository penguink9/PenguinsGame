using UnityEngine;
using System.IO;
public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "word";

    public FileDataHandler(string dataDirPath, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.useEncryption = useEncryption;
    }
    public void SetFileName(string fileName)
    {
        dataFileName = fileName;
    }
    public void SaveData<T>(T data)
    {
        string filePath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // Ensure the directory exists
            if (!Directory.Exists(dataDirPath))
            {
                Directory.CreateDirectory(dataDirPath);
            }
            // Check if the data is null
            if (data == null)
            {
                Debug.LogError("Data to save is null.");
                return;
            }
            if (useEncryption)
            {
                // Encrypt data before saving
                string encryptedData = Encrypt(JsonUtility.ToJson(data));
                File.WriteAllText(filePath, encryptedData);
            }
            else
            {
                // Save data as plain JSON
                File.WriteAllText(filePath, JsonUtility.ToJson(data));
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Failed to save data to {filePath}: {e.Message}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"An error occurred while saving data: {e.Message}");
        }

    }
    public T LoadData<T>()
    {
        string filePath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                if (useEncryption)
                {
                    // Decrypt data after loading
                    fileContent = Decrypt(fileContent);
                }
                return JsonUtility.FromJson<T>(fileContent);
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Failed to load data from {filePath}: {e.Message}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"An error occurred while loading data: {e.Message}");
        }
        return default(T); // Return default value if file does not exist
    }
    private string Encrypt(string data)
    {
        // Simple encryption logic (for demonstration purposes)
        char[] encryptedChars = new char[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            encryptedChars[i] = (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return new string(encryptedChars);
    }
    private string Decrypt(string data)
    {
        // Simple decryption logic (same as encryption)
        return Encrypt(data); // Since the encryption is symmetric
    }

}
