using Microsoft.EntityFrameworkCore;
using PSK.ApiService.Data;
using PSK.ApiService.Repositories.Interfaces;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories;

public class DiscussionRepository : BaseRepository<Discussion>, IDiscussionRepository
{
    public DiscussionRepository(AppDbContext context) : base(context) { }
}