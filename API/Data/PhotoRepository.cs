using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;
        public PhotoRepository(DataContext context)
        {
            _context = context;
        }
      

        public async Task<Photo> GetPhotoById(int id)
        {
            return await _context.Photos.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos()
        {
            return await _context.Photos
                .IgnoreQueryFilters()
                .Where(p => p.IsApproved == false)
                .Select(p => new PhotoForApprovalDto
                {
                    PhotoId = p.Id,
                    Url = p.Url,
                    Username = p.AppUser.UserName,
                    IsApproved = p.IsApproved
                })
                .ToListAsync();
        }

        public void RemovePhoto(Photo photo)
        {
            _context.Photos.Remove(photo);
        }
    }
}