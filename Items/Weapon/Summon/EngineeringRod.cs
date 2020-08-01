using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Weapon.Summon
{
	public class EngineeringRod : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Engineering Rod");
			Tooltip.SetDefault("Summons a stationary Tesla Turret");
		}


		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.QueenSpiderStaff); //only here for values we haven't defined ourselves yet
			item.damage = 75;  //placeholder damage :3
			item.mana = 16;   //somehow I think this might be too much...? -thegamemaster1234
			item.width = 40;
			item.height = 40;
			item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.knockBack = 2.5f;
			item.UseSound = SoundID.Item25;
			item.shoot = ModContent.ProjectileType<TeslaTurret>();
			item.shootSpeed = 0f;
		}

		public override bool CanUseItem(Player player)
		{
			player.FindSentryRestingSpot(item.shoot, out int worldX, out int worldY, out _);
			worldX /= 16;
			worldY /= 16;
			worldY-= 3;
			return !WorldGen.SolidTile(worldX, worldY);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
			worldY -= 16;
			Projectile.NewProjectile(worldX, worldY - pushYUp, speedX, speedY, type, damage, knockBack, player.whoAmI);
			player.UpdateMaxTurrets();
			return false;
		}

	}
}
