﻿using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.FileManagers;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class FileService : IFileService
    {
        private readonly CatchUpDbContext _context;
        private readonly IFileStorage _fileStorage;
        public FileService(FileStorageFactory fileStorageFactory, CatchUpDbContext context)
        {
            _context = context;
            _fileStorage = fileStorageFactory.CreateFileStorage();
        }

        public async Task<FileDto> UploadFile(IFormFile newFile, int? materialID)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{newFile.FileName}";
            using Stream fileStream = newFile.OpenReadStream();
            var fileSource = await _fileStorage.UploadFile(uniqueFileName, fileStream);

            FileModel fileModel = new FileModel(newFile.FileName, newFile.ContentType, fileSource);
            await _context.Files.AddAsync(fileModel);
            await _context.SaveChangesAsync();

            if (materialID != null)
                await AddToMaterial(fileModel.Id, (int)materialID);

            await _context.SaveChangesAsync();

            return new FileDto
            {
                Id = fileModel.Id,
                Name = fileModel.Name,
                Type = fileModel.Type,
                Source = fileModel.Source
            };
        }

        public async Task<bool> DeleteFile(int fileId)
        {
            var file = await _context.Files.FindAsync(fileId);
            if (file == null)
                return false;
            
            file.State = StateEnum.Deleted;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ArchiveFile(int fileId)
        {
            var file = await _context.Files.FindAsync(fileId);
            if (file == null)
                return false;

            file.State = StateEnum.Archived;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<FileDto> GetById(int fileId)
        {
            var file = await _context.Files.FindAsync(fileId);

            if (file == null || file.State != StateEnum.Active)
                return null;

            return new FileDto { Id = file.Id, Name = file.Name, Type = file.Type, Source = file.Source };
        }

        public async Task<Stream> DownloadFile(int fileId)
        {
            var file = await _context.Files.FindAsync(fileId);
            if (file == null || file.State != StateEnum.Active)
                return null;

            return await _fileStorage.DownloadFile(file.Source);
        }
        public async Task<bool> AddToMaterial(int fileId, int materialId)
        {
            var file = await _context.Files.FindAsync(fileId);
            if (file == null || file.State != StateEnum.Active)
                return false;

            var fim = await _context.FileInMaterials.FindAsync(fileId, materialId);
            if (fim != null)
            {
                fim.State = StateEnum.Active;
                await _context.SaveChangesAsync();
                return true;
            }

            if (await _context.Materials.FindAsync(materialId) == null)
                return false;

            var connectFileMaterial = new FileInMaterial(materialId, fileId);
            await _context.AddAsync(connectFileMaterial);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FileDto>> GetFiles(int materialId)
        {
            return await _context.FileInMaterials
                .Where(fim => fim.MaterialId == materialId && fim.State == StateEnum.Active)
                .Join(
                    _context.Files,
                    fim => fim.FileId,
                    file => file.Id,
                    (fim, file) => new FileDto
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Type = file.Type,
                        Source = file.Source,
                    })
                .ToListAsync();
        }

        public async Task<List<FileDto>> GetAllFiles()
        {
            if (await _context.Files.AnyAsync())
            {
                return await _context.Files
                    .Where(file => file.State == StateEnum.Active)
                    .Select(file => new FileDto
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Type = file.Type
                    })
                    .ToListAsync();
            }

            return new List<FileDto>();
        }
    }
}
