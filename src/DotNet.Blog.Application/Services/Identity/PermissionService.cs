using AutoMapper;
using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepository<Permission> _repository;
        private readonly IMapper _mapper;
        public PermissionService(IRepository<Permission> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PermissionDto>> GetListAsync()
        {
            // TODO: sort order
            var permissions = await _repository.GetListAsync(null);

            return _mapper.Map<List<PermissionDto>>(permissions);
        }


        public async Task<List<PermissionTreeDto>> GetPermissionTreesAsync()
        {
            // TODO:sort order
            var permissions = await _repository.GetListAsync(null);

            var treeDtos = new List<PermissionTreeDto>();

            foreach (var item in permissions.Where(p => p.Code == null).OrderBy(p => p.SortOrder))
            {
                PermissionTreeDto dto = new()
                {
                    Code = item.Code,
                    Name = item.Name,
                    ParentCode = item.ParentCode,
                    SortOrder = item.SortOrder,
                    ChildPermissions = permissions.Where(p => p.ParentCode == item.Code)
                        .OrderBy(p => p.SortOrder)
                        .Select(p => new PermissionTreeDto()
                        {
                            Code = item.Code,
                            Name = item.Name,
                            ParentCode = item.ParentCode,
                            SortOrder = item.SortOrder,
                        }).ToList()
                };
            }
            throw new NotImplementedException();
        }
    }
}
