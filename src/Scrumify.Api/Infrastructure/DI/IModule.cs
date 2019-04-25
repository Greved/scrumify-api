using DryIoc;

namespace Scrumify.Api.Infrastructure.DI
{
    public interface IModule
    {
        void Load(IRegistrator builder);
    }
}