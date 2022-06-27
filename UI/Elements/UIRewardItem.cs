using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

namespace SpiritMod.UI.Elements
{
    public class UIRewardItem : UIPanel
    {
        private Item _item;
        public Item Item { get => _item; }

		public UIRewardItem(int itemID, int stack) : base()
		{
			Item item = new Item();
			item.SetDefaults(itemID);
			item.stack = stack;
			_item = item;
		}

		public UIRewardItem(Item item) : base()
        {
            _item = item;
        }

        public override void OnInitialize()
        {
            int size = 42;

            Width.Set(size, 0);
            Height.Set(size, 0);

            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float prevScale = Main.inventoryScale;
			Texture2D prevTexture = TextureAssets.InventoryBack.Value;

			TextureAssets.InventoryBack.Value = SpiritMod.Instance.GetTexture("UI/QuestUI/Textures/RewardItemBack");
			Main.inventoryScale = 0.8f;

            CalculatedStyle style = GetDimensions();


			ItemSlot.Draw(spriteBatch, ref _item, 1, new Vector2(style.X, style.Y));
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.hoverItemName = _item.Name;
                Main.HoverItem = _item.Clone();
            }

            Main.inventoryScale = prevScale;
			TextureAssets.InventoryBack.Value = prevTexture;
		}
    }
}
