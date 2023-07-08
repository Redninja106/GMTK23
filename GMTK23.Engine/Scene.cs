using GMTK23.Engine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Engine;
public class Scene : Entity
{
    public static Scene Active { get; private set; }

    public PhysicsManager? Physics => GetComponent<PhysicsManager>();

    public Scene() : base()
    {

    }

    public override void Render(ICanvas canvas)
    {
        base.Render(canvas);
    }

    private void RenderComponent(Component component)
    {
    }

    public void Initialize()
    {
        Initialize(null!);
    }

    public void SetActive()
    {
        Active = this;
    }

    public void UpdatePhysics()
    {
        Physics?.UpdateWorld();
    }
}