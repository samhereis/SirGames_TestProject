using System.Threading.Tasks;
using UnityEngine;

namespace DI
{
    public interface IDIDependent
    {
        public void LoadDependencies()
        {
            DIBox.InjectDataTo(this);

            /*if(this is MonoBehaviour)
            {
                foreach (var monobeh in (this as MonoBehaviour).GetComponentsInChildren<IDIDependent>(true))
                {
                    DIBox.InjectDataTo(monobeh);
                }
            }*/
        }
    }
}