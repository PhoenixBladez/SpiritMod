using SpiritMod.Projectiles.Thrown;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;

namespace SpiritMod.Items.Sets.ThrownMisc.FlaskofGore
{
	public class FlaskOfGore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flask Of Gore");
			Tooltip.SetDefault("Flasks may create a crimson skull upon popping\nPicking up crimson skulls temporarily increases the flask's damage");
		}


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.width = 22;
			item.height = 22;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item106;
			item.ranged = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<FlaskOfGoreProj>();
			item.useAnimation = 24;
			item.useTime = 24;
			item.consumable = true;
			item.maxStack = 999;
			item.shootSpeed = 11f;
			item.damage = 15;
			item.knockBack = 4.5f;
			item.value = Item.sellPrice(0, 0, 0, 25);
			item.rare = ItemRarityID.Green;
			item.autoReuse = false;
			item.consumable = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.HasBuff(mod.BuffType("CrimsonSkullBuff"))) {
				damage *= 2;
			}
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
	}
}