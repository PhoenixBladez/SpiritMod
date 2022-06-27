using SpiritMod.Projectiles.Thrown;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 22;
			Item.height = 22;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item106;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<FlaskOfGoreProj>();
			Item.useAnimation = 24;
			Item.useTime = 24;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.shootSpeed = 11f;
			Item.damage = 15;
			Item.knockBack = 4.5f;
			Item.value = Item.sellPrice(0, 0, 0, 25);
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = false;
			Item.consumable = true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.HasBuff(Mod.Find<ModBuff>("CrimsonSkullBuff").Type))
				damage *= 2;
		}
	}
}