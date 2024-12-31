using System;
using System.IO;

namespace ArchGen
{
    public class ArchitectureGenerator : IArchitectureGenerator
    {
        private readonly IFileService _fileService;
        private readonly IProjectService _projectService;

        public ArchitectureGenerator(IFileService fileService, IProjectService projectService)
        {
            _fileService = fileService;
            _projectService = projectService;
        }

        public void GenerateNLayerArchitecture(string basePath, string solutionName)
        {
            // Create solution directory if it doesn't exist
            if (!_fileService.DirectoryExists(basePath))
            {
                _fileService.CreateDirectory(basePath);
            }

            // Create solution
            _projectService.CreateSolution(basePath, solutionName);

            // Create projects
            var projects = new[]
            {
                $"{solutionName}.Core",
                $"{solutionName}.Business",
                $"{solutionName}.DataAccess",
                $"{solutionName}.Entities",
                $"{solutionName}.API"
            };

            foreach (var project in projects)
            {
                var projectPath = Path.Combine(basePath, project);
                _fileService.CreateDirectory(projectPath);

                // Create project
                _projectService.CreateProject(projectPath, project.EndsWith(".API") ? "api" : "classlib");

                // Add project to solution
                _projectService.AddProjectToSolution(basePath, projectPath);

                // Add package references and project references
                AddNLayerReferences(basePath, projectPath, project, solutionName);

                // Create base classes and interfaces
                CreateNLayerFiles(projectPath, project);
            }
        }

        private void AddNLayerReferences(string basePath, string projectPath, string project, string solutionName)
        {
            if (project.EndsWith(".Core"))
            {
                _projectService.AddPackageReference(projectPath, project, "Microsoft.Extensions.DependencyInjection", "7.0.0");
            }
            else if (project.EndsWith(".Business"))
            {
                _projectService.AddPackageReference(projectPath, project, "Microsoft.Extensions.DependencyInjection", "7.0.0");
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Core", $"{solutionName}.Core.csproj"));
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Entities", $"{solutionName}.Entities.csproj"));
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.DataAccess", $"{solutionName}.DataAccess.csproj"));
            }
            else if (project.EndsWith(".DataAccess"))
            {
                _projectService.AddPackageReference(projectPath, project, "Microsoft.EntityFrameworkCore", "7.0.0");
                _projectService.AddPackageReference(projectPath, project, "Microsoft.EntityFrameworkCore.SqlServer", "7.0.0");
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Core", $"{solutionName}.Core.csproj"));
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Entities", $"{solutionName}.Entities.csproj"));
            }
            else if (project.EndsWith(".Entities"))
            {
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Core", $"{solutionName}.Core.csproj"));
            }
            else if (project.EndsWith(".API"))
            {
                _projectService.AddPackageReference(projectPath, project, "Microsoft.AspNetCore.Mvc.Core", "7.0.0");
                _projectService.AddPackageReference(projectPath, project, "Microsoft.EntityFrameworkCore.Design", "7.0.0");
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Core", $"{solutionName}.Core.csproj"));
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Business", $"{solutionName}.Business.csproj"));
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.DataAccess", $"{solutionName}.DataAccess.csproj"));
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Entities", $"{solutionName}.Entities.csproj"));
            }
        }

        private void CreateNLayerFiles(string projectPath, string project)
        {
            switch (project)
            {
                case var p when p.EndsWith(".Core"):
                    CreateCoreFiles(projectPath);
                    break;
                case var p when p.EndsWith(".Business"):
                    CreateBusinessFiles(projectPath);
                    break;
                case var p when p.EndsWith(".DataAccess"):
                    CreateDataAccessFiles(projectPath);
                    break;
                case var p when p.EndsWith(".Entities"):
                    CreateEntityFiles(projectPath);
                    break;
                case var p when p.EndsWith(".API"):
                    CreateApiFiles(projectPath);
                    break;
            }
        }

        public void GenerateOnionArchitecture(string basePath, string solutionName)
        {
            // Create solution directory if it doesn't exist
            if (!_fileService.DirectoryExists(basePath))
            {
                _fileService.CreateDirectory(basePath);
            }

            // Create solution
            _projectService.CreateSolution(basePath, solutionName);

            // Create projects
            var projects = new[]
            {
                $"{solutionName}.Domain",
                $"{solutionName}.Application",
                $"{solutionName}.Infrastructure",
                $"{solutionName}.Persistence",
                $"{solutionName}.API"
            };

            foreach (var project in projects)
            {
                var projectPath = Path.Combine(basePath, project);
                _fileService.CreateDirectory(projectPath);

                // Create project
                _projectService.CreateProject(projectPath, project.EndsWith(".API") ? "api" : "classlib");

                // Add project to solution
                _projectService.AddProjectToSolution(basePath, projectPath);

                // Add package references and project references
                AddOnionReferences(basePath, projectPath, project, solutionName);

                // Create base classes and interfaces
                CreateOnionFiles(projectPath, project);
            }
        }

        private void AddOnionReferences(string basePath, string projectPath, string project, string solutionName)
        {
            if (project.EndsWith(".Domain"))
            {
                _projectService.AddPackageReference(projectPath, project, "Microsoft.Extensions.DependencyInjection", "7.0.0");
            }
            else if (project.EndsWith(".Application"))
            {
                _projectService.AddPackageReference(projectPath, project, "Microsoft.Extensions.DependencyInjection", "7.0.0");
                _projectService.AddPackageReference(projectPath, project, "Microsoft.EntityFrameworkCore", "7.0.0");
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Domain", $"{solutionName}.Domain.csproj"));
            }
            else if (project.EndsWith(".Infrastructure"))
            {
                _projectService.AddPackageReference(projectPath, project, "Microsoft.EntityFrameworkCore", "7.0.0");
                _projectService.AddPackageReference(projectPath, project, "Microsoft.EntityFrameworkCore.SqlServer", "7.0.0");
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Domain", $"{solutionName}.Domain.csproj"));
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Application", $"{solutionName}.Application.csproj"));
            }
            else if (project.EndsWith(".Persistence"))
            {
                _projectService.AddPackageReference(projectPath, project, "Microsoft.EntityFrameworkCore", "7.0.0");
                _projectService.AddPackageReference(projectPath, project, "Microsoft.EntityFrameworkCore.SqlServer", "7.0.0");
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Domain", $"{solutionName}.Domain.csproj"));
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Application", $"{solutionName}.Application.csproj"));
            }
            else if (project.EndsWith(".API"))
            {
                _projectService.AddPackageReference(projectPath, project, "Microsoft.AspNetCore.Mvc.Abstractions", "2.2.0");
                _projectService.AddPackageReference(projectPath, project, "Microsoft.EntityFrameworkCore.Design", "7.0.0");
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Domain", $"{solutionName}.Domain.csproj"));
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Application", $"{solutionName}.Application.csproj"));
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Infrastructure", $"{solutionName}.Infrastructure.csproj"));
                _projectService.AddProjectReference(projectPath, project, Path.Combine(basePath, $"{solutionName}.Persistence", $"{solutionName}.Persistence.csproj"));
            }
        }

        private void CreateOnionFiles(string projectPath, string project)
        {
            switch (project)
            {
                case var p when p.EndsWith(".Domain"):
                    CreateDomainFiles(projectPath);
                    break;
                case var p when p.EndsWith(".Application"):
                    CreateApplicationFiles(projectPath);
                    break;
                case var p when p.EndsWith(".Infrastructure"):
                    CreateInfrastructureFiles(projectPath);
                    break;
                case var p when p.EndsWith(".Persistence"):
                    CreatePersistenceFiles(projectPath);
                    break;
                case var p when p.EndsWith(".API"):
                    CreateApiFiles(projectPath);
                    break;
            }
        }

        private void CreateCoreFiles(string projectPath)
        {
            var projectNamespace = _fileService.GetProjectNamespace(projectPath);

            // Create Interfaces folder
            var interfacesPath = Path.Combine(projectPath, "Interfaces");
            _fileService.CreateDirectory(interfacesPath);

            // Create IEntity interface
            var iEntityContent = $@"namespace {projectNamespace}.Core.Interfaces
{{
    public interface IEntity
    {{
        int Id {{ get; set; }}
    }}
}}";
            _fileService.WriteAllText(Path.Combine(interfacesPath, "IEntity.cs"), iEntityContent);

            // Create IRepository interface
            var iRepositoryContent = $@"using System.Linq.Expressions;

namespace {projectNamespace}.Core.Interfaces
{{
    public interface IRepository<T> where T : class, IEntity
    {{
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }}
}}";
            _fileService.WriteAllText(Path.Combine(interfacesPath, "IRepository.cs"), iRepositoryContent);
        }

        private void CreateBusinessFiles(string projectPath)
        {
            var projectNamespace = _fileService.GetProjectNamespace(projectPath);

            // Create Services folder
            var servicesPath = Path.Combine(projectPath, "Services");
            _fileService.CreateDirectory(servicesPath);

            // Create BaseService
            var baseServiceContent = $@"using {projectNamespace}.Core.Interfaces;

namespace {projectNamespace}.Business.Services
{{
    public abstract class BaseService<T> where T : class, IEntity
    {{
        protected readonly IRepository<T> _repository;

        protected BaseService(IRepository<T> repository)
        {{
            _repository = repository;
        }}

        public virtual async Task<T> GetByIdAsync(int id)
        {{
            return await _repository.GetByIdAsync(id);
        }}

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {{
            return await _repository.GetAllAsync();
        }}

        public virtual async Task AddAsync(T entity)
        {{
            await _repository.AddAsync(entity);
        }}

        public virtual async Task UpdateAsync(T entity)
        {{
            await _repository.UpdateAsync(entity);
        }}

        public virtual async Task DeleteAsync(T entity)
        {{
            await _repository.DeleteAsync(entity);
        }}
    }}
}}";
            _fileService.WriteAllText(Path.Combine(servicesPath, "BaseService.cs"), baseServiceContent);
        }

        private void CreateDataAccessFiles(string projectPath)
        {
            var projectNamespace = _fileService.GetProjectNamespace(projectPath);

            // Create Repositories folder
            var repositoriesPath = Path.Combine(projectPath, "Repositories");
            _fileService.CreateDirectory(repositoriesPath);

            // Create BaseRepository
            var baseRepositoryContent = $@"using Microsoft.EntityFrameworkCore;
using {projectNamespace}.Core.Interfaces;
using System.Linq.Expressions;

namespace {projectNamespace}.DataAccess.Repositories
{{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {{
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {{
            _context = context;
            _dbSet = context.Set<T>();
        }}

        public virtual async Task<T> GetByIdAsync(int id)
        {{
            return await _dbSet.FindAsync(id);
        }}

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {{
            return await _dbSet.ToListAsync();
        }}

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {{
            return await _dbSet.Where(predicate).ToListAsync();
        }}

        public virtual async Task AddAsync(T entity)
        {{
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }}

        public virtual async Task UpdateAsync(T entity)
        {{
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }}

        public virtual async Task DeleteAsync(T entity)
        {{
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }}
    }}
}}";
            _fileService.WriteAllText(Path.Combine(repositoriesPath, "BaseRepository.cs"), baseRepositoryContent);
        }

        private void CreateEntityFiles(string projectPath)
        {
            var projectNamespace = _fileService.GetProjectNamespace(projectPath);

            // Create Models folder
            var modelsPath = Path.Combine(projectPath, "Models");
            _fileService.CreateDirectory(modelsPath);

            // Create BaseEntity
            var baseEntityContent = $@"using {projectNamespace}.Core.Interfaces;

namespace {projectNamespace}.Entities.Models
{{
    public abstract class BaseEntity : IEntity
    {{
        public int Id {{ get; set; }}
        public DateTime CreatedAt {{ get; set; }}
        public DateTime? UpdatedAt {{ get; set; }}
    }}
}}";
            _fileService.WriteAllText(Path.Combine(modelsPath, "BaseEntity.cs"), baseEntityContent);
        }

        private void CreateApiFiles(string projectPath)
        {
            var projectNamespace = _fileService.GetProjectNamespace(projectPath);

            // Create Controllers folder
            var controllersPath = Path.Combine(projectPath, "Controllers");
            _fileService.CreateDirectory(controllersPath);

            // Determine if it's Onion or N-Layer based on project namespace
            bool isOnion = projectPath.Contains(".API") && (projectPath.Contains(".Domain") || projectPath.Contains(".Application"));

            // Create BaseController with appropriate namespaces
            var baseControllerContent = isOnion ?
                CreateOnionBaseController(projectNamespace) :
                CreateNLayerBaseController(projectNamespace);

            _fileService.WriteAllText(Path.Combine(controllersPath, "BaseController.cs"), baseControllerContent);
        }

        private string CreateNLayerBaseController(string projectNamespace)
        {
            return $@"using Microsoft.AspNetCore.Mvc;
using {projectNamespace}.Core.Interfaces;
using {projectNamespace}.Business.Services;

namespace {projectNamespace}.API.Controllers
{{
    [ApiController]
    [Route(""api/[controller]"")]
    public abstract class BaseController<T> : ControllerBase where T : class, IEntity
    {{
        protected readonly BaseService<T> _service;

        protected BaseController(BaseService<T> service)
        {{
            _service = service;
        }}

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {{
            var entities = await _service.GetAllAsync();
            return Ok(entities);
        }}

        [HttpGet(""{{id}}"")]
        public virtual async Task<IActionResult> GetById(int id)
        {{
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }}

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] T entity)
        {{
            await _service.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new {{ id = entity.Id }}, entity);
        }}

        [HttpPut(""{{id}}"")]
        public virtual async Task<IActionResult> Update(int id, [FromBody] T entity)
        {{
            if (id != entity.Id)
                return BadRequest();

            await _service.UpdateAsync(entity);
            return NoContent();
        }}

        [HttpDelete(""{{id}}"")]
        public virtual async Task<IActionResult> Delete(int id)
        {{
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            await _service.DeleteAsync(entity);
            return NoContent();
        }}
    }}
}}";
        }

        private string CreateOnionBaseController(string projectNamespace)
        {
            return $@"using Microsoft.AspNetCore.Mvc;
using {projectNamespace}.Domain.Entities;
using {projectNamespace}.Application.Services;
using {projectNamespace}.Application.Interfaces;

namespace {projectNamespace}.API.Controllers
{{
    [ApiController]
    [Route(""api/[controller]"")]
    public abstract class BaseController<T> : ControllerBase where T : BaseEntity
    {{
        protected readonly IService<T> _service;

        protected BaseController(IService<T> service)
        {{
            _service = service;
        }}

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {{
            var entities = await _service.GetAllAsync();
            return Ok(entities);
        }}

        [HttpGet(""{{id}}"")]
        public virtual async Task<IActionResult> GetById(int id)
        {{
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }}

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] T entity)
        {{
            await _service.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new {{ id = entity.Id }}, entity);
        }}

        [HttpPut(""{{id}}"")]
        public virtual async Task<IActionResult> Update(int id, [FromBody] T entity)
        {{
            if (id != entity.Id)
                return BadRequest();

            await _service.UpdateAsync(entity);
            return NoContent();
        }}

        [HttpDelete(""{{id}}"")]
        public virtual async Task<IActionResult> Delete(int id)
        {{
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            await _service.DeleteAsync(entity);
            return NoContent();
        }}
    }}
}}";
        }

        private void CreateDomainFiles(string projectPath)
        {
            var projectNamespace = _fileService.GetProjectNamespace(projectPath);

            // Create Entities folder
            var entitiesPath = Path.Combine(projectPath, "Entities");
            _fileService.CreateDirectory(entitiesPath);

            // Create BaseEntity
            var baseEntityContent = $@"namespace {projectNamespace}.Domain.Entities
{{
    public abstract class BaseEntity
    {{
        public int Id {{ get; set; }}
        public DateTime CreatedAt {{ get; set; }}
        public DateTime? UpdatedAt {{ get; set; }}
    }}
}}";
            _fileService.WriteAllText(Path.Combine(entitiesPath, "BaseEntity.cs"), baseEntityContent);

            // Create Interfaces folder
            var interfacesPath = Path.Combine(projectPath, "Interfaces");
            _fileService.CreateDirectory(interfacesPath);

            // Create IRepository interface
            var iRepositoryContent = $@"using System.Linq.Expressions;
using {projectNamespace}.Domain.Entities;

namespace {projectNamespace}.Domain.Interfaces
{{
    public interface IRepository<T> where T : BaseEntity
    {{
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }}
}}";
            _fileService.WriteAllText(Path.Combine(interfacesPath, "IRepository.cs"), iRepositoryContent);
        }

        private void CreateApplicationFiles(string projectPath)
        {
            var projectNamespace = _fileService.GetProjectNamespace(projectPath);

            // Create Services folder
            var servicesPath = Path.Combine(projectPath, "Services");
            _fileService.CreateDirectory(servicesPath);

            // Create Interfaces folder
            var interfacesPath = Path.Combine(projectPath, "Interfaces");
            _fileService.CreateDirectory(interfacesPath);

            // Create DTOs folder
            var dtosPath = Path.Combine(projectPath, "DTOs");
            _fileService.CreateDirectory(dtosPath);

            // Create IService interface
            var iServiceContent = $@"using {projectNamespace}.Domain.Entities;

namespace {projectNamespace}.Application.Interfaces
{{
    public interface IService<T> where T : BaseEntity
    {{
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }}
}}";
            _fileService.WriteAllText(Path.Combine(interfacesPath, "IService.cs"), iServiceContent);

            // Create BaseService
            var baseServiceContent = $@"using {projectNamespace}.Application.Interfaces;
using {projectNamespace}.Domain.Entities;
using {projectNamespace}.Domain.Interfaces;

namespace {projectNamespace}.Application.Services
{{
    public abstract class BaseService<T> : IService<T> where T : BaseEntity
    {{
        protected readonly IRepository<T> _repository;

        protected BaseService(IRepository<T> repository)
        {{
            _repository = repository;
        }}

        public virtual async Task<T> GetByIdAsync(int id)
        {{
            return await _repository.GetByIdAsync(id);
        }}

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {{
            return await _repository.GetAllAsync();
        }}

        public virtual async Task AddAsync(T entity)
        {{
            await _repository.AddAsync(entity);
        }}

        public virtual async Task UpdateAsync(T entity)
        {{
            await _repository.UpdateAsync(entity);
        }}

        public virtual async Task DeleteAsync(T entity)
        {{
            await _repository.DeleteAsync(entity);
        }}
    }}
}}";
            _fileService.WriteAllText(Path.Combine(servicesPath, "BaseService.cs"), baseServiceContent);
        }

        private void CreateInfrastructureFiles(string projectPath)
        {
            var projectNamespace = _fileService.GetProjectNamespace(projectPath);

            // Create Services folder
            var servicesPath = Path.Combine(projectPath, "Services");
            _fileService.CreateDirectory(servicesPath);

            // Create Email service example
            var emailServiceContent = $@"namespace {projectNamespace}.Infrastructure.Services
{{
    public interface IEmailService
    {{
        Task SendEmailAsync(string to, string subject, string body);
    }}

    public class EmailService : IEmailService
    {{
        public async Task SendEmailAsync(string to, string subject, string body)
        {{
            // Implement email sending logic
            await Task.CompletedTask;
        }}
    }}
}}";
            _fileService.WriteAllText(Path.Combine(servicesPath, "EmailService.cs"), emailServiceContent);
        }

        private void CreatePersistenceFiles(string projectPath)
        {
            var projectNamespace = _fileService.GetProjectNamespace(projectPath);

            // Create Repositories folder
            var repositoriesPath = Path.Combine(projectPath, "Repositories");
            _fileService.CreateDirectory(repositoriesPath);

            // Create Contexts folder
            var contextsPath = Path.Combine(projectPath, "Contexts");
            _fileService.CreateDirectory(contextsPath);

            // Create BaseRepository
            var baseRepositoryContent = $@"using Microsoft.EntityFrameworkCore;
using {projectNamespace}.Domain.Entities;
using {projectNamespace}.Domain.Interfaces;
using System.Linq.Expressions;

namespace {projectNamespace}.Persistence.Repositories
{{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {{
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {{
            _context = context;
            _dbSet = context.Set<T>();
        }}

        public virtual async Task<T> GetByIdAsync(int id)
        {{
            return await _dbSet.FindAsync(id);
        }}

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {{
            return await _dbSet.ToListAsync();
        }}

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {{
            return await _dbSet.Where(predicate).ToListAsync();
        }}

        public virtual async Task AddAsync(T entity)
        {{
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }}

        public virtual async Task UpdateAsync(T entity)
        {{
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }}

        public virtual async Task DeleteAsync(T entity)
        {{
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }}
    }}
}}";
            _fileService.WriteAllText(Path.Combine(repositoriesPath, "BaseRepository.cs"), baseRepositoryContent);

            // Create ApplicationDbContext
            var dbContextContent = $@"using Microsoft.EntityFrameworkCore;
using {projectNamespace}.Domain.Entities;

namespace {projectNamespace}.Persistence.Contexts
{{
    public class ApplicationDbContext : DbContext
    {{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {{
        }}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {{
            base.OnModelCreating(modelBuilder);
            // Add your entity configurations here
        }}
    }}
}}";
            _fileService.WriteAllText(Path.Combine(contextsPath, "ApplicationDbContext.cs"), dbContextContent);
        }
    }
}