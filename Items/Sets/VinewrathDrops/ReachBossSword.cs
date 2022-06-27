using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Projectiles.Sword;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.VinewrathDrops
{
	public class ReachBossSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodthorn Blade");
			Tooltip.SetDefault("Increases in speed as health wanes\nOccasionally shoots out a bloody wave\nFires waves more frequently when below 1/2 health\nMelee critical hits poison enemies and inflict 'Withering Leaf'");
		}

		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = DamageClass.Melee;
			Item.width = 64;
			Item.height = 62;
			Item.useTime = 39;
			Item.useAnimation = 39;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			Item.shoot = ModContent.ProjectileType<BloodWave>();
			Item.rare = ItemRarityID.Green;
			Item.shootSpeed = 8f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.statLife <= player.statLifeMax2 / 4) {
				Item.useTime = 32;
				Item.useAnimation = 32;
			}
			else if (player.statLife <= player.statLifeMax2 / 3) {
				Item.useTime = 34;
				Item.useAnimation = 34;
			}
			else if (player.statLife <= player.statLifeMax2 / 2) { 
				Item.useTime = 36;
				Item.useAnimation = 36;
			}
			else {
				Item.useTime = 39;
				Item.useAnimation = 39;
			}
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (Main.rand.Next(4) == 1 && player.statLife >= player.statLifeMax2 / 2) {
				SoundEngine.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 20);
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, 0, player.whoAmI);
				return false;
			}
			else if (Main.rand.Next(2) == 1) {
				SoundEngine.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 20);
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, 0, player.whoAmI);
				return false;

			}
			return false;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (crit) {
				target.AddBuff(BuffID.Poisoned, 240);
				target.AddBuff(ModContent.BuffType<WitheringLeaf>(), 120, true);
			}
		}
	}
}