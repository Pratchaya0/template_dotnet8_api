using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using template_dotnet8_api.Data;
using template_dotnet8_api.DTOs.Pagination;
using template_dotnet8_api.Helpers;
using template_dotnet8_api.Models;

namespace template_dotnet8_api.Services
{
    public class ServiceBase
    {
        private readonly AppDBContext _dBContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public ServiceBase(AppDBContext dBContext, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _dBContext = dBContext;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public Func<DateTime> Now { get; private set; } = () => DateTime.Now;

        public void SetNow(DateTime now) => Now = () => now;

        public void ResetNow() => Now = () => DateTime.Now;

        public string GetUserId() => _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public string GetUserName() => _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        public string GetPersonId() => _httpContext.HttpContext.User.FindFirstValue("PersonId");

        public IEnumerable<string> GetUserRoles()
        {
            return _httpContext.HttpContext.User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();
        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>() where TEntity : class
        {
            var entities = await _dBContext.Set<TEntity>().AsNoTracking().ToListAsync();
            var dtos = _mapper.Map<List<TDTO>>(entities);
            return dtos;
        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>(PaginationDto paginationDto) where TEntity : class
        {
            var queryable = _dBContext.Set<TEntity>().AsNoTracking().AsQueryable();
            await _httpContext.HttpContext.InsertPaginationParametersInResponse(queryable, paginationDto.RecordsPerPage, paginationDto.Page);
            var entities = await queryable.Paginate(paginationDto).ToListAsync();
            var dtos = _mapper.Map<List<TDTO>>(entities);

            return dtos;
        }

        protected async Task<TDTO> Get<TEntity, TDTO>(string id) where TEntity : class, IId
        {
            var guid = Guid.Parse(id);
            var entity = await _dBContext.Set<TEntity>().FindAsync(guid);
            var dto = _mapper.Map<TDTO>(entity);

            return dto;
        }

        protected async Task<TDTO> Post<TAdd, TEntity, TDTO>(TAdd newItem) where TEntity : class, IId
        {
            var entity = _mapper.Map<TEntity>(newItem);
            entity.Id = new Guid();
            _dBContext.Set<TEntity>().Add(entity);
            await _dBContext.SaveChangesAsync();
            var dto = _mapper.Map<TDTO>(entity);
            return dto;
        }

        protected async Task<TDTO> Put<TUpdate, TEntity, TDTO>(string id, TUpdate newItem) where TEntity : class, IId
        {
            var guid = Guid.Parse(id);
            var entity = await _dBContext.Set<TEntity>().FindAsync(guid);

            entity = _mapper.Map(newItem, entity);

            _dBContext.Set<TEntity>().Update(entity);
            await _dBContext.SaveChangesAsync();

            var dto = _mapper.Map<TDTO>(entity);

            return dto;
        }

        protected async Task<TDTO> Delete<TEntity, TDTO>(string id) where TEntity : class, IId, new()
        {
            var guid = Guid.Parse(id);
            //var entity = await _dBContext.Set<TEntity>().FindAsync(guid);
            var entity = new TEntity() { Id = guid };
            _dBContext.Set<TEntity>().Remove(entity);
            await _dBContext.SaveChangesAsync();

            var dto = _mapper.Map<TDTO>(entity);

            return dto;
        }
    }
}
