using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Weapon.Summon
{
	public class HedronStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hedron Staff");
			Tooltip.SetDefault("Summons a Hedron to explode on your enemies");
		}


		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.QueenSpiderStaff); //only here for values we haven't defined ourselves yet
			item.damage = 48;  //placeholder damage :3
			item.mana = 25;   //somehow I think this might be too much...? -thegamemaster1234
			item.width = 40;
			item.height = 40;
			item.value = Terraria.Item.sellPrice(0, 8, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.knockBack = 2.5f;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.UseSound = SoundID.Item25;
			item.shoot = ModContent.ProjectileType<HedronMinion>();
			item.shootSpeed = 0f;
		}

		public override bool CanUseItem(Player player)
		{
			player.FindSentryRestingSpot(item.shoot, out int worldX, out int worldY, out _);
			worldX /= 16;
			worldY /= 16;
			worldY--;
			return !WorldGen.SolidTile(worldX, worldY);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
			Projectile.NewProjectile(worldX, worldY - pushYUp, speedX, speedY, type, damage, knockBack, player.whoAmI);
			player.UpdateMaxTurrets();
			return false;
		}

	}
}
