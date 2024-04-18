using UnityEngine;
namespace Interfaces
{
    public interface IBoat
    {
        public void DockBoat(Vector3 dockLocation, Dock.Dock dockToUndock);

        public void UndockBoat(Dock.Dock dockToUndock);
    }    
}