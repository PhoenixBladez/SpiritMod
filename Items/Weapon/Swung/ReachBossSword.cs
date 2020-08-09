using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Projectiles.Sword;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
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
			item.damage = 25;
			item.melee = true;
			item.width = 64;
			item.height = 62;
			item.useTime = 39;
			item.useAnimation = 39;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			item.shoot = ModContent.ProjectileType<BloodWave>();
			item.rare = ItemRarityID.Green;
			item.shootSpeed = 8f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.statLife <= player.statLifeMax2 / 2) {
				item.useTime = 32;
				item.useAnimation = 32;
			}
			else if (player.statLife <= player.statLifeMax2 / 3) {
				item.useTime = 34;
				item.useAnimation = 34;
			}
			else if (player.statLife <= player.statLifeMax2 / 4) {
				item.useTime = 36;
				item.useAnimation = 36;
			}
			else {
				item.useTime = 39;
				item.useAnimation = 39;
			}
			return base.CanUseItem(player);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (Main.rand.Next(4) == 1 && player.statLife >= player.statLifeMax2 / 2) {
				Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 20);
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, 0, player.whoAmI);
				return false;
			}
			else if (Main.rand.Next(2) == 1) {
				Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 20);
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, 0, player.whoAmI);
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