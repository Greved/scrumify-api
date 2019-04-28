using DryIoc;

namespace Scrumify.Core.DI
{
    public interface IModule
    {
        void Load(IRegistrator builder);
    }
}