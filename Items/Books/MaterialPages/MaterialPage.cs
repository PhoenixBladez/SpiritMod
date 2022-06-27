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
			Item.noMelee = true;
			Item.useTurn = true;
			//item.channel = true; //Channel so that you can held the weapon [Important]
			Item.rare = ItemRarityID.Green;
			Item.width = 54;
			Item.height = 50;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = false;
			Item.noUseGraphic = false;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
	}
}
