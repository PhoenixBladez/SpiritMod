using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;

namespace SpiritMod.Mechanics.Interfaces.Hosts
{
    public class ComponentManager<T> where T : Entity, IComponent
    {
        protected List<T> Objects = new List<T>();

        protected virtual void OnUpdate() { }

        public void Update()
        {
            OnUpdate();
            foreach (T TV in Objects.ToArray())
            {
                if (TV != null)
                    TV.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (T TV in Objects.ToArray())
            {
                if(TV != null)
                TV.Draw(spriteBatch);
            }
        }

        public void Clear() => Objects.Clear();

        public void AddElement(T TV)
        {
			//Load logic maybe

            /*foreach (T TOV in Objects)
            {
                if (TOV.position == TV.position)
                    return;
            }*/

            Objects.Add(TV);
        }
    }
}
