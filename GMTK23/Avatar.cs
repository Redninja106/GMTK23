using GMTK23.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23
{
    internal class Avatar : IGameComponent, ISaveable
    {
        private Transform transform { get; set; }
        private Vector2 targetPos;
        private float walkSpeed;

        public RenderLayer RenderLayer => RenderLayer.Avatar;

        public void Render(ICanvas canvas)
        {
            canvas.ApplyTransform(transform);
            canvas.DrawRect(0, 0, 1, 2);
        }

        public void Update()
        {

        }

        public void setTargetPos(Vector2 targetPos)
        {
            this.targetPos = targetPos;
        }

        public IEnumerable<string> Save()
        {
            yield return transform.ToString();
        }

        public static IGameComponent Load(string[] args)
        {
            Transform transform = Transform.Parse(args[0]);
            Avatar av = new Avatar();
            av.transform = transform;
            return av;
        }
    }
}
