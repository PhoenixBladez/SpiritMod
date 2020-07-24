using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class Tenderizer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Tenderizer");
			Tooltip.SetDefault("Occasionally inflicts Ichor\nKilling enemies with this weapon heals life");
		}


		public override void SetDefaults()
		{
			item.damage = 26;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 19;
			item.useAnimation = 19;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 3;
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			item.value = Item.sellPrice(0, 1, 50, 0);
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (Main.rand.Next(6) == 0) {
				target.AddBuff(BuffID.Ichor, 240);
			}
			if (target.life <= 0) {
				int healNumber = Main.rand.Next(5, 8);
				player.HealEffect(healNumber);
				if (player.statLife <= player.statLifeMax - healNumber)
					player.statLife += healNumber;
				else
					player.statLife += player.statLifeMax - healNumber;
			}
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(5) == 0) {
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 5);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void UseStyle(Player player)
		{
			float cosRot = (float)Math.Cos(player.itemRotation - 0.78f * player.direction * player.gravDir);
			float sinRot = (float)Math.Sin(player.itemRotation - 0.78f * player.direction * player.gravDir);
			for (int i = 0; i < 1; i++) {
				float length = (item.width * 1.2f - i * item.width / 9) * item.scale + 16; //length to base + arm displacement
				int dust = Dust.NewDust(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, 5, player.velocity.X * 0.9f, player.velocity.Y * 0.9f, 100, Color.Transparent, 1.8f);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].scale = 0.5f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}