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
        public PermissionService(IRepository<Permission> repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PermissionDto>> GetListAsync()
        {
            var permissions = await _repository.GetListAsync(null);

            return _mapper.Map<List<PermissionDto>>(permissions);
        }


        public Task<List<PermissionTreeDto>> GetPermissionTreesAsync()
        {
            // TODO:
            throw new NotImplementedException();
        }
    }
}
