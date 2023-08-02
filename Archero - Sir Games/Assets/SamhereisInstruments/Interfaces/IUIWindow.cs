using System.Threading.Tasks;

namespace Interfaces
{
    public interface IUIWindow
    {
        public void Enable(float? duration = null);

        public void Disable(float? duration = null);
    }
}