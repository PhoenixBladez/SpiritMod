using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace SpiritMod.Items.Books.UI
{
    class InvisibleUIScrollbar : FixedUIScrollbar
    {
        internal UserInterface userInterface;
        public InvisibleUIScrollbar(UserInterface userInterface) : base(userInterface)
        {
            this.userInterface = userInterface;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            UserInterface temp = UserInterface.ActiveInstance;
            UserInterface.ActiveInstance = userInterface;
            //base.DrawSelf(spriteBatch);
            UserInterface.ActiveInstance = temp;
        }
    }
}