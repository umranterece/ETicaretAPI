using ETicaretAPI.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ETicaretAPI.Infrastructure.Services.Storage.Local;

public class LocalStorage: Storage, ILocalStorage
{
    readonly IWebHostEnvironment _env;

    public LocalStorage(IWebHostEnvironment env)
    {
        _env = env;
    }
    public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
    {
        string uploadPath = Path.Combine(_env.WebRootPath, path);
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        List<(string fileName, string path)> datas = new();
        foreach (IFormFile file in files)
        {
            string fileNewName= await FileRenameAsync(path , file.Name, HasFile);

            await CopyFileAsync(Path.Combine(uploadPath, fileNewName), file);
            datas.Add((fileNewName, Path.Combine(path, fileNewName).Replace("\\", "/")));
        }
        return datas;
    }
    
    async Task<bool> CopyFileAsync(string path, IFormFile file)
    {
        try
        {
            await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None,
                1024 * 1024, useAsync: false);

            await file.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task DeleteAsync(string path, string fileName)
    {
         File.Delete(Path.Combine(path, fileName));
        //Delete calismazsa hocaninkini yaz
    }

    public List<string> GetFiles(string path)
    {
        DirectoryInfo dir = new(path);
        return dir.GetFiles().Select(f => f.Name).ToList();
    }

    public bool HasFile(string path, string fileName)=>File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path, fileName));
    
}