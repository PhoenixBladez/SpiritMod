using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Pet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Pets
{
	public class PowerRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ring of Willpower");
			Tooltip.SetDefault("Summons a Lantern Power Battery to light the way");
		}

		public override void SetDefaults()
		{

			Item.damage = 0;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<Lantern>();
			Item.width = 16;
			Item.height = 30;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = ItemRarityID.Orange;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 3, 50, 0);
			Item.buffType = ModContent.BuffType<LanternBuff>();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(Item.buffType, 3600, true);
			}
		}

		public override bool CanUseItem(Player player)
		{
			return player.miscEquips[1].IsAir;
		}

		public override void AddRecipes()
		{

			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ItemID.MeteoriteBar, 10);
			modRecipe.AddIngredient(ItemID.FallenStar, 3);
			modRecipe.AddIngredient(ItemID.Emerald, 1);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.Register();
		}
	}
}