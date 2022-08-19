using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.BossLoot.AtlasDrops
{
	public class Mountain : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Mountain");
			Tooltip.SetDefault("'Swinging the blade strengthens you'\nOccasionally inflicts foes with 'Unstable Affliction'");
		}

		int charger;

		public override void SetDefaults()
		{
			Item.damage = 88;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 58;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 7;
			Item.value = Item.sellPrice(0, 8, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<PrismaticBolt>();
			Item.shootSpeed = 12;
		}

		public override bool? UseItem(Player player)
		{
			player.AddBuff(BuffID.Ironskin, 300);
			return null;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			charger++;
			if (charger >= 7)
			{
				for (int I = 0; I < 4; I++)
					Projectile.NewProjectile(source, position.X - 8, position.Y + 8, velocity.X + ((float)Main.rand.Next(-230, 230) / 300), velocity.Y + ((float)Main.rand.Next(-230, 230) / 300), ModContent.ProjectileType<AtlasBolt>(), 50, knockback, player.whoAmI, 0f, 0f);
				charger = 0;
			}
			return true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit) => target.AddBuff(ModContent.BuffType<Buffs.DoT.Afflicted>(), 180);
	}
}