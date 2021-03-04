using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Books.MaterialPages
{
	public abstract class MaterialPage : ModItem
	{
		public override void SetDefaults() {
			item.noMelee = true;
			item.useTurn = true;
			//item.channel = true; //Channel so that you can held the weapon [Important]
			item.rare = 2;
			item.width = 54;
			item.height = 50;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.autoReuse = false;
			item.noUseGraphic = false;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
	}
}
